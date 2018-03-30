using AppRopio.Base.API;
using AppRopio.Feedback.API.Services;
using AppRopio.Feedback.API.Services.Fakes;
using AppRopio.Feedback.API.Services.Implementation;
using MvvmCross.Platform;

namespace AppRopio.Feedback.API
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.RegisterType<IReviewsService>(() => new ReviewsFakeService());
            else
                Mvx.RegisterType<IReviewsService>(() => new ReviewsService());
        }
    }
}