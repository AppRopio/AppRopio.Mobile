using System;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Settings.Core.Models.Bundle;
using AppRopio.Base.Settings.Core.ViewModels.Languages;
using AppRopio.Base.Settings.Core.ViewModels.Regions;

namespace AppRopio.Base.Settings.Core.Services.Implementation
{
    public class SettingsVmNavigationService : BaseVmNavigationService, ISettingsVmNavigationService
    {
        public void NavigateToLanguages(SettingsPickerBundle bundle)
        {
            NavigateTo<ILanguagesViewModel>(bundle);
        }

        public void NavigateToRegions(SettingsPickerBundle bundle)
        {
            NavigateTo<IRegionsViewModel>(bundle);
        }
    }
}
