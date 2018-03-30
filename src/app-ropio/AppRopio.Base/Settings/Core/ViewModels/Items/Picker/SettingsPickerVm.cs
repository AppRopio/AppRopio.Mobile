using System;
using AppRopio.Base.Settings.Core.Models;

namespace AppRopio.Base.Settings.Core.ViewModels.Items.Picker
{
    public class SettingsPickerVm : SettingsItemVm, ISettingsPickerVm
    {
        private string _selectedValueId;

		public string SelectedValueId
        {
            get { return _selectedValueId; }
            set { SetProperty(ref _selectedValueId, value); }
        }

        private string _selectedValueTitle;

        public string SelectedValueTitle
        {
            get { return _selectedValueTitle; }
            set { SetProperty(ref _selectedValueTitle, value); }
        }

        public SettingsPickerVm(string title, SettingsElementType elementType) : base(title, elementType)
        {

        }
    }
}