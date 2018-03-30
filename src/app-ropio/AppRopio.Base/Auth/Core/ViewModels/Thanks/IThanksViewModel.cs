using System.Windows.Input;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Auth.Core.ViewModels.Thanks
{
	public interface IThanksViewModel : IMvxViewModel
	{
		ICommand StartCommand { get; }
	}
}
