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
using MvvmCross;
using MvvmCross.IoC;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Auth.Core
{
	public class App : MvvmCross.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
			(new API.App()).Initialize();

            Mvx.IoCProvider.LazyConstructAndRegisterSingleton<ISessionService, SessionService>();

			Mvx.IoCProvider.RegisterSingleton<IAuthConfigService>(() => new AuthConfigService());
			Mvx.IoCProvider.RegisterType(typeof(IPasswordNewVmService), typeof(PasswordNewVmService));
			Mvx.IoCProvider.RegisterType(typeof(IResetPasswordVmService), typeof(ResetPasswordVmService));
			Mvx.IoCProvider.RegisterType(typeof(IResetPasswordSmsVmService), typeof(ResetPasswordSmsVmService));
			Mvx.IoCProvider.RegisterType(typeof(ISignInVmService), typeof(SignInVmService));
			Mvx.IoCProvider.RegisterType(typeof(ISignUpVmService), typeof(SignUpVmService));
			Mvx.IoCProvider.RegisterType(typeof(IAuthVmService), typeof(AuthVmService));
			Mvx.IoCProvider.RegisterType(typeof(ISignUpItemFactoryService), typeof(SignUpItemFactoryService));

			var vmLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();

			vmLookupService.Register<IAuthViewModel>(typeof(AuthViewModel));
			vmLookupService.Register<ISignUpViewModel>(typeof(SignUpViewModel));
			vmLookupService.Register<ISignInViewModel>(typeof(SignInViewModel));
			vmLookupService.Register<IResetPasswordViewModel>(typeof(ResetPasswordViewModel));
			vmLookupService.Register<IResetPasswordSmsViewModel>(typeof(ResetPasswordSmsViewModel));
			vmLookupService.Register<IPasswordNewViewModel>(typeof(PasswordNewViewModel));
			vmLookupService.Register<IThanksViewModel>(typeof(ThanksViewModel));

			Mvx.IoCProvider.RegisterType<IAuthNavigationVmService>(() => new AuthNavigationVmService());

			if (!string.IsNullOrEmpty(AuthSettings.Token))
				Mvx.IoCProvider.CallbackWhenRegistered<IMvxMessenger>(() => Mvx.IoCProvider.Resolve<ISessionService>().StartByToken(AuthSettings.Token).ConfigureAwait(false));
		}
	}
}
