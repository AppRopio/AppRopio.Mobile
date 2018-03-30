using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Feedback.Core.Models.Bundle;

namespace AppRopio.Feedback.Core.Services
{
    public interface IFeedbackNavigationVmService : IBaseVmNavigationService
	{
        void NavigateToReviewDetails(ReviewBundle review);

        void NavigateToReviewApplication(ReviewBundle bundle);
	}
}