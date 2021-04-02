using System;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Feedback.API.Services;
using AppRopio.Feedback.Core.Models.Bundle;
using AppRopio.Feedback.Core.Services;
using AppRopio.Feedback.Core.ViewModels.Reviews.Items;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Feedback.Core.ViewModels.Reviews.Services
{
    public class ReviewsVmService : BaseVmService, IReviewsVmService
    {
		#region Services

		protected IReviewsService ApiService { get { return Mvx.IoCProvider.Resolve<IReviewsService>(); } }

		#endregion

        public async Task<MvxObservableCollection<IReviewItemVm>> LoadReviews(string productGroupId, string productId, int count, int offset = 0)
        {
			MvxObservableCollection<IReviewItemVm> dataSource = null;

			try
			{
                var reviews = await ApiService.GetReviews(productGroupId, productId, count, offset);

                dataSource = new MvxObservableCollection<IReviewItemVm>(reviews.Select(r => new ReviewItemVm(r)).ToList());
			}
			catch (ConnectionException ex)
			{
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
				OnException(ex);
			}

			return dataSource;
        }

		public void HandleItemSelection(IReviewItemVm item)
		{
			var navigationVmService = Mvx.IoCProvider.Resolve<IFeedbackNavigationVmService>();
            var bundle = new ReviewBundle(reviewId: item.Id, navigationType: NavigationType.Push);
			navigationVmService.NavigateToReviewDetails(bundle);
		}

		public void NavigateToReviewApplication(string productGroupId, string productId)
		{
			var navigationVmService = Mvx.IoCProvider.Resolve<IFeedbackNavigationVmService>();
            var bundle = new ReviewBundle(reviewId: null, productGroupId: productGroupId, productId: productId, navigationType: NavigationType.Push);
			navigationVmService.NavigateToReviewApplication(bundle);
		}
	}
}