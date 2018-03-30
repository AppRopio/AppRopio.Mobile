using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Models.Device;
using AppRopio.Base.Core.Services.Device;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.API.Services;
using AppRopio.Payments.Best2Pay.API;
using AppRopio.Payments.Best2Pay.API.Extentions;
using AppRopio.Payments.Best2Pay.Core.Models;
using AppRopio.Payments.Best2Pay.Core.Services;
using MvvmCross.Platform;

namespace AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay.Services
{
    public class Best2PayVmService : BaseVmService, IBest2PayVmService
	{
        private IB2PRequest _purchaseRequest;

        protected API.Best2Pay _best2Pay;

		protected IPaymentService ApiService { get { return Mvx.Resolve<IPaymentService>(); } }

        protected Best2PayConfig Config { get { return Mvx.Resolve<IBest2PayConfigService>().Config; } }

        private string _forwardUrl;
        private string _forwardVerifyUrl;
        private bool _ePayment = false;

        public Best2PayVmService()
        {
            var platform = Mvx.Resolve<IDeviceService>().Platform;

            _ePayment = Config.EPayment;
            _forwardUrl = platform == PlatformType.iPhone || platform == PlatformType.iPad ? @"https://best2pay.ru/mobileAPI/iOS" : @"https://best2pay.ru/mobileAPI/Android";
            _forwardVerifyUrl = "https://best2pay.ru/mobileAPI/Phone";

            _best2Pay = new API.Best2Pay(Config.Sector, Config.Password, _forwardUrl, _forwardVerifyUrl);
        }

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

        public Task<int> RegisterOrder(decimal amount, int currency, string email, string phone, string orderId)
        {
            var tcs = new TaskCompletionSource<int>();

			var registerRequest = new B2PRegisterRequest()
			{
                amount = (int)amount * 100, //в копейках 
                currency = currency,
                email = email,
                phone = phone,
                reference = orderId,
                description = "Оплата заказа №" + orderId,
                deviceID = Mvx.Resolve<IDeviceService>().Token
			};

            _best2Pay.RegisterOrder(registerRequest, (r, e) =>
			{
                if (r != null)
                    tcs.TrySetResult(r.ID);
                else
                    tcs.TrySetResult(0);
			});

            return tcs.Task;
        }

        public string GetPurchaseUrl(int orderId, string email = null)
		{
            if (!_ePayment)
            {
                _purchaseRequest = new B2PPurchaseRequest()
                {
                    ID = orderId,
                    get_token = true
                };

                return _best2Pay.PurchaseURL((B2PPurchaseRequest)_purchaseRequest);
            }
			else
			{
				_purchaseRequest = new B2PEpaymentRequest()
				{
                    ID = orderId,
					email = email,
				};
                return _best2Pay.EpaymentURL((B2PEpaymentRequest)_purchaseRequest);
			}
        }

        public async Task<bool> ProcessPayment(string url)
        {
            if ((url.IndexOf(_forwardUrl, StringComparison.CurrentCulture) != -1) || (url.IndexOf(_forwardVerifyUrl, StringComparison.CurrentCulture) != -1))
            {
                string query = url;
				Dictionary<string, string> dict = Parameters.dictionaryWithQueryString(query.Substring(query.IndexOf('?') + 1));
				int errorCode = Parameters.ConvertToInt(Parameters.stringFromParsedDictionary(dict, "error"));
				B2PError error = (errorCode != 0) ? new B2PError(errorCode, "") : null;

				IB2PResponse response = _purchaseRequest.ResponseForRequest();
				response.fillFromDictionary(dict);

				if (error != null && error.Code == 127)
				{
                    return false;
				}

				var operationResponse = (B2POperationResponse)response;
				var operationId = operationResponse.ID;
                var orderId = operationResponse.order_id;

				if (operationResponse.state == "APPROVED")
				{
                    return true;
				}
				else
                    return await Operation(operationId, orderId);
            }

            return false;
        }

        protected Task<bool> Operation(int operationId, int orderId)
		{
            var tcs = new TaskCompletionSource<bool>();

            _best2Pay.Operation(new B2POperationRequest()
			{
                ID = orderId,
                operation = operationId,
                get_token = !_ePayment
			}, async (response, error) =>
            {
                if (response != null && (response.state == "REJECTED" || response.state == "TIMEOUT"))
                {
                    string errorText = GetErrorText(response.reason_code);

                    if (response.reason_code == 4 || response.reason_code == 5)
                    {
                        //_card = null;
                    }

                    await UserDialogs.Error(errorText ?? $"Не удалось произвести оплату");
                    tcs.TrySetResult(false);
                    return;
                }

                if (error == null)
                {
                    //if (!_ePayment)
                    //{
                    ////сохраняем токен
                    //var cardInfo = new CreditCardInfo();

                    //cardInfo.PAN = response.pan;
                    //cardInfo.Token = response.token;

                    //если нет ошибки, показываем спасибоэкран
                    //OnPaymentCompleted();
                    //}
                    //else
                    tcs.TrySetResult(true);
                }
                else
                {
                    //иначе показываем пользователю сообщение об ошибке
                    await UserDialogs.Error($"Не удалось произвести оплату");
                    tcs.TrySetResult(false);
                }
            });

            return tcs.Task;
		}

		protected string GetErrorText(int reasonCode)
		{
			switch (reasonCode)
			{
				case 2:
				case 6:
				case 12:
					return "Платеж отклонен. Возможные причины: недостаточно средств на счете, были указаны неверные реквизиты карты, по вашей карте запрещены расчеты через Интернет. Пожалуйста, попробуйте выполнить платеж повторно или обратитесь в Банк, выпустивший вашу карту.";
				case 3:
					return "Платеж отклонен. Пожалуйста, обратитесь в Банк, выпустивший вашу карту.";
				case 4:
				case 5:
					//if (_isPurchaseByToken)
					//    return "Попробуйте повторить операцию с полным вводом реквизитов карты, возможно ваш банк запретил операцию по зарегистрированной карте.";
					//else
					return "Платёж отклонён. Пожалуйста, обратитесь в Банк, выпустивший Вашу карту.";
				case 7:
				case 8:
				case 9:
				case 10:
				case 11:
				case 15:
				case 16:
					return "Платеж отклонен. Пожалуйста, обратитесь в Интернет-магазин.";
				case 13:
				case 0:
					return "Платеж отклонен. Пожалуйста, попробуйте выполнить платеж позднее или обратитесь в Интернет-магазин.";
				default:
					return "Операция завершилась неуспешно, попробуйте еще раз.";
			}
		}
	}
}