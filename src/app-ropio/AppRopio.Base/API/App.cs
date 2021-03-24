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
            Mvx.RegisterType<IErrorService, ErrorService>();
            Mvx.RegisterType<IPushService, PushService>();
        }
    }
}
