using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using AppRopio.Analytics.AppsFlyer.Core.Services.Implementation;
using AppRopio.Analytics.AppsFlyer.Core.Services;
using MvvmCross.Plugins.Messenger;

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