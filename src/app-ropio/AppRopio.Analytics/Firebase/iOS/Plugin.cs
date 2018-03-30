using System;
using MvvmCross.Platform.Plugins;
using MvvmCross.Platform;
using AppRopio.Analytics.Firebase.Core.Services;
using AppRopio.Analytics.Firebase.iOS.Services;
using System.IO;
using Foundation;

namespace AppRopio.Analytics.Firebase.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            try
            {
                string plistPath = Path.Combine(NSBundle.MainBundle.BundlePath, "GoogleService-Info.plist");
                global::Firebase.Core.App.Configure(new global::Firebase.Core.Options(plistPath));
            }
            catch {}

            Mvx.RegisterSingleton<IFirebaseService>(() => new FirebaseService());
        }
    }
}
