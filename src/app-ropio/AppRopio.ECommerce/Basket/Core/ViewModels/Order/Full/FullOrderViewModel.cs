using System;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.Basket.Core.Messages.Order;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services;
using AppRopio.Payments.Core.Messages;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Full
{
    public class FullOrderViewModel : BaseViewModel, IFullOrderViewModel
    {
        #region Fields

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
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<object>(OnItemSelected));
            }
        }

        private IMvxCommand _nextCommand;
        public IMvxCommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new MvxAsyncCommand(OnNextExecute, () => !Loading));
            }
        }

        #endregion

        #region Properties

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

        public IUserViewModel UserViewModel { get; private set; }

        public IDeliveryViewModel DeliveryViewModel { get; private set; }

        private MvxObservableCollection<IMvxViewModel> _items;
        public MvxObservableCollection<IMvxViewModel> Items
        {
            get
            {
                return _items;
            }
            set
            {
                var selectedItem = value?.FirstOrDefault(x => x is IDeliveryTypeItemVM delivery && delivery.IsSelected) as IDeliveryTypeItemVM;

                _items = value;
                RaisePropertyChanged(() => Items);
            }
        }

        #endregion

        #region Services

        protected IOrderVmService OrderVmService => Mvx.IoCProvider.Resolve<IOrderVmService>();

        protected IDeliveryVmService DeliveryVmService => Mvx.IoCProvider.Resolve<IDeliveryVmService>();

        protected IUserVmService UserVmService => Mvx.IoCProvider.Resolve<IUserVmService>();

        protected new IBasketNavigationVmService NavigationVmService => Mvx.IoCProvider.Resolve<IBasketNavigationVmService>();

        decimal IOrderViewModel.BasketAmount => DeliveryViewModel.BasketAmount;

        decimal IOrderViewModel.Amount => DeliveryViewModel.Amount;

        #endregion

        #region Constructor

        public FullOrderViewModel()
        {
            VmNavigationType = NavigationType.Push;

            UserViewModel = (IUserViewModel)Activator.CreateInstance(LookupService.Resolve<IUserViewModel>());
            DeliveryViewModel = (IDeliveryViewModel)Activator.CreateInstance(LookupService.Resolve<IDeliveryViewModel>());
        }

        #endregion

        #region Private

        private void UnsubscribeTokens()
        {
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

                await NavigationVmService.ChangePresentation(new Base.Core.PresentationHints.NavigateToDefaultViewModelHint());

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

        private void DeliveryViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(DeliveryViewModel.Loading))
                Loading = DeliveryViewModel.Loading;
        }

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var basketBundle = parameters.ReadAs<BasketBundle>();

            this.InitFromBundle(basketBundle);

            parameters.Data["NavigationType"] = ((int)NavigationType.InsideScreen).ToString();

            UserViewModel.Prepare(parameters);
            DeliveryViewModel.Prepare(parameters);
        }

        protected virtual void InitFromBundle(BasketBundle basketBundle)
        {

        }

        #endregion

        protected async Task LoadContent()
        {
            CanGoNext = false;

            Loading = true;

            var userTask = UserViewModel.LoadContent();
            var deliveryTask = DeliveryViewModel.LoadContent();

            await Task.WhenAll(userTask, deliveryTask);

            var dataSource = new MvxObservableCollection<IMvxViewModel>();
            dataSource.AddRange(UserViewModel.Items);
            dataSource.AddRange(DeliveryViewModel.Items);
            Items = dataSource;

            CanGoNext = true;

            Loading = false;

            DeliveryViewModel.PropertyChanged += DeliveryViewModel_PropertyChanged;
        }

        protected virtual async Task OnNextExecute()
        {
            if (!CanGoNext)
                return;

            CanGoNext = false;

            if (!await UserViewModel.ValidateAndSaveInput(Items.Where(x => x is IOrderFieldsGroupVM).Select(x => x as IOrderFieldsGroupVM)) ||
                !await DeliveryViewModel.ValidateDelivery(Items.Where(x => x is IDeliveryTypeItemVM).Select(x => x as IDeliveryTypeItemVM)))
            {
                CanGoNext = true;
                return;
            }

            Loading = true;

            NextCommand.RaiseCanExecuteChanged();

            if (_orderProcessingToken == null)
                _orderProcessingToken = Messenger.Subscribe<OrderProcessingMessage>(DeliveryViewModel.OrderProcessingChanged);

            if (_orderPaidToken == null)
                _orderPaidToken = Messenger.Subscribe<OrderPaidMessage>(DeliveryViewModel.OnOrderPaidMessageRecieved);

            if (_paymentSelectedToken == null)
                _paymentSelectedToken = Messenger.Subscribe<PaymentSelectedMessage>(DeliveryViewModel.OnPaymentSelectedMessage);
            
            var selectedDelivery = Items?.FirstOrDefault(x => x is IDeliveryTypeItemVM deliveryItem && deliveryItem.IsSelected) as IDeliveryTypeItemVM;

            var isNeedToSelectPayment = await OrderVmService.IsPaymentNecessary(selectedDelivery?.Id);
            if (isNeedToSelectPayment)
                NavigationVmService.NavigateToPayment(new PaymentBundle(selectedDelivery?.Id, NavigationType.PresentModal));
            else
                await DeliveryViewModel.CreateOrder();

            CanGoNext = true;

            Loading = false;
        }

        protected virtual void OnItemSelected(object item)
        {
            if (item is IOrderFieldItemVM orderItem)
                UserViewModel.OnUserItemSelected(orderItem);
            else if (item is IDeliveryTypeItemVM deliveryItem)
                DeliveryViewModel.OnDeliveryChanged(deliveryItem);
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        public override void Unbind()
        {
            base.Unbind();

            UserViewModel.Unbind();
            DeliveryViewModel.Unbind();

            UnsubscribeTokens();

            DeliveryViewModel.PropertyChanged -= DeliveryViewModel_PropertyChanged;
        }

        #endregion
    }
}
