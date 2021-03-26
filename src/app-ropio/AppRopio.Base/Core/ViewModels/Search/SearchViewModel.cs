using System.Windows.Input;
using MvvmCross.Commands;

namespace AppRopio.Base.Core.ViewModels.Search
{
    public abstract class SearchViewModel : BaseViewModel, ISearchViewModel
    {
        #region Commands

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new MvxCommand(SearchCommandExecute));
            }
        }

        private ICommand _cancelSearchCommand;
        public ICommand CancelSearchCommand
        {
            get
            {
                return _cancelSearchCommand ?? (_cancelSearchCommand = new MvxCommand(CancelSearchExecute));
            }
        }

        #endregion

        #region Properties

        protected string _searchText;
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;

                    RaisePropertyChanged(() => SearchText);

                    OnSearchTextChanged(SearchText);
                }
            }
        }

        #endregion

        #region Protected

        protected abstract void OnSearchTextChanged(string searchText);

        protected abstract void SearchCommandExecute();

        protected abstract void CancelSearchExecute();

        #endregion
    }
}
