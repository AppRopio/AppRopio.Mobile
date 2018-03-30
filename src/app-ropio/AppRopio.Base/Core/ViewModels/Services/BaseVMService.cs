using System;
using System.Collections;
using System.Collections.Generic;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Services.Analytics;
using AppRopio.Base.Core.Services.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace AppRopio.Base.Core.ViewModels.Services
{
    public class BaseVmService : MvxNavigatingObject
    {
        #region Properties

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
        }

        #endregion

        #region Protected

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

        #endregion
    }
}

