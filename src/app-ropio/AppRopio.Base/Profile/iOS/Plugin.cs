using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Profile.Core;
using AppRopio.Base.Profile.Core.ViewModels.MenuHeader;
using AppRopio.Base.Profile.iOS.View.MenuHeader;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Profile.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

			var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

			viewLookupService.Register<ProfileMenuHeaderViewModel, ProfileMenuHeaderView>();
		}
	}
}
