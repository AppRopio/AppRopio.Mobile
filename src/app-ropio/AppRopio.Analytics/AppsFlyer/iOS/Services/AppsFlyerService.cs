using System;
using AppRopio.Analytics.AppsFlyer.Core.Services;
using AppsFlyerXamarinBinding;
using Foundation;

namespace AppRopio.Analytics.AppsFlyer.iOS.Services
{
    public class AppsFlyerService : IAppsFlyerService
    {
        #region IAppsFlyerService implementation

        public void TrackScreen(string screenName)
        {
            var screenEvent = new NSDictionary(AFEventParameter.AFEventParamContentId, screenName,
                                               AFEventParameter.AFEventParamContentType, "screen opened");

            AppsFlyerTracker.SharedTracker().TrackEvent(AFEventName.AFEventUpdate, screenEvent);
        }

        public void TrackEvent(string category, string action, string label, object model)
        {
            var cutsomEvent = new NSDictionary(AFEventParameter.AFEventParamContentId, category,
                                               AFEventParameter.AFEventParamContentType, action,
                                               AFEventParameter.AFEventParamDescription, label);

            AppsFlyerTracker.SharedTracker().TrackEvent(AFEventName.AFEventUpdate, cutsomEvent);
        }

        public void TrackECommerce(decimal fullPrice, float quantity, string orderId, string currency)
        {
            var makeOrderEvent = new NSDictionary(AFEventParameter.AFEventParamContentId, "order",
                                                  AFEventParameter.AFEventParamContentType, "order created",
                                                  AFEventParameter.AFEventParamReceiptId, orderId,
                                                  AFEventParameter.AFEventParamRevenue, fullPrice.ToString(),
                                                  AFEventParameter.AFEventParamQuantity, quantity.ToString(),
                                                  AFEventParameter.AFEventParamCurrency, currency ?? "RUB");

            AppsFlyerTracker.SharedTracker().TrackEvent(AFEventName.AFEventPurchase, makeOrderEvent);
        }

        public void TrackException(string message, bool isFatal)
        {
            var exceptionEvent = new NSDictionary(AFEventParameter.AFEventParamContentId, "exception",
                                                  AFEventParameter.AFEventParamContentType, message,
                                                  AFEventParameter.AFEventParamDescription, $"Critical error: {isFatal}");

            AppsFlyerTracker.SharedTracker().TrackEvent("af_exception", exceptionEvent);
        }

        #endregion
    }
}
