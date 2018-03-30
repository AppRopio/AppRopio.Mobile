using System;
namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.History
{
    public class HistorySearchItemVM : IHistorySearchItemVM
    {
        public string SearchText { get; }
        
        public HistorySearchItemVM(string searchText)
        {
            SearchText = searchText;
        }
    }
}
