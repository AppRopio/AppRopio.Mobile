using System;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Multiline
{
    public class MultilinePciVm : ProductDetailsItemVM, IMultilinePciVm
    {
        private string _text;
        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value, nameof(Text));
        }

        public MultilinePciVm(ProductParameter parameter)
            : base(parameter)
        {
            Text = parameter.Content;
        }

        public override void ClearSelectedValue()
        {
            Text = string.Empty;
        }
    }
}
