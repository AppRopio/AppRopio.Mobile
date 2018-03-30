using AppRopio.Analytics.Firebase.Core.Services;
using Firebase.Analytics;
using Foundation;
using Newtonsoft.Json;
using FirebaseAnalytics = Firebase.Analytics.Analytics;

namespace AppRopio.Analytics.Firebase.iOS.Services
{
    public class FirebaseService : IFirebaseService
    {
        public void TrackScreen(string screenName)
        {
            if (screenName.Length >= 36)
                screenName = screenName.Substring(screenName.Length - 35);
            
            NSString[] keys = { ParameterNamesConstants.ItemId };
            NSObject[] objects = { new NSString(screenName) };

            FirebaseAnalytics.LogEvent(EventNamesConstants.ViewItem, NSDictionary<NSString, NSObject>.FromObjectsAndKeys(objects, keys));
        }

        public void TrackEvent(string category, string action, string label, object model)
        {
            NSString[] keys = { new NSString(nameof(action)), new NSString(nameof(label)), new NSString(nameof(model)) };
            NSObject[] objects = { new NSString(action), new NSString(label ?? ""), (model == null ? new NSObject() : new NSString(JsonConvert.SerializeObject(model))) };

            FirebaseAnalytics.LogEvent(category, NSDictionary<NSString, NSObject>.FromObjectsAndKeys(objects, keys));
        }

        public void TrackECommerce(decimal fullPrice, string orderId, string currency)
        {
            NSString[] keys = { ParameterNamesConstants.Price, ParameterNamesConstants.TransactionId, ParameterNamesConstants.Currency };
            NSObject[] objects = { new NSNumber((double)fullPrice), new NSString(orderId), new NSString(currency) };

            FirebaseAnalytics.LogEvent(EventNamesConstants.EcommercePurchase, NSDictionary<NSString, NSObject>.FromObjectsAndKeys(objects, keys));
        }

        public void TrackException(string message, bool isFatal)
        {
            NSString[] keys = { new NSString(nameof(message)), new NSString(nameof(isFatal)) };
            NSObject[] objects = { new NSString(message), new NSNumber(isFatal) };

            FirebaseAnalytics.LogEvent("app_exception", NSDictionary<NSString, NSObject>.FromObjectsAndKeys(objects, keys));
        }
    }
}
