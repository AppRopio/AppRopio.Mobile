using Android.OS;
using Android.Views;
using Android.Webkit;
using AppRopio.Base.Droid.Views;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Payments.Droid.Views
{
    public class CardPaymentFragment : CommonFragment<ICardPaymentViewModel>
    {
        protected WebView WebView { get; private set; }

        public CardPaymentFragment()
            : base (Resource.Layout.app_payment_card)
        {
            Title = "Оплата картой";
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            WebView = view.FindViewById<WebView>(Resource.Id.webView);

            var threeDSService = Mvx.Resolve<IPayment3DSService>();
            threeDSService.SetWebView(WebView);
        }
    }
}
