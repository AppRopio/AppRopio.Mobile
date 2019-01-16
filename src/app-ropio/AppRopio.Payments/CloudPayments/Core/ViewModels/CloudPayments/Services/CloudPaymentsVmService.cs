using System;
using System.Net.Http;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.API.Services;
using AppRopio.Payments.CloudPayments.API.Responses;
using AppRopio.Payments.CloudPayments.API.Services;
using AppRopio.Payments.CloudPayments.Core.Models;
using AppRopio.Payments.CloudPayments.Core.Services;
using AppRopio.Payments.Core.Models;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.Core.ViewModels.Services;
using MvvmCross.Platform;

namespace AppRopio.Payments.CloudPayments.Core.ViewModels.CloudPayments.Services
{
    public class CloudPaymentsVmService : BaseVmService, ICloudPaymentsVmService, ICardPaymentVmService
    {
        protected ICloudPaymentsService Service { get { return Mvx.Resolve<ICloudPaymentsService>(); } }

        protected IPaymentService ApiService { get { return Mvx.Resolve<IPaymentService>(); } }

        protected CloudPaymentsConfig Config { get { return Mvx.Resolve<ICloudPaymentsConfigService>().Config; } }

        protected IPayment3DSService ThreeDSService { get { return Mvx.Resolve<IPayment3DSService>(); } }

        public async Task<PaymentOrderInfo> GetPaymentInfo(string orderId)
        {
            PaymentOrderInfo info = null;

            try
            {
                info = await ApiService.OrderInfo(orderId);
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return info;
        }

        public Task<PaymentResult> PayWithCard(string cardNumber, string expirateDate, string cvv, string cardHolderName, decimal amount, string currency, Action threeDSCallback, string orderId)
        {
            var cryptogram = Service.CreateCryptogramPacket(cardNumber, expirateDate, cvv, CloudPaymentsConstants.PUBLIC_KEY, Config.StoreId);

            return ProcessPayment(cryptogram, amount, currency, cardHolderName, threeDSCallback, orderId);
        }

        public Task<PaymentResult> PayWithApplePay(string token, decimal amount, string currency, string orderId)
        {
            return ProcessPayment(token, amount, currency, null, null, orderId);
        }

        private async Task<PaymentResult> ProcessPayment(string cryptogram, decimal amount, string currency, string cardHolderName, Action threeDSCallback, string orderId)
        {
            try
            {
                Response<ChargeResponse> chargeResult = null;

                if (Config.MessageScheme == MessageSchemeType.Single)
                    chargeResult = await Service.Charge(cryptogram, amount, currency, cardHolderName, Config.StoreId, Config.ApiSecret, orderId);
                else if (Config.MessageScheme == MessageSchemeType.Dual)
                    chargeResult = await Service.Auth(cryptogram, amount, currency, cardHolderName, Config.StoreId, Config.ApiSecret, orderId);

                if (chargeResult.Success)
                    return new PaymentResult { Succeeded = true };

                if (chargeResult.Model?.AcsUrl != null)
                {
                    //3DSecure
                    var parameters = Service.Get3DSPaymentParams(chargeResult.Model, Config.ThreeDSRedirectUrl);

                    threeDSCallback?.Invoke();

                    var threeDSResult = await ThreeDSService.Process3DS(chargeResult.Model.AcsUrl, Config.ThreeDSRedirectUrl, parameters);
                    if (threeDSResult == null || !threeDSResult.ContainsKey("PaRes"))
                    {
                        return new PaymentResult { Succeeded = false };
                    }

                    var complete3DSResult = await Service.Completed3DSPayment(threeDSResult["PaRes"], chargeResult.Model.TransactionId, Config.StoreId, Config.ApiSecret);
                    if (complete3DSResult.Success)
                        return new PaymentResult { Succeeded = true, TransactionId = chargeResult.Model.TransactionId };
                }

                if (chargeResult.Model?.CardHolderMessage != null && !chargeResult.Model.CardHolderMessage.IsNullOrEmpty())
                {
                    return new PaymentResult { Succeeded = false, ErrorMessage = chargeResult.Model.CardHolderMessage };
                }

                if (!chargeResult.Message.IsNullOrEmpty())
                    return new PaymentResult { Succeeded = false, ErrorMessage = chargeResult.Message };
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return new PaymentResult { Succeeded = false };
        }
    }
}