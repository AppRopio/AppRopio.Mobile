using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked;
using AppRopio.ECommerce.Marked.Droid.Views.Marked;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Marked.Droid
{
	public class Plugin : IMvxPlugin
	{
		public void Load()
		{
			var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<IMarkedViewModel>(typeof(MarkedFragment));
		}
	}
}