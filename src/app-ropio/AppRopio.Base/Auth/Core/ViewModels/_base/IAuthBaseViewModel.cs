using System;
using MvvmCross.ViewModels;
namespace AppRopio.Base.Auth.Core.ViewModels._base
{
	public interface IAuthBaseViewModel : IMvxViewModel
 	{
		bool PropertiesValid { get; }
	}
}
