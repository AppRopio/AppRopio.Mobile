using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.Core;
using AppRopio.Payments.Core.ViewModels;
using AppRopio.Payments.Droid.Views;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Payments.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin
    {
        public override void Load()
        {
            base.Load();

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<ICardPaymentViewModel>(typeof(CardPaymentFragment));
        }
    }
}
