using System;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Items
{
    public class CollectionItemVM : MvxViewModel
    {
        public string Id { get; private set; }

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                RaisePropertyChanged(() => Selected);
            }
        }

        public string ValueName { get; private set; }

        public string Value { get; private set; }

        public ProductDataType DataType { get; private set; }

        public CollectionItemVM(ProductDataType dataType, ProductParameterValue value)
        {
            DataType = dataType;
            Id = value.Id;

            ValueName = !value.ValueName.IsNullOrEmtpy() ? value.ValueName : value.Value;
            Value = value.Value;
        }
    }
}
