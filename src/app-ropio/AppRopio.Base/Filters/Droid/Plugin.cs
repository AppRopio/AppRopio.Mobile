using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Filters.Core;
using AppRopio.Base.Filters.Core.ViewModels.Filters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Selection;
using AppRopio.Base.Filters.Core.ViewModels.Sort;
using AppRopio.Base.Filters.Droid.Views.Filters;
using AppRopio.Base.Filters.Droid.Views.Filters.Selection;
using AppRopio.Base.Filters.Droid.Views.Sort;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Filters.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Filters";

        public override void Load()
        {
            base.Load();

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<ISortViewModel, SortFragment>();
            viewLookupService.Register<IFiltersViewModel, FiltersFragment>();
            viewLookupService.Register<IFilterSelectionViewModel, SelectionFragment>();
        }
    }
}
