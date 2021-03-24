using AppRopio.Base.Core.ViewModels;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard
{
    public interface IProductCardViewModel : IBaseViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        IMvxCommand MarkCommand { get; }

        IMvxCommand ShareCommand { get; }

        bool Marked { get; }

        bool MarkedLoading { get; }

        bool ShareLoading { get; }

        bool MarkEnabled { get; }

        IMvxViewModel BasketBlockViewModel { get; }

        MvxObservableCollection<IMvxViewModel> Items { get; }
    }
}

