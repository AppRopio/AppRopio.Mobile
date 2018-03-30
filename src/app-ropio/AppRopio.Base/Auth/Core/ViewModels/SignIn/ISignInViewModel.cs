using System.Windows.Input;
using AppRopio.Base.Auth.Core.ViewModels._base;

namespace AppRopio.Base.Auth.Core.ViewModels.SignIn
{
	public interface ISignInViewModel : IAuthBaseViewModel
    {
        string Identity { get; }

        string Password { get; }

        ICommand RecoveryPassCommand { get; }
        
        ICommand SignInCommand { get; }

        ICommand SkipCommand { get; }

    }
}
