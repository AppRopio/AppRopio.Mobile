using AppRopio.Base.API;
using AppRopio.Feedback.API.Services;
using AppRopio.Feedback.API.Services.Fakes;
using AppRopio.Feedback.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.Feedback.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.IoCProvider.RegisterType<IReviewsService>(() => new ReviewsFakeService());
            else
                Mvx.IoCProvider.RegisterType<IReviewsService>(() => new ReviewsService());
        }
    }
}