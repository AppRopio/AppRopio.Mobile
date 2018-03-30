using System;
using System.Collections.ObjectModel;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using MvvmCross.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.Base.Core.Extentions;
using AppRopio.Models.Basket.Responses.Order;
using System.Collections.Generic;
using MvvmCross.Platform;
using AppRopio.ECommerce.Basket.Core.Messages.Autocomplete;
using System.Threading.Tasks;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Services;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order
{
    public class OrderFieldAutocompleteVM : BaseViewModel, IOrderFieldAutocompleteVM
    {
        #region Fields

        protected Dictionary<string, string> DependentFieldsValues;

        #endregion

        #region Commands

        private IMvxCommand _valueChangedCommand;
        public IMvxCommand ValueChangedCommand => _valueChangedCommand ?? (_valueChangedCommand = new MvxCommand(OnValueChangedExecute));

        private IMvxCommand _selectionChangedCommand;
        public IMvxCommand SelectionChangedCommand => _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<IOrderFieldAutocompleteItemVM>(OnSelectionChangedExecute));

        private IMvxCommand _applyCommand;
        public IMvxCommand ApplyCommand => _applyCommand ?? (_applyCommand = new MvxCommand(OnApplyExecute));

        #endregion

        #region Properties

        private IOrderFieldItemVM _orderFieldItem;
        public IOrderFieldItemVM OrderFieldItem
        {
            get => _orderFieldItem;
            set => SetProperty(ref _orderFieldItem, value, nameof(OrderFieldItem));
        }

        private ObservableCollection<IOrderFieldAutocompleteItemVM> _items;
        public ObservableCollection<IOrderFieldAutocompleteItemVM> Items
        {
            get => _items;
            set => SetProperty(ref _items, value, nameof(Items));
        }

        #endregion

        #region Services

        protected IOrderVmService OrderVmService => Mvx.Resolve<IOrderVmService>();

        #endregion

        #region Constructor

        public OrderFieldAutocompleteVM()
        {
            VmNavigationType = NavigationType.Push;
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var orderFieldBundle = parameters.ReadAs<OrderFieldAutocompleteBundle>();
            this.InitFromBundle(orderFieldBundle);
        }

        protected virtual void InitFromBundle(OrderFieldAutocompleteBundle orderFieldBundle)
        {
            OrderFieldItem = new OrderFieldItemVM(new OrderField
            {
                Id = orderFieldBundle.Id,
                Name = orderFieldBundle.Name,
                Value = orderFieldBundle.Value,
                Type = orderFieldBundle.Type,
                IsRequired = orderFieldBundle.IsRequired,
                HasAutocomplete = true,
                AutocompleteStartIndex = orderFieldBundle.AutocompleteStartIndex
            })
            {
                InAutocompleteMode = true
            };
            DependentFieldsValues = orderFieldBundle.DependentFieldsValues;
            VmNavigationType = orderFieldBundle.NavigationType;
        }

        protected virtual async Task LoadContent()
        {
            var items = await OrderVmService.LoadAutocompleteValues(OrderFieldItem.Id, OrderFieldItem.Value, DependentFieldsValues);
            InvokeOnMainThread(() => Items = items);
        }

        protected virtual void OnValueChangedExecute()
        {
            LoadContent();
        }

        protected virtual void OnSelectionChangedExecute(IOrderFieldAutocompleteItemVM autocompleteItem)
        {
            OrderFieldItem.Value = autocompleteItem.Value;
        }

        protected virtual void OnApplyExecute()
        {
            //Messenger.Publish(new AutocompleteApplyMessage(this) { FieldId = OrderFieldItem.Id, FieldValue = OrderFieldItem.Value });
            Close(this);
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        public override void ViewDisappearing()
        {
            base.ViewDisappearing();

            Messenger.Publish(new AutocompleteApplyMessage(this) { FieldId = OrderFieldItem.Id, FieldValue = OrderFieldItem.Value });
        }

        #endregion
    }
}
