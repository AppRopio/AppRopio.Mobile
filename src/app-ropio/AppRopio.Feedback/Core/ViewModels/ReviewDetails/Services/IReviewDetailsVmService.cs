using System;
using System.Threading.Tasks;

namespace AppRopio.Feedback.Core.ViewModels.ReviewDetails.Services
{
    public interface IReviewDetailsVmService
    {
        Task<AppRopio.Models.Feedback.Responses.ReviewDetails> LoadReviewDetails(string reviewId);

        Task<bool> DeleteReview(string reviewId);

        void NavigateToProduct(string productId, string groupId);

        void NavigateToReviewApplication(string reviewId, string productGroupId, string productId);
    }
}