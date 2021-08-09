using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Messages.Analytics;
using AppRopio.Base.Core.Models.Analytics;
using AppRopio.Base.Core.Services.Device;
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Plugin.Messenger;
using MvvmCross.Base;

namespace AppRopio.Base.Core.Services.Analytics
{
    public class AnalyticsNotifyingService : IAnalyticsNotifyingService
    {
        #region Services

        protected IMvxMessenger Messenger { get { return MvxSingleton<IMvxIoCProvider>.Instance?.CanResolve<IMvxMessenger>() ?? false ? Mvx.IoCProvider.Resolve<IMvxMessenger>() : null; } }

        protected IDeviceService DeviceService { get { return MvxSingleton<IMvxIoCProvider>.Instance?.CanResolve<IDeviceService>() ?? false ? Mvx.IoCProvider.Resolve<IDeviceService>() : null; } }

        #endregion

        #region Private

        private Dictionary<string, string> CostructDefaultData ()
        {
            return new Dictionary<string, string>
            {
                { nameof(AppSettings.CompanyID), AppSettings.CompanyID },
                { "DeviceToken", DeviceService?.Token ?? string.Empty },
                { "DeviceDateTime", DateTime.Now.ToString("O") }
            };
        }

        #endregion

        #region IAnalyticsNotifyingService implementaiton

        public void NotifyApp(AppState state, Dictionary<string, string> data = null)
        {
            Messenger?.Publish(new AppAnalyticsMessage(this, state, data ?? CostructDefaultData()));
        }

        public void NotifyScreen(string screenName, ScreenState state, Dictionary<string, string> data = null)
        {
            Messenger?.Publish(new ScreenAnalyticsMessage(this, screenName, state, data ?? CostructDefaultData()));
        }

        public void NotifyEventIsHandled(string category, string action, string label = null, object model = null, Dictionary<string, string> data = null)
        {
            Messenger?.Publish(new EventAnalyticsMessage(this, category, action, label, model, data ?? CostructDefaultData()));
        }

        public void NotifyOrderPurchased(decimal fullPrice, float quantity, string orderId, string currency, Dictionary<string, string> data = null)
        {
            Messenger?.Publish(new OrderAnalyticsMessage(this, fullPrice, quantity, orderId, currency, data ?? CostructDefaultData()));
        }

        public void NotifyExceptionHandled(string message, string stackTrace, bool isFatal, Dictionary<string, string> data = null)
        {
            Messenger?.Publish(new ExceptionAnalyticsMessage(this, message, stackTrace, isFatal, data ?? CostructDefaultData()));
        }

        #endregion
    }
}
