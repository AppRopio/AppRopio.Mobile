using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay.Services;
using AppRopio.Payments.Core.Bundle;
using AppRopio.Payments.Core.Services;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay
{
    public class Best2PayViewModel : BaseViewModel, IBest2PayViewModel
    {
        protected string OrderId { get; set; }

        protected PaymentOrderInfo PaymentInfo { get; set; }

        private string _paymentUrl;

        public string PaymentUrl
        {
            get { return _paymentUrl; }
            set { SetProperty(ref _paymentUrl, value); }
        }

		#region Commands

		private IMvxCommand _shouldLoadCommand;

		public IMvxCommand ShouldLoadCommand
		{
			get
			{
                return _shouldLoadCommand ?? (_shouldLoadCommand = new MvxCommand<string>(x => {}, ShouldLoad));
			}
		}

		#endregion

		#region Services

		protected IBest2PayVmService VmService { get { return Mvx.Resolve<IBest2PayVmService>(); } }

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

            Title = LocalizationService.GetLocalizableString(Best2PayConstants.RESX_NAME, "Title");
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

            await Purchase();
        }

        protected virtual async Task Purchase()
        {
            var oid = await VmService.RegisterOrder(PaymentInfo.Amount, PaymentInfo.Currency == "RUB" ? 643 : 0, PaymentInfo.CustomerEmail, PaymentInfo.CustomerPhone, OrderId);
            if (oid != 0)
                PaymentUrl = VmService.GetPurchaseUrl(oid, PaymentInfo.CustomerEmail);
            else
            {
                await UserDialogs.Error(LocalizationService.GetLocalizableString(Best2PayConstants.RESX_NAME, "Purchase_Failed"));
            }
        }

        protected virtual bool ShouldLoad(string url)
        {
            HandleUrlChange(url);
            return true;
        }

        protected virtual async Task HandleUrlChange(string url)
        {
			if (await VmService.ProcessPayment(url))
			{
				await PaymentsVmService.OrderPaid(OrderId);
			}

		}
    }
}