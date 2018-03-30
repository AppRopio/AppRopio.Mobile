using System;
using MvvmCross.Core.ViewModels;
using AppRopio.Models.Filters.Responses;
namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items
{
    public class CollectionItemVM : MvxViewModel
    {
        public string Id { get; private set; }

        private bool _selected;
        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                RaisePropertyChanged(() => Selected);
            }
        }

        public string ValueName { get; private set; }

        public string Value { get; private set; }

        public FilterDataType DataType { get; private set; }

        public CollectionItemVM(FilterDataType dataType, FilterValue value, bool selected)
        {
            DataType = dataType;
            Id = value.Id;

            ValueName = !value.ValueName.IsNullOrEmtpy() ? value.ValueName : value.Value;
            Value = value.Value;

            Selected = selected;
        }
    }
}
