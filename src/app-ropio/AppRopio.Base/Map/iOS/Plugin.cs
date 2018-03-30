using AppRopio.Base.Core.Services.ViewLookup;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;
using AppRopio.Base.Map.iOS.Services;
using AppRopio.Base.Map.iOS.Services.Implementation;
using AppRopio.Base.Map.Core.ViewModels.Points;
using AppRopio.Base.Map.Core.ViewModels.Points.List;
using AppRopio.Base.Map.Core.ViewModels.Points.Map;
using AppRopio.Base.Map.iOS.Views.Points;
using AppRopio.Base.Map.iOS.Views.Points.List;
using AppRopio.Base.Map.iOS.Views.Points.Map;

namespace AppRopio.Base.Map.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IMapThemeConfigService>(() => new MapThemeConfigService());

            var viewLookupService = Mvx.Resolve<IViewLookupService>();
            viewLookupService.Register<IPointsListViewModel>(typeof(PointsListViewController));
            viewLookupService.Register<IPointsMapViewModel>(typeof(PointsMapViewController));
            viewLookupService.Register<IPointAdditionalInfoVM>(typeof(PointAdditionalInfoVC));
        }
    }
}
