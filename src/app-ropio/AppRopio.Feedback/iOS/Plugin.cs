using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Feedback.Core;
using AppRopio.Feedback.Core.ViewModels.MyReviews;
using AppRopio.Feedback.Core.ViewModels.ReviewDetails;
using AppRopio.Feedback.Core.ViewModels.ReviewPost;
using AppRopio.Feedback.Core.ViewModels.Reviews;
using AppRopio.Feedback.iOS.Services;
using AppRopio.Feedback.iOS.Services.Implementation;
using AppRopio.Feedback.iOS.Views.MyReviews;
using AppRopio.Feedback.iOS.Views.ReviewDetails;
using AppRopio.Feedback.iOS.Views.ReviewPost;
using AppRopio.Feedback.iOS.Views.Reviews;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Feedback.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IFeedbackThemeConfigService>(() => new FeedbackThemeConfigService());

			var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<IMyReviewsViewModel, MyReviewsViewController>();
            viewLookupService.Register<IReviewsViewModel, ReviewsViewController>();
            viewLookupService.Register<IReviewDetailsViewModel, ReviewDetailsViewController>();
            viewLookupService.Register<IScoreViewModel, ScoreView>();
            viewLookupService.Register<IReviewPostViewModel, ReviewPostViewController>();
		}
	}
}