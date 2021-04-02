using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Marked.Core;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked;
using AppRopio.ECommerce.Marked.iOS.Services;
using AppRopio.ECommerce.Marked.iOS.Services.Implementation;
using AppRopio.ECommerce.Marked.iOS.Views.Marked;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Marked.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IMarkedThemeConfigService>(() => new MarkedThemeConfigService());

			var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<IMarkedViewModel>(typeof(MarkedViewController));
		}
	}
}