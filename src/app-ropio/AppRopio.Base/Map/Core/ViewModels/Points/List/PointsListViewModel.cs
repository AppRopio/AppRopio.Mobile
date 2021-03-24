using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points.List
{
    public class PointsListViewModel : PointsCollectionVM, IPointsListViewModel
    {
        #region Fields

        protected const int LOADING_COUNT = 10;

        #endregion

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

        private IMvxCommand _mapCommand;
        public IMvxCommand MapCommand => _mapCommand ?? (_mapCommand = new MvxCommand(OnMapExecute));

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

        #region Init

        protected override void InitFromBundle(Base.Core.Models.Bundle.BaseBundle bundle)
        {
            VmNavigationType = bundle.NavigationType == Base.Core.Models.Navigation.NavigationType.None ? Base.Core.Models.Navigation.NavigationType.ClearAndPush : bundle.NavigationType;
        }

        #endregion

        protected virtual void OnMapExecute()
        {
            NavigationVmService.NavigateToMap(new Base.Core.Models.Bundle.BaseBundle(Base.Core.Models.Navigation.NavigationType.Push));
        }

        protected void OnSearchTextChanged(string searchText)
        {

        }

        protected void SearchCommandExecute()
        {
            Task.Run(LoadContent);
        }

        protected void CancelSearchExecute()
        {
            SearchText = null;
            Task.Run(LoadContent);
        }

        protected override async Task LoadContent()
        {
            Loading = true;

            Items = await VmService.LoadPoints(SearchText, 0, LOADING_COUNT);

            CanLoadMore = !Items.IsNullOrEmpty() && Items.Count >= LOADING_COUNT;

            LoadMoreCommand.RaiseCanExecuteChanged();

            Loading = false;
        }

        protected override Task ReloadContent()
        {
            return Task.Run(async () =>
            {
                if (Loading || Reloading)
                    return;
                
                Reloading = true;

                var items = await VmService.LoadPoints(SearchText, 0, LOADING_COUNT);
                InvokeOnMainThread(() => Items = items);
                CanLoadMore = !items.IsNullOrEmpty() && items.Count == LOADING_COUNT;

                Reloading = false;
            });
        }

        protected override Task LoadMoreContent()
        {
            return Task.Run(async () =>
            {
                var moreItems = await VmService.LoadPoints(SearchText, Items?.Count ?? 0, LOADING_COUNT);

                InvokeOnMainThread(() => Items.AddRange(moreItems));

                CanLoadMore = !moreItems.IsNullOrEmpty() && moreItems.Count == LOADING_COUNT;

                LoadMoreCommand.RaiseCanExecuteChanged();
            });
        }

        #endregion

        #region Public

        #endregion
    }
}
