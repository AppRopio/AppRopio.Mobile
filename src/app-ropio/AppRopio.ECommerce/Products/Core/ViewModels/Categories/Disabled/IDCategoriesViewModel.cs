using System.Collections.ObjectModel;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items.Banners;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Disabled
{
    public interface IDCategoriesViewModel : ICatalogViewModel
    {
        IMvxCommand BannerSelectionChangedCommand { get; }

        ObservableCollection<IBannerItemVM> TopBanners { get; }

        ObservableCollection<IBannerItemVM> BottomBanners { get;  }
    }
}
