using System;
using System.Text;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.Core.Bundle;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa;
using AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa.Services;
using MvvmCross;
using Newtonsoft.Json.Linq;

namespace AppRopio.Payments.YandexKassa.Core.Services.Implementation
{
    public class YandexKassaPaymentNavigationVmService : BaseVmNavigationService, IYandexKassaPaymentNavigationVmService
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

        private void ProcessCardPayment(PaymentOrderBundle bundle)
        {
            NavigateTo<IYandexKassaViewModel>(bundle);
        }

        private async void ProcessNativePayment(PaymentOrderBundle bundle)
        {
            var vmService = Mvx.Resolve<IYandexKassaVmService>();
            var paymentsVmService = Mvx.Resolve<IPaymentsVmService>();
            var nativePaymentService = Mvx.Resolve<INativePaymentService>();

            var paymentInfo = await vmService.GetPaymentInfo(bundle.OrderId);
            var paymentToken = await nativePaymentService.Pay(paymentInfo);

            if (paymentToken == null)
                return;

			var paymentTokenObject = JObject.Parse(paymentToken);
			var paymentData = paymentTokenObject["paymentData"].ToString();

            if (await vmService.PayWithApplePay(paymentData, paymentInfo.Amount, paymentInfo.Currency, bundle.OrderId))
            {
                await paymentsVmService.OrderPaid(bundle.OrderId);
                nativePaymentService.CompleteSuccess();
            }
            else
                nativePaymentService.CompleteFail();
        }
    }   
}
