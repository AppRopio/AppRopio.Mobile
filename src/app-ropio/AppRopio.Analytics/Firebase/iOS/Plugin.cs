﻿using System.IO;
using AppRopio.Analytics.Firebase.Core;
using AppRopio.Analytics.Firebase.Core.Services;
using AppRopio.Analytics.Firebase.iOS.Services;
using AppRopio.Base.Core.Plugins;
using Foundation;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Analytics.Firebase.iOS
{
    [MvxPlugin]
    [MvvmCross.Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Firebase analytics";

        public override void Load()
        {
            base.Load();
            try
            {
                string plistPath = Path.Combine(NSBundle.MainBundle.BundlePath, "GoogleService-Info.plist");
                global::Firebase.Core.App.Configure(new global::Firebase.Core.Options(plistPath));
            }
            catch {}

            Mvx.IoCProvider.RegisterSingleton<IFirebaseService>(() => new FirebaseService());
        }
    }
}
