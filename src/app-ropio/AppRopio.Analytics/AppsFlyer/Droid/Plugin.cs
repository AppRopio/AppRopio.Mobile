using System;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;
using AppRopio.Analytics.AppsFlyer.Core.Services;
using AppRopio.Analytics.AppsFlyer.Droid.Services;
using Com.Appsflyer;
using System.Collections.Generic;
using Android.App;
using MvvmCross.Platform.Droid.Platform;

namespace AppRopio.Analytics.AppsFlyer.Droid
{
    public class Plugin : IMvxPlugin
    {
        private static CustomAppsFlyerConversionDelegate _trackerDelegate;

        protected Activity CurrentActivity
        {
            get { return Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
        }

        public void Load()
        {
            var config = Mvx.Resolve<IAFConfigService>().Config;

            AppsFlyerLib.Instance.StartTracking(CurrentActivity.Application, config.DevKey);

            _trackerDelegate = new CustomAppsFlyerConversionDelegate();

            AppsFlyerLib.Instance.RegisterConversionListener(Application.Context, _trackerDelegate);

            Mvx.RegisterSingleton<IAppsFlyerService>(() => new AppsFlyerService());
        }

        #region Delegate

        public class CustomAppsFlyerConversionDelegate : Java.Lang.Object, IAppsFlyerConversionListener
        {
            public void OnAppOpenAttribution(IDictionary<string, string> p0)
            {

            }

            public void OnAttributionFailure(string p0)
            {

            }

            public void OnInstallConversionDataLoaded(IDictionary<string, string> p0)
            {

            }

            public void OnInstallConversionFailure(string p0)
            {

            }
        }

        #endregion
    }
}
