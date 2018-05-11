using System;
using System.Collections;
using System.Collections.Generic;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Services.Analytics;
using AppRopio.Base.Core.Services.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;
using AppRopio.Base.Core.Messages.Localization;

namespace AppRopio.Base.Core.ViewModels.Services
{
    public class BaseVmService : MvxNavigatingObject, IDisposable
    {
        #region Fields

        private bool _disposed;

        #endregion

        #region Properties

        private MvxSubscriptionToken _languageToken;

        protected Dictionary<string, IEnumerable> CachedObjects { get; set; }

        #endregion

        #region Services

        protected IUserDialogs UserDialogs => Mvx.Resolve<IUserDialogs>();

        protected IAnalyticsNotifyingService AnalyticsNotifyingService => Mvx.Resolve<IAnalyticsNotifyingService>();

        #endregion

        #region Constructor

        public BaseVmService()
        {
            if (AppSettings.ApiKey.IsNullOrEmtpy())
                throw new ArgumentNullException(nameof(AppSettings.ApiKey));

            CachedObjects = new Dictionary<string, IEnumerable>();

            Mvx.CallbackWhenRegistered<IMvxMessenger>(service =>
            {
                _languageToken = service.Subscribe<LanguageChangedMessage>(OnLanguageChanged);
            });
        }

        ~BaseVmService()
        {
            Dispose(false);
        }

        #endregion

        #region Protected

        protected virtual void OnLanguageChanged(LanguageChangedMessage msg)
        {
            CachedObjects = new Dictionary<string, IEnumerable>();
        }

        protected virtual void OnException(Exception ex, string message = null)
        {
            if (!message.IsNullOrEmtpy())
                UserDialogs.Error(message);

            if (ex != null)
                MvxTrace.TaggedTrace(MvxTraceLevel.Error, this.GetType().FullName, ex?.BuildAllMessagesAndStackTrace() ?? string.Empty);
            if (ex != null || message != null)
                AnalyticsNotifyingService.NotifyExceptionHandled(message ?? string.Empty, ex?.BuildAllMessagesAndStackTrace() ?? string.Empty, false);
        }

        protected virtual async void OnConnectionException(ConnectionException ex)
        {
            var requestResult = ex?.RequestResult;

            if (requestResult == null || requestResult.RequestCanceled)
                return;

            if (requestResult.ResponseContent != null)
            {
                var contentAsString = await requestResult.ResponseContent.ReadAsStringAsync();
                await UserDialogs.Error(contentAsString ?? requestResult.Exception?.Message);
            }

            MvxTrace.TaggedTrace(MvxTraceLevel.Warning, this.GetType().FullName, Newtonsoft.Json.JsonConvert.SerializeObject(requestResult, Newtonsoft.Json.Formatting.Indented));

            if (ex != null)
                MvxTrace.TaggedTrace(MvxTraceLevel.Warning, this.GetType().FullName, ex.BuildAllMessagesAndStackTrace());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _languageToken?.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}

