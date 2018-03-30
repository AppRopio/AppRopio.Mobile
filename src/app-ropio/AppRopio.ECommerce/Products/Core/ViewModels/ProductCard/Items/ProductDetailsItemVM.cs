using System;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items
{
    public abstract class ProductDetailsItemVM : BaseViewModel, IProductDetailsItemVM
    {
        protected string Id { get; }

        public string Name { get; }

        public ProductWidgetType WidgetType { get; }

        public ProductDataType DataType { get; }

        public string CustomType { get; }

        public string Content { get; }

        protected ApplyedProductParameter _selectedValue;
        public virtual ApplyedProductParameter SelectedValue { get => _selectedValue; protected set => SetProperty(ref _selectedValue, value, nameof(SelectedValue)); }

        protected ProductDetailsItemVM(ProductParameter parameter)
        {
            Id = parameter.Id;
            Name = parameter.Name;
            WidgetType = parameter.WidgetType;
            DataType = parameter.DataType;
            CustomType = parameter.CustomType;
            Content = parameter.Content;

            TrackInAnalytics = false;
        }

        public abstract void ClearSelectedValue();
    }
}
