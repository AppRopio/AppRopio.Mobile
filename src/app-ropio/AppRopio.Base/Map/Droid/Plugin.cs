using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Map.Core;
using AppRopio.Base.Map.Core.ViewModels.Points;
using AppRopio.Base.Map.Core.ViewModels.Points.List;
using AppRopio.Base.Map.Core.ViewModels.Points.Map;
using AppRopio.Base.Map.Droid.Views.Points;
using AppRopio.Base.Map.Droid.Views.Points.List;
using AppRopio.Base.Map.Droid.Views.Points.Map;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Map.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Map";

        public override void Load()
        {
            base.Load();

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();
            viewLookupService.Register<IPointsListViewModel>(typeof(PointsListFragment));
            viewLookupService.Register<IPointsMapViewModel>(typeof(PointsMapFragment));
            viewLookupService.Register<IPointAdditionalInfoVM>(typeof(PointAdditionalInfoFragment));
        }
    }
}
