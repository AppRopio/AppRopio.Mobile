using System;
using AppRopio.Analytics.AppsFlyer.Core.Services;
using Android.App;
using Com.Appsflyer;
using System.Collections.Generic;

namespace AppRopio.Analytics.AppsFlyer.Droid.Services
{
    public class AppsFlyerService : IAppsFlyerService
    {
        #region IAppsFlyerService implementation

        public void TrackScreen(string screenName)
        {
            AppsFlyerLib.Instance.TrackEvent(Application.Context, AFInAppEventType.Update,
                    new Dictionary<string, Java.Lang.Object>
                    {
                        {AFInAppEventParameterName.ContentId, screenName},
                        {AFInAppEventParameterName.ContentType, "screen opened"}
                    });
        }

        public void TrackEvent(string category, string action, string label, object model)
        {
            AppsFlyerLib.Instance.TrackEvent(Application.Context, AFInAppEventType.Update,
                    new Dictionary<string, Java.Lang.Object>
                    {
                        {AFInAppEventParameterName.ContentId, category},
                        {AFInAppEventParameterName.ContentType, action},
                        {AFInAppEventParameterName.Description, label}
                    });
        }

        public void TrackECommerce(decimal fullPrice, float quantity, string orderId, string currency)
        {
            AppsFlyerLib.Instance.TrackEvent(Application.Context, AFInAppEventType.Purchase,
                    new Dictionary<string, Java.Lang.Object>
                    {
                        {AFInAppEventParameterName.ContentId, "order"},
                        {AFInAppEventParameterName.ContentType, "order created"},
                        {AFInAppEventParameterName.ReceiptId, orderId},
                        {AFInAppEventParameterName.Revenue, fullPrice.ToString()},
                        {AFInAppEventParameterName.Quantity, quantity.ToString()},
                        {AFInAppEventParameterName.Currency, currency ?? "RUB"}
                    });
        }

        public void TrackException(string message, bool isFatal)
        {
            AppsFlyerLib.Instance.TrackEvent(Application.Context, "af_exception",
                    new Dictionary<string, Java.Lang.Object>
                    {
                        {AFInAppEventParameterName.ContentId, "exception"},
                        {AFInAppEventParameterName.ContentType, message},
                        {AFInAppEventParameterName.Description, $"Critical error: {isFatal}"}
                    });
        }

        #endregion
    }
}
