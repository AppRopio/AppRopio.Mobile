using System;
using System.Net.Http;
using System.Threading.Tasks;
using MvvmCross;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin.DownloadCache;
using ModernHttpClient;

namespace AppRopio.Base.iOS.Helpers
{
    public class ARMvxFileDownloadRequest
    {
        public ARMvxFileDownloadRequest(string url, string downloadPath)
        {
            Url = url;
            DownloadPath = downloadPath;
        }

        public string DownloadPath { get; private set; }
        public string Url { get; private set; }

        public event EventHandler<MvxFileDownloadedEventArgs> DownloadComplete;

        public event EventHandler<MvxValueEventArgs<Exception>> DownloadFailed;

        public void Start()
        {
            Task.Run(async () =>
            {
                try
                {
                    byte[] result;

                    using (var client = new HttpClient(new NativeMessageHandler()))
                    {
                        result = await client.GetByteArrayAsync(Url).ConfigureAwait(false);
                    }

                    ProcessResponse(result);
                }
                catch (Exception ex)
                {
                    FireDownloadFailed(ex);

                    Mvx.Resolve<IMvxTrace>().Trace(MvxTraceLevel.Error, $"{GetType().FullName}\n", ex.BuildAllMessagesAndStackTrace());
                }
            });
        }

        private void ProcessResponse(byte[] array)
        {
            try
            {
                var fileService = MvxFileStoreHelper.SafeGetFileStore();
                var tempFilePath = DownloadPath + ".tmp";

                fileService.WriteFile(tempFilePath, array);

                fileService.TryMove(tempFilePath, DownloadPath, true);
            }
            catch (Exception ex)
            {
                FireDownloadFailed(ex);
                Mvx.Resolve<MvvmCross.Platform.Platform.IMvxTrace>().Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Error, $"{GetType().FullName}\n", ex.BuildAllMessagesAndStackTrace());
                return;
            }

            Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(FireDownloadComplete);
        }

        private void FireDownloadFailed(Exception exception)
        {
            var handler = DownloadFailed;
            handler?.Invoke(this, new MvxValueEventArgs<Exception>(exception));
        }

        private void FireDownloadComplete()
        {
            var handler = DownloadComplete;
            handler?.Invoke(this, new MvxFileDownloadedEventArgs(Url, DownloadPath));
        }
    }
}
