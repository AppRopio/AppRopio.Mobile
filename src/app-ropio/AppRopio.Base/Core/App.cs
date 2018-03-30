using System.Threading.Tasks;
using AppRopio.Base.Core.Models.Analytics;
using AppRopio.Base.Core.Services.Analytics;
using AppRopio.Base.Core.Services.Location;
using AppRopio.Base.Core.Services.Push;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.TasksQueue;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Core
{
    public static class App
    {
        public static void Initialize()
        {
            (new API.App()).Initialize();

            Mvx.RegisterSingleton<IAnalyticsNotifyingService>(() => new AnalyticsNotifyingService());
            Mvx.RegisterSingleton<IViewLookupService>(() => new ViewLookupService());
            Mvx.RegisterSingleton<IViewModelLookupService>(() => new ViewModelLookupService());
            Mvx.RegisterSingleton<IRouterService>(() => new RouterService());
            Mvx.RegisterSingleton<IPushNotificationsService>(() => new PushNotificationsService());
            Mvx.RegisterSingleton<ILocationService>(() => new LocationService());
            Mvx.RegisterSingleton<ITasksQueueService>(() => new TasksQueueService());

            MvxTrace.Trace(MvxTraceLevel.Diagnostic, "Base module is loaded");

            Mvx.CallbackWhenRegistered<IMvxMessenger>(async () =>
            {
                await Task.Delay(1000);
                Mvx.Resolve<IAnalyticsNotifyingService>().NotifyApp(AppState.Started);
            });
        }
    }
}
