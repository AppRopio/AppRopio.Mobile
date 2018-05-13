using System;
using MvvmCross.Core.ViewModels;
using AppRopio.Models.Products.Responses;
using AppRopio.Base.Core.Services.Localization;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops.Items
{
    public class ShopAvailabilityItemVM : MvxViewModel, IShopAvailabilityItemVM
    {
        protected Shop Model { get; }

        public bool IsProductAvailable => Model.IsProductAvailable;

        public string Name => Model.Name;

        public string Address => Model.Address;

        public string Count => Model.Count.HasValue && Model.Count.Value > 0 ? 
                                    $"{Model.Count.Value} {Model.Count.Value.StringPostfixCase(LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "OneProduct"), LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "TwoProducts"), LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "FiveProducts"))}" 
                                        : 
                                    (DataType == ProductDataType.ShopsAvailability_Count ? LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "OutOfStock") : null);

        public ProductDataType DataType { get; }

        public ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();

        public ShopAvailabilityItemVM(ProductDataType dataType, Shop model)
        {
            DataType = dataType;
            Model = model;
        }
    }
}
