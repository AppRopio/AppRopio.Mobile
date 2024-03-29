﻿using System.Collections.Generic;
using Android.App;
using AppRopio.Analytics.AppsFlyer.Core;
using AppRopio.Analytics.AppsFlyer.Core.Services;
using AppRopio.Analytics.AppsFlyer.Droid.Services;
using AppRopio.Base.Core.Plugins;
using Com.Appsflyer;
using MvvmCross;
using MvvmCross.Base;
using MvvmCross.Platforms.Android;
using MvvmCross.Platforms.Android.Core;
using MvvmCross.Plugin;

namespace AppRopio.Analytics.AppsFlyer.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "AppsFlyer";

        private static CustomAppsFlyerConversionDelegate _trackerDelegate;

        protected Activity CurrentActivity {
            get { return Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>().Activity; }
        }

        public override void Load()
        {
            base.Load();
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxMainThreadAsyncDispatcher>(() =>
            {
                if (CurrentActivity == null)
                {
                    Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>().ActivityChanged += Handle_ActivityChanged;
                }
                else
                {
                    Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
                   {
                       var config = Mvx.IoCProvider.Resolve<IAFConfigService>().Config;

                       AppsFlyerLib.Instance.StartTracking(CurrentActivity.Application, config.DevKey);

                       _trackerDelegate = new CustomAppsFlyerConversionDelegate();

                       AppsFlyerLib.Instance.RegisterConversionListener(Application.Context, _trackerDelegate);

                       Mvx.IoCProvider.RegisterSingleton<IAppsFlyerService>(() => new AppsFlyerService());
                   });
                }
            });
        }

        private void Handle_ActivityChanged(object sender, MvvmCross.Platforms.Android.Views.MvxActivityEventArgs e)
        {
            Mvx.IoCProvider.Resolve<IMvxAndroidActivityLifetimeListener>().ActivityChanged -= Handle_ActivityChanged;

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
