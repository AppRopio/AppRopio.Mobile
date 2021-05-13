using System.Threading.Tasks;
using AppRopio.Base.Core.Models.Analytics;
using AppRopio.Base.Core.Services.Analytics;
using AppRopio.Base.Core.Services.Location;
using AppRopio.Base.Core.Services.Push;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.TasksQueue;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Core
{
	public static class App
    {
        public static void Initialize()
        {
            (new API.App()).Initialize();

            Mvx.IoCProvider.RegisterType<IBaseVmNavigationService>(() => new BaseVmNavigationService());

            Mvx.IoCProvider.RegisterSingleton<IAnalyticsNotifyingService>(() => new AnalyticsNotifyingService());
            Mvx.IoCProvider.RegisterSingleton<IViewLookupService>(() => new ViewLookupService());
            Mvx.IoCProvider.RegisterSingleton<IViewModelLookupService>(() => new ViewModelLookupService());
            Mvx.IoCProvider.RegisterSingleton<IRouterService>(() => new RouterService());
            Mvx.IoCProvider.RegisterSingleton<IPushNotificationsService>(() => new PushNotificationsService());
            Mvx.IoCProvider.RegisterSingleton<ILocationService>(() => new LocationService());
            Mvx.IoCProvider.RegisterSingleton<ITasksQueueService>(() => new TasksQueueService());

            Mvx.IoCProvider.Resolve<IMvxLog>().Info("Base module is loaded");

            Mvx.IoCProvider.CallbackWhenRegistered<IMvxMessenger>(async () =>
            {
                await Task.Delay(1000);
                Mvx.IoCProvider.Resolve<IAnalyticsNotifyingService>().NotifyApp(AppState.Started);
            });
        }
    }
}
