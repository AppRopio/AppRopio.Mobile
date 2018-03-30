using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.Basket.Core.Messages.Order;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment
{
    public class PaymentViewModel : BaseViewModel, IPaymentViewModel
    {
        #region Fields

        private string _deliveryId;

        #endregion

        #region Commands

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IPaymentItemVM>(OnItemSelected));
            }
        }

        private ICommand _cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                return _cancelCommand ?? (_cancelCommand = new MvxCommand(OnCancelExecute));
            }
        }

        #endregion

        #region Properties

        private ObservableCollection<IPaymentItemVM> _items;
        public ObservableCollection<IPaymentItemVM> Items
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

        #endregion

        #region Services

        protected IOrderVmService OrderVmService { get { return Mvx.Resolve<IOrderVmService>(); } }

        protected IBasketNavigationVmService NavigationVmService => Mvx.Resolve<IBasketNavigationVmService>();

        #endregion

        #region Constructor

        public PaymentViewModel()
        {
        }

        #endregion

        #region Private

        private async Task LoadContent()
        {
            Loading = true;

            var items = await OrderVmService.LoadPayments(_deliveryId);

            Loading = false;

            Items = items;
        }

        private void OnItemSelected(IPaymentItemVM item)
        {
            Items.ForEach(x => x.IsSelected = false);
            item.IsSelected = true;

            Close(this);

            Messenger.Publish(new PaymentSelectedMessage(this, item.Payment));
        }

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var categoryBundle = parameters.ReadAs<PaymentBundle>();
            this.InitFromBundle(categoryBundle);
        }

        protected virtual void InitFromBundle(PaymentBundle parameters)
        {
            _deliveryId = parameters.DeliveryId;
            VmNavigationType = parameters.NavigationType;
        }

        #endregion

        protected virtual void OnCancelExecute()
        {
            Messenger.Publish(new OrderProcessingMessage(this, false));

            Close(this);
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
