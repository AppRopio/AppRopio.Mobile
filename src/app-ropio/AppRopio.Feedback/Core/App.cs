using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Feedback.Core.Services;
using AppRopio.Feedback.Core.Services.Implementation;
using AppRopio.Feedback.Core.ViewModels.Items.Score;
using AppRopio.Feedback.Core.ViewModels.Items.Text;
using AppRopio.Feedback.Core.ViewModels.Items.TotalScore;
using AppRopio.Feedback.Core.ViewModels.MyReviews;
using AppRopio.Feedback.Core.ViewModels.MyReviews.Services;
using AppRopio.Feedback.Core.ViewModels.ReviewDetails;
using AppRopio.Feedback.Core.ViewModels.ReviewDetails.Services;
using AppRopio.Feedback.Core.ViewModels.ReviewPost;
using AppRopio.Feedback.Core.ViewModels.ReviewPost.Services;
using AppRopio.Feedback.Core.ViewModels.Reviews;
using AppRopio.Feedback.Core.ViewModels.Reviews.Services;
using MvvmCross;
using MvvmCross.ViewModels;

namespace AppRopio.Feedback.Core
{
    public class App : MvxApplication
	{
		public override void Initialize()
		{
            new API.App().Initialize();

            Mvx.IoCProvider.RegisterSingleton<IMyReviewsVmService>(() => new MyReviewsVmService());
            Mvx.IoCProvider.RegisterSingleton<IReviewsVmService>(() => new ReviewsVmService());
            Mvx.IoCProvider.RegisterSingleton<IReviewDetailsVmService>(() => new ReviewDetailsVmService());
            Mvx.IoCProvider.RegisterSingleton<IReviewPostVmService>(() => new ReviewPostVmService());
            Mvx.IoCProvider.RegisterSingleton<IFeedbackNavigationVmService>(() => new FeedbackNavigationVmService());

			#region VMs registration

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
            vmLookupService.Register<IMyReviewsViewModel, MyReviewsViewModel>();
            vmLookupService.Register<IReviewsViewModel, ReviewsViewModel>();
            vmLookupService.Register<IReviewDetailsViewModel, ReviewDetailsViewModel>();
            vmLookupService.Register<IScoreViewModel, ScoreViewModel>();
            vmLookupService.Register<IReviewPostViewModel, ReviewPostViewModel>();

            vmLookupService.Register<ITotalScoreItemVm, TotalScoreItemVm>();
            vmLookupService.Register<IScoreItemVm, ScoreItemVm>();
            vmLookupService.Register<ITextItemVm, TextItemVm>();
			#endregion

			//register start point for current navigation module
			var routerService = Mvx.IoCProvider.Resolve<IRouterService>();
            routerService.Register<IMyReviewsViewModel>(new FeedbackRouterSubscriber());
		}
	}
}