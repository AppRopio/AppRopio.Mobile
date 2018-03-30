using System;
namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Hint
{
    public class HintItemVM : IHintItemVM
    {
        public string SearchText { get; }

        public HintItemVM(string searchText)
        {
            SearchText = searchText;
        }
    }
}
