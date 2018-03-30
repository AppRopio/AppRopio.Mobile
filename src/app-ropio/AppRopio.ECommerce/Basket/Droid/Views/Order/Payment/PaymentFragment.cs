using System;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment;
using Android.Graphics;
using Android.Animation;

namespace AppRopio.ECommerce.Basket.Droid.Views.Order.Payment
{
    public class PaymentFragment : CommonFragment<IPaymentViewModel>
    {
        public PaymentFragment()
            : base(Resource.Layout.app_basket_payment)
        {
        }

        public override void OnResume()
        {
            base.OnResume();

            var backgroundAnimator = ValueAnimator.OfArgb(Color.Transparent.ToArgb(), new Color(0, 0, 0, 126).ToArgb()); 
            backgroundAnimator.Update += (object sender, ValueAnimator.AnimatorUpdateEventArgs e) => 
            {
                View.SetBackgroundColor(new Color((int)e.Animation.AnimatedValue));
            };
            backgroundAnimator.StartDelay = 300;
            backgroundAnimator.SetDuration(300);
            backgroundAnimator.Start();
        }

        public override void OnPause()
        {
            View.SetBackgroundColor(Color.Transparent);

            base.OnPause();
        }
    }
}
