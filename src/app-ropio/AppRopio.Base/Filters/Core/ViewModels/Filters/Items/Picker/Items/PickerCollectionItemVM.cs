using System;
using MvvmCross.Core.ViewModels;
using AppRopio.Models.Filters.Responses;
namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Picker.Items
{
    public class PickerCollectionItemVM : MvxViewModel
    {
        public string Id { get; private set; }

        public string Value { get; protected set; }

        public string ValueName { get; protected set; }

        public PickerCollectionItemVM(FilterValue value)
        {
            Id = value.Id;

            Value = value.Value;
            ValueName = value.ValueName;
        }

        public override string ToString()
        {
            return ValueName;
        }
    }
}
