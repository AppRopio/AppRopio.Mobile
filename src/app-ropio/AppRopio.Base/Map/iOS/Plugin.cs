using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Map.Core;
using AppRopio.Base.Map.Core.ViewModels.Points;
using AppRopio.Base.Map.Core.ViewModels.Points.List;
using AppRopio.Base.Map.Core.ViewModels.Points.Map;
using AppRopio.Base.Map.iOS.Services;
using AppRopio.Base.Map.iOS.Services.Implementation;
using AppRopio.Base.Map.iOS.Views.Points;
using AppRopio.Base.Map.iOS.Views.Points.List;
using AppRopio.Base.Map.iOS.Views.Points.Map;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Map.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Map";

        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IMapThemeConfigService>(() => new MapThemeConfigService());

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();
            viewLookupService.Register<IPointsListViewModel>(typeof(PointsListViewController));
            viewLookupService.Register<IPointsMapViewModel>(typeof(PointsMapViewController));
            viewLookupService.Register<IPointAdditionalInfoVM>(typeof(PointAdditionalInfoVC));
        }
    }
}
