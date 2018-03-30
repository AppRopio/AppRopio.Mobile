using AppRopio.Payments.Core.Bundle;

namespace AppRopio.Payments.YandexKassa.Core.Services
{
    public interface IYandexKassaPaymentNavigationVmService
    {
        void NavigateToInAppPayment(PaymentOrderBundle bundle);
    }
}