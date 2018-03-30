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
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Feedback.Core.ViewModels.MyReviews.Services
{
    public class MyReviewsVmService : BaseVmService, IMyReviewsVmService
    {
		#region Services

		protected IReviewsService ApiService { get { return Mvx.Resolve<IReviewsService>(); } }

		#endregion

		public async Task<MvxObservableCollection<IReviewItemVm>> LoadReviews(int count, int offset = 0)
		{
			MvxObservableCollection<IReviewItemVm> dataSource = null;

			try
			{
                var reviews = await ApiService.GetMyReviews(count, offset);
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
			var navigationVmService = Mvx.Resolve<IFeedbackNavigationVmService>();
            var bundle = new ReviewBundle(reviewId: item.Id, navigationType: NavigationType.Push);
            navigationVmService.NavigateToReviewDetails(bundle);
        }
	}
}