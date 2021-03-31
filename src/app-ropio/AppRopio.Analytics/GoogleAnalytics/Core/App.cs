using AppRopio.Analytics.GoogleAnalytics.Core.Services;
using AppRopio.Analytics.GoogleAnalytics.Core.Services.Implementation;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Analytics.GoogleAnalytics.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<IGAConfigService>(new GAConfigService());

            Mvx.IoCProvider.RegisterSingleton<INotificationsSubscriber>(new NotificationsSubscriber());
        }
    }
}
