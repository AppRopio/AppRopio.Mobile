using AppRopio.Base.Core.ViewModels;
using AppRopio.Feedback.Core.ViewModels.Reviews.Items;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.MyReviews
{
    public interface IMyReviewsViewModel : ILoadMoreViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        MvxObservableCollection<IReviewItemVm> Reviews { get; }
    }
}