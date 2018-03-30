using System;
using AppRopio.Analytics.Firebase.Core.Services;
using Firebase.Analytics;
using Android.App;
using Newtonsoft.Json;

namespace AppRopio.Analytics.Firebase.Droid.Services
{
    public class FirebaseService : IFirebaseService
    {
        #region Fields

        private FirebaseAnalytics _fAnalytics;

        #endregion

        #region Constructor

        public FirebaseService()
        {
            _fAnalytics = FirebaseAnalytics.GetInstance(Application.Context);
        }

        #endregion

        #region IFirebaseService implementation

        public void TrackScreen(string screenName)
        {
            if (screenName.Length >= 36)
                screenName = screenName.Substring(screenName.Length - 35);
            
            var bundle = new Android.OS.Bundle();
            bundle.PutString(FirebaseAnalytics.Param.ItemId, screenName);

            _fAnalytics.LogEvent(FirebaseAnalytics.Event.ViewItem, bundle);
        }

        public void TrackEvent(string category, string action, string label, object model)
        {
            var bundle = new Android.OS.Bundle();
            bundle.PutString(nameof(action), action);
            bundle.PutString(nameof(label), label ?? "");
            bundle.PutString(nameof(model), model == null ? "" : JsonConvert.SerializeObject(model));

            _fAnalytics.LogEvent(category, bundle);
        }

        public void TrackECommerce(decimal fullPrice, string orderId, string currency)
        {
            var bundle = new Android.OS.Bundle();
            bundle.PutDouble(FirebaseAnalytics.Param.Price, (double)fullPrice);
            bundle.PutString(FirebaseAnalytics.Param.TransactionId, orderId);
            bundle.PutString(FirebaseAnalytics.Param.Currency, currency);

            _fAnalytics.LogEvent(FirebaseAnalytics.Event.EcommercePurchase, bundle);
        }

        public void TrackException(string message, bool isFatal)
        {
            var bundle = new Android.OS.Bundle();
            bundle.PutString(nameof(message), message);
            bundle.PutBoolean(nameof(isFatal), isFatal);

            _fAnalytics.LogEvent("app_exception", bundle);
        }

        #endregion
    }
}
