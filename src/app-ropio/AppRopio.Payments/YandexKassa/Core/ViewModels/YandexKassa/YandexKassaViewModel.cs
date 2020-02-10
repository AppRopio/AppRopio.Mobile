using System;
using System.Net.Http;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.PresentationHints;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Basket.Responses.Enums;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.Core.Bundle;
using AppRopio.Payments.YandexKassa.Core.Models;
using AppRopio.Payments.YandexKassa.Core.Services;
using AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa
{
    public class YandexKassaViewModel : BaseViewModel, IYandexKassaViewModel
    {
        #region Commands

        private IMvxCommand _shouldLoadCommand;

        public IMvxCommand ShouldLoadCommand
        {
            get
            {
                return _shouldLoadCommand ?? (_shouldLoadCommand = new MvxCommand<string>(x => { }, ShouldLoad));
            }
        }

        #endregion

        protected IYandexKassaVmService VmService { get { return Mvx.Resolve<IYandexKassaVmService>(); } }

        protected YandexKassaConfig Config { get { return Mvx.Resolve<IYandexKassaConfigService>().Config; } }

        protected string OrderId { get; set; }

        protected PaymentType PaymentType { get; set; }

        protected PaymentOrderInfo PaymentInfo { get; set; }

        private string _paymentUrl;

        public string PaymentUrl
        {
            get { return _paymentUrl; }
            set { SetProperty(ref _paymentUrl, value); }
        }

        private HttpContent _paymentParams;

        public HttpContent PaymentParams
        {
            get { return _paymentParams; }
            set { SetProperty(ref _paymentParams, value); }
        }

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var navigationBundle = parameters.ReadAs<PaymentOrderBundle>();
            this.InitFromBundle(navigationBundle);
        }

        protected virtual void InitFromBundle(PaymentOrderBundle parameters)
        {
            VmNavigationType = parameters.NavigationType == NavigationType.None ?
                                                            NavigationType.ClearAndPush :
                                                            parameters.NavigationType;
            OrderId = parameters.OrderId;
            PaymentType = parameters.PaymentType;
        }

        #endregion

        public override Task Initialize()
        {
            return LoadContent();
        }

        protected virtual async Task LoadContent()
        {
            Loading = true;

            await LoadPaymentInfo();

            Loading = false;
        }

        protected virtual async Task LoadPaymentInfo()
        {
            PaymentInfo = await VmService.GetPaymentInfo(OrderId);

            InvokeOnMainThread(PayWithCard);
        }

        protected virtual async void PayWithCard()
        {
            var paymentParams = VmService.GetPaymentParams(PaymentInfo.Amount, PaymentInfo.Currency, OrderId, PaymentInfo.Items, PaymentInfo.CustomerPhone);
            PaymentParams = new FormUrlEncodedContent(paymentParams);
            PaymentUrl = YandexKassaConstants.PAY_URL;
        }

        protected bool ShouldLoad(string url)
        {
            if (url.StartsWith(Config.SuccessUrl, StringComparison.OrdinalIgnoreCase))
            {
                OnPaymentSuccess();
                return false;
            }

            if (url.StartsWith(Config.FailUrl, StringComparison.OrdinalIgnoreCase))
            {
                OnPaymentFailed();
                return false;
            }

            //handle "back to store" action
            if (url.StartsWith(Config.ShopUrl, StringComparison.OrdinalIgnoreCase)) 
            {
                OnPaymentFailed();
                return false;
            }

            return true;
        }

        protected virtual async void OnPaymentSuccess()
        {
            await VmService.OrderPaid(OrderId);
        }

        protected virtual void OnPaymentFailed()
        {
            UserDialogs.Error($"{LocalizationService.GetLocalizableString(YandexKassaConstants.RESX_NAME, "PaymentFailed_OrderNumber")}{OrderId}");

            Close(this);
        }
    }
}