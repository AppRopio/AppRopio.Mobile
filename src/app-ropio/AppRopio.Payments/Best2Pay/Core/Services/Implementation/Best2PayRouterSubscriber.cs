using System;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Payments.Core.Bundle;
using MvvmCross.Platform;

namespace AppRopio.Payments.Best2Pay.Core.Services.Implementation
{
    public class Best2PayRouterSubscriber : RouterSubsriber
    {
        public override bool CanNavigatedTo(string type, Base.Core.Models.Bundle.BaseBundle bundle = null)
        {
			var paymentBundle = bundle as PaymentOrderBundle;
			if (paymentBundle != null)
			{
				Mvx.Resolve<IBest2PayPaymentNavigationVmService>().NavigateToInAppPayment(paymentBundle);
				return true;
			}

			return true;
        }
    }
}