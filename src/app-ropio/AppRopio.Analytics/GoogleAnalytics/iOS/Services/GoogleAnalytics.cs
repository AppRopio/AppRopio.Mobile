using System;
using AppRopio.Analytics.GoogleAnalytics.Core.Services;
using Foundation;
using Google.Analytics;
using MvvmCross;

namespace AppRopio.Analytics.GoogleAnalytics.iOS.Services
{
    public class GoogleAnalytics : IGoogleAnalytics
    {
        private ITracker _tracker;
        private ITracker Tracker { get { return _tracker; } }

        public GoogleAnalytics()
        {
            Gai.SharedInstance.TrackUncaughtExceptions = true;
            Gai.SharedInstance.DispatchInterval = 5;

            _tracker = Gai.SharedInstance.GetTracker(Mvx.IoCProvider.Resolve<IGAConfigService>().Config.AppId);
        }

        public void TrackScreen(string screenName)
        {
            Tracker?.Set(GaiConstants.ScreenName, screenName);
            Tracker?.Send(DictionaryBuilder.CreateScreenView().Build());
        }

        public void TrackEvent(string eventType, string eventName, string label = null)
        {
            Tracker?.Send(DictionaryBuilder.CreateEvent(eventType, eventName, label, null).Build());
        }

        public void TrackException(string msg, bool isFatal)
        {
            Tracker?.Send(DictionaryBuilder.CreateException(msg, NSNumber.FromBoolean(isFatal)).Build());
        }

        public void TrackECommerce(decimal fullPrice, string orderId, string currency)
        {
            Tracker?.Send(DictionaryBuilder.CreateTransaction(
                orderId,
                string.Empty,
                new NSNumber(Convert.ToInt64(fullPrice)),
                tax: 0, shipping: 0, currencyCode: currency).Build());
        }
    }
}
