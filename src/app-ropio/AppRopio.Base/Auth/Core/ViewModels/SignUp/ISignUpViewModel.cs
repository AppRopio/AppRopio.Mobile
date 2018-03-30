using System.Collections.Generic;
using System.Windows.Input;
using AppRopio.Base.Auth.Core.ViewModels._base;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;

namespace AppRopio.Base.Auth.Core.ViewModels.SignUp
{
	public interface ISignUpViewModel : IAuthBaseViewModel
	{
		ICommand SignUpCommand { get; }

		List<ISignUpItemBaseViewModel> Items { get; }

	}
}
