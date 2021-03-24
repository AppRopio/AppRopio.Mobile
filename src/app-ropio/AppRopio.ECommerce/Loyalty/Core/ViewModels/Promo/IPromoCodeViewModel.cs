using AppRopio.ECommerce.Loyalty.Abstractions;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo
{
    public interface IPromoCodeViewModel : IMvxNotifyPropertyChanged, ILoyaltyViewModel
    {
        string Code { get; }

        IMvxCommand ApplyCommand { get; }
    }
}
