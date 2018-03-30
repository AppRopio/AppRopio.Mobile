using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items.Banners;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories
{
    public interface ISSCategoriesViewModel : ICategoriesViewModel
    {
        ICommand BannerSelectionChangedCommand { get; }

        ObservableCollection<IBannerItemVM> TopBanners { get; }

        ObservableCollection<IBannerItemVM> BottomBanners { get; }
    }
}

