using System.Threading.Tasks;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.ApplePay.Services;
using AppRopio.Payments.CloudPayments.Core.Services;
using AppRopio.Payments.Core.Services;

namespace AppRopio.Payments.CloudPayments.iOS.Services.Implementation
{
    public class NativePaymentService : INativePaymentService
    {
        private ApplePayService _applePayService = new ApplePayService();

        public async Task<string> Pay(PaymentOrderInfo info)
        {
            return await _applePayService.Pay(info);
        }

		public void CompleteFail()
		{
            _applePayService.Complete(PassKit.PKPaymentAuthorizationStatus.Failure);
		}

		public void CompleteSuccess()
		{
			_applePayService.Complete(PassKit.PKPaymentAuthorizationStatus.Success);
		}

	}
}