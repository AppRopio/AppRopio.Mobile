using System.Threading.Tasks;
using AppRopio.Feedback.Core.ViewModels.Reviews.Items;
using MvvmCross.ViewModels;

namespace AppRopio.Feedback.Core.ViewModels.Reviews.Services
{
    public interface IReviewsVmService
    {
        Task<MvxObservableCollection<IReviewItemVm>> LoadReviews(string productGroupId, string productId, int count, int offset = 0);

        void HandleItemSelection(IReviewItemVm item);

        void NavigateToReviewApplication(string productGroupId, string productId);
    }
}