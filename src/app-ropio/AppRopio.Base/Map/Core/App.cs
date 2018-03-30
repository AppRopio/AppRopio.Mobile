using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using AppRopio.Base.Map.Core.ViewModels.Points;
using AppRopio.Base.Map.Core.ViewModels.Points.List;
using AppRopio.Base.Map.Core.ViewModels.Points.Map;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Map.Core.Services.Implementation;
using AppRopio.Base.Map.Core.Services;
using AppRopio.Base.Map.Core.ViewModels.Points.Services;
using AppRopio.Base.Map.Core.ViewModels.Points.Services.Implementation;

namespace AppRopio.Base.Map.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterType<IMapNavigationVmService>(() => new MapNavigationVmService());

            Mvx.RegisterSingleton<IMapConfigService>(() => new MapConfigService());

            Mvx.RegisterSingleton<IPointsVmService>(() => new PointsVmService());

            #region VMs registration

            var vmLookupService = Mvx.Resolve<IViewModelLookupService>();
            vmLookupService.Register<IPointsListViewModel>(typeof(PointsListViewModel));
            vmLookupService.Register<IPointsMapViewModel>(typeof(PointsMapViewModel));
            vmLookupService.Register<IPointAdditionalInfoVM>(typeof(PointAdditionalInfoVM));

            #endregion

            #region RouterSubscriber registration

            var routerService = Mvx.Resolve<IRouterService>();
            routerService.Register<IPointsListViewModel>(new MapRouterSubscriber());

            #endregion
        }
    }
}
