using AppRopio.Base.Auth.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;

namespace AppRopio.Base.Auth.Core.Services
{
	public interface IAuthNavigationVmService : IBaseVmNavigationService
	{
		void NavigateToAuthorization(BaseBundle bundle);

		void NavigateToNewPassword(BaseBundle bundle);

		void NavigateToResetPassword(BaseBundle bundle);

		void NavigateToResetSms(ResetSmsBundle bundle);

		void NavigateToSignIn(BaseBundle bundle);

		void NavigateToSignUp(BaseBundle bundle);

		void NavigateToThanks(BaseBundle bundle);
	}
}
