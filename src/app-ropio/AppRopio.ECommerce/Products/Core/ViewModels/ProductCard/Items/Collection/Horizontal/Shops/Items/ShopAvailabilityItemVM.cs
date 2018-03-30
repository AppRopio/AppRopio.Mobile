using System;
using MvvmCross.Core.ViewModels;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops.Items
{
    public class ShopAvailabilityItemVM : MvxViewModel, IShopAvailabilityItemVM
    {
        protected Shop Model { get; }

        public bool IsProductAvailable => Model.IsProductAvailable;

        public string Name => Model.Name;

        public string Address => Model.Address;

        public string Count => Model.Count.HasValue && Model.Count.Value > 0 ? $"{Model.Count.Value} {Model.Count.Value.StringPostfixCase("ТОВАР", "ТОВАРА", "ТОВАРОВ")}" : (DataType == ProductDataType.ShopsAvailability_Count ? "ОТСУТСТВУЕТ" : null);

        public ProductDataType DataType { get; }

        public ShopAvailabilityItemVM(ProductDataType dataType, Shop model)
        {
            DataType = dataType;
            Model = model;
        }
    }
}
