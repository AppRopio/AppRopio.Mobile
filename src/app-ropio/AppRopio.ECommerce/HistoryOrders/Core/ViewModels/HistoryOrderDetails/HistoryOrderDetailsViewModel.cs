using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.HistoryOrders.Core.Models.Bundle;
using AppRopio.ECommerce.HistoryOrders.Core.Services;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders.Services;
using AppRopio.Models.HistoryOrders.Responses;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders
{
	public class HistoryOrderDetailsViewModel : BaseViewModel, IHistoryOrderDetailsViewModel
    {
        #region Commands

        private IMvxCommand _goToItemsCommand;

        public IMvxCommand GoToItemsCommand
        {
            get
            {
                return _goToItemsCommand ?? (_goToItemsCommand = new MvxCommand(GoToItems));
            }
        }

        private IMvxCommand _repeatOrderCommand;

        public IMvxCommand RepeatOrderCommand
        {
            get
            {
                return _repeatOrderCommand ?? (_repeatOrderCommand = new MvxCommand(RepeatOrder));
            }
        }

        #endregion

        #region Properties

        private HistoryOrderDetails _orderDetails;
        protected HistoryOrderDetails OrderDetails
        {
            get { return _orderDetails; }
            set
            {
                if (SetProperty(ref _orderDetails, value))
                {
                    RaisePropertyChanged(nameof(OrderStatus));
                    RaisePropertyChanged(nameof(DeliveryName));
                    RaisePropertyChanged(nameof(DeliveryPrice));
                    RaisePropertyChanged(nameof(DeliveryPointName));
                    RaisePropertyChanged(nameof(DeliveryPointAddress));
                    RaisePropertyChanged(nameof(PaymentName));
                }
            }
        }

        public string OrderId { get; private set; }

        public string OrderNumber { get; private set; }

        public decimal TotalPrice { get; private set; }

        public int ItemsCount { get; private set; }

        public List<string> OrderStatus => OrderDetails?.OrderStatus;

        public string DeliveryName => OrderDetails?.Delivery?.Name;

        public decimal? DeliveryPrice => OrderDetails?.Delivery?.Price;

        public string DeliveryPointName => OrderDetails?.DeliveryPoint?.Name;

        public string DeliveryPointAddress => OrderDetails?.DeliveryPoint?.Address;

        public string PaymentName => OrderDetails?.Payment?.Name;

        #endregion

        #region Services

        protected IHistoryOrderDetailsVmService VmService { get { return Mvx.IoCProvider.Resolve<IHistoryOrderDetailsVmService>(); } }

        #endregion

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var navigationBundle = parameters.ReadAs<HistoryOrderBundle>();
            this.InitFromBundle(navigationBundle);
        }

        protected virtual void InitFromBundle(HistoryOrderBundle parameters)
        {
            OrderId = parameters.OrderId;
            OrderNumber = parameters.OrderNumber;
            TotalPrice = parameters.TotalPrice;
            ItemsCount = parameters.ItemsCount;

            VmNavigationType = parameters.NavigationType == NavigationType.None ?
                                                            NavigationType.ClearAndPush :
                                                            parameters.NavigationType;
        }

        #endregion

        public override Task Initialize()
        {
            return LoadContent();
        }

        //загрузка деталей
        protected virtual async Task LoadContent()
        {
            Loading = true;

            var dataSource = await VmService.LoadOrderDetails(OrderId);

            InvokeOnMainThread(() => OrderDetails = dataSource);

            Loading = false;
        }

        protected virtual void GoToItems()
        {
            VmService.NavigateToProducts(OrderId);
        }

        protected virtual async void RepeatOrder()
        {
            Loading = true;

            var response = await VmService.RepeatOrder(OrderId);

            Loading = false;

            if (response != null)
            {
                Messenger.Publish(new ProductAddToBasketMessage(this));
                
                if (await UserDialogs.Confirm(response.Message, LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderDetails_ToBasket")))
                {
                    var historyOrdersNavigationVmService = Mvx.IoCProvider.Resolve<IHistoryOrdersNavigationVmService>();
                    historyOrdersNavigationVmService.NavigateToBasket(new BaseBundle(NavigationType.Push));
                }
            }
        }
    }
}