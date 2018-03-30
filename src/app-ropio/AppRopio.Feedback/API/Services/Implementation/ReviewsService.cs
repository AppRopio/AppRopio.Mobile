using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Feedback.Requests;
using AppRopio.Models.Feedback.Responses;

namespace AppRopio.Feedback.API.Services.Implementation
{
    public class ReviewsService : BaseService, IReviewsService
    {
        protected string MY_REVIEWS_URL = "reviews/my";

        protected string REVIEWS_URL = "reviews/product";

        protected string REVIEW_DETAILS_URL = "reviews/details";

        protected string REVIEW_APPLICATION_URL = "reviews/application";

		protected string REVIEW_POST_URL = "reviews/send";

        protected string REVIEW_DELETE_URL = "reviews/delete";

		public async Task<List<Review>> GetMyReviews(int count, int offset)
		{
            return await Get<List<Review>>($"{MY_REVIEWS_URL}?offset={offset}&count={count}");
		}

		public async Task<List<Review>> GetReviews(string productGroupId, string productId, int count, int offset)
		{
            return await Post<List<Review>>(REVIEWS_URL, ToStringContent(new ReviewRequest { ProductId = productId, ProductGroupId = productGroupId, Count = count, Offset = offset }));
		}

        public async Task<ReviewDetails> GetReviewDetails(string reviewId)
		{
            return await Get<ReviewDetails>($"{REVIEW_DETAILS_URL}?reviewId={reviewId}");
		}

        public async Task<List<ReviewParameter>> GetReviewApplication(string reviewId, string productGroupId, string productId)
		{
            return await Post<List<ReviewParameter>>(REVIEW_APPLICATION_URL, ToStringContent(new ApplicationRequest { ReviewId = reviewId, ProductGroupId = productGroupId, ProductId = productId }));
		}

		public Task PostReview(string reviewId, string productGroupId, string productId, List<ReviewParameter> parameters)
		{
            return Post(REVIEW_POST_URL, ToStringContent(new ApplicationResultRequest()
            {
                ReviewId = reviewId,
                ProductId = productId,
                ProductGroupId = productGroupId,
                Parameters = parameters
            }));
		}

		public Task DeleteReview(string reviewId)
		{
            return Get($"{REVIEW_DELETE_URL}?reviewId={reviewId}");
		}
    }
}