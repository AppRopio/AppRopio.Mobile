using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.API.Services.Fakes
{
    public class BannersFakeService : IBannersService
    {
        public async Task<List<Banner>> LoadBanners(string categoryId = null, BannerPosition position = BannerPosition.Bottom | BannerPosition.Top)
        {
            await Task.Delay(500);

            return string.IsNullOrEmpty(categoryId) ?
                        new List<Banner>
                        {
                            new Banner { Id = "1", ImageUrl = "https://image.shutterstock.com/z/stock-photo-retro-toned-poppy-flowers-at-sunrise-shallow-depth-of-field-ratio-304321280.jpg", Position = BannerPosition.Top, Deeplink = "category://categoryId=3&navigationType=2" },
                            new Banner { Id = "1", ImageUrl = "https://image.shutterstock.com/z/stock-photo-vintage-toned-poppy-flowers-at-sunrise-shallow-depth-of-field-ratio-304321328.jpg", Position = BannerPosition.Top, Deeplink = "products://categoryId=30&navigationType=1" },
                            new Banner { Id = "1", ImageUrl = "https://image.shutterstock.com/z/stock-photo-white-ornithogalum-grass-lily-flowers-with-blue-leaves-aspect-ratio-260556620.jpg", Position = BannerPosition.Bottom, Deeplink = "product://productId=1&navigationType=1" }
                        } :
                        new List<Banner>();
        }
    }
}
