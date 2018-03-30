using System;
namespace AppRopio.Base.Settings.Core.ViewModels.Items.Picker
{
    public interface ISettingsPickerVm : ISettingsItemVm
    {
		string SelectedValueId { get; set; }

        string SelectedValueTitle { get; set; }
    }
}