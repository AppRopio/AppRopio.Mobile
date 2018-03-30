using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Auth.Core.Services.Implementation;
using AppRopio.Base.Auth.Core.ViewModels.Auth;
using AppRopio.Base.Auth.Core.ViewModels.Auth.Services;
using AppRopio.Base.Auth.Core.ViewModels.Password.New;
using AppRopio.Base.Auth.Core.ViewModels.Password.New.Services;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main.Services;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms.Services;
using AppRopio.Base.Auth.Core.ViewModels.SignIn;
using AppRopio.Base.Auth.Core.ViewModels.SignIn.Services;
using AppRopio.Base.Auth.Core.ViewModels.SignUp;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Services;
using AppRopio.Base.Auth.Core.ViewModels.Thanks;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Auth.Core
{
	public class App : MvvmCross.Core.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
			(new API.App()).Initialize();

            Mvx.LazyConstructAndRegisterSingleton<ISessionService, SessionService>();

			Mvx.RegisterSingleton<IAuthConfigService>(() => new AuthConfigService());
			Mvx.RegisterType(typeof(IPasswordNewVmService), typeof(PasswordNewVmService));
			Mvx.RegisterType(typeof(IResetPasswordVmService), typeof(ResetPasswordVmService));
			Mvx.RegisterType(typeof(IResetPasswordSmsVmService), typeof(ResetPasswordSmsVmService));
			Mvx.RegisterType(typeof(ISignInVmService), typeof(SignInVmService));
			Mvx.RegisterType(typeof(ISignUpVmService), typeof(SignUpVmService));
			Mvx.RegisterType(typeof(IAuthVmService), typeof(AuthVmService));
			Mvx.RegisterType(typeof(ISignUpItemFactoryService), typeof(SignUpItemFactoryService));

			var vmLookupService = Mvx.Resolve<IViewModelLookupService>();

			vmLookupService.Register<IAuthViewModel>(typeof(AuthViewModel));
			vmLookupService.Register<ISignUpViewModel>(typeof(SignUpViewModel));
			vmLookupService.Register<ISignInViewModel>(typeof(SignInViewModel));
			vmLookupService.Register<IResetPasswordViewModel>(typeof(ResetPasswordViewModel));
			vmLookupService.Register<IResetPasswordSmsViewModel>(typeof(ResetPasswordSmsViewModel));
			vmLookupService.Register<IPasswordNewViewModel>(typeof(PasswordNewViewModel));
			vmLookupService.Register<IThanksViewModel>(typeof(ThanksViewModel));

			Mvx.RegisterType<IAuthNavigationVmService>(() => new AuthNavigationVmService());

			if (!string.IsNullOrEmpty(AuthSettings.Token))
				Mvx.CallbackWhenRegistered<IMvxMessenger>(() => Mvx.Resolve<ISessionService>().StartByToken(AuthSettings.Token).ConfigureAwait(false));
		}
	}
}
