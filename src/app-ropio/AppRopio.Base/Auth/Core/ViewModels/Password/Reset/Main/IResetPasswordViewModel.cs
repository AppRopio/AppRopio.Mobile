using System.Windows.Input;
using AppRopio.Base.Auth.Core.ViewModels._base;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main
{
	public interface IResetPasswordViewModel : IAuthBaseViewModel
	{
		string Identity { get; }

		bool SuccessViewVisible { get; }

		ICommand ForgotCmd { get; }

		ICommand CloseCmd { get; }
	}
}
