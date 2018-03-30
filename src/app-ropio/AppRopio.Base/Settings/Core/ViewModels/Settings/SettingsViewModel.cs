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
using AppRopio.Base.Settings.Core.ViewModels.Regions;
using AppRopio.Base.Settings.Core.ViewModels.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Base.Settings.Core.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel, ISettingsViewModel
    {
		private MvxSubscriptionToken _subscribtionToken;

		#region Services

		protected ISettingsVmService VmService { get { return Mvx.Resolve<ISettingsVmService>(); } }

        protected ISettingsVmNavigationService NavigationService => Mvx.Resolve<ISettingsVmNavigationService>();

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

        protected SettingsConfig Config { get { return Mvx.Resolve<ISettingsConfigService>().Config; } }

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

            var region = await VmService.LoadRegion(AppSettings.RegionID);

			Items = elements.Select(e =>
			{
				switch (e.Type)
				{
                    case SettingsElementType.Region:
                        return (ISettingsItemVm)new SettingsPickerVm(e.Title, e.Type) { SelectedValueId = region.Id, SelectedValueTitle = region.Title, OnValueChanged = OnSettingsValueChanged };

                    case SettingsElementType.Geolocation:
                        return (ISettingsItemVm)new SettingsSwitchVm(e.Title, e.Type) { Enabled = AppSettings.IsGeolocationEnabled ?? e.IsEnabled, OnValueChanged = OnSettingsValueChanged };

                    case SettingsElementType.Notifications:
                        return (ISettingsItemVm)new SettingsSwitchVm(e.Title, e.Type) { Enabled = AppSettings.IsNotificationsEnabled ?? e.IsEnabled, OnValueChanged = OnSettingsValueChanged };

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
                    var pickerItem = (ISettingsPickerVm)item;
                    NavigationService.NavigateToRegions(new SettingsPickerBundle(NavigationType.Push, pickerItem.SelectedValueId, pickerItem.SelectedValueTitle));
                    break;
            }
        }

		protected virtual void OnSettingsReloadMessageReceived(SettingsReloadMessage msg)
        {
            if (Items.IsNullOrEmpty())
                return;
            
            var item = Items.FirstOrDefault(i => i.ElementType == msg.ElementType && i is ISettingsPickerVm) as ISettingsPickerVm;
            if (item != null)
            {
                item.SelectedValueId = msg.Id;
                item.SelectedValueTitle = msg.ValueTitle;
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