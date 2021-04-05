using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Messages.Order;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery
{
    public abstract class BaseDeliveryVM : BaseViewModel, IBaseDeliveryVM
    {
        #region Fields

        #endregion

        #region Commands

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = new MvxAsyncCommand(OnCloseExecute));
            }
        }

        private IMvxCommand _nextCommand;
        public IMvxCommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new MvxAsyncCommand(OnNextExecute));
            }
        }

        #endregion

        #region Properties

        public string DeliveryId { get; protected set; }

        public decimal BasketAmount { get; protected set; }

        private decimal _amount;
        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                RaisePropertyChanged(() => Amount);
            }
        }

        private decimal? _deliveryPrice;
        public decimal? DeliveryPrice
        {
            get
            {
                return _deliveryPrice;
            }
            set
            {
                _deliveryPrice = value;
                RaisePropertyChanged(() => DeliveryPrice);
            }
        }

        private bool _canGoNext;
        public bool CanGoNext
        {
            get
            {
                return _canGoNext;
            }
            set
            {
                _canGoNext = value;
                RaisePropertyChanged(() => CanGoNext);
            }
        }

        #endregion

        #region Services

        protected IDeliveryVmService DeliveryVmService { get { return Mvx.IoCProvider.Resolve<IDeliveryVmService>(); } }
        protected new IBasketNavigationVmService NavigationVmService { get { return Mvx.IoCProvider.Resolve<IBasketNavigationVmService>(); } }

        #endregion

        #region Constructor

        public BaseDeliveryVM()
        {
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var deliveryBundle = parameters.ReadAs<DeliveryBundle>();
            this.InitFromBundle(deliveryBundle);
        }

        protected virtual void InitFromBundle(DeliveryBundle deliveryBundle)
        {
            DeliveryId = deliveryBundle.Id;
            BasketAmount = deliveryBundle.BasketAmount;
            Amount = deliveryBundle.BasketAmount;
            VmNavigationType = deliveryBundle.NavigationType;
        }

        protected abstract Task LoadContent();

        protected abstract Task<bool> ValidateAndSaveInput();

        protected virtual async Task OnNextExecute()
        {
            if (!await ValidateAndSaveInput())
                return;
                
            var oldDeliveyPrice = DeliveryPrice;
            DeliveryPrice = await DeliveryVmService.LoadDeliveryPrice(DeliveryId);

            Mvx.IoCProvider.Resolve<IMvxMessenger>().Publish(new DeliveryConfirmedMessage(this)
            {
                DeliveryId = DeliveryId,
                DeliveryPrice = DeliveryPrice
            });

            if (DeliveryPrice.HasValue && oldDeliveyPrice != DeliveryPrice)
            {
                if (VmNavigationType == NavigationType.PresentModal)
                    await UserDialogs.Alert($"{LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_DeliveryPriceAlert")} {DeliveryPrice.Value.ToString($"# ### ##0.## {AppSettings.SettingsCulture.NumberFormat.CurrencySymbol}").Trim()}");
                else
                    await UserDialogs.Confirm($"{LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_DeliveryPriceAlert")} {DeliveryPrice.Value.ToString($"# ### ##0.## {AppSettings.SettingsCulture.NumberFormat.CurrencySymbol}").Trim()}", LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_AlertOk"));
            }

            if (VmNavigationType == NavigationType.PresentModal)
                await NavigationVmService.Close(this);
            else
                NavigationVmService.NavigateToPayment(new PaymentBundle(DeliveryId, NavigationType.PresentModal));
        }

        protected virtual async Task OnCloseExecute()
        {
            await NavigationVmService.Close(this);
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        #endregion
    }
}
