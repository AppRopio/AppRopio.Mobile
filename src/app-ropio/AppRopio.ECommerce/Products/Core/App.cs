using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.Services.Implementation;
using AppRopio.ECommerce.Products.Core.ViewModels;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Cataloged;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Disabled;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services.Banners;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Services.DataBase;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductTextContent;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductWebContent;
using MvvmCross.Platform;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.ECommerce.Products.Core.ViewModels.ModalProductCard;

namespace AppRopio.ECommerce.Products.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterSingleton<IProductConfigService>(() => new ProductConfigService());

            Mvx.RegisterType<IProductsNavigationVmService>(() => new ProductsNavigationVmService());

            Mvx.LazyConstructAndRegisterSingleton<IProductsVmService, ProductsVmService>();
            Mvx.LazyConstructAndRegisterSingleton<ICatalogVmService, CatalogVmService>();
            Mvx.LazyConstructAndRegisterSingleton<IBannersVmService, BannersVmService>();
            Mvx.LazyConstructAndRegisterSingleton<ICategoriesVmService, CategoriesVmService>();
            Mvx.LazyConstructAndRegisterSingleton<IMarkProductVmService, MarkProductVmService>();

            Mvx.LazyConstructAndRegisterSingleton<IProductCardVmService, ProductCardVmService>();
            Mvx.LazyConstructAndRegisterSingleton<IProductDetailsSelectionVmService, ProductDetailsSelectionVmService>();
            Mvx.LazyConstructAndRegisterSingleton<IProductsShareVmService, ProductsShareVmService>();
            Mvx.LazyConstructAndRegisterSingleton<IMarkProductVmService, MarkProductVmService>();

            Mvx.RegisterType<IHistorySearchDbService>(() => new HistorySearchDbService());
            Mvx.LazyConstructAndRegisterSingleton<IContentSearchVmService, ContentSearchVmService>();

            #region VMs registration

            var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

            var config = Mvx.Resolve<IProductConfigService>().Config;
            switch (config.CategoriesType)
            {
                case Models.CategoriesType.StepByStep:
                    vmLookupService.Register<IProductsViewModel>(typeof(SSCategoriesViewModel));
                    vmLookupService.Register<ICategoriesViewModel>(typeof(SSCategoriesViewModel));
                    vmLookupService.Register<ICatalogViewModel>(typeof(CatalogViewModel));

                    vmLookupService.Register<IContentSearchInternalViewModel>(typeof(CatalogViewModel));
                    break;
                case Models.CategoriesType.Cataloged:
                    vmLookupService.Register<IProductsViewModel>(typeof(CCategoriesViewModel));
                    vmLookupService.Register<ICategoriesViewModel>(typeof(CCategoriesViewModel));
                    vmLookupService.Register<ICatalogViewModel>(typeof(CCategoriesPageViewModel));

                    vmLookupService.Register<IContentSearchInternalViewModel>(typeof(CCategoriesPageViewModel));
                    break;
                case Models.CategoriesType.Disabled:
                    vmLookupService.Register<IProductsViewModel>(typeof(DCategoriesViewModel));

                    vmLookupService.Register<IContentSearchInternalViewModel>(typeof(DCategoriesViewModel));
                    break;
                case Models.CategoriesType.Mixed:
                    vmLookupService.Register<IProductsViewModel>(typeof(SSCategoriesViewModel)); 
                    vmLookupService.Register<ISSCategoriesViewModel>(typeof(SSCategoriesViewModel));
                    vmLookupService.Register<ICategoriesViewModel>(typeof(CCategoriesViewModel));
                    vmLookupService.ReplaceDeeplink(typeof(CCategoriesViewModel));
                    vmLookupService.Register<ICatalogViewModel>(typeof(CCategoriesPageViewModel));

                    vmLookupService.Register<IContentSearchInternalViewModel>(typeof(CCategoriesPageViewModel));
                    break;
            }

            if (config.ProductCardNavigationType == NavigationType.PresentModal)
                vmLookupService.Register<IProductCardViewModel>(typeof(ModalProductCardViewModel));
            else
                vmLookupService.Register<IProductCardViewModel>(typeof(ProductCardViewModel));

            vmLookupService.Register<IContentSearchViewModel, ContentSearchViewModel>();
            vmLookupService.Register<IProductDetailsSelectionViewModel, ProductDetailsSelectionViewModel>();
            vmLookupService.Register<IProductTextContentViewModel, ProductTextContentViewModel>();
            vmLookupService.Register<IProductWebContentViewModel, ProductWebContentViewModel>();

            #endregion

            #region RouterSubscriber registration

            var routerService = Mvx.Resolve<IRouterService>();

            routerService.Register<IProductsViewModel>(new ProductRouterSubscriber());

            #endregion
        }
    }
}

