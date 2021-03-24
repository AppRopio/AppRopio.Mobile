using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Settings.API.Services;
using AppRopio.Base.Settings.Core.Models;
using AppRopio.Base.Settings.Core.Models.Bundle;
using AppRopio.Base.Settings.Core.ViewModels.Messages;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Settings.Core.Services.Implementation
{
    public class RegionService : BaseVmService, IRegionService
    {
        #region Services

        protected ISettingsService ApiService { get { return Mvx.Resolve<ISettingsService>(); } }

        protected ISettingsVmNavigationService NavigationService => Mvx.Resolve<ISettingsVmNavigationService>();

        protected ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();

        #endregion

        public async Task CheckRegion()
        {
            try
            {
                var detectedRegion = await ApiService.GetCurrentRegion();

                if (AppSettings.RegionID != detectedRegion.Id)
                {
                    if (AppSettings.RegionID == null) //first launch
                    {
                        AppSettings.RegionID = detectedRegion.Id;

                        if (await UserDialogs.Confirm($"{LocalizationService.GetLocalizableString(SettingsConstants.RESX_NAME, "Region_YourCity")} {detectedRegion.Title}", LocalizationService.GetLocalizableString(SettingsConstants.RESX_NAME, "Region_Change")))
                            NavigationService.NavigateToRegions(new SettingsPickerBundle(NavigationType.Push, detectedRegion.Id, detectedRegion.Title));
                    }
                    else //previous selection
                    {
                        var selectedRegion = await ApiService.GetRegion(AppSettings.RegionID);
                        if (await UserDialogs.Confirm($"{LocalizationService.GetLocalizableString(SettingsConstants.RESX_NAME, "Region_YourCity")} {selectedRegion.Title}", LocalizationService.GetLocalizableString(SettingsConstants.RESX_NAME, "Region_Change")))
                            NavigationService.NavigateToRegions(new SettingsPickerBundle(NavigationType.Push, detectedRegion.Id, detectedRegion.Title));
                    }
                }
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        public void ChangeSelectedRegion(string regionId, string regionTitle)
        {
            AppSettings.RegionID = regionId;

            Mvx.Resolve<IMvxMessenger>().Publish(new SettingsReloadMessage(this, SettingsElementType.Region, regionId, regionTitle));
        }
    }
}