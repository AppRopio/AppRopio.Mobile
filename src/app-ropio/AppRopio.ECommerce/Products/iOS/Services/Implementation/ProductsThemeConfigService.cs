using AppRopio.Base.iOS.Services.ThemeConfig;
using AppRopio.ECommerce.Products.Core;
using AppRopio.ECommerce.Products.iOS.Models;

namespace AppRopio.ECommerce.Products.iOS.Services.Implementation
{
    public class ProductsThemeConfigService : BaseThemeConfigService<ProductsThemeConfig>, IProductsThemeConfigService
    {
        protected override string ConfigName
        {
            get
            {
                return ProductsConstants.CONFIG_NAME;
            }
        }
    }
}
