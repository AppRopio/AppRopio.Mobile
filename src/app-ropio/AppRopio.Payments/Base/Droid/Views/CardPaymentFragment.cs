using Android.OS;
using Android.Views;
using Android.Webkit;
using AppRopio.Base.Droid.Views;
using AppRopio.Payments.Core;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.Core.ViewModels;
using MvvmCross;

namespace AppRopio.Payments.Droid.Views
{
    public class CardPaymentFragment : CommonFragment<ICardPaymentViewModel>
    {
        protected WebView WebView { get; private set; }

        public CardPaymentFragment()
            : base (Resource.Layout.app_payment_card)
        {
            Title = LocalizationService.GetLocalizableString(PaymentsConstants.RESX_NAME, "Card_Payment");
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            WebView = view.FindViewById<WebView>(Resource.Id.webView);

            var threeDSService = Mvx.IoCProvider.Resolve<IPayment3DSService>();
            threeDSService.SetWebView(WebView);
        }
    }
}
