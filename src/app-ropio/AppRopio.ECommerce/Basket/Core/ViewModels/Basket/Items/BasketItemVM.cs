using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Services.UserDialogs;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items.Services;
using AppRopio.Models.Basket.Responses.Basket;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items
{
    public class BasketItemVM : MvxViewModel, IBasketItemVM
    {
        #region Fields

        #endregion

        #region Commands

        private ICommand _incCommand;
        public ICommand IncCommand
        {
            get
            {
                return _incCommand ?? (_incCommand = new MvxCommand(() => OnIncrementExecute()));
            }
        }

        private ICommand _decCommand;
        public ICommand DecCommand
        {
            get
            {
                return _decCommand ?? (_decCommand = new MvxCommand(() => OnDecrementExecute()));
            }
        }

        private ICommand _deleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new MvxCommand(OnDeleteExecute));
            }
        }

        #endregion

        #region Properties

        public string GroupId { get; private set; }

        public string ProductId { get; private set; }

        public string Name { get; private set; }

        public string ImageUrl { get; private set; }

        private decimal _price;
        public decimal Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                RaisePropertyChanged(() => Price);
            }
        }

        private decimal? _oldPrice;
        public decimal? OldPrice
        {
            get
            {
                return _oldPrice;
            }
            set
            {
                _oldPrice = value;
                RaisePropertyChanged(() => OldPrice);
            }
        }

        private bool _isMarked;
        public bool IsMarked
        {
            get
            {
                return _isMarked;
            }
            set
            {
                _isMarked = value;
                RaisePropertyChanged(() => IsMarked);
            }
        }

        private ProductState _state;
        public ProductState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                RaisePropertyChanged(() => State);
            }
        }

        private List<ProductBadge> _badges;
        public List<ProductBadge> Badges
        {
            get
            {
                return _badges;
            }
            set
            {
                _badges = value;
                RaisePropertyChanged(() => Badges);
            }
        }

        public string UnitName { get; private set; }

        public float UnitStep { get; private set; }

        private float _quantity;
        public float Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                _quantity = value;
                SetQuantityString();
            }
        }

        private string _quantityString;
        public string QuantityString
        {
            get => _quantityString;
            set => SetProperty(ref _quantityString, value, nameof(QuantityString));
        }

        private bool _quantityLoading;
        public bool QuantityLoading
        {
            get
            {
                return _quantityLoading;
            }
            set
            {
                _quantityLoading = value;
                InvokeOnMainThread(() => RaisePropertyChanged(() => QuantityLoading));
            }
        }

        #endregion

        #region Services

        protected IBasketItemVmService VmService { get { return Mvx.Resolve<IBasketItemVmService>(); } }

        #endregion

        #region Constructor

        public BasketItemVM(BasketItem model)
        {
            GroupId = model.GroupId;
            ProductId = model.Id;
            ImageUrl = model.ImageUrls?.FirstOrDefault()?.SmallUrl;
            Name = model.Name;
            Price = model.Price;
            OldPrice = model.OldPrice;
            IsMarked = model.IsMarked;
            State = model.State;
            Badges = model.Badges;
            UnitName = model.UnitName;
            UnitStep = model.UnitStep;
            Quantity = model.Quantity;
        }

        #endregion

        #region Private

        private void SetQuantityString()
        {
            _quantityString = string.Format("{0} {1}", _quantity.ToString("### ###.###").Trim(), UnitName);
            RaisePropertyChanged(() => QuantityString);
        }

        private async Task OnQuantityChanged()
        {
            try
            {
                var quantityResponse = await VmService.SetQuantity(ProductId, Quantity);

                if (quantityResponse != null && quantityResponse.Quantity > 0)
                {
                    Quantity = quantityResponse.Quantity;

                    Mvx.Resolve<IMvxMessenger>().Publish(new ProductQuantityChangedMessage(this));

                    if (!string.IsNullOrEmpty(quantityResponse.Error))
                        await Mvx.Resolve<IUserDialogs>().Alert(quantityResponse.Error);
                }
            }
            catch (OperationCanceledException)
            {
                //nothing
            }
        }

        #endregion

        #region Protected

        protected virtual async Task OnIncrementExecute()
        {
            QuantityLoading = true;

            Quantity += UnitStep;

            await OnQuantityChanged();

            QuantityLoading = false;
        }

        protected virtual async Task OnDecrementExecute()
        {
            if ((Quantity - UnitStep <= 0))
            {
                if (await Mvx.Resolve<IUserDialogs>().Confirm($"Удалить \"{Name}\"?", "Да"))
                    OnDeleteExecute();
                return;
            }

            QuantityLoading = true;

            Quantity -= UnitStep;

            await OnQuantityChanged();

            QuantityLoading = false;
        }

        protected virtual void OnDeleteExecute()
        {
            VmService.Delete(ProductId);
        }

        #endregion
    }
}
