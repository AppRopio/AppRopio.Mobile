using System;
using MvvmCross.ViewModels;
using AppRopio.Models.Products.Responses;
namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops.Items
{
    public interface IShopAvailabilityItemVM : IMvxViewModel
    {
        ProductDataType DataType { get; }
        
        bool IsProductAvailable { get; }

        string Name { get; }

        string Address { get; }

        string Count { get; }
    }
}
