using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Filters.Core.ViewModels.Filters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Selection;
using AppRopio.Base.Filters.Core.ViewModels.Sort;
using AppRopio.Base.Filters.iOS.Binding;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.Filters.iOS.Services.Implementation;
using AppRopio.Base.Filters.iOS.Views.Filters;
using AppRopio.Base.Filters.iOS.Views.Filters.Selection;
using AppRopio.Base.Filters.iOS.Views.Sort;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross;
using MvvmCross.Plugin;
using UIKit;

namespace AppRopio.Base.Filters.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            Mvx.RegisterSingleton<IFiltersThemeConfigService>(() => new FiltersThemeConfigService());

            var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<ISortViewModel, SortViewController>();
            viewLookupService.Register<IFiltersViewModel, FiltersViewController>();
            viewLookupService.Register<IFilterSelectionViewModel, SelectionViewController>();

            Mvx.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>((registry) => registry.RegisterCustomBindingFactory<UITextField>("FiltersDateBinding", view => new FiltersDateBinding(view)));
        }
    }
}
