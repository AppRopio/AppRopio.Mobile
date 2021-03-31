using MvvmCross.ViewModels;
using MvvmCross;
using AppRopio.Analytics.AppsFlyer.Core.Services.Implementation;
using AppRopio.Analytics.AppsFlyer.Core.Services;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Analytics.AppsFlyer.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.CallbackWhenRegistered<IMvxMessenger>(() => Mvx.IoCProvider.RegisterSingleton<INotificationsSubscriber>(new NotificationsSubscriber()));
            Mvx.IoCProvider.RegisterSingleton<IAFConfigService>(new AFConfigService());
        }
    }
}