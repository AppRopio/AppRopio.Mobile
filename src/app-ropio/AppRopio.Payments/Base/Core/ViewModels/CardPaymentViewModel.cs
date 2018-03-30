using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.Core.Bundle;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.Core.Utils;
using AppRopio.Payments.Core.ViewModels.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Payments.Core.ViewModels
{
    public class CardPaymentViewModel : BaseViewModel, ICardPaymentViewModel
    {
        #region Commands

        private IMvxCommand _payCommand;

        public IMvxCommand PayCommand => _payCommand ?? (_payCommand = new MvxCommand(OnPay, () => CanGoNext));

        #endregion

        #region Properties

        private string _cardNumber;
        public string CardNumber
        {
            get { return _cardNumber; }
            set
            {
                if (SetProperty(ref _cardNumber, value))
                {
                    CardNumber = CardsHelper.FormatCardNumber(_cardNumber);
                    UpdateCanGoNext();
                }
            }
        }

        private string _expirationDate;
        public string ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                if (SetProperty(ref _expirationDate, value))
                {
                    ExpirationDate = CardsHelper.FormatExpirationDate(_expirationDate);
                    UpdateCanGoNext();
                }
            }
        }

        private string _cvv;
        public string Cvv
        {
            get { return _cvv; }
            set
            {
                if (SetProperty(ref _cvv, value))
                {
                    UpdateCanGoNext();
                }
            }
        }

        private string _cardHolder;
        public string CardHolder
        {
            get { return _cardHolder; }
            set
            {
                if (SetProperty(ref _cardHolder, value))
                {
                    UpdateCanGoNext();
                }
            }
        }

        private bool _showWebView;
        public bool ShowWebView
        {
            get { return _showWebView; }
            protected set { SetProperty(ref _showWebView, value); }
        }

        private bool _canGoNext;
        public bool CanGoNext
        {
            get { return _canGoNext; }
            protected set { SetProperty(ref _canGoNext, value); }
        }

        protected string OrderId { get; set; }

        protected PaymentType PaymentType { get; set; }

        protected PaymentOrderInfo PaymentInfo { get; set; }

        #endregion

        #region Services

        protected ICardPaymentVmService VmService { get { return Mvx.Resolve<ICardPaymentVmService>(); } }

        protected IPaymentsVmService PaymentsVmService { get { return Mvx.Resolve<IPaymentsVmService>(); } }

        #endregion

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


        protected virtual async void OnPay()
        {
            await ProcessCardPayment(PaymentInfo);
        }

        protected virtual async Task LoadPaymentInfo()
        {
            PaymentInfo = await VmService.GetPaymentInfo(OrderId);
        }

        protected virtual async Task ProcessCardPayment(PaymentOrderInfo paymentInfo)
        {
            Loading = true;

            var paymentResult = await VmService.PayWithCard(CardNumber.Without(' '), ExpirationDate, Cvv, CardHolder, paymentInfo.Amount, paymentInfo.Currency, () =>
            {
                Loading = false;
            }, OrderId);

            if (paymentResult.Succeeded)
                await PaymentsVmService.OrderPaid(OrderId);
            else
                await UserDialogs.Error(paymentResult.ErrorMessage.IsNullOrEmtpy() ? "Не удалось совершить платеж, проверьте введенные данные. Если ошибка повторится попробуйте совершить платеж позже" : paymentResult.ErrorMessage);

            Loading = false;
        }

        protected virtual void UpdateCanGoNext()
        {
            CanGoNext = CardsHelper.IsValidNumber(CardNumber.Without(' ')) &&
                        CardsHelper.IsValidCvc(Cvv) &&
                        CardsHelper.IsValidExpirationDate(ExpirationDate) &&
                        !string.IsNullOrWhiteSpace(CardHolder);

            PayCommand.RaiseCanExecuteChanged();
        }
    }
}