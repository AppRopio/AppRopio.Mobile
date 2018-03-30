using AppRopio.Payments.Core.Bundle;

namespace AppRopio.Payments.CloudPayments.Core.Services
{
    public interface ICloudPaymentsPaymentNavigationVmService
    {
        void NavigateToInAppPayment(PaymentOrderBundle bundle);
    }
}