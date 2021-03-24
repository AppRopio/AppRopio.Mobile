using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.API.Services;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.YandexKassa.API.Services;
using AppRopio.Payments.YandexKassa.Core.Models;
using AppRopio.Payments.YandexKassa.Core.Services;
using MvvmCross;

namespace AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa.Services
{
    public class YandexKassaVmService : BaseVmService, IYandexKassaVmService
    {
        protected IPaymentService ApiService { get { return Mvx.Resolve<IPaymentService>(); } }

		protected IPaymentsVmService PaymentsVmService { get { return Mvx.Resolve<IPaymentsVmService>(); } }

		protected YandexKassaConfig Config { get { return Mvx.Resolve<IYandexKassaConfigService>().Config; } }

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

		public async Task<bool> OrderPaid(string orderId)
		{
			try
			{
				await PaymentsVmService.OrderPaid(orderId);
				return true;
			}
			catch (ConnectionException ex)
			{
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
				OnException(ex);
			}

			return false;
		}

        public Dictionary<string, string> GetPaymentParams(decimal amount, string currency, string orderId, IList<PaymentOrderItem> items, string customerPhone)
        {
			var successUrl = Config.SuccessUrl;
			var failUrl = Config.FailUrl;

			var postParams = new Dictionary<string, string>();
			postParams["shopId"] = Config.ShopId;
			postParams["scid"] = Config.Scid;
			postParams["sum"] = amount.ToString();
			postParams["customerNumber"] = orderId; //т.к. индентификатора плательщика нет, а он обязательный, передаем номер заказа
			postParams["shopSuccessURL"] = successUrl;
			postParams["shopFailURL"] = failUrl;

            return postParams;
        }

		public async Task<bool> PayWithApplePay(string paymentData, decimal amount, string currency, string orderId)
		{
            var service = Mvx.Resolve<IYandexKassaService>();
            await service.DsrpWallet(Config.ShopId, orderId, amount, currency, paymentData);
            return false;
		}
    }
}