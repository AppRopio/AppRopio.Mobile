using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Feedback.Responses;

namespace AppRopio.Feedback.API.Services
{
    public interface IReviewsService
    {
        Task<List<Review>> GetMyReviews(int count, int offset);

        Task<List<Review>> GetReviews(string productGroupId, string productId, int count, int offset);

        Task<ReviewDetails> GetReviewDetails(string reviewId);

        Task<List<ReviewParameter>> GetReviewApplication(string reviewId, string productGroupId, string productId);

        Task PostReview(string reviewId, string groupId, string productId, List<ReviewParameter> parameters);

        Task DeleteReview(string reviewId);
    }
}