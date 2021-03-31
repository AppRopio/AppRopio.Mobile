using System;
using AppRopio.Analytics.MobileCenter.Core.Services;
using AppRopio.Analytics.MobileCenter.Core.Services.Implementation;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Analytics.MobileCenter.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IMCConfigService>(new MCConfigService());

            Mvx.IoCProvider.RegisterSingleton<INotificationsSubscriber>(new NotificationsSubscriber());
        }
    }
}
