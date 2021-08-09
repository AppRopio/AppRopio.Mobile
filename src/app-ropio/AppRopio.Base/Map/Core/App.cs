using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Map.Core.Services;
using AppRopio.Base.Map.Core.Services.Implementation;
using AppRopio.Base.Map.Core.ViewModels.Points;
using AppRopio.Base.Map.Core.ViewModels.Points.List;
using AppRopio.Base.Map.Core.ViewModels.Points.Map;
using AppRopio.Base.Map.Core.ViewModels.Points.Services;
using AppRopio.Base.Map.Core.ViewModels.Points.Services.Implementation;
using MvvmCross;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Map.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            new API.App().Initialize();

            Mvx.IoCProvider.RegisterType<IMapNavigationVmService>(() => new MapNavigationVmService());

            Mvx.IoCProvider.RegisterSingleton<IMapConfigService>(() => new MapConfigService());

            Mvx.IoCProvider.RegisterSingleton<IPointsVmService>(() => new PointsVmService());

            #region VMs registration

            var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
            vmLookupService.Register<IPointsListViewModel>(typeof(PointsListViewModel));
            vmLookupService.Register<IPointsMapViewModel>(typeof(PointsMapViewModel));
            vmLookupService.Register<IPointAdditionalInfoVM>(typeof(PointAdditionalInfoVM));

            #endregion

            #region RouterSubscriber registration

            var routerService = Mvx.IoCProvider.Resolve<IRouterService>();
            routerService.Register<IPointsListViewModel>(new MapRouterSubscriber());

            #endregion
        }
    }
}
