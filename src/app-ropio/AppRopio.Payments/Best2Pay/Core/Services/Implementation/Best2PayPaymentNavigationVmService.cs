using System;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay;
using AppRopio.Payments.Core.Bundle;

namespace AppRopio.Payments.Best2Pay.Core.Services.Implementation
{
    public class Best2PayPaymentNavigationVmService : BaseVmNavigationService, IBest2PayPaymentNavigationVmService
    {
        public void NavigateToInAppPayment(PaymentOrderBundle bundle)
        {
            NavigateTo<IBest2PayViewModel>(bundle);
        }
    }
}
