using System.Collections.Generic;
using System.Windows.Input;
using AppRopio.Models.Products.Responses;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items
{
    public interface IBasketItemVM : IMvxViewModel
    {
        string GroupId { get; }

        string ProductId { get; }

        string ImageUrl { get; }

        string Name { get; }

        decimal Price { get; }

        decimal? OldPrice { get; }

        bool IsMarked { get; set; }

        ProductState State { get; }

        List<ProductBadge> Badges { get; }

        string UnitName { get; }

        float UnitStep { get; }

        float Quantity { get; }

        string QuantityString { get; }

        bool QuantityLoading { get; }

        ICommand IncCommand { get; }

        ICommand DecCommand { get; }

        ICommand DeleteCommand { get; }
    }
}
