using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.API.Services;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.Basket.Core.Messages.Order;
using AppRopio.ECommerce.Basket.Core.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.CartIndicator
{
    public class BasketCartIndicatorViewModel : BaseViewModel, IBasketCartIndicatorViewModel
    {
        #region Fields

        private MvxSubscriptionToken _addedToken;
        private MvxSubscriptionToken _quantityToken;
        private MvxSubscriptionToken _orderFinishedToken;

        protected const string ADDED_TAG = "ADDED_TAG";
        protected const string QUANTITY_TAG = "QUANTITY_TAG";
        protected const string ORDER_TAG = "ORDER_TAG";

        #endregion

        #region Commands

        private IMvxCommand _basketCommand;
        public IMvxCommand BasketCommand => _basketCommand ?? (_basketCommand = new MvxCommand(OnBasketExecute));

        #endregion

        #region Properties

        protected bool IsStarted { get; set; }

        private string _quantity;
        public string Quantity
        {
            get => _quantity;
            set => SetProperty(ref _quantity, value, nameof(Quantity));
        }

        #endregion

        #region Services

        private IBasketService _apiService;
        public IBasketService ApiService => _apiService ?? (_apiService = Mvx.IoCProvider.Resolve<IBasketService>());

        private IBasketNavigationVmService _navigationVmService;
        public new IBasketNavigationVmService NavigationVmService => _navigationVmService ?? (_navigationVmService = Mvx.IoCProvider.Resolve<IBasketNavigationVmService>());

        #endregion

        #region Private

        private void InitSubscriptionTokens()
        {
            _addedToken = Messenger.Subscribe<ProductAddToBasketMessage>(OnQuantityChanged, tag: ADDED_TAG);
            _quantityToken = Messenger.Subscribe<ProductQuantityChangedMessage>(OnQuantityChanged, tag: QUANTITY_TAG);
            _orderFinishedToken = Messenger.Subscribe<OrderCreationFinishedMessage>(OnQuantityChanged, tag: ORDER_TAG);

            Mvx.IoCProvider.CallbackWhenRegistered<IBasketService>(() => OnQuantityChanged(null));
        }

        private void ReleaseSubscriptionTokens()
        {
            if (_addedToken != null)
            {
                Messenger.Unsubscribe<ProductAddToBasketMessage>(_addedToken); 
                _addedToken.Dispose();
                _addedToken = null;
            }

            if (_quantityToken != null)
            {
                Messenger.Unsubscribe<ProductQuantityChangedMessage>(_addedToken);
                _quantityToken.Dispose();
                _quantityToken = null;
            }

            if (_orderFinishedToken != null)
            {
                Messenger.Unsubscribe<OrderCreationFinishedMessage>(_orderFinishedToken);
                _orderFinishedToken.Dispose();
                _orderFinishedToken = null;
            }
        }

        private void OnQuantityChanged(MvxMessage msg)
        {
            Task.Run(async () =>
            {
                try
                {
                    var quantity = await ApiService.GetQuantity();
                    Quantity = quantity == 0 ? null : quantity.ToString();
                }
                catch (Exception ex)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Warn($"{this.GetType().FullName}: {ex.BuildAllMessagesAndStackTrace()}");
                }
            });
        }

        #endregion

        #region Protected

        protected virtual void OnBasketExecute()
        {
            NavigationVmService.NavigateToBasket(new BaseBundle(Base.Core.Models.Navigation.NavigationType.ClearAndPush));
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return Task.Run(() =>
            {
                if (!IsStarted)
                {
                    IsStarted = true;

                    InitSubscriptionTokens();
                }
            });
        }

        public override void Unbind()
        {
            IsStarted = false;

            ReleaseSubscriptionTokens();
        }

        #endregion
    }
}
