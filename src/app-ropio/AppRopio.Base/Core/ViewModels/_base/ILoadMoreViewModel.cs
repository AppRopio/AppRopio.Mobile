using MvvmCross.Commands;

namespace AppRopio.Base.Core.ViewModels {
	public interface ILoadMoreViewModel : IBaseViewModel
    {
        IMvxCommand LoadMoreCommand { get; }

        bool LoadingMore { get; }

        bool CanLoadMore { get; }
    }
}
