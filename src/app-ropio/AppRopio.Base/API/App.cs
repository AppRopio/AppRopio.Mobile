using AppRopio.Base.API.Services;
using AppRopio.Base.API.Services.Implementations;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Base.API
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<IErrorService, ErrorService>();
            Mvx.IoCProvider.RegisterType<IPushService, PushService>();
        }
    }
}
