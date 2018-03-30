using AppRopio.Base.Auth.Core.Models.Bundle;
using AppRopio.Base.Auth.Core.ViewModels.Auth;
using AppRopio.Base.Auth.Core.ViewModels.Password.New;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms;
using AppRopio.Base.Auth.Core.ViewModels.SignIn;
using AppRopio.Base.Auth.Core.ViewModels.SignUp;
using AppRopio.Base.Auth.Core.ViewModels.Thanks;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;

namespace AppRopio.Base.Auth.Core.Services.Implementation
{
	public class AuthNavigationVmService :BaseVmNavigationService, IAuthNavigationVmService
	{
		public AuthNavigationVmService()
		{
		}

		#region IAuthNavigationVmService implementation

		public void NavigateToAuthorization(BaseBundle bundle)
		{
			NavigateTo<IAuthViewModel>(bundle);
		}

		public void NavigateToNewPassword(BaseBundle bundle)
		{
			NavigateTo<IPasswordNewViewModel>(bundle);
		}

		public void NavigateToResetPassword(BaseBundle bundle)
		{
			NavigateTo<IResetPasswordViewModel>(bundle);
		}

		public void NavigateToResetSms(ResetSmsBundle bundle)
		{
			NavigateTo<IResetPasswordSmsViewModel>(bundle);
		}

		public void NavigateToSignIn(BaseBundle bundle)
		{
			NavigateTo<ISignInViewModel>(bundle);
		}

		public void NavigateToSignUp(BaseBundle bundle)
		{
			NavigateTo<ISignUpViewModel>(bundle);
		}

		public void NavigateToThanks(BaseBundle bundle)
		{
			NavigateTo<IThanksViewModel>(bundle);
		}

		#endregion
	}
}
