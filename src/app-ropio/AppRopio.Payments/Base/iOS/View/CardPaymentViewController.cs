using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Payments.Core;
using AppRopio.Payments.Core.Services;
using AppRopio.Payments.Core.ViewModels;
using AppRopio.Payments.iOS.Models;
using AppRopio.Payments.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Payments.CloudPayments.iOS.View
{
    public partial class CardPaymentViewController : CommonViewController<ICardPaymentViewModel>
    {
		private const int MAX_NUMBER_LENGTH = 19;
		private const int MAX_DATE_LENGTH = 5;
		private const int MAX_CVC_LENGTH = 3;
        private const int MAX_CARDHOLDER_LENGTH = 256;

        private UIButton _accessoryButton;

        protected PaymentsThemeConfig Theme { get { return Mvx.Resolve<IPaymentsThemeConfigService>().ThemeConfig; } }

        protected UIWebView ThreeDSWebView
        {
            get { return WebView; }
        }

		public CardPaymentViewController() : base("CardPaymentViewController", null)
        {

        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(PaymentsConstants.RESX_NAME, "Card_Payment");

            var threeDSService = Mvx.Resolve<IPayment3DSService>();
            threeDSService.SetWebView(WebView);

			SetupAccessoryButton(_accessoryButton ?? (_accessoryButton = new UIButton()));
            SetupCardNumberTextField(CardNumberTextField);
            SetupCardHolderTextField(CardHolderTextField);
            SetupExpirationDateTextField(ExpirationDateTextField);
            SetupCvvTextField(CvvTextField);
            SetupPayButton(PayButton);
		}

        protected virtual void SetupTextField(UITextField textField, AppRopio.Base.iOS.Models.ThemeConfigs.TextField style)
        {
            textField.SetupStyle(style);
			textField.ShouldReturn = (sender) =>
            {
	            sender.EndEditing(true);
	            return true;
            };
            textField.InputAccessoryView = _accessoryButton;
        }

        protected virtual void SetupCardNumberTextField(UITextField cardNumber)
        {
            cardNumber.Placeholder = LocalizationService.GetLocalizableString(PaymentsConstants.RESX_NAME, "CardNumber");

            SetupTextField(cardNumber, Theme.CardPayment.CardNumberTextField);

            cardNumber.ShouldChangeCharacters = (textField, range, replacementString) => 
            {
                return textField.Text.Length - range.Length + replacementString.Length <= MAX_NUMBER_LENGTH;
            };
        }

		protected virtual void SetupCardHolderTextField(UITextField cardHolder)
		{
            cardHolder.Placeholder = LocalizationService.GetLocalizableString(PaymentsConstants.RESX_NAME, "CardHolder");

            SetupTextField(cardHolder, Theme.CardPayment.CardHolderTextField);

            cardHolder.ShouldChangeCharacters = (textField, range, replacementString) =>
			{
                return textField.Text.Length - range.Length + replacementString.Length <= MAX_CARDHOLDER_LENGTH;
			};
		}

		protected virtual void SetupExpirationDateTextField(UITextField expirationDate)
		{
            expirationDate.Placeholder = LocalizationService.GetLocalizableString(PaymentsConstants.RESX_NAME, "ExpirationDate");

            SetupTextField(expirationDate, Theme.CardPayment.ExpirationDateTextField);

            expirationDate.ShouldChangeCharacters = (textField, range, replacementString) =>
			{
                return textField.Text.Length - range.Length + replacementString.Length <= MAX_DATE_LENGTH;
			};
		}

		protected virtual void SetupCvvTextField(UITextField cvv)
		{
            cvv.Placeholder = LocalizationService.GetLocalizableString(PaymentsConstants.RESX_NAME, "Cvv");

            SetupTextField(cvv, Theme.CardPayment.CvvTextField);

            cvv.ShouldChangeCharacters = (textField, range, replacementString) =>
			{
                return textField.Text.Length - range.Length + replacementString.Length <= MAX_CVC_LENGTH;
			};
		}

		protected virtual void SetupPayButton(UIButton button)
        {
            button.WithTitleForAllStates(LocalizationService.GetLocalizableString(PaymentsConstants.RESX_NAME, "Pay"));
            button.SetupStyle(Theme.CardPayment.PayButton);
        }

		protected virtual void SetupAccessoryButton(UIButton button)
		{
			button.SetTitle(LocalizationService.GetLocalizableString("Base", "Done"), UIControlState.Normal);
			button.ChangeFrame(w: DeviceInfo.ScreenWidth, h: 44);
            button.SetupStyle(Theme.CardPayment.AccessoryNextButton);
		}

        protected override void BindControls()
        {
            var bindingSet = this.CreateBindingSet<CardPaymentViewController, ICardPaymentViewModel>();

            BindCardNumber(CardNumberTextField, bindingSet);
            BindExpirationDate(ExpirationDateTextField, bindingSet);
            BindCvv(CvvTextField, bindingSet);
            BindCardHolder(CardHolderTextField, bindingSet);
            BindPayButton(PayButton, bindingSet);
            BindAccessoryButton(_accessoryButton, bindingSet);

            bindingSet.Apply();
        }

		#endregion

		#region BindingControls

        protected virtual void BindCardNumber(UITextField cardNumber, MvxFluentBindingDescriptionSet<CardPaymentViewController, ICardPaymentViewModel> set)
		{
            set.Bind(cardNumber).To(vm => vm.CardNumber);
		}

		protected virtual void BindExpirationDate(UITextField expirationDate, MvxFluentBindingDescriptionSet<CardPaymentViewController, ICardPaymentViewModel> set)
		{
            set.Bind(expirationDate).To(vm => vm.ExpirationDate);
		}

		protected virtual void BindCvv(UITextField cvv, MvxFluentBindingDescriptionSet<CardPaymentViewController, ICardPaymentViewModel> set)
		{
			set.Bind(cvv).To(vm => vm.Cvv);
		}

		protected virtual void BindCardHolder(UITextField cardHolder, MvxFluentBindingDescriptionSet<CardPaymentViewController, ICardPaymentViewModel> set)
		{
            set.Bind(cardHolder).To(vm => vm.CardHolder);
		}

        protected virtual void BindPayButton(UIButton payButton, MvxFluentBindingDescriptionSet<CardPaymentViewController, ICardPaymentViewModel> set)
		{
            set.Bind(payButton).To(vm => vm.PayCommand);
            //set.Bind(payButton).For(v => v.Enabled).To(vm => vm.CanGoNext);
		}

        protected virtual void BindAccessoryButton(UIButton accessoryButton, MvxFluentBindingDescriptionSet<CardPaymentViewController, ICardPaymentViewModel> set)
        {
            accessoryButton.TouchUpInside += AccessoryButton_TouchUpInside;
        }

        #endregion

        protected virtual void AccessoryButton_TouchUpInside(object sender, System.EventArgs e)
        {
            View.EndEditing(true);
        }

        protected override void CleanUp()
        {
            base.CleanUp();

            _accessoryButton.TouchUpInside -= AccessoryButton_TouchUpInside;
        }
    }
}