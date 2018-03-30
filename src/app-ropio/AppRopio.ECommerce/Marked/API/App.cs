using AppRopio.Base.API;
using AppRopio.ECommerce.Marked.API.Services;
using AppRopio.ECommerce.Marked.API.Services.Implementation;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Marked.API
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.RegisterType<IMarkedService>(() => new Services.Fakes.FakeMarkedSerivce());
            else
                Mvx.RegisterType<IMarkedService>(() => new MarkedService());
        }
    }
}
