using System;
using MvvmCross.ViewModels;
using AppRopio.Models.Basket.Responses.Order;
namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items
{
    public class OrderFieldAutocompleteItemVM : MvxViewModel, IOrderFieldAutocompleteItemVM
    {
        public string Value { get; private set; }

        public OrderFieldAutocompleteItemVM(OrderFieldAutocompleteValue model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            
            Value = model.Value;
        }
    }
}
