using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Filters.Core.ViewModels.Filters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Selection;
using AppRopio.Base.Filters.Core.ViewModels.Sort;
using AppRopio.Base.Filters.Droid.Views.Filters;
using AppRopio.Base.Filters.Droid.Views.Filters.Selection;
using AppRopio.Base.Filters.Droid.Views.Sort;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Base.Filters.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<ISortViewModel, SortFragment>();
            viewLookupService.Register<IFiltersViewModel, FiltersFragment>();
            viewLookupService.Register<IFilterSelectionViewModel, SelectionFragment>();
        }
    }
}
