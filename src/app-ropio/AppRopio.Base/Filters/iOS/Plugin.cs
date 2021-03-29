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
using AppRopio.Base.Filters.Core;

namespace AppRopio.Base.Filters.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IFiltersThemeConfigService>(() => new FiltersThemeConfigService());

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<ISortViewModel, SortViewController>();
            viewLookupService.Register<IFiltersViewModel, FiltersViewController>();
            viewLookupService.Register<IFilterSelectionViewModel, SelectionViewController>();

            Mvx.IoCProvider.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(() =>
            {
                Mvx.IoCProvider.Resolve<IMvxTargetBindingFactoryRegistry>().RegisterCustomBindingFactory<UITextField>("FiltersDateBinding", view => new FiltersDateBinding(view));
            });
        }
    }
}
