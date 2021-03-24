using System;
using MvvmCross.ViewModels;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker.Items
{
    public class PickerCollectionItemVM : MvxViewModel
    {
        public string Id { get; private set; }

        public string Value { get; protected set; }

        public string ValueName { get; protected set; }

        public PickerCollectionItemVM(ProductParameterValue value)
        {
            Id = value.Id;

            Value = value.Value;
            ValueName = value.ValueName;
        }

        public override string ToString()
        {
            return ValueName;
        }
    }
}
