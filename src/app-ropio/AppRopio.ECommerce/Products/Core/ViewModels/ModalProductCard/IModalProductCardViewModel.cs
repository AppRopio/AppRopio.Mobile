using System;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ModalProductCard
{
    public interface IModalProductCardViewModel : IProductCardViewModel
    {
        IMvxCommand CloseCommand { get; }
    }
}
