using System.Collections.ObjectModel;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Autocomplete;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Hint;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.History;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch
{
    public interface IContentSearchViewModel : ISearchViewModel
    {
        IContentSearchInternalViewModel ContentVm { get; }

        ICommand HistorySelectionCommand { get; }

        ICommand ClearHistoryCommand { get; }

        ICommand HintSelectionCommand { get; }

        ICommand AutocompleteSelectionCommand { get; }

        bool ContentVisible { get; }

        bool HistoryVisible { get; }

        ObservableCollection<IHistorySearchItemVM> HistoryItems { get; }

        bool HintsVisible { get; }

        ObservableCollection<IHintItemVM> HintsItems { get; }

        ObservableCollection<IAutocompleteItemVM> AutocomleteItems { get; }
    }
}
