using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.ApplePay.Models;
using Foundation;
using MvvmCross;
using Newtonsoft.Json;
using PassKit;

namespace AppRopio.Payments.ApplePay.Services
{
    public class ApplePayService : PKPaymentAuthorizationControllerDelegate
    {
        private TaskCompletionSource<string> _tcs;
        private Action<PKPaymentAuthorizationStatus> _completion;

        protected ApplePayConfig Config { get { return Mvx.Resolve<IApplePayConfigService>().Config; } }

        public Task<string> Pay(PaymentOrderInfo info, Action authorizationCallback = null)
        {
            if (info == null)
                return Task.FromResult(string.Empty);

            _tcs = new TaskCompletionSource<string>();

            var request = new PKPaymentRequest();
            request.MerchantIdentifier = Config.MerchantId;
            request.SupportedNetworks = Config.SupportedNetworks.Select(n => new NSString(n)).ToArray();
            request.MerchantCapabilities = PKMerchantCapability.ThreeDS;
            request.CountryCode = Config.CountryCode;
            request.CurrencyCode = info.Currency;

            var paymentItems = new List<PKPaymentSummaryItem>();

            if (!info.Items.IsNullOrEmpty())
                paymentItems = info.Items.Select(item => new PKPaymentSummaryItem()
                {
                    Label = item.Title,
                    Amount = new NSDecimalNumber(item.Amount.ToString())
                }).ToList();

            //добавляем итоговую стоимость
            paymentItems.Add(new PKPaymentSummaryItem()
            {
                Label = Mvx.Resolve<ILocalizationService>().GetLocalizableString(ApplePayConstants.RESX_NAME, "Payment_Sum"),
                Amount = new NSDecimalNumber(info.Amount.ToString())
            });

            request.PaymentSummaryItems = paymentItems.ToArray();

            var paymentViewController = new PKPaymentAuthorizationController(request);
            paymentViewController.Delegate = this;
            paymentViewController.Present(null);

            return _tcs.Task;
        }

        public void Complete(PKPaymentAuthorizationStatus status)
        {
            _completion(status);
        }

        public override void DidFinish(PKPaymentAuthorizationController controller)
        {
            controller.Dismiss(null);
        }

        public override void DidAuthorizePayment(PKPaymentAuthorizationController controller, PKPayment payment, Action<PKPaymentAuthorizationStatus> completion)
        {
            _completion = completion;

            var paymentData = NSString.FromData(payment.Token.PaymentData, NSStringEncoding.UTF8);

            string paymentType;
            switch (payment.Token.PaymentMethod.Type)
            {
                case PKPaymentMethodType.Debit:
                    paymentType = @"debit";
                    break;
                case PKPaymentMethodType.Credit:
                    paymentType = @"credit";
                    break;
                case PKPaymentMethodType.Store:
                    paymentType = @"store";
                    break;
                case PKPaymentMethodType.Prepaid:
                    paymentType = @"prepaid";
                    break;
                default:
                    paymentType = @"unknown";
                    break;
            }

            var paymentMethod = new Dictionary<string, string>
            {
                ["network"] = payment.Token.PaymentMethod.Network,
                ["type"] = paymentType,
                ["displayName"] = payment.Token.PaymentMethod.DisplayName
            };

            var token = JsonConvert.SerializeObject(new
            {
                paymentData = JsonConvert.DeserializeObject(paymentData),
                transactionIdentifier = payment.Token.TransactionIdentifier,
                paymentMethod = paymentMethod
            });

            _tcs.TrySetResult(token);
        }
    }
}