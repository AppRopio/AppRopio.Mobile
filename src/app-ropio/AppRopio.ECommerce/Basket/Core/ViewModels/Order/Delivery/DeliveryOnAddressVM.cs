using System.Threading.Tasks;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using MvvmCross.Core.ViewModels;
using AppRopio.Base.Core.Services.Location;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using AppRopio.ECommerce.Basket.Core.Messages.Autocomplete;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using System.Linq;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery
{
    public class DeliveryOnAddressVM : BaseDeliveryVM, IDeliveryOnAddressVM
    {
        #region Fields

        private MvxSubscriptionToken _autocompleteStartToken;
        private MvxSubscriptionToken _autocompleteApplyToken;

        #endregion

        #region Commands

        #endregion

        #region Properties

        private MvxObservableCollection<IOrderFieldItemVM> _addressFieldsItems;
        public MvxObservableCollection<IOrderFieldItemVM> AddressFieldsItems
        {
            get
            {
                return _addressFieldsItems;
            }
            set
            {
                _addressFieldsItems = value;
                RaisePropertyChanged(() => AddressFieldsItems);
            }
        }

        #endregion

        #region Services

        protected ILocationService LocationService => Mvx.Resolve<ILocationService>();

        #endregion

        #region Constructor

        public DeliveryOnAddressVM()
        {
        }

        #endregion

        #region Private

        private void UnsubscribeTokens()
        {
            if (_autocompleteStartToken != null)
            {
                Messenger.Unsubscribe<AutocompleteStartMessage>(_autocompleteStartToken);
                _autocompleteStartToken = null;
            }

            if (_autocompleteApplyToken != null)
            {
                Messenger.Unsubscribe<AutocompleteApplyMessage>(_autocompleteApplyToken);
                _autocompleteApplyToken = null;
            }
        }

        #endregion

        #region Protected

        protected override async Task LoadContent()
        {
            Loading = true;

            var position = await LocationService.GetCurrentLocation();

            AddressFieldsItems = await DeliveryVmService.LoadDeliveryAddressFields(DeliveryId, position);

            Loading = false;

            CanGoNext = true;
        }

        protected override async Task<bool> ValidateAndSaveInput()
        {
            Loading = true;

            var isValid = await DeliveryVmService.ValidateAndSaveDeliveryAddressFields(DeliveryId, AddressFieldsItems);

            Loading = false;

            return isValid;
        }

        protected virtual void OnAutocompleteStart(AutocompleteStartMessage message)
        {
            var autocompleteField = AddressFieldsItems.FirstOrDefault(x => x.Id.Equals(message.FieldId));
            if (autocompleteField == null)
                return;
            
            var dependentFields = AddressFieldsItems.Where(x => autocompleteField.DependentFieldsIds?.Contains(x.Id) ?? false);
            var dependentFieldsValues = dependentFields?.ToDictionary(x => x.Id, x => x.Value);
            NavigationVmService.NavigateToOrderFieldAutocomplete(new OrderFieldAutocompleteBundle(autocompleteField, dependentFieldsValues));
        }

        protected virtual void OnAutocompleteApply(AutocompleteApplyMessage message)
        {
            var autocompleteField = AddressFieldsItems.FirstOrDefault(x => x.Id.Equals(message.FieldId));
            if (autocompleteField == null)
                return;

            autocompleteField.InAutocompleteMode = false;
            autocompleteField.Value = message.FieldValue;
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            _autocompleteStartToken = Messenger.Subscribe<AutocompleteStartMessage>(OnAutocompleteStart);
            _autocompleteApplyToken = Messenger.Subscribe<AutocompleteApplyMessage>(OnAutocompleteApply);
            
            return base.Initialize();
        }

        public override void Unbind()
        {
            base.Unbind();

            UnsubscribeTokens();
        }

        #endregion
    }
}
