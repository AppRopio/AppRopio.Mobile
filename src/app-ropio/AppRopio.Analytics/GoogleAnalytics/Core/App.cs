using AppRopio.Analytics.GoogleAnalytics.Core.Services;
using AppRopio.Analytics.GoogleAnalytics.Core.Services.Implementation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Analytics.GoogleAnalytics.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterSingleton<IGAConfigService>(new GAConfigService());

            Mvx.RegisterSingleton<INotificationsSubscriber>(new NotificationsSubscriber());
        }
    }
}
