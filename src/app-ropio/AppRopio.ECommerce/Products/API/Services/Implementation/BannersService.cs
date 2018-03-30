using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Products;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.API.Services.Implementation
{
    public class BannersService : BaseService, IBannersService
    {
        protected string BANNERS_URL = "banners";

        public async Task<List<Banner>> LoadBanners(string categoryId = null, BannerPosition position = BannerPosition.Bottom | BannerPosition.Top)
        {
            return await Post<List<Banner>>(BANNERS_URL, ToStringContent(new { id = categoryId, position }));
        }
    }
}
