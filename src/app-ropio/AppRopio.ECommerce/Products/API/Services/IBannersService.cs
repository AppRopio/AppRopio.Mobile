using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.API.Services
{
    public interface IBannersService
    {
        Task<List<Banner>> LoadBanners(string categoryId = null, BannerPosition position = BannerPosition.Bottom | BannerPosition.Top);
    }
}
