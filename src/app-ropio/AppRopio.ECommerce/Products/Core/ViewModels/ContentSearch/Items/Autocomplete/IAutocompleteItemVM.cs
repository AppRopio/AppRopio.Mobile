using System;
namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Autocomplete
{
    public interface IAutocompleteItemVM
    {
        string AutocompleteText { get; }

        string ResultSearchText { get; }
    }
}
