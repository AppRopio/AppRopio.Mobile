using System;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Payments.Core.Bundle;
using MvvmCross.Platform;

namespace AppRopio.Payments.YandexKassa.Core.Services.Implementation
{
    public class YandexKassaRouterSubscriber : RouterSubsriber
    {
    	public override bool CanNavigatedTo(string type, Base.Core.Models.Bundle.BaseBundle bundle = null)
    	{
    		var paymentBundle = bundle as PaymentOrderBundle;
    		if (paymentBundle != null)
    		{
    			Mvx.Resolve<IYandexKassaPaymentNavigationVmService>().NavigateToInAppPayment(paymentBundle);
    			return true;
    		}

    		return false;
    	}
    }
}