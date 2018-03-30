using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items.Banners;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services.Banners
{
    public interface IBannersVmService
    {
        Task<ObservableCollection<IBannerItemVM>> LoadBannersFor(string categoryId = null);

        Task<ObservableCollection<IBannerItemVM>> LoadTopBannersFor(string categoryId = null);

        Task<ObservableCollection<IBannerItemVM>> LoadBottomBannersFor(string categoryId = null);
    }
}
