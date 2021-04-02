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
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Services.Implementation;
using AppRopio.ECommerce.Products.iOS.Views.Catalog;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Header;
using AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged;
using AppRopio.ECommerce.Products.iOS.Views.Categories.Disabled;
using AppRopio.ECommerce.Products.iOS.Views.Categories.StepByStep;
using AppRopio.ECommerce.Products.iOS.Views.ContentSearch;
using AppRopio.ECommerce.Products.iOS.Views.ModalProductCard;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Selection;
using AppRopio.ECommerce.Products.iOS.Views.ProductTextContent;
using AppRopio.ECommerce.Products.iOS.Views.ProductWebContent;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Products.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IProductsThemeConfigService>(() => new ProductsThemeConfigService());

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            var config = Mvx.IoCProvider.Resolve<IProductConfigService>().Config;
            switch (config.CategoriesType)
            {
                case CategoriesType.StepByStep:
                    viewLookupService.Register<IProductsViewModel>(typeof(SSCategoriesViewController));
                    viewLookupService.Register<ICatalogViewModel, CatalogViewController>();

                    viewLookupService.Register<IContentSearchInternalViewModel>(typeof(CatalogViewController));
                    break;
                case CategoriesType.Cataloged:
                    viewLookupService.Register<IProductsViewModel>(typeof(CCategoriesPageViCoWrapper));
                    viewLookupService.Register<ICatalogViewModel, CCategoriesPagedViewController>();

                    viewLookupService.Register<IContentSearchInternalViewModel>(typeof(CCategoriesPagedViewController));
                    break;
                case CategoriesType.Disabled:
                    viewLookupService.Register<IProductsViewModel>(typeof(DCategoriesViewController));

                    viewLookupService.Register<IContentSearchInternalViewModel>(typeof(DCategoriesViewController));
                    break;
                case CategoriesType.Mixed:
                    viewLookupService.Register<IProductsViewModel>(typeof(SSCategoriesViewController));
                    viewLookupService.Register<ICategoriesViewModel>(typeof(CCategoriesPageViCoWrapper));
                    viewLookupService.Register<ICatalogViewModel, CCategoriesPagedViewController>();

                    viewLookupService.Register<IContentSearchInternalViewModel>(typeof(CCategoriesPagedViewController));
                    break;
            }

            if (config.ProductCardNavigationType == Base.Core.Models.Navigation.NavigationType.PresentModal)
                viewLookupService.Register<IProductCardViewModel>(typeof(ModalProductCardViewController));
            else
                viewLookupService.Register<IProductCardViewModel, ProductCardViewController>();

            viewLookupService.Register<CatalogSortFiltersHeaderVM, CatalogSortFiltersHeader>();

            viewLookupService.Register<IContentSearchViewModel>(typeof(ContentSearchViewController));
            viewLookupService.Register<IProductTextContentViewModel, ProductTextContentViewController>();
            viewLookupService.Register<IProductWebContentViewModel, ProductWebContentViewController>();
            viewLookupService.Register<IProductDetailsSelectionViewModel, PDSelectionViewController>();
        }
    }
}

