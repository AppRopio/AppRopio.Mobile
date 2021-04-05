using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Products.Core;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Header;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductTextContent;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductWebContent;
using AppRopio.ECommerce.Products.Droid.Views.Catalog;
using AppRopio.ECommerce.Products.Droid.Views.Catalog.Header;
using AppRopio.ECommerce.Products.Droid.Views.Categories.Cataloged;
using AppRopio.ECommerce.Products.Droid.Views.Categories.Disabled;
using AppRopio.ECommerce.Products.Droid.Views.Categories.StepByStep;
using AppRopio.ECommerce.Products.Droid.Views.ContentSearch;
using AppRopio.ECommerce.Products.Droid.Views.ModalProductCard;
using AppRopio.ECommerce.Products.Droid.Views.ProductCard;
using AppRopio.ECommerce.Products.Droid.Views.ProductCard.Selection;
using AppRopio.ECommerce.Products.Droid.Views.ProductTextContent;
using AppRopio.ECommerce.Products.Droid.Views.ProductWebContent;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Products.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Products";

        public override void Load()
        {
            base.Load();

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            var config = Mvx.IoCProvider.Resolve<IProductConfigService>().Config;
            switch (config.CategoriesType)
            {
                case CategoriesType.StepByStep:
                    viewLookupService.Register<IProductsViewModel>(typeof(SSCategoriesFragment));
                    viewLookupService.Register<ICatalogViewModel, CatalogFragment>();

                    viewLookupService.Register<IContentSearchInternalViewModel>(typeof(CatalogFragment));
                    break;
                case CategoriesType.Cataloged:
                    viewLookupService.Register<IProductsViewModel>(typeof(CCategoriesPagerFragment));
                    viewLookupService.Register<ICatalogViewModel, CCategoriesCatalogFragment>();

                    viewLookupService.Register<IContentSearchInternalViewModel>(typeof(CCategoriesCatalogFragment));
                    break;
                case CategoriesType.Disabled:
                    viewLookupService.Register<IProductsViewModel>(typeof(DCategoriesFragment));

                    viewLookupService.Register<IContentSearchInternalViewModel>(typeof(DCategoriesFragment));
                    break;
                case CategoriesType.Mixed:
                    viewLookupService.Register<IProductsViewModel>(typeof(SSCategoriesFragment));
                    viewLookupService.Register<ICategoriesViewModel>(typeof(CCategoriesPagerFragment));
                    viewLookupService.Register<ICatalogViewModel, CCategoriesCatalogFragment>();

                    viewLookupService.Register<IContentSearchInternalViewModel>(typeof(CCategoriesCatalogFragment));
                    break;
            }

            if (config.ProductCardNavigationType == Base.Core.Models.Navigation.NavigationType.PresentModal)
                viewLookupService.Register<IProductCardViewModel>(typeof(ModalProductCardFragment));
            else
                viewLookupService.Register<IProductCardViewModel, ProductCardFragment>();

            viewLookupService.Register<CatalogSortFiltersHeaderVM, CatalogSortFiltersHeaderView>();

            viewLookupService.Register<IContentSearchViewModel>(typeof(ContentSearchFragment));
            viewLookupService.Register<IProductTextContentViewModel, ProductTextContentFragment>();
            viewLookupService.Register<IProductWebContentViewModel, ProductWebContentFragment>();
            viewLookupService.Register<IProductDetailsSelectionViewModel, SelectionFragment>();
        }
    }
}
