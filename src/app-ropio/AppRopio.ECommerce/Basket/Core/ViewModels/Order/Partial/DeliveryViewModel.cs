using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.Basket.Core.Messages.Order;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services;
using AppRopio.Payments.Core.Bundle;
using AppRopio.Payments.Core.Messages;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using PaymentModel = AppRopio.Models.Basket.Responses.Order.Payment;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial
{
    public class DeliveryViewModel : BaseViewModel, IDeliveryViewModel
    {
        #region Fields

        private MvxSubscriptionToken _deliveryConfirmedToken;

        private MvxSubscriptionToken _orderProcessingToken;
        private MvxSubscriptionToken _orderPaidToken;
        private MvxSubscriptionToken _paymentSelectedToken;

        #endregion

        #region Commands

        private IMvxCommand _selectionChangedCommand;
        public IMvxCommand SelectionChangedCommand
		{
			get
			{
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IDeliveryTypeItemVM>(OnDeliveryChanged));
			}
		}

        private ICommand _applyDeliveryTimeCommand;
        public ICommand ApplyDeliveryTimeCommand
        {
            get
            {
                return _applyDeliveryTimeCommand ?? (_applyDeliveryTimeCommand = new MvxCommand<IDeliveryTimeItemVM>(OnDeliveryTimeApply));
            }
        }

        private IMvxCommand _nextCommand;
        public IMvxCommand NextCommand => _nextCommand ?? (_nextCommand = new MvxCommand(OnNextExecute));

        #endregion

        #region Properties

        private MvxObservableCollection<IDeliveryTypeItemVM> _items;
        public MvxObservableCollection<IDeliveryTypeItemVM> Items
		{
			get
			{
				return _items;
			}
			set
			{
				_items = value;
				RaisePropertyChanged(() => Items);
			}
		}

        private MvxObservableCollection<IDeliveryDayItemVM> _daysItems;
        public MvxObservableCollection<IDeliveryDayItemVM> DaysItems
        {
            get
            {
                return _daysItems;
            }
            set
            {
                if (value == null || value.Count == 0)
                    IsShowDeliveryTimePicker = false;
                
                _daysItems = value;
                RaisePropertyChanged(() => DaysItems);
            }
        }

        private MvxObservableCollection<IDeliveryTimeItemVM> _timeItems;
        public MvxObservableCollection<IDeliveryTimeItemVM> TimeItems
        {
            get => _timeItems;
            set => SetProperty(ref _timeItems, value, nameof(TimeItems));
        }

        private bool _isShowDeliveryTimePicker;
        public bool IsShowDeliveryTimePicker
        {
            get
            {
                return _isShowDeliveryTimePicker;
            }
            set
            {
                _isShowDeliveryTimePicker = value;
                RaisePropertyChanged(() => IsShowDeliveryTimePicker);
            }
        }

        private IDeliveryDayItemVM _selectedDeliveryDay;
        public IDeliveryDayItemVM SelectedDeliveryDay
        {
            get => _selectedDeliveryDay;
            set 
            {
                SetProperty(ref _selectedDeliveryDay, value, nameof(SelectedDeliveryDay));
                TimeItems = new MvxObservableCollection<IDeliveryTimeItemVM>(value.Times);
            }
        }

        private IDeliveryTimeItemVM _selectedDeliveryTime;
        public IDeliveryTimeItemVM SelectedDeliveryTime
        {
            get
            {
                return _selectedDeliveryTime;
            }
            set
            {
                _selectedDeliveryTime = value;
                RaisePropertyChanged(() => SelectedDeliveryTime);

                var deliveryDay = DaysItems?.FirstOrDefault(x => x.Times.Contains(value));
                if (value == null || deliveryDay == null)
                    SelectedDeliveryTimeValue = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "Order_DeliveryTimeAndDay");
                else
                    SelectedDeliveryTimeValue = $"{deliveryDay.Name}, {value.Name}";
            }
        }

        private string _selectedDeliveryTimeValue;
        public string SelectedDeliveryTimeValue
        {
            get
            {
                return _selectedDeliveryTimeValue;
            }
            set
            {
                _selectedDeliveryTimeValue = value;
                RaisePropertyChanged(() => SelectedDeliveryTimeValue);
            }
        }

        private bool _deliveryTimeLoading;
        public bool DeliveryTimeLoading
        {
            get
            {
                return _deliveryTimeLoading;
            }
            set
            {
                _deliveryTimeLoading = value;
                RaisePropertyChanged(() => DeliveryTimeLoading);
            }
        }

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

        protected IOrderVmService OrderVmService { get { return Mvx.Resolve<IOrderVmService>(); } }
        protected IDeliveryVmService DeliveryVmService { get { return Mvx.Resolve<IDeliveryVmService>(); } }
        protected IBasketNavigationVmService NavigationVmService { get { return Mvx.Resolve<IBasketNavigationVmService>(); } }

        #endregion

        #region Constructor

        public DeliveryViewModel()
        {
            VmNavigationType = NavigationType.Push;
            DaysItems = new MvxObservableCollection<IDeliveryDayItemVM>();
            TimeItems = new MvxObservableCollection<IDeliveryTimeItemVM>();
        }

        #endregion

        #region Private

        private void UnsubscribeTokens()
        {
            if (_deliveryConfirmedToken != null)
            {
                Messenger.Unsubscribe<DeliveryConfirmedMessage>(_deliveryConfirmedToken);
                _deliveryConfirmedToken = null;
            }

            if (_orderProcessingToken != null)
            {
                Messenger.Unsubscribe<OrderProcessingMessage>(_orderProcessingToken);
                _orderProcessingToken = null;
            }

            if (_orderPaidToken != null)
            {
                Messenger.Unsubscribe<OrderPaidMessage>(_orderPaidToken);
                _orderPaidToken = null;
            }

            if (_paymentSelectedToken != null)
            {
                Messenger.Unsubscribe<PaymentSelectedMessage>(_paymentSelectedToken);
                _paymentSelectedToken = null;
            }
        }

        private async Task FinishOrderingProcess(string orderID, bool isPaid)
        {
            if (await OrderVmService.ConfirmOrder(orderID, isPaid))
            {
                SendAnalyticsData(orderID);

                ChangePresentation(new Base.Core.PresentationHints.NavigateToDefaultViewModelHint());

                NavigationVmService.NavigateToThanks(new ThanksBundle(orderID, NavigationType.PresentModal));

                Messenger.Publish(new OrderCreationFinishedMessage(this, true));
            }
        }

        private void SendAnalyticsData(string orderID)
        {
            Task.Run(async () =>
            {
                AnalyticsNotifyingService.NotifyEventIsHandled("order", "order_creation_finished", orderID);

                var confirmedOrderInfo = await OrderVmService.OrderInfo(orderID);

                AnalyticsNotifyingService.NotifyOrderPurchased(confirmedOrderInfo?.Price ?? 0, confirmedOrderInfo?.Quantity ?? 0, orderID, confirmedOrderInfo?.Currency, null);
            });
        }

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var basketBundle = parameters.ReadAs<BasketBundle>();
            this.InitFromBundle(basketBundle);
        }

        protected virtual void InitFromBundle(BasketBundle basketBundle)
        {
            BasketAmount = basketBundle.BasketAmount;
            Amount = basketBundle.BasketAmount;
        }

        #endregion

        protected virtual void OnDeliveryConfirmed(DeliveryConfirmedMessage message)
        {
            DeliveryPrice = message.DeliveryPrice;
            Amount = BasketAmount + (DeliveryPrice ?? 0);

            Items.ForEach(x =>
            {
                if (x is IDeliveryTypeItemVM delivery)
                    delivery.IsSelected = false;
            });

            var selectedItem = Items.FirstOrDefault(x => x is IDeliveryTypeItemVM delivery && delivery.Id.Equals(message.DeliveryId)) as IDeliveryTypeItemVM;
            selectedItem.IsSelected = true;

            SelectedDeliveryTime = null;
            IsShowDeliveryTimePicker = selectedItem.IsDeliveryTimeRequired;

            if (IsShowDeliveryTimePicker)
                LoadDeliveryTime(selectedItem.Id);
            else
                CanGoNext = true;
        }

        protected virtual async Task LoadDeliveryTime(string deliveryId)
        {
            CanGoNext = false;

            DeliveryTimeLoading = true;

            DaysItems = await DeliveryVmService.LoadDeliveryTime(deliveryId);

            DeliveryTimeLoading = false;

            CanGoNext = true;
        }

        protected virtual void OnDeliveryTimeApply(IDeliveryTimeItemVM deliveryTime)
        {
            if (deliveryTime == null)
                return;

            SelectedDeliveryTime = deliveryTime;

            Task.Run(async () =>
            {
                if (!await DeliveryVmService.ConfirmDeliveryTime(deliveryTime.Id))
                {
                    var selectedItem = Items.First(x => x is IDeliveryTypeItemVM delivery && delivery.IsSelected) as IDeliveryTypeItemVM;
                    LoadDeliveryTime(selectedItem.Id);
                    return;
                }

                InvokeOnMainThread(() =>
                {
                    SelectedDeliveryTime = deliveryTime;
                    CanGoNext = true;
                });
            });
        }

        protected async void OnNextExecute()
        {
            if (!Items.IsNullOrEmpty() && !Items.Any(x => x is IDeliveryTypeItemVM deliveryItem && deliveryItem.IsSelected))
            {
                CanGoNext = true;
                UserDialogs.Error("Не выбран способ доставки");
                return;
            }

            if (IsShowDeliveryTimePicker && !DaysItems.IsNullOrEmpty() && SelectedDeliveryTime == null)
            {
                CanGoNext = true;
                UserDialogs.Error("Не выбрано время доставки");
                return;
            }

            Loading = true;

            NextCommand.RaiseCanExecuteChanged();

            if (_orderProcessingToken == null)
                _orderProcessingToken = Messenger.Subscribe<OrderProcessingMessage>(OrderProcessingChanged);

            if (_orderPaidToken == null)
                _orderPaidToken = Messenger.Subscribe<OrderPaidMessage>(OnOrderPaidMessageRecieved);

            if (_paymentSelectedToken == null)
                _paymentSelectedToken = Messenger.Subscribe<PaymentSelectedMessage>(OnPaymentSelectedMessage);
            
            var selectedDelivery = Items?.FirstOrDefault(x => x is IDeliveryTypeItemVM deliveryItem && deliveryItem.IsSelected) as IDeliveryTypeItemVM;

            var isNeedToSelectPayment = await OrderVmService.IsPaymentNecessary(selectedDelivery?.Id);
            if (isNeedToSelectPayment)
                NavigationVmService.NavigateToPayment(new PaymentBundle(selectedDelivery?.Id, NavigationType.PresentModal));
            else
                await CreateOrder();

            CanGoNext = true;

            Loading = false;
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        public virtual async Task LoadContent()
        {
            CanGoNext = false;

            Loading = true;

            Items = await DeliveryVmService.LoadDeliveryTypes();

            Amount = BasketAmount + (DeliveryPrice ?? 0);

            if (!Items.IsNullOrEmpty())
            {
                var selectedDelivery = Items.FirstOrDefault(x => x.IsSelected);
                if (selectedDelivery != null && selectedDelivery.IsDeliveryTimeRequired)
                {
                    SelectedDeliveryTime = null;
                    IsShowDeliveryTimePicker = true;
                    await LoadDeliveryTime(selectedDelivery.Id);
                }
            }

            CanGoNext = true;

            Loading = false;
        }

        public virtual void OnDeliveryChanged(IDeliveryTypeItemVM deliveryItem)
        {
            if (!deliveryItem.Message.IsNullOrEmpty())
                UserDialogs.Alert(deliveryItem.Message);

            if (deliveryItem.NotAvailable)
                return;

            if (_deliveryConfirmedToken == null)
                _deliveryConfirmedToken = Messenger.Subscribe<DeliveryConfirmedMessage>(OnDeliveryConfirmed);

            if (deliveryItem.IsRequiredDataEntry)
                NavigationVmService.NavigateToDelivery(new DeliveryBundle(deliveryItem.Id, deliveryItem.Type, BasketAmount, NavigationType.PresentModal));
            else
            {
                Task.Run(async () =>
                {
                    Loading = true;

                    if (await DeliveryVmService.ValidateAndSaveDelivery(deliveryItem.Id))
                        OnDeliveryConfirmed(new DeliveryConfirmedMessage(this) { DeliveryId = deliveryItem.Id, DeliveryPrice = deliveryItem.Price });

                    Loading = false;
                });
            }
        }

        public virtual Task<bool> ValidateDelivery(IEnumerable<IDeliveryTypeItemVM> deliveries)
        {
            if (!deliveries.IsNullOrEmpty() && !deliveries.Any(x => x.IsSelected))
            {
                CanGoNext = true;
                UserDialogs.Error("Не выбран способ доставки");
                return Task.FromResult(false);
            }

            if (IsShowDeliveryTimePicker && !DaysItems.IsNullOrEmpty() && SelectedDeliveryTime == null)
            {
                CanGoNext = true;
                UserDialogs.Error("Не выбрано время доставки");
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }

        public virtual void OrderProcessingChanged(OrderProcessingMessage msg)
        {
            InvokeOnMainThread(() =>
            {
                Loading = msg.OrderingInProcess;
                NextCommand.RaiseCanExecuteChanged();
            });
        }

        public virtual async void OnOrderPaidMessageRecieved(OrderPaidMessage msg)
        {
            await FinishOrderingProcess(msg.OrderId, true);
        }

        public virtual void OnPaymentSelectedMessage(PaymentSelectedMessage msg)
        {
            OrderProcessingChanged(new OrderProcessingMessage(this, true));

            AnalyticsNotifyingService.NotifyEventIsHandled("order", "order_creation_started");

            AnalyticsNotifyingService.NotifyEventIsHandled("payment", "payment_item_selected", msg.Payment.Name);

            Task.Run(async () =>
            {
                var confirmed = await OrderVmService.ConfirmPayment(msg.Payment.Id);

                if (!confirmed)
                    UserDialogs.Error("К сожалению выбранный способ оплаты недоступен, пожалуйста выберите другой");
                else
                    CreateOrder(msg.Payment);
            });
        }

        public virtual async Task CreateOrder(PaymentModel payment = null)
        {
            var order = await OrderVmService.CreateOrder();

            if (order == null)
            {
                OrderProcessingChanged(new OrderProcessingMessage(this, false));

                UserDialogs.Error("Не удалось оформить заказ, повторите попытку позже");

                AnalyticsNotifyingService.NotifyEventIsHandled("order", "order_creation_failed");

                return;
            }

            if (payment?.InAppPurchase ?? false)
            {
                OrderProcessingChanged(new OrderProcessingMessage(this, false));
                NavigationVmService.NavigateToInAppPayment(new PaymentOrderBundle(order.Id, payment.Type, NavigationType.Push));
            }
            else
            {
                await FinishOrderingProcess(order.Id, false);
                OrderProcessingChanged(new OrderProcessingMessage(this, false));
            }
        }

        public override void Unbind()
        {
            base.Unbind();

            UnsubscribeTokens();
        }

        #endregion
    }
}
