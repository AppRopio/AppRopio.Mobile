using System.Windows.Input;
using AppRopio.Base.Auth.Core.ViewModels._base;
namespace AppRopio.Base.Auth.Core.ViewModels.Password.New
{
	public interface IPasswordNewViewModel : IAuthBaseViewModel
	{
		string Password { get; }

		string PasswordConfirm { get; }

		ICommand DoneCommand { get; }
	}
}
