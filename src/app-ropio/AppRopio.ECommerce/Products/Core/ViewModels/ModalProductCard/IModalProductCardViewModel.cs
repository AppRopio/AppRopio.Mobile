using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard;
using MvvmCross.Commands;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ModalProductCard
{
    public interface IModalProductCardViewModel : IProductCardViewModel
    {
        IMvxCommand CloseCommand { get; }
    }
}
