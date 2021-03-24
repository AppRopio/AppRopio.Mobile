using System;
using Android.Gms.Analytics;
using AppRopio.Analytics.GoogleAnalytics.Core.Services;
using Android.App;
using Android.Gms.Analytics.Ecommerce;
using MvvmCross;

namespace AppRopio.Analytics.GoogleAnalytics.Droid.Services
{
    public class GoogleAnalytics : IGoogleAnalytics
    {
        public Tracker Tracker { get; private set; }

        public GoogleAnalytics()
        {
            var gaInstance = Android.Gms.Analytics.GoogleAnalytics.GetInstance(Application.Context);
#if DEBUG
            gaInstance.SetDryRun(true);
#endif
            gaInstance.SetLocalDispatchPeriod(5);

            Tracker = gaInstance.NewTracker(Mvx.Resolve<IGAConfigService>().Config.AppId);
            Tracker.EnableExceptionReporting(true);
            Tracker.EnableAdvertisingIdCollection(false);
            Tracker.EnableAutoActivityTracking(false);
        }

        public void TrackEvent(string category, string action, string label)
        {
            var builder = new HitBuilders.EventBuilder();
            builder.SetCategory(category);
            builder.SetAction(action);
            builder.SetLabel(label);

            Tracker.Send(builder.Build());
        }

        public void TrackException(string msg, bool isFatal)
        {
            var builder = new HitBuilders.ExceptionBuilder();
            builder.SetDescription(msg);
            builder.SetFatal(isFatal);

            Tracker.Send(builder.Build());
        }

        public void TrackECommerce(decimal fullPrice, string orderId, string currency)
        {
            var builder = new HitBuilders.ScreenViewBuilder();

            builder.SetProductAction(
                new ProductAction(ProductAction.ActionPurchase)
                .SetTransactionId(orderId)
                .SetTransactionAffiliation("")
                .SetTransactionRevenue((double)fullPrice)
                .SetTransactionTax(0)
                .SetTransactionShipping(0)
            );

            Tracker.Set("&cu", currency);
            Tracker.Send(builder.Build());
        }

        public void TrackScreen(string screenName)
        {
            Tracker.SetScreenName(screenName);
            Tracker.Send(new HitBuilders.ScreenViewBuilder().Build());
        }
    }
}
