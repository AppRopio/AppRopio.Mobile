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
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

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
                SetProperty(ref _cardNumber, CardsHelper.FormatCardNumber(value));

                UpdateCanGoNext();
            }
        }

        private string _expirationDate;
        public string ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                SetProperty(ref _expirationDate, CardsHelper.FormatExpirationDate(value));

                UpdateCanGoNext();
            }
        }

        private string _cvv;
        public string Cvv
        {
            get { return _cvv; }
            set
            {
                SetProperty(ref _cvv, value);

                UpdateCanGoNext();
            }
        }

        private string _cardHolder;
        public string CardHolder
        {
            get { return _cardHolder; }
            set
            {
                SetProperty(ref _cardHolder, value);

                UpdateCanGoNext();
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

        protected ICardPaymentVmService VmService { get { return Mvx.IoCProvider.Resolve<ICardPaymentVmService>(); } }

        protected IPaymentsVmService PaymentsVmService { get { return Mvx.IoCProvider.Resolve<IPaymentsVmService>(); } }

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
                await UserDialogs.Error(paymentResult.ErrorMessage.IsNullOrEmtpy() ? LocalizationService.GetLocalizableString(PaymentsConstants.RESX_NAME, "Error_PaymentFailed") : paymentResult.ErrorMessage);

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