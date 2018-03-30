using System;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Autocomplete
{
    public class AutocompleteItemVM : IAutocompleteItemVM
    {
        public string AutocompleteText { get; }

        public string ResultSearchText { get; }

        public AutocompleteItemVM(AppRopio.Models.Products.Responses.Autocomplete autocomplete)
        {
            AutocompleteText = autocomplete.Hint;
            ResultSearchText = autocomplete.Result;
        }
    }
}
