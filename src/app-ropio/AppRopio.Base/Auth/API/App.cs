using AppRopio.Base.API;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Auth.API.Services.Implementation;
using AppRopio.Base.Auth.API.Services.Implementation.Fake;
using MvvmCross;

namespace AppRopio.Base.Auth.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
            if (ApiSettings.DebugServiceEnabled)
			{
                Mvx.IoCProvider.RegisterType<IAuthService>(() => new AuthServiceFake());
                Mvx.IoCProvider.RegisterType<IUserService>(() => new UserServiceFake());
            }
            else
            {
                Mvx.IoCProvider.RegisterType<IAuthService>(() => new AuthService());
                Mvx.IoCProvider.RegisterType<IUserService>(() => new UserService());
            }
		}
	}
}
