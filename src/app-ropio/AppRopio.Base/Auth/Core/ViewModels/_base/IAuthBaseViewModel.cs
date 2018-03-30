using System;
using MvvmCross.Core.ViewModels;
namespace AppRopio.Base.Auth.Core.ViewModels._base
{
	public interface IAuthBaseViewModel : IMvxViewModel
 	{
		bool PropertiesValid { get; }
	}
}
