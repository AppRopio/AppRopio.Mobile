using System;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Settings.Core.Models.Bundle;

namespace AppRopio.Base.Settings.Core.Services
{
    public interface ISettingsVmNavigationService : IBaseVmNavigationService
    {
        void NavigateToRegions(SettingsPickerBundle bundle);

        void NavigateToLanguages(SettingsPickerBundle bundle);
    }
}
