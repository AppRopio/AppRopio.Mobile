using System.Threading.Tasks;
using MvvmCross.Commands;

namespace AppRopio.Base.Core.ViewModels
{
	public abstract class LoadMoreViewModel : BaseViewModel, ILoadMoreViewModel
    {
        #region Fields

        #endregion

        #region Commands

        private IMvxCommand _loadMoreCommand;
        public IMvxCommand LoadMoreCommand => _loadMoreCommand ?? (_loadMoreCommand = new MvxCommand(OnMoreExecute, () => CanLoadMore));

        #endregion

        #region Properties

        private bool _loadingMore;
        public bool LoadingMore
        {
            get
            {
                return _loadingMore;
            }
            set
            {
                _loadingMore = value;
                InvokeOnMainThread(() => RaisePropertyChanged(() => LoadingMore));
            }
        }

        private bool _canLoadMore = true;
        public bool CanLoadMore
        {
            get
            {
                return _canLoadMore;
            }
            set
            {
                _canLoadMore = value;
                InvokeOnMainThread(() => RaisePropertyChanged(() => CanLoadMore));
            }
        }

        #endregion

        #region Protected

        protected virtual async void OnMoreExecute()
        {
            if (LoadingMore)
                return;

            LoadingMore = true;

            await LoadMoreContent();

            LoadingMore = false;
        }

        #endregion

        #region Protected

        protected abstract Task LoadMoreContent();

        #endregion
    }
}
