using AppRopio.Base.API;
using AppRopio.ECommerce.Marked.API.Services;
using AppRopio.ECommerce.Marked.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.ECommerce.Marked.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.IoCProvider.RegisterType<IMarkedService>(() => new Services.Fakes.FakeMarkedSerivce());
            else
                Mvx.IoCProvider.RegisterType<IMarkedService>(() => new MarkedService());
        }
    }
}
