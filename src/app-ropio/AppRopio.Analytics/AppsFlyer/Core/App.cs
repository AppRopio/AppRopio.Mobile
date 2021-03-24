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
            Mvx.CallbackWhenRegistered<IMvxMessenger>(() => Mvx.RegisterSingleton<INotificationsSubscriber>(new NotificationsSubscriber()));
            Mvx.RegisterSingleton<IAFConfigService>(new AFConfigService());
        }
    }
}