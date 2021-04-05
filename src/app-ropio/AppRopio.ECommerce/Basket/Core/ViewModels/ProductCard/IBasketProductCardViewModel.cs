using AppRopio.Base.Core.ViewModels;
using MvvmCross.Commands;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard
{
    public interface IBasketProductCardViewModel : IBaseViewModel
    {
        IMvxCommand BuyCommand { get; }

        IMvxCommand DecrementCommand { get; }

        IMvxCommand IncrementCommand { get; }

        IMvxCommand QuantityChangedCommand { get; }

        bool BasketLoading { get; }

        bool BasketVisible { get; }

        bool UnitStepVisible { get; }

        string UnitName { get; }

        string QuantityString { get; }
    }
}
