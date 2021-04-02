using AppRopio.Base.Core.Models.Navigation;
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
using AppRopio.ECommerce.Products.Core.ViewModels.ModalProductCard;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductTextContent;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductWebContent;
using MvvmCross;
using MvvmCross.IoC;

namespace AppRopio.ECommerce.Products.Core
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            new API.App().Initialize();

            Mvx.IoCProvider.RegisterSingleton<IProductConfigService>(() => new ProductConfigService());

            Mvx.IoCProvider.RegisterType<IProductsNavigationVmService>(() => new ProductsNavigationVmService());

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductsVmService, ProductsVmService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ICatalogVmService, CatalogVmService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IBannersVmService, BannersVmService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ICategoriesVmService, CategoriesVmService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMarkProductVmService, MarkProductVmService>();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductCardVmService, ProductCardVmService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductDetailsSelectionVmService, ProductDetailsSelectionVmService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IProductsShareVmService, ProductsShareVmService>();
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IMarkProductVmService, MarkProductVmService>();

            Mvx.IoCProvider.RegisterType<IHistorySearchDbService>(() => new HistorySearchDbService());
            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<IContentSearchVmService, ContentSearchVmService>();

            #region VMs registration

            var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();

            var config = Mvx.IoCProvider.Resolve<IProductConfigService>().Config;
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

            var routerService = Mvx.IoCProvider.Resolve<IRouterService>();

            routerService.Register<IProductsViewModel>(new ProductRouterSubscriber());

            #endregion
        }
    }
}

