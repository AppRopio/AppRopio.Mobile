using System;
using AppRopio.Payments.Core.Bundle;

namespace AppRopio.Payments.Best2Pay.Core.Services
{
    public interface IBest2PayPaymentNavigationVmService
    {
        void NavigateToInAppPayment(PaymentOrderBundle bundle);
    }
}