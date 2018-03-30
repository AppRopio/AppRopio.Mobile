using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked;
using AppRopio.ECommerce.Marked.iOS.Services;
using AppRopio.ECommerce.Marked.iOS.Services.Implementation;
using AppRopio.ECommerce.Marked.iOS.Views.Marked;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.ECommerce.Marked.iOS
{
	public class Plugin : IMvxPlugin
	{
		public void Load()
		{
            Mvx.RegisterSingleton<IMarkedThemeConfigService>(() => new MarkedThemeConfigService());

			var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<IMarkedViewModel>(typeof(MarkedViewController));
		}
	}
}