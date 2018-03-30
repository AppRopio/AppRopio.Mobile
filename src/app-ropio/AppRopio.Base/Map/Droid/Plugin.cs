using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Map.Core.ViewModels.Points;
using AppRopio.Base.Map.Core.ViewModels.Points.List;
using AppRopio.Base.Map.Core.ViewModels.Points.Map;
using AppRopio.Base.Map.Droid.Views.Points;
using AppRopio.Base.Map.Droid.Views.Points.List;
using AppRopio.Base.Map.Droid.Views.Points.Map;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Base.Map.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            var viewLookupService = Mvx.Resolve<IViewLookupService>();
            viewLookupService.Register<IPointsListViewModel>(typeof(PointsListFragment));
            viewLookupService.Register<IPointsMapViewModel>(typeof(PointsMapFragment));
            viewLookupService.Register<IPointAdditionalInfoVM>(typeof(PointAdditionalInfoFragment));
        }
    }
}
