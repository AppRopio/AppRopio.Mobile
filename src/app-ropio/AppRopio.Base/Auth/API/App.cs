using AppRopio.Base.API;
using AppRopio.Base.Auth.API.Services;
using AppRopio.Base.Auth.API.Services.Implementation;
using AppRopio.Base.Auth.API.Services.Implementation.Fake;
using MvvmCross.Platform;

namespace AppRopio.Base.Auth.API
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
            if (ApiSettings.DebugServiceEnabled)
			{
                Mvx.RegisterType<IAuthService>(() => new AuthServiceFake());
                Mvx.RegisterType<IUserService>(() => new UserServiceFake());
            }
            else
            {
                Mvx.RegisterType<IAuthService>(() => new AuthService());
                Mvx.RegisterType<IUserService>(() => new UserService());
            }
		}
	}
}
