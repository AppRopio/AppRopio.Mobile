using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.Best2Pay.Core;
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay;
using AppRopio.Payments.Best2Pay.iOS.Views;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Payments.Best2Pay.iOS
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Best2Pay";

        public override void Load()
        {
            base.Load();

			var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

			viewLookupService.Register<IBest2PayViewModel, Best2PayViewController>();
		}
	}
}