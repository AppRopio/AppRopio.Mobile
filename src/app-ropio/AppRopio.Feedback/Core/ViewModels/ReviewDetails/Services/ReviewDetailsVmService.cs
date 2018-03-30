using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.Feedback.API.Services;
using AppRopio.Feedback.Core.Models.Bundle;
using AppRopio.Feedback.Core.Services;
using AppRopio.Feedback.Core.ViewModels.ReviewPost;
using MvvmCross.Platform;

namespace AppRopio.Feedback.Core.ViewModels.ReviewDetails.Services
{
    public class ReviewDetailsVmService : BaseVmService, IReviewDetailsVmService
    {
		#region Services

		protected IReviewsService ApiService { get { return Mvx.Resolve<IReviewsService>(); } }

		#endregion

		public async Task<AppRopio.Models.Feedback.Responses.ReviewDetails> LoadReviewDetails(string reviewId)
        {
			AppRopio.Models.Feedback.Responses.ReviewDetails details = null;

			try
			{
                details = await ApiService.GetReviewDetails(reviewId);
			}
			catch (ConnectionException ex)
			{
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
				OnException(ex);
			}

            return details;
        }

		public async Task<bool> DeleteReview(string reviewId)
		{
			try
			{
                await ApiService.DeleteReview(reviewId);

				return true;
			}
			catch (ConnectionException ex)
			{
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
				OnException(ex);
			}

			return false;
		}

		public void NavigateToProduct(string productId, string groupId)
		{
			var productsNavigationVmService = Mvx.Resolve<IProductsNavigationVmService>();
            var productBundle = new ProductCardBundle(groupId, productId, NavigationType.Push);
			productsNavigationVmService.NavigateToProduct(productBundle);
		}

        public void NavigateToReviewApplication(string reviewId, string productGroupId, string productId)
		{
            var navigationVmService = Mvx.Resolve<IFeedbackNavigationVmService>();
            var bundle = new ReviewBundle(reviewId: reviewId, productGroupId: productGroupId, productId: productId, navigationType: NavigationType.Push);
            navigationVmService.NavigateToReviewApplication(bundle);
		}
    }
}