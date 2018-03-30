using System;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Feedback.Core.Models.Bundle;
using AppRopio.Feedback.Core.ViewModels.ReviewDetails;
using AppRopio.Feedback.Core.ViewModels.ReviewPost;

namespace AppRopio.Feedback.Core.Services.Implementation
{
    public class FeedbackNavigationVmService : BaseVmNavigationService, IFeedbackNavigationVmService
    {
        public void NavigateToReviewDetails(ReviewBundle bundle)
		{
			NavigateTo<IReviewDetailsViewModel>(bundle);
		}

        public void NavigateToReviewApplication(ReviewBundle bundle)
        {
            NavigateTo<IReviewPostViewModel>(bundle);
        }
    }
}