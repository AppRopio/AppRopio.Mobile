using System.Windows.Input;
using AppRopio.Base.Auth.Core.ViewModels._base;

namespace AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms
{
	public interface IResetPasswordSmsViewModel: IAuthBaseViewModel
	{
		string VerifyCode { get; }

		string DescriptionWithPhone { get; }

		ICommand ValidateCodeCmd { get; }

		ICommand ResendCodeCmd { get; }
	}
}
