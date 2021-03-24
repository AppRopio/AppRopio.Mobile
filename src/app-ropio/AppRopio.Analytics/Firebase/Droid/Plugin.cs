using System;
using AppRopio.Analytics.Firebase.Core.Services;
using AppRopio.Analytics.Firebase.Droid.Services;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Analytics.Firebase.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            try
            {
                //string plistPath = Path.Combine(NSBundle.MainBundle.BundlePath, "GoogleService-Info.plist");
                //global::Firebase.Core.App.Configure(new global::Firebase.Core.Options(plistPath));
            }
            catch { }

            Mvx.RegisterSingleton<IFirebaseService>(() => new FirebaseService());
        }
    }
}