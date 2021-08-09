using System;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Payments.Core.Bundle;
using MvvmCross;

namespace AppRopio.Payments.Best2Pay.Core.Services.Implementation
{
    public class Best2PayRouterSubscriber : RouterSubsriber
    {
        public override bool CanNavigatedTo(string type, Base.Core.Models.Bundle.BaseBundle bundle = null)
        {
			var paymentBundle = bundle as PaymentOrderBundle;
			if (paymentBundle != null)
			{
				Mvx.IoCProvider.Resolve<IBest2PayPaymentNavigationVmService>().NavigateToInAppPayment(paymentBundle);
				return true;
			}

			return true;
        }
    }
}