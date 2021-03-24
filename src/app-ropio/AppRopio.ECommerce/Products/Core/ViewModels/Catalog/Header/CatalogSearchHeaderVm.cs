using System;
using MvvmCross.ViewModels;
using AppRopio.Base.Core.ViewModels.Search;
namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Header
{
    public class CatalogSearchHeaderVM : SearchViewModel
    {
        public Action CancelExecute { get; set; }

        public Action SearchExecute { get; set; }

        public Action<string> SearchTextChanged { get; set; }

        protected override void CancelSearchExecute()
        {
            CancelExecute?.Invoke();
        }

        protected override void OnSearchTextChanged(string searchText)
        {
            SearchTextChanged?.Invoke(searchText);
        }

        protected override void SearchCommandExecute()
        {
            SearchExecute?.Invoke();
        }
    }
}
