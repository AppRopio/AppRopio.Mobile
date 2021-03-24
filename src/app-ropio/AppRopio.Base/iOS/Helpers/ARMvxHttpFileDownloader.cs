using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Platform.Core;
using MvvmCross.Plugin.DownloadCache;

namespace AppRopio.Base.iOS.Helpers
{
    public class ARMvxHttpFileDownloader
        : MvxLockableObject
        , IMvxHttpFileDownloader
    {
        private readonly Dictionary<ARMvxFileDownloadRequest, bool> _currentRequests =
            new Dictionary<ARMvxFileDownloadRequest, bool>();

        private const int DefaultMaxConcurrentDownloads = 30;
        private readonly int _maxConcurrentDownloads;
        private readonly Queue<ARMvxFileDownloadRequest> _queuedRequests = new Queue<ARMvxFileDownloadRequest>();

        public ARMvxHttpFileDownloader(int maxConcurrentDownloads = DefaultMaxConcurrentDownloads)
        {
            _maxConcurrentDownloads = maxConcurrentDownloads;
        }

        #region IMvxHttpFileDownloader Members

        public void RequestDownload(string url, string downloadPath, Action success, Action<Exception> error)
        {
            var request = new ARMvxFileDownloadRequest(url, downloadPath);
            request.DownloadComplete += (sender, args) =>
            {
                OnRequestFinished(request);
                success?.Invoke();
            };
            request.DownloadFailed += (sender, args) =>
            {
                OnRequestFinished(request);
                error?.Invoke(args.Value);
            };

            RunSyncOrAsyncWithLock(() =>
            {
                _queuedRequests.Enqueue(request);
                if (_currentRequests.Count < _maxConcurrentDownloads)
                {
                    StartNextQueuedItem();
                }
            });
        }

        #endregion IMvxHttpFileDownloader Members

        private void OnRequestFinished(ARMvxFileDownloadRequest request)
        {
            RunSyncOrAsyncWithLock(() =>
            {
                _currentRequests.Remove(request);
                if (_queuedRequests.Any())
                {
                    StartNextQueuedItem();
                }
            });
        }

        private void StartNextQueuedItem()
        {
            if (_currentRequests.Count >= _maxConcurrentDownloads)
                return;

            RunSyncOrAsyncWithLock(() =>
            {
                if (_currentRequests.Count >= _maxConcurrentDownloads)
                    return;

                if (!_queuedRequests.Any())
                    return;

                var request = _queuedRequests.Dequeue();
                _currentRequests.Add(request, true);
                request.Start();
            });
        }
    }

}
