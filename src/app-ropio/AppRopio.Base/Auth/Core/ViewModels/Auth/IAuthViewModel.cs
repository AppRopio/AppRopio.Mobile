using System.Windows.Input;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Auth.Core.ViewModels.Auth
{
    public interface IAuthViewModel : IMvxViewModel
    {
        ICommand NavigateToSignInCommand { get; }

        ICommand NavigateToSignUpCommand { get; }

        ICommand SkipAuthCommand { get; }

        ICommand VkCommand { get; }

        ICommand FacebookCommand { get; }

        string SignInText { get; }

        string SignUpText { get; }

        string SkipText { get; }
    }
}
