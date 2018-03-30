using System;
using System.Globalization;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard.Services;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard
{
    public class BasketProductCardViewModel : BaseViewModel, IBasketProductCardViewModel
    {
        #region Fields

        private MvxSubscriptionToken _reloadingToken;

        #endregion

        #region Commands

        private IMvxCommand _buyCommand;
        public IMvxCommand BuyCommand => _buyCommand ?? (_buyCommand = new MvxCommand(OnBuyCommandExecute, () => !BasketLoading && !ReloadingByParameter && !QuantityLoading));

        private IMvxCommand _incrementCommand;
        public IMvxCommand IncrementCommand => _incrementCommand ?? (_incrementCommand = new MvxCommand(OnIncrementExecute, () => !BasketLoading && !ReloadingByParameter && !QuantityLoading));

        private IMvxCommand _decrementCommand;
        public IMvxCommand DecrementCommand => _decrementCommand ?? (_decrementCommand = new MvxCommand(OnDecrementExecute, () => !BasketLoading && !ReloadingByParameter && !QuantityLoading));

        private IMvxCommand _quantityChangedCommand;
        public IMvxCommand QuantityChangedCommand => _quantityChangedCommand ?? (_quantityChangedCommand = new MvxCommand(OnQuantityChangedExecute, () => !BasketLoading && !ReloadingByParameter && !QuantityLoading));

        #endregion

        #region Properties

        protected Product Model { get; private set; }

        protected bool ReloadingByParameter { get; set; }

        private bool _basketLoading;
        public bool BasketLoading
        {
            get => _basketLoading;
            set => SetProperty(ref _basketLoading, value, nameof(BasketLoading));
        }

        private bool _basketVisible;
        public bool BasketVisible
        {
            get => _basketVisible;
            set => SetProperty(ref _basketVisible, value, nameof(BasketVisible));
        }

        private bool _unitStepVisible;
        public bool UnitStepVisible
        {
            get => _unitStepVisible;
            set => SetProperty(ref _unitStepVisible, value, nameof(UnitStepVisible));
        }

        private string _unitName;
        public string UnitName
        {
            get => _unitName;
            set => SetProperty(ref _unitName, value, nameof(UnitName));
        }

        private string _quantityString;
        public string QuantityString
        {
            get => _quantityString;
            set
            {
                _quantityString = value.Replace(" ", "");
                RaisePropertyChanged(() => QuantityString);
            }
        }

        private float _quantity;
        public float Quantity
        {
            get => _quantity;
            set
            {
                if (Math.Abs(value - _quantity) > 0)
                {
                    _quantity = value;

                    SetQuantityString();
                }
            }
        }

        private bool _quantityLoading;
        public bool QuantityLoading
        {
            get => _quantityLoading;
            set => SetProperty(ref _quantityLoading, value, nameof(QuantityLoading));
        }

        #endregion

        #region Services

        protected IProductQuantityVmService ProductQuantityVmService => Mvx.Resolve<IProductQuantityVmService>();

        protected IProductDeleteVmService ProductDeleteVmService => Mvx.Resolve<IProductDeleteVmService>();

        protected IBasketProductCardVmService VmService => Mvx.Resolve<IBasketProductCardVmService>();

        protected IBasketNavigationVmService NavigationVmService => Mvx.Resolve<IBasketNavigationVmService>();

        #endregion

        #region Constructor

        public BasketProductCardViewModel()
        {
            BasketVisible = true;
        }

        #endregion

        #region Private

        private async Task InitProperties()
        {
            BasketVisible = (Model.State == null || Model.State.Type == ProductStateType.InStock);
            UnitName = Model.UnitName;

            if (!Model.Id.IsNullOrEmpty())
            {
                QuantityLoading = true;

                RaiseCommandsCanExecuteChanged();

                var basketProductQuantity = await VmService.BasketProductQuantity(Model.Id);
                if (basketProductQuantity != null && basketProductQuantity.HasValue)
                {
                    BasketVisible = false;

                    UnitStepVisible = true;

                    Quantity = basketProductQuantity.Value;

                    SetQuantityString();
                }
                else
                {
                    BasketVisible = true;

                    UnitStepVisible = false;

                    Quantity = default(float);
                }

                QuantityLoading = false;

                RaiseCommandsCanExecuteChanged();
            }
        }

        private void RaiseCommandsCanExecuteChanged()
        {
            BuyCommand.RaiseCanExecuteChanged();

            IncrementCommand.RaiseCanExecuteChanged();
            DecrementCommand.RaiseCanExecuteChanged();
            QuantityChangedCommand.RaiseCanExecuteChanged();
        }

        private void SetQuantityString()
        {
            _quantityString = _quantity.ToString("# ### ##0.###").Trim();
            RaisePropertyChanged(() => QuantityString);
        }

        private async Task OnQuantityChanged()
        {
            try
            {
                var result = await ProductQuantityVmService.ChangeQuantityTo(Model.Id, Quantity);

                Quantity = result.Quantity;

                Messenger.Publish(new ProductQuantityChangedMessage(this));
            }
            catch (OperationCanceledException)
            {
                //nothing
            }
        }

        private async Task DeleteProductFromBasket()
        {
            if (Model == null)
                return;
            
            BasketLoading = true;

            RaiseCommandsCanExecuteChanged();

            var result = await ProductDeleteVmService.DeleteProduct(Model.Id);
            if (result)
            {
                BasketVisible = true;

                Quantity = default(float);

                UnitStepVisible = false;

                Messenger.Publish(new ProductQuantityChangedMessage(this));

                AnalyticsNotifyingService.NotifyEventIsHandled("catalog", "catalog_product_card_deleted_decrement_button", Model.Id);
            }

            BasketLoading = false;

            RaiseCommandsCanExecuteChanged();
        }

        #endregion

        #region Protected

        #region Init

        public override void Prepare(MvvmCross.Core.ViewModels.IMvxBundle parameter)
        {
            var productCardBundle = parameter.ReadAs<ProductCardBundle>();
            this.InitFromBundle(productCardBundle);
        }

        protected virtual void InitFromBundle(ProductCardBundle bundle)
        {
            if (Model == null || (Model != null && bundle.Product != null && (Model.GroupId != bundle.Product.GroupId || Model.Id != bundle.Product.Id)))
            {
                Model = bundle.Product;

                Task.Run(InitProperties);

                _reloadingToken = Messenger.Subscribe<ProductDetailsReloadingByParameterMessage>(msg =>
                {
                    ReloadingByParameter = msg.Reloading;
                    RaiseCommandsCanExecuteChanged();
                });
            }
        }

        #endregion

        protected virtual void OnBuyCommandExecute()
        {
            if (Model == null)
                return;
            
            BasketLoading = true;

            RaiseCommandsCanExecuteChanged();

            AnalyticsNotifyingService.NotifyEventIsHandled("catalog", "catalog_product_card_basket_button", Model.Id);

            Task.Run(async () =>
            {
                var result = await VmService.AddProductToBasket(Model.GroupId, Model.Id);

                InvokeOnMainThread(() =>
                {
                    BasketLoading = false;

                    if (result)
                    {
                        BasketVisible = false;

                        Quantity = Model.UnitStep;

                        UnitStepVisible = true;

                        AnalyticsNotifyingService.NotifyEventIsHandled("catalog", "catalog_product_card_added_to_basket", Model.Id);
                    }

                    RaiseCommandsCanExecuteChanged();
                });

                if (result)
                {
                    Messenger.Publish(new ProductAddToBasketMessage(this, Model.Id));

                    var confirmed = await UserDialogs.Confirm("Добавлено в корзину", "ПЕРЕЙТИ");
                    if (confirmed)
                    {
                        AnalyticsNotifyingService.NotifyEventIsHandled("catalog", "catalog_product_card_notify_basket_button", Model.Id);

                        NavigationVmService.NavigateToBasket(new BaseBundle(Base.Core.Models.Navigation.NavigationType.ClearAndPush));
                    }
                }
            });
        }

        protected virtual async void OnIncrementExecute()
        {
            if (Model == null)
                return;
            
            Quantity += Model.UnitStep;

            AnalyticsNotifyingService.NotifyEventIsHandled("catalog", "catalog_product_card_change_quantity_increment_button", Model.Id);

            await OnQuantityChanged();
        }

        protected virtual async void OnDecrementExecute()
        {
            if (Model == null)
                return;
            
            if (Quantity - Model.UnitStep <= 0)
            {
                await DeleteProductFromBasket();

                return;
            }

            AnalyticsNotifyingService.NotifyEventIsHandled("catalog", "catalog_product_card_change_quantity_decrement_button", Model.Id);

            Quantity -= Model.UnitStep;

            await OnQuantityChanged();
        }

        protected virtual async void OnQuantityChangedExecute()
        {
            if (Model == null)
                return;
            
            AnalyticsNotifyingService.NotifyEventIsHandled("catalog", "catalog_product_card_change_quantity_text_field", Model.Id);

            if (float.TryParse(QuantityString.Replace(" ", ""), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out float result))
            {
                _quantity = result;

                if (_quantity - Model.UnitStep <= 0)
                {
                    await DeleteProductFromBasket();
                    return;
                }

                await OnQuantityChanged();

                SetQuantityString();

                var confirmed = await UserDialogs.Confirm($"Количество товара в корзине: {QuantityString} {UnitName}", "ПЕРЕЙТИ", true);
                if (confirmed)
                    NavigationVmService.NavigateToBasket(new BaseBundle(Base.Core.Models.Navigation.NavigationType.ClearAndPush));
            }
            else
            {
                SetQuantityString();
            }
        }

        #endregion

        #region Public

        public override void Unbind()
        {
            Model = null;
            ReloadingByParameter = false;
            BasketLoading = false;
            BasketVisible = true;
            UnitStepVisible = false;
            QuantityLoading = false;

            base.Unbind();
        }

        #endregion
    }
}
