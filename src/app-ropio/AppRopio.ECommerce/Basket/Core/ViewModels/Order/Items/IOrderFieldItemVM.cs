using System.Collections.Generic;
using AppRopio.Models.Basket.Responses.Enums;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items
{
    public interface IOrderFieldItemVM : IMvxViewModel
    {
        string Id { get; }

        string Name { get; }

        string Value { get; set; }

        List<string> Values { get; }

        OrderFieldType Type { get; }

        bool Editable { get; }

        bool IsRequired { get; }

        bool IsOptional { get; }

        bool IsValid { get; set; }

        bool Expanded { get; set; }

        IMvxCommand ActionCommand { get; }

        bool HasAutocomplete { get; }

        int AutocompleteStartIndex { get; }

        List<string> DependentFieldsIds { get; }

        IMvxCommand AutocompleteCommand { get; }

        bool InAutocompleteMode { get; set; }
    }
}
