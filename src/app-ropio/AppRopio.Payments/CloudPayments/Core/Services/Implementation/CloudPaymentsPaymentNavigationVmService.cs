using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.CloudPayments.Core.ViewModels.CloudPayments.Services;
using AppRopio.Payments.Core.Bundle;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.Core.ViewModels;
using MvvmCross;

namespace AppRopio.Payments.CloudPayments.Core.Services.Implementation
{
    public class CloudPaymentsPaymentNavigationVmService : BaseVmNavigationService, ICloudPaymentsPaymentNavigationVmService
    {
        public void NavigateToInAppPayment(PaymentOrderBundle bundle)
        {
            switch (bundle.PaymentType)
            {
                case PaymentType.CreditCard:
                    ProcessCardPayment(bundle);
                    break;
                case PaymentType.Native:
                    ProcessNativePayment(bundle);
                    break;
                default:
                    break;
            }
        }

        private async void ProcessCardPayment(PaymentOrderBundle bundle)
        {
            await NavigateTo<ICardPaymentViewModel>(bundle);
        }

        private async void ProcessNativePayment(PaymentOrderBundle bundle)
        {
            var vmService = Mvx.IoCProvider.Resolve<ICloudPaymentsVmService>();
            var paymentsVmService = Mvx.IoCProvider.Resolve<IPaymentsVmService>();
            var nativePaymentService = Mvx.IoCProvider.Resolve<INativePaymentService>();

            var paymentInfo = await vmService.GetPaymentInfo(bundle.OrderId);
            var paymentToken = await nativePaymentService.Pay(paymentInfo);

            if (paymentToken == null)
                return;

            var paymentResult = await vmService.PayWithApplePay(paymentToken, paymentInfo.Amount, paymentInfo.Currency, bundle.OrderId);
            if (paymentResult.Succeeded)
            {
                await paymentsVmService.OrderPaid(bundle.OrderId);
                nativePaymentService.CompleteSuccess();
            }
            else
                nativePaymentService.CompleteFail();
        }
    }
}
