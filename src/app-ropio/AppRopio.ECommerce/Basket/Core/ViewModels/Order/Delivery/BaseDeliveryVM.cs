using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Messages.Order;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using AppRopio.Base.Core;

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
                return _closeCommand ?? (_closeCommand = new MvxCommand(() => Close(this)));
            }
        }

        private IMvxCommand _nextCommand;
        public IMvxCommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new MvxCommand(OnNext));
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

        protected IDeliveryVmService DeliveryVmService { get { return Mvx.Resolve<IDeliveryVmService>(); } }
        protected IBasketNavigationVmService NavigationVmService { get { return Mvx.Resolve<IBasketNavigationVmService>(); } }

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

        protected virtual void OnNext()
        {
            Task.Run(async () =>
            {
                if (!await ValidateAndSaveInput())
                    return;
                
                var oldDeliveyPrice = DeliveryPrice;
                DeliveryPrice = await DeliveryVmService.LoadDeliveryPrice(DeliveryId);

                Mvx.Resolve<IMvxMessenger>().Publish(new DeliveryConfirmedMessage(this)
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
                    Close(this);
                else
                    NavigationVmService.NavigateToPayment(new PaymentBundle(DeliveryId, NavigationType.PresentModal));
            });
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
