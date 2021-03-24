using System.Collections.Generic;
using Android.App;
using AppRopio.Analytics.AppsFlyer.Core.Services;
using AppRopio.Analytics.AppsFlyer.Droid.Services;
using Com.Appsflyer;
using MvvmCross.Platforms.Android.Core;
using MvvmCross;
using MvvmCross.Platform.Core;
using MvvmCross.Platforms.Android;
using MvvmCross.Plugin;

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
            Mvx.CallbackWhenRegistered<IMvxMainThreadDispatcher>(service =>
            {
                if (CurrentActivity == null)
                {
                    Mvx.Resolve<IMvxAndroidActivityLifetimeListener>().ActivityChanged += Handle_ActivityChanged;
                }
                else
                {
                    service.RequestMainThreadAction(() =>
                   {
                       var config = Mvx.Resolve<IAFConfigService>().Config;

                       AppsFlyerLib.Instance.StartTracking(CurrentActivity.Application, config.DevKey);

                       _trackerDelegate = new CustomAppsFlyerConversionDelegate();

                       AppsFlyerLib.Instance.RegisterConversionListener(Application.Context, _trackerDelegate);

                       Mvx.RegisterSingleton<IAppsFlyerService>(() => new AppsFlyerService());
                   });
                }
            });
        }

        private void Handle_ActivityChanged(object sender, MvvmCross.Platforms.Android.Views.MvxActivityEventArgs e)
        {
            Mvx.Resolve<IMvxAndroidActivityLifetimeListener>().ActivityChanged -= Handle_ActivityChanged;

            Load();
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
