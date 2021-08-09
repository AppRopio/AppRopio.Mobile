using MvvmCross;
using AppRopio.ECommerce.Products.API.Services;
using AppRopio.ECommerce.Products.API.Services.Implementation;
using AppRopio.Base.API;
using AppRopio.ECommerce.Products.API.Services.Fakes;

namespace AppRopio.ECommerce.Products.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
            {
                Mvx.IoCProvider.RegisterType<IBannersService>(() => new BannersFakeService());
                Mvx.IoCProvider.RegisterType<ICategoriesService>(() => new CategoriesFakeService());
                Mvx.IoCProvider.RegisterType<IProductService>(() => new ProductFakeService());
                Mvx.IoCProvider.RegisterType<IContentSearchService>(() => new ContentSearchFakeService());
            }
            else
            {
                Mvx.IoCProvider.RegisterType<IBannersService>(() => new BannersService());
                Mvx.IoCProvider.RegisterType<ICategoriesService>(() => new CategoriesService());
                Mvx.IoCProvider.RegisterType<IProductService>(() => new ProductService());
                Mvx.IoCProvider.RegisterType<IContentSearchService>(() => new ContentSearchService());
            }
        }
    }
}

