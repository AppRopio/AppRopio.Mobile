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
                Mvx.RegisterType<IBannersService>(() => new BannersFakeService());
                Mvx.RegisterType<ICategoriesService>(() => new CategoriesFakeService());
                Mvx.RegisterType<IProductService>(() => new ProductFakeService());
                Mvx.RegisterType<IContentSearchService>(() => new ContentSearchFakeService());
            }
            else
            {
                Mvx.RegisterType<IBannersService>(() => new BannersService());
                Mvx.RegisterType<ICategoriesService>(() => new CategoriesService());
                Mvx.RegisterType<IProductService>(() => new ProductService());
                Mvx.RegisterType<IContentSearchService>(() => new ContentSearchService());
            }
        }
    }
}

