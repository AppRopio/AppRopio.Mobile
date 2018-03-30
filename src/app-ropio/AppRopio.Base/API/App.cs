using AppRopio.Base.API.Services;
using AppRopio.Base.API.Services.Implementations;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

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
