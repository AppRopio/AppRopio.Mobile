using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Messages.Module;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.Basket.Core.Messages.Basket;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Services;
using AppRopio.ECommerce.Loyalty.Abstractions;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Basket
{
    public class BasketViewModel : BaseViewModel, IBasketViewModel
    {
        #region Fields

        private bool _started;

        private int _quantityChangedRequests = 0;

        protected string VersionId { get; set; }

        private MvxSubscriptionToken _quantityChangedToken;
        private MvxSubscriptionToken _itemDeletedToken;
        private MvxSubscriptionToken _needUpdateToken;

        #endregion

        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IBasketItemVM>(OnItemSelected));
            }
        }

        private ICommand _deleteItemCommand;
        public ICommand DeleteItemCommand
        {
            get
            {
                return _deleteItemCommand ?? (_deleteItemCommand = new MvxCommand<IBasketItemVM>(OnDeleteItemExecute));
            }
        }

        private IMvxCommand _nextCommand;
        public IMvxCommand NextCommand
        {
            get
            {
                return _nextCommand ?? (_nextCommand = new MvxCommand(OnNextExecute, () => !AmountLoading));
            }
        }

        private IMvxCommand _catalogCommand;
        public IMvxCommand CatalogCommand => _catalogCommand ?? (_catalogCommand = new MvxCommand(OnCatalogExecute));

        #endregion

        #region Properties

        private bool _isEmpty;
        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetProperty(ref _isEmpty, value, nameof(IsEmpty));
        }

        private bool _amountLoading;
        public bool AmountLoading
        {
            get => _amountLoading;
            set => SetProperty(ref _amountLoading, value, nameof(AmountLoading));
        }

        public ObservableCollection<IBasketItemVM> _items;
        public ObservableCollection<IBasketItemVM> Items
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

        private IMvxViewModel _loyaltyVm;
        public IMvxViewModel LoyaltyVm
        {
            get
            {
                return _loyaltyVm;
            }
            set
            {
                _loyaltyVm = value;
                RaisePropertyChanged(() => LoyaltyVm);
            }
        }

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

        protected IBasketVmService VmService { get { return Mvx.Resolve<IBasketVmService>(); } }
        protected IBasketNavigationVmService NavigationVmService { get { return Mvx.Resolve<IBasketNavigationVmService>(); } }

        #endregion

        #region Constructor

        public BasketViewModel()
        {
            VmNavigationType = NavigationType.ClearAndPush;

            LoyaltyVm = VmService.LoadLoyaltyVmIfExist();
            (LoyaltyVm as ILoyaltyViewModel)?.RegisterUpdateMessage(new NeedUpdateMessage(LoyaltyVm));

            Items = new ObservableCollection<IBasketItemVM>();
        }

        #endregion

        #region Private

        private void UnsubscribeTokens()
        {
            if (_quantityChangedToken != null)
            {
                Messenger.Unsubscribe<ProductQuantityChangedMessage>(_quantityChangedToken);
                _quantityChangedToken = null;
            }

            if (_itemDeletedToken != null)
            {
                Messenger.Unsubscribe<ItemDeletedMessage>(_itemDeletedToken);
                _itemDeletedToken = null;
            }

            if (_needUpdateToken != null)
            {
                Messenger.Unsubscribe<NeedUpdateMessage>(_needUpdateToken);
                _needUpdateToken = null;
            }
        }

        private async void DeleteItem(string id)
        {
            if (Items.IsNullOrEmpty())
                return;

            var item = Items.FirstOrDefault(x => x.ProductId.Equals(id));
            if (item != null)
            {
                CanGoNext = false;
                NextCommand.RaiseCanExecuteChanged();

                InvokeOnMainThread(() => Items.Remove(item));
                await VmService.DeleteItem(id);
                //RecalcAmount();
                Messenger.Publish(new ProductQuantityChangedMessage(this));

                CanGoNext = true;
                NextCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Protected

        protected async Task LoadContent()
        {
            CanGoNext = false;

            Loading = true;

            NextCommand.RaiseCanExecuteChanged();

            VersionId = VmService.LoadedBasket?.VersionId;

            Items = await VmService.LoadItemsIfNeeded(VersionId);

            IsEmpty = Items.IsNullOrEmpty();

            VersionId = VmService.LoadedBasket?.VersionId;

            if (!IsEmpty)
            {
                Items.ForEach(item =>
                {
                    var propertyChangedItem = (item as IMvxNotifyPropertyChanged);
                    if (propertyChangedItem != null)
                    {
                        propertyChangedItem.PropertyChanged += (sender, e) =>
                        {
                            if (e.PropertyName == nameof(item.QuantityLoading))
                            {
                                _quantityChangedRequests = item.QuantityLoading ? _quantityChangedRequests + 1 : _quantityChangedRequests - 1;

                                CanGoNext = _quantityChangedRequests <= 0;
                                NextCommand.RaiseCanExecuteChanged();
                            }
                        };
                    }
                });

                RecalcAmount();
            }
            else
                Amount = 0;

            Loading = false;

            CanGoNext = true;

            NextCommand.RaiseCanExecuteChanged();
        }

        protected virtual void OnItemSelected(IBasketItemVM item)
        {
            NavigationVmService.NavigateToProduct(new ProductCardBundle(item.GroupId, item.ProductId, NavigationType.DoublePush));
        }

        protected virtual async void OnNextExecute()
        {
            if (!CanGoNext || AmountLoading)
                return;

            Loading = true;

            var isValid = await VmService.IsBasketValid(VersionId, OnUnbindCTS.Token);

            Loading = false;

            if (isValid)
                NavigationVmService.NavigateToOrder(new BasketBundle(NavigationType.Push, Amount));

            //TODO: сделать верификацию если корзина не валидна (IsNeedToLoad + LoadIfNeeded)
        }

        protected virtual void OnCatalogExecute()
        {
            NavigationVmService.NavigateToProducts(new BaseBundle(NavigationType.ClearAndPush));
        }

        protected virtual void OnQuantityChanged(ProductQuantityChangedMessage message)
        {
            RecalcAmount();
        }

        protected virtual void OnItemDeleted(ItemDeletedMessage message)
        {
            AnalyticsNotifyingService.NotifyEventIsHandled("basket", "basket_product_deleted", "item_decrement_button", message.Id);

            DeleteItem(message.Id);
        }

        protected virtual void OnDeleteItemExecute(IBasketItemVM item)
        {
            AnalyticsNotifyingService.NotifyEventIsHandled("basket", "basket_product_deleted", "item_delete_button", item.ProductId);

            DeleteItem(item.ProductId);
        }

        protected virtual async void RecalcAmount()
        {
            if (Items == null || Items.Count == 0)
            {
                InvokeOnMainThread(() =>
                {
                    IsEmpty = true;
                    Amount = 0;
                });
                Messenger.Publish(new ModulesInteractionMessage<int>(this, 0));
                return;
            }

            InvokeOnMainThread(() =>
            {
                AmountLoading = true;
                NextCommand.RaiseCanExecuteChanged();
            });

            //amount calculate on server side
            try
            {
                var amount = await VmService.LoadBasketSummaryAmount();
                InvokeOnMainThread(() =>
                {
                    Amount = amount;
                    AmountLoading = false;
                    NextCommand.RaiseCanExecuteChanged();
                });

                //TODO: сделать верификацию если корзина не валидна (IsNeedToLoad + LoadIfNeeded)
            }
            catch (OperationCanceledException)
            {
                //nothing
            }

        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            LoyaltyVm?.Initialize();

            _quantityChangedToken = Messenger.Subscribe<ProductQuantityChangedMessage>(OnQuantityChanged);
            _itemDeletedToken = Messenger.Subscribe<ItemDeletedMessage>(OnItemDeleted);
            _needUpdateToken = Messenger.Subscribe<NeedUpdateMessage>((m) => Task.Run(LoadContent));

            return LoadContent();
        }

        public override void ViewAppeared()
        {
            if (_started)
                Task.Run(LoadContent);

            _started = true;

            base.ViewAppeared();
        }

        public override void Unbind()
        {
            base.Unbind();

            UnsubscribeTokens();
        }

        #endregion
    }
}
