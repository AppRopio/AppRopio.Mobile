using System;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Settings.Core.ViewModels.Items.Switch
{
    public interface ISettingsSwitchVm : ISettingsItemVm
    {
        bool Enabled { get; set; }
    }
}