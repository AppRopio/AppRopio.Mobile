using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Autocomplete;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Hint;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.History;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Services
{
    public interface IContentSearchVmService
    {
        Task<ObservableCollection<IHistorySearchItemVM>> LoadSearchHistory();

        Task SaveSearchRequestInHistory(string searchText);

        Task ClearHistory();

        Task<ObservableCollection<IHintItemVM>> LoadHints(string searchText, CancellationToken token);

        Task<ObservableCollection<IAutocompleteItemVM>> LoadAutocompletes(string searchText, CancellationToken token);
    }
}
