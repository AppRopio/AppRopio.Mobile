using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.Contacts;
using AppRopio.ECommerce.Basket.Core.Messages.Autocomplete;
using AppRopio.Models.Basket.Responses.Enums;
using AppRopio.Models.Basket.Responses.Order;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items
{
    public class OrderFieldItemVM : MvxViewModel, IOrderFieldItemVM
    {
        #region Properties

        public string Id { get; private set; }

        public string Name { get; private set; }

        private string _value;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                IsValid = true;
                _value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        private List<string> _values;
        public List<string> Values
        {
            get => _values;
            set => SetProperty(ref _values, value, nameof(Values));
        }

        public OrderFieldType Type { get; private set; }

        public bool Editable { get; private set; }

        public bool IsRequired { get; private set; }

        public bool IsOptional { get; private set; }

        private bool _isValid = true;
        public bool IsValid
        {
            get
            {
                return _isValid;
            }
            set
            {
                _isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        private bool _expanded;
        public bool Expanded
        {
            get => _expanded;
            set => SetProperty(ref _expanded, value, nameof(Expanded));
        }

        public bool HasAutocomplete { get; private set; }

        public int AutocompleteStartIndex { get; private set; }

        public List<string> DependentFieldsIds { get; private set; }

        public bool InAutocompleteMode { get; set; }

        #endregion

        #region Commands

        private IMvxCommand _actionCommand;
        public IMvxCommand ActionCommand => _actionCommand ?? (_actionCommand = new MvxCommand(OnActionExecute));

        private IMvxCommand _autocompleteCommand;
        public IMvxCommand AutocompleteCommand => _autocompleteCommand ?? (_autocompleteCommand = new MvxCommand(OnAutocompleteExecute));

        #endregion

        #region Constructor

        public OrderFieldItemVM(OrderField model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            Id = model.Id;
            Name = model.Name;
            Value = model.Value;
            Values = model.Values;
            Type = model.Type;
            Editable = model.Editable;
            IsRequired = model.IsRequired;
            IsOptional = model.IsOptional;
            HasAutocomplete = model.HasAutocomplete;
            AutocompleteStartIndex = model.AutocompleteStartIndex;
            DependentFieldsIds = model.DependentFieldsIds;

            InAutocompleteMode = false;
        }

        #endregion

        #region Protected

        protected virtual void OnActionExecute()
        {
            switch (Type)
            {
                case OrderFieldType.Phone:
                    Task.Run(async () =>
                    {
                        var phone = await Mvx.Resolve<IContactsService>().SelectPhone();
                        if (phone != null)
                            InvokeOnMainThread(() => Value = phone.FullValue);
                    });
                    break;
                default:
                    break;
            }
        }

        protected virtual void OnAutocompleteExecute()
        {
            if (HasAutocomplete && Value.Length > AutocompleteStartIndex)
            {
                if (InAutocompleteMode)
                {
                    //хз, пока логика в IOrderFieldAutocompleteVM
                }
                else
                {
                    InAutocompleteMode = true;
                    Mvx.Resolve<IMvxMessenger>().Publish(new AutocompleteStartMessage(this) { FieldId = Id, FieldValue = Value });
                }
            }
        }

        #endregion
    }
}
