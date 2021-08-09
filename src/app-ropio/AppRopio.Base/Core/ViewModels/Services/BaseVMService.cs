using System;
using System.Collections;
using System.Collections.Generic;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Messages.Localization;
using AppRopio.Base.Core.Services.Analytics;
using AppRopio.Base.Core.Services.UserDialogs;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Core.ViewModels.Services
{
	public class BaseVmService : IDisposable
    {
        #region Fields

        private bool _disposed;

        #endregion

        #region Properties

        private MvxSubscriptionToken _languageToken;

        protected Dictionary<string, IEnumerable> CachedObjects { get; set; }

        #endregion

        #region Services

        protected IUserDialogs UserDialogs => Mvx.IoCProvider.Resolve<IUserDialogs>();

        protected IAnalyticsNotifyingService AnalyticsNotifyingService => Mvx.IoCProvider.Resolve<IAnalyticsNotifyingService>();

        #endregion

        #region Constructor

        public BaseVmService()
        {
            if (AppSettings.ApiKey.IsNullOrEmtpy())
                throw new ArgumentNullException(nameof(AppSettings.ApiKey));

            CachedObjects = new Dictionary<string, IEnumerable>();

            Mvx.IoCProvider.CallbackWhenRegistered<IMvxMessenger>(() =>
            {
                _languageToken = Mvx.IoCProvider.Resolve<IMvxMessenger>().Subscribe<LanguageChangedMessage>(OnLanguageChanged);
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
                Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{this.GetType().FullName}: {ex?.BuildAllMessagesAndStackTrace() ?? string.Empty}");
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

            Mvx.IoCProvider.Resolve<IMvxLog>().Warn($"{this.GetType().FullName}: {Newtonsoft.Json.JsonConvert.SerializeObject(requestResult, Newtonsoft.Json.Formatting.Indented)}");

            if (ex != null)
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn($"{this.GetType().FullName}: {ex.BuildAllMessagesAndStackTrace()}");
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

