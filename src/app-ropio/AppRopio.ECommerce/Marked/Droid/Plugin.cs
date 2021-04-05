using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.ECommerce.Marked.Core;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked;
using AppRopio.ECommerce.Marked.Droid.Views.Marked;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Marked.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Marked";

        public override void Load()
        {
            base.Load();

			var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<IMarkedViewModel>(typeof(MarkedFragment));
		}
	}
}