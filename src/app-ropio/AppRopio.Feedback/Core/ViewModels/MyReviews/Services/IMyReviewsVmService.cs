using System.Threading.Tasks;
using AppRopio.Feedback.Core.ViewModels.Reviews.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.MyReviews.Services
{
    public interface IMyReviewsVmService
    {
        Task<MvxObservableCollection<IReviewItemVm>> LoadReviews(int count, int offset = 0);

        void HandleItemSelection(IReviewItemVm item);
    }
}