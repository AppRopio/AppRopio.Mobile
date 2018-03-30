using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.PresentationHints;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.CloudPayments.Core.Services;
using AppRopio.Payments.CloudPayments.Core.Utils;
using AppRopio.Payments.CloudPayments.Core.ViewModels.CloudPayments.Services;
using AppRopio.Payments.Core.Bundle;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Payments.CloudPayments.Core.ViewModels.CloudPayments
{
    public class CloudPaymentsViewModel : BaseViewModel, ICloudPaymentsViewModel
    {

        #region Commands

        private IMvxCommand _payCommand;

        public IMvxCommand PayCommand
        {
            get
            {
                return _payCommand ?? (_payCommand = new MvxCommand(OnPay));
            }
        }

        #endregion

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

        #region Services

        protected ICloudPaymentsVmService VmService { get { return Mvx.Resolve<ICloudPaymentsVmService>(); } }

        public ICloudPayments3DSService ThreeDSService
        {
            get { return VmService.ThreeDSService; }
            set { VmService.ThreeDSService = value; }
        }


        #endregion

        #region Init

        protected override void InitFromBundle(IMvxBundle parameters)
        {
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

        public override void Start()
        {
            base.Start();

            Task.Run(LoadContent);
        }

        protected virtual async Task LoadContent()
        {
            Loading = true;

            await LoadPaymentInfo();

			Loading = false;
        }

        
        protected virtual void OnPay()
        {
            ProcessCardPayment(PaymentInfo);
        }

        protected virtual async Task LoadPaymentInfo()
        {
            PaymentInfo = await VmService.GetPaymentInfo(OrderId);
        }

        protected virtual async Task ProcessCardPayment(PaymentOrderInfo paymentInfo)
        {
            Loading = true;

            if (await VmService.PayWithCard(CardNumber.Without(' '), ExpirationDate, Cvv, CardHolder, paymentInfo.Amount, paymentInfo.Currency, () => {
				Loading = false;
            }))
            {
                UserDialogs.Confirm($"Заказ №{OrderId} успешно оплачен", "ОК");
                //TODO переходить на экран успеха
				this.ChangePresentation(new MoveToDefaultPH());
            }

			Loading = false;
        }

        protected virtual void UpdateCanGoNext()
        {
            CanGoNext = CardsHelper.IsValidNumber(CardNumber.Without(' ')) &&
                        CardsHelper.IsValidCvc(Cvv) &&
                        CardsHelper.IsValidExpirationDate(ExpirationDate) &&
                        !string.IsNullOrWhiteSpace(CardHolder);
        }
	}
}