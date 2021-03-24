using AppRopio.Base.Auth.Core.Services;
using AppRopio.Base.Auth.Core.ViewModels.Auth;
using AppRopio.Base.Auth.Core.ViewModels.Password.New;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms;
using AppRopio.Base.Auth.Core.ViewModels.SignIn;
using AppRopio.Base.Auth.Core.ViewModels.SignUp;
using AppRopio.Base.Auth.Core.ViewModels.Thanks;
using AppRopio.Base.Auth.iOS.Services;
using AppRopio.Base.Auth.iOS.Services.Implementation;
using AppRopio.Base.Auth.iOS.Views.Auth;
using AppRopio.Base.Auth.iOS.Views.Password.New;
using AppRopio.Base.Auth.iOS.Views.Password.Reset.Main;
using AppRopio.Base.Auth.iOS.Views.Password.Reset.Sms;
using AppRopio.Base.Auth.iOS.Views.SignIn;
using AppRopio.Base.Auth.iOS.Views.SignUp;
using AppRopio.Base.Auth.iOS.Views.Thanks;
using AppRopio.Base.Core.Services.ViewLookup;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Auth.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<IAuthViewModel, AuthViewController>();
            viewLookupService.Register<ISignInViewModel, SignInViewController>();
            viewLookupService.Register<ISignUpViewModel, SignUpViewController>();
            viewLookupService.Register<IResetPasswordViewModel, ResetPasswordViewController>();
            viewLookupService.Register<IResetPasswordSmsViewModel, ResetSmsViewController>();
            viewLookupService.Register<IPasswordNewViewModel, PasswordNewViewController>();
            viewLookupService.Register<IThanksViewModel, ThanksViewController>();

            Mvx.RegisterSingleton<IOAuthService>(() => new OAuthService());
            Mvx.RegisterSingleton<IAuthThemeConfigService>(() => new AuthThemeConfigService());
        }
    }
}
