using System;
using AppRopio.Base.Core.Services.ViewLookup;
using MvvmCross;
using MvvmCross.Plugin;
using AppRopio.Base.Profile.Core.ViewModels.MenuHeader;
using AppRopio.Base.Profile.iOS.View.MenuHeader;

namespace AppRopio.Base.Profile.iOS
{
	public class Plugin: IMvxPlugin
	{
		public void Load()
		{
			var viewLookupService = Mvx.Resolve<IViewLookupService>();

			viewLookupService.Register<ProfileMenuHeaderViewModel, ProfileMenuHeaderView>();
		}
	}
}
