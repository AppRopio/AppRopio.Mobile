using System;
using AppRopio.Analytics.MobileCenter.Core.Services;
using AppRopio.Analytics.MobileCenter.Core.Services.Implementation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Analytics.MobileCenter.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterSingleton<IMCConfigService>(new MCConfigService());

            Mvx.RegisterSingleton<INotificationsSubscriber>(new NotificationsSubscriber());
        }
    }
}
