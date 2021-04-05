using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using MvvmCross.Commands;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery
{
    public class DeliveryOnPointVM : BaseDeliveryVM, IDeliveryOnPointVM, ISearchViewModel
    {
        #region Commands

        private ICommand _deliveryPointChagedCommand;
        public ICommand DeliveryPointChangedCommand
        {
            get
            {
                return _deliveryPointChagedCommand ?? (_deliveryPointChagedCommand = new MvxCommand<IDeliveryPointItemVM>(OnDeliveryPointChanged));
            }
        }

        private ICommand _searchCommand;
        public ICommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new MvxCommand(SearchCommandExecute));
            }
        }

        private ICommand _cancelSearchCommand;
        public ICommand CancelSearchCommand
        {
            get
            {
                return _cancelSearchCommand ?? (_cancelSearchCommand = new MvxCommand(CancelSearchExecute));
            }
        }

        #endregion

        #region Properties

        private ObservableCollection<IDeliveryPointItemVM> _deliveryPointsItems;
        public ObservableCollection<IDeliveryPointItemVM> DeliveryPointsItems
        {
            get
            {
                return _deliveryPointsItems;
            }
            set
            {
                _deliveryPointsItems = value;
                RaisePropertyChanged(() => DeliveryPointsItems);
            }
        }

        private IDeliveryPointItemVM _selectedDeliveryPoint;
        public IDeliveryPointItemVM SelectedDeliveryPoint
        {
            get
            {
                return _selectedDeliveryPoint;
            }
            set
            {
                if (Equals(_selectedDeliveryPoint, value))
                    return;

                _selectedDeliveryPoint = value;
                RaisePropertyChanged(() => SelectedDeliveryPoint);

                CanGoNext = true;
            }
        }

        protected string _searchText;
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;

                    RaisePropertyChanged(() => SearchText);

                    OnSearchTextChanged(SearchText);
                }
            }
        }

        #endregion

        #region Services

        #endregion

        #region Constructor

        public DeliveryOnPointVM()
        {
        }

        #endregion

        #region Private

        private void OnDeliveryPointChanged(IDeliveryPointItemVM deliveryPointItem)
        {
            AnalyticsNotifyingService.NotifyEventIsHandled("order", "order_delivery_pickup_item_selected", deliveryPointItem.Id);
            
            DeliveryPointsItems.ForEach(x => x.IsSelected = false);

            SelectedDeliveryPoint = deliveryPointItem;
            CanGoNext = SelectedDeliveryPoint != null;

            if (!CanGoNext)
                return;
            
            deliveryPointItem.IsSelected = true;

        }

        #endregion

        #region Protected

        protected void OnSearchTextChanged(string searchText)
        {

        }

        protected void SearchCommandExecute()
        {
            LoadContent();
        }

        protected void CancelSearchExecute()
        {
            SearchText = null;
            LoadContent();
        }

        protected override async Task LoadContent()
        {
            Loading = true;

            DeliveryPointsItems = await DeliveryVmService.LoadDeliveryPoints(DeliveryId, SearchText);

            Loading = false;
        }

        protected override async Task<bool> ValidateAndSaveInput()
        {
            Loading = true;

            var isValid = await DeliveryVmService.ValidateAndSaveDeliveryPoint(DeliveryId, SelectedDeliveryPoint.Id);
            
            Loading = false;

            return isValid;
        }

        protected override Task OnNextExecute()
        {
            AnalyticsNotifyingService.NotifyEventIsHandled("order", "order_delivery_pickup_confirmed_button");

            return base.OnNextExecute();
        }

        #endregion

        #region Public

        #endregion
    }
}
