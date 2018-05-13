using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Feedback.Core.ViewModels.MyReviews;
using AppRopio.Feedback.Core.ViewModels.ReviewDetails;
using AppRopio.Feedback.Core.ViewModels.ReviewPost;
using AppRopio.Feedback.Core.ViewModels.Reviews;
using AppRopio.Feedback.Droid.Views.MyReviews;
using AppRopio.Feedback.Droid.Views.ReviewDetails;
using AppRopio.Feedback.Droid.Views.ReviewPost;
using AppRopio.Feedback.Droid.Views.Reviews;
using AppRopio.Feedback.Droid.Views.Score;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Feedback.Droid
{
    public class Plugin : IMvxPlugin
	{
		public void Load()
		{
			var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<IMyReviewsViewModel, MyReviewsFragment>();
            viewLookupService.Register<IReviewsViewModel, ReviewsFragment>();
            viewLookupService.Register<IReviewDetailsViewModel, ReviewDetailsFragment>();
            viewLookupService.Register<IScoreViewModel, ScoreView>();
            viewLookupService.Register<IReviewPostViewModel, ReviewPostFragment>();
		}
	}
}