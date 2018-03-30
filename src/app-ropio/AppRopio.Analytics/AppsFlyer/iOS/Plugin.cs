using System;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;
using AppRopio.Analytics.AppsFlyer.Core.Services;
using AppRopio.Analytics.AppsFlyer.iOS.Services;
using Foundation;
using System.Collections.Generic;
using AppsFlyerXamarinBinding;

namespace AppRopio.Analytics.AppsFlyer.iOS
{
    public class Plugin : IMvxPlugin
    {
        private static CustomAppsFlyerDelegate _trackerDelegate;

        public void Load()
        {
            var config = Mvx.Resolve<IAFConfigService>().Config;
            
            AppsFlyerTracker.SharedTracker().AppleAppID = config.AppId;
            AppsFlyerTracker.SharedTracker().AppsFlyerDevKey = config.DevKey;

            _trackerDelegate = new CustomAppsFlyerDelegate();

            AppsFlyerTracker.SharedTracker().TrackAppLaunch();
            AppsFlyerTracker.SharedTracker().LoadConversionDataWithDelegate(_trackerDelegate);
            
            Mvx.RegisterSingleton<IAppsFlyerService>(() => new AppsFlyerService());
        }

        #region Delegate

        private class CustomAppsFlyerDelegate : AppsFlyerTrackerDelegate
        {
            private static bool _installDataReceived;

            public override void OnAppOpenAttribution(NSDictionary attributionData)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("\nOnAppOpenAttribution\n");
#endif
                //var obj = new NSObject();
                //if (attributionData.TryGetValue(new NSString("link"), out obj))
                //{
                //    var link = (obj as NSString).ToString();
                //    var constUrl = @"https://lowprices.onelink.me/578688345?";

                //    link = link.Replace(constUrl, "");

                //    var splitted = link.Split('&');
                //    var linkDictionary = new Dictionary<string, string>();
                //    for (int i = 0; i < splitted.Length; i++)
                //    {
                //        var values = splitted[i].Split('=');
                //        if (values.Length % 2 == 0)
                //            linkDictionary.Add(values[0], values[1]);
                //    }

                //    if (linkDictionary.ContainsKey("af_dp"))
                //    {
                //        var deeplink = linkDictionary["af_dp"].Replace("%3A", ":").Replace("%2F", "/").Replace("%3D", "=");
                //        var splittedParams = deeplink.Replace("lowprices://", "").Split('=');

                //        var deeplinkDictionary = new Dictionary<string, string>();
                //        if (splittedParams.Length % 2 == 0)
                //        {
                //            for (int i = 0; i < splittedParams.Length; i += 2)
                //                deeplinkDictionary.Add(splittedParams[i], splittedParams[i + 1]);
                //        }

                //        if (deeplinkDictionary.ContainsKey("productId"))
                //        {
                //            var productID = deeplinkDictionary["productId"];

                //            if (_launched)
                //            {
                //                if (Mvx.CanResolve<IMvxMessenger>() && !AppSettings.FirstRun)
                //                    Mvx.Resolve<IMvxMessenger>().Publish(new ProductDeeplinkMessage(this, productID));
                //                else
                //                    LPSettings.DeeplinkProductId = productID;
                //            }
                //            else
                //                LPSettings.DeeplinkProductId = productID;

                //            _launched = false;
                //        }
                //    }
                //}
            }

            public override void OnAppOpenAttributionFailure(NSError error)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("\nOnAppOpenAttributionFailure\n");
#endif
            }

            public override void OnConversionDataReceived(NSDictionary installData)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("\nOnConversionDataReceived\n");
#endif
                //try
                //{
                //    var af_sub1 = installData.ObjectForKey(new NSString("af_sub1"));
                //    var af_dp = installData.ObjectForKey(new NSString("af_dp"));

                //    if (af_dp != null && !_installDataReceived)
                //    {
                //        var deeplink = (af_dp as NSString).ToString().Replace("%3A", ":").Replace("%2F", "/").Replace("%3D", "=");
                //        var splittedParams = deeplink.Replace("lowprices://", "").Split('=');

                //        var deeplinkDictionary = new Dictionary<string, string>();
                //        if (splittedParams.Length % 2 == 0)
                //        {
                //            for (int i = 0; i < splittedParams.Length; i += 2)
                //                deeplinkDictionary.Add(splittedParams[i], splittedParams[i + 1]);
                //        }

                //        if (deeplinkDictionary.ContainsKey("productId"))
                //        {
                //            var productID = deeplinkDictionary["productId"];

                //            LPSettings.DeeplinkProductId = productID;

                //            _installDataReceived = true;
                //        }
                //    }
                //}
                //catch
                //{

                //}
            }

            public override void OnConversionDataRequestFailure(NSError error)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine("\nOnConversionDataRequestFailure\n");
#endif
            }
        }

        #endregion
    }
}
