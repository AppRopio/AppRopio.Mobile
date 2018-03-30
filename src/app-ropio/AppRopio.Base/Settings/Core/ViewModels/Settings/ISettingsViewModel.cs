using System.Collections.Generic;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Settings.Core.ViewModels.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Settings.Core.ViewModels.Settings
{
    public interface ISettingsViewModel : IBaseViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        List<ISettingsItemVm> Items { get; }
    }
}