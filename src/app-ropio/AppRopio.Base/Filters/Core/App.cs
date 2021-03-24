using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Filters.Core.Services;
using AppRopio.Base.Filters.Core.Services.Implementation;
using AppRopio.Base.Filters.Core.ViewModels.Filters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Selection;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Selection.Services;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Services;
using AppRopio.Base.Filters.Core.ViewModels.Sort;
using AppRopio.Base.Filters.Core.ViewModels.Sort.Services;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Base.Filters.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterType<IFiltersNavigationVmService>(() => new FiltersNavigationVmService());

            Mvx.RegisterSingleton<IFiltersConfigService>(() => new FiltersConfigService());

            Mvx.RegisterSingleton<ISortVmService>(() => new SortVmService());
            Mvx.RegisterSingleton<IFiltersVmService>(() => new FiltersVmService());
            Mvx.RegisterSingleton<IFilterSelectionVmService>(() => new FilterSelectionVmService());

            #region VMs registration

            var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

            vmLookupService.Register<ISortViewModel>(typeof(SortViewModel));
            vmLookupService.Register<IFiltersViewModel>(typeof(FiltersViewModel));
            vmLookupService.Register<IFilterSelectionViewModel>(typeof(FilterSelectionViewModel));

            #endregion
        }
    }
}
