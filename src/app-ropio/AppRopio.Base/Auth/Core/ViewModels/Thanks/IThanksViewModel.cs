using System.Windows.Input;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Auth.Core.ViewModels.Thanks
{
	public interface IThanksViewModel : IMvxViewModel
	{
		ICommand StartCommand { get; }
	}
}
