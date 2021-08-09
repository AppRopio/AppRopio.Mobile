using System;
using System.Collections.Generic;
using AppRopio.Base.Droid.FCM;
using AppRopio.Navigation.Menu.Droid;
using AppRopio.Navigation.Menu.Droid.Views;

namespace AppRopio.Test.Droid
{
	public class Setup : MenuSetup
    {
        public Setup() 
            : base()
        {
            FcmSettings.Instance.ColorHex = "#FFD535";
            FcmSettings.Instance.IconResourceId = Droid.Resource.Drawable.logo;
            FcmSettings.Instance.ActivityType = typeof(MenuActivity);
        }

        protected override IEnumerable<Type> GetPluginTypes()
        {
            return new Type[]
            {
                typeof(AppRopio.Analytics.AppsFlyer.Droid.Plugin),
                typeof(AppRopio.Analytics.Firebase.Droid.Plugin),
                typeof(AppRopio.Analytics.GoogleAnalytics.Droid.Plugin),
                typeof(AppRopio.Analytics.MobileCenter.Droid.Plugin),
                typeof(AppRopio.Base.Auth.Droid.Plugin),
                typeof(AppRopio.Base.Contacts.Droid.Plugin),
                typeof(AppRopio.Base.Filters.Droid.Plugin),
                typeof(AppRopio.Base.Information.Droid.Plugin),
                typeof(AppRopio.Base.Map.Droid.Plugin),
                typeof(AppRopio.Base.Settings.Droid.Plugin),
                typeof(AppRopio.Feedback.Droid.Plugin),
                typeof(AppRopio.ECommerce.Basket.Droid.Plugin),
                typeof(AppRopio.ECommerce.Products.Droid.Plugin),
                typeof(AppRopio.ECommerce.Marked.Droid.Plugin),
                typeof(AppRopio.ECommerce.Loyalty.Droid.Plugin),
                typeof(AppRopio.ECommerce.HistoryOrders.Droid.Plugin),
                typeof(AppRopio.Payments.CloudPayments.Droid.Plugin),
                typeof(AppRopio.Payments.Droid.Plugin),
                typeof(AppRopio.Payments.YandexKassa.Droid.Plugin),
                typeof(AppRopio.Payments.Best2Pay.Droid.Plugin),

                typeof(MvvmCross.Plugin.File.Platforms.Android.Plugin),
                typeof(MvvmCross.Plugin.Json.Plugin),
                typeof(MvvmCross.Plugin.Messenger.Plugin),
                typeof(MvvmCross.Plugin.Network.Platforms.Android.Plugin),
                typeof(MvvmCross.Plugin.Share.Platforms.Android.Plugin),
                typeof(MvvmCross.Plugin.Visibility.Platforms.Android.Plugin)
            };
        }
    }
}
