using AppRopio.Base.Core.Services.Router;
using AppRopio.Payments.CloudPayments.Core.ViewModels.CloudPayments.Services;
using AppRopio.Payments.Core.Bundle;
using AppRopio.Payments.Core.ViewModels.Services;
using MvvmCross;

namespace AppRopio.Payments.CloudPayments.Core.Services.Implementation
{
    public class CloudPaymentsRouterSubscriber : RouterSubsriber
    {
        public override bool CanNavigatedTo(string type, Base.Core.Models.Bundle.BaseBundle bundle = null)
        {
            var paymentBundle = bundle as PaymentOrderBundle;
            if (paymentBundle != null)
            {
                var paymentVmService = new CloudPaymentsVmService();

                Mvx.RegisterSingleton<ICardPaymentVmService>(paymentVmService);
                Mvx.RegisterSingleton<ICloudPaymentsVmService>(paymentVmService);

                Mvx.Resolve<ICloudPaymentsPaymentNavigationVmService>().NavigateToInAppPayment(paymentBundle);

                return true;
            }
            
            return false;
        }
    }
}