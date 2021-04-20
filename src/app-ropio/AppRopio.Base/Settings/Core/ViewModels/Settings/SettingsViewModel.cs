using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Settings.Core.Models;
using AppRopio.Base.Settings.Core.Models.Bundle;
using AppRopio.Base.Settings.Core.Services;
using AppRopio.Base.Settings.Core.ViewModels.Items;
using AppRopio.Base.Settings.Core.ViewModels.Items.Picker;
using AppRopio.Base.Settings.Core.ViewModels.Items.Switch;
using AppRopio.Base.Settings.Core.ViewModels.Messages;
using AppRopio.Base.Settings.Core.ViewModels.Services;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.Plugin.Messenger;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Settings.Core.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel, ISettingsViewModel
    {
		private MvxSubscriptionToken _subscribtionToken;

		#region Services

		protected ISettingsVmService VmService { get { return Mvx.IoCProvider.Resolve<ISettingsVmService>(); } }

        protected ISettingsVmNavigationService NavigationService => Mvx.IoCProvider.Resolve<ISettingsVmNavigationService>();

        #endregion

        #region Commands

        private IMvxCommand _selectionChangedCommand;

		public IMvxCommand SelectionChangedCommand
		{
			get
			{
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<ISettingsItemVm>(OnItemSelected));
			}
		}

        #endregion

        protected SettingsConfig Config { get { return Mvx.IoCProvider.Resolve<ISettingsConfigService>().Config; } }

        private List<ISettingsItemVm> _items;
        private CancellationTokenSource _notificationCTS;

        public List<ISettingsItemVm> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        #region Init

		public override void Prepare(IMvxBundle parameters)
		{
            base.Prepare(parameters);

			var navigationBundle = parameters.ReadAs<BaseBundle>();
			this.InitFromBundle(navigationBundle);
		}

        protected virtual void InitFromBundle(BaseBundle parameters)
		{
			VmNavigationType = parameters.NavigationType == NavigationType.None ?
															NavigationType.ClearAndPush :
															parameters.NavigationType;
		}

		#endregion

		public override Task Initialize()
		{
			if (_subscribtionToken == null)
				_subscribtionToken = Messenger.Subscribe<SettingsReloadMessage>(OnSettingsReloadMessageReceived);

			return LoadContent();
		}

        public override void Unbind()
        {
            base.Unbind();

            if (_subscribtionToken != null)
            {
                _subscribtionToken.Dispose();
                _subscribtionToken = null;
            }
        }

        protected virtual async Task SetupItems()
        {
			var elements = Config.Elements;

            var region = await VmService.LoadRegion(AppSettings.RegionID ?? AppSettings.DefaultRegionID);

            //TODO: show some dialog
            if (region == null)
                return;

			Items = elements.Select(e =>
			{
                var title = LocalizationService.GetLocalizableString(SettingsConstants.RESX_NAME, Enum.GetName(typeof(SettingsElementType), e.Type));
				switch (e.Type)
				{
                    case SettingsElementType.Region:
                        return (ISettingsItemVm)new SettingsPickerVm(title, e.Type) { SelectedValueId = region.Id, SelectedValueTitle = region.Title, OnValueChanged = OnSettingsValueChanged };

                    case SettingsElementType.Geolocation:
                        return (ISettingsItemVm)new SettingsSwitchVm(title, e.Type) { Enabled = AppSettings.IsGeolocationEnabled ?? e.IsEnabled, OnValueChanged = OnSettingsValueChanged };

                    case SettingsElementType.Notifications:
                        return (ISettingsItemVm)new SettingsSwitchVm(title, e.Type) { Enabled = AppSettings.IsNotificationsEnabled ?? e.IsEnabled, OnValueChanged = OnSettingsValueChanged };

                    case SettingsElementType.Language:
                        return (ISettingsItemVm)new SettingsPickerVm(title, e.Type) { SelectedValueId = AppSettings.SettingsCulture.Name, SelectedValueTitle = AppSettings.SettingsCulture.NativeName.ToFirstCharUppercase() };

                    default:
                        throw new Exception("Unknown element type");
				}
            }).ToList();
        }

        protected virtual async Task LoadContent()
        {
            Loading = true;

			await SetupItems();

            Loading = false;
        }

        protected virtual void OnItemSelected(ISettingsItemVm item)
        {
            switch (item.ElementType)
            {
                case SettingsElementType.Region:
                    var regionPickerItem = (ISettingsPickerVm)item;
                    NavigationService.NavigateToRegions(new SettingsPickerBundle(NavigationType.Push, regionPickerItem.SelectedValueId, regionPickerItem.SelectedValueTitle));
                    break;
                case SettingsElementType.Language:
                    var languagePickerItem = (ISettingsPickerVm)item;
                    NavigationService.NavigateToLanguages(new SettingsPickerBundle(NavigationType.Push, languagePickerItem.SelectedValueId, languagePickerItem.SelectedValueTitle));
                    break;
            }
        }

		protected virtual async void OnSettingsReloadMessageReceived(SettingsReloadMessage msg)
        {
            if (Items.IsNullOrEmpty())
                return;

            if (msg.ElementType == SettingsElementType.Language)
                await SetupItems();
            else
            {
                var item = Items.FirstOrDefault(i => i.ElementType == msg.ElementType && i is ISettingsPickerVm) as ISettingsPickerVm;
                if (item != null)
                {
                    item.SelectedValueId = msg.Id;
                    item.SelectedValueTitle = msg.ValueTitle;
                }
            }
        }

        protected virtual async void OnSettingsValueChanged(ISettingsItemVm item)
        {
            switch (item.ElementType)
            {
                case SettingsElementType.Region:
                    //var regionId = ((ISettingsPickerVm)item).SelectedValueId;
                    //AppSettings.RegionID = regionId;
                    //ChangeRegionHeaderTo(regionId);
                    break;

                case SettingsElementType.Geolocation:
                    AppSettings.IsGeolocationEnabled = ((SettingsSwitchVm)item).Enabled;
                    break;

                case SettingsElementType.Notifications:
                    try
                    {
                        if (_notificationCTS != null)
                        {
                            _notificationCTS.Cancel();
                        }

                        _notificationCTS = new CancellationTokenSource();

                        var enabled = ((SettingsSwitchVm)item).Enabled;
                        await VmService.ChangeNotifications(enabled, _notificationCTS.Token);
                        AppSettings.IsNotificationsEnabled = enabled;
                    }
                    catch (OperationCanceledException)
                    {
                    }
                    break;
            }
        }
    }
}