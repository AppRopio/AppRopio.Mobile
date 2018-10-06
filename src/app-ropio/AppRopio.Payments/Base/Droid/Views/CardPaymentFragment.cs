using AppRopio.Base.Droid.Views;
using AppRopio.Payments.Core.ViewModels;

namespace AppRopio.Payments.Droid.Views
{
    public class CardPaymentFragment : CommonFragment<ICardPaymentViewModel>
    {
        public CardPaymentFragment()
            : base (Resource.Layout.app_payment_card)
        {
            Title = "Оплата картой";
        }
    }
}
