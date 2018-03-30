using System;
using AppRopio.Base.iOS;
using AppRopio.Base.Settings.Core.ViewModels.Items.Switch;
using AppRopio.Base.Settings.iOS.Models;
using AppRopio.Base.Settings.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Settings.iOS.Views.Cells.Switch
{
    public partial class SettingsSwitchCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("SettingsSwitchCell");
        public static readonly UINib Nib;

		protected SettingsThemeConfig ThemeConfig { get { return Mvx.Resolve<ISettingsThemeConfigService>().ThemeConfig; } }

		static SettingsSwitchCell()
        {
            Nib = UINib.FromName("SettingsSwitchCell", NSBundle.MainBundle);
        }

		protected SettingsSwitchCell(IntPtr handle) : base(handle)
        {
			this.DelayBind(() =>
			{
				InitializeControls();
				BindControls();
			});
		}

		#region IntializationControls

		protected virtual void InitializeControls()
		{
            SetupTitle(TitleLabel);
            SetupSwitch(ValueSwitch);

            this.SetupStyle(ThemeConfig.SettingsCell);
		}

		protected virtual void SetupTitle(UILabel title)
		{
            title.SetupStyle(ThemeConfig.SettingsCell.Title);
		}

		protected virtual void SetupSwitch(UISwitch uiSwitch)
		{
            uiSwitch.SetupStyle(ThemeConfig.SettingsCell.Switch);
		}

		#endregion

		#region BindingControls

		protected virtual void BindControls()
		{
			var bindingSet = this.CreateBindingSet<SettingsSwitchCell, ISettingsSwitchVm>();

            BindTitle(TitleLabel, bindingSet);
            BindSwitch(ValueSwitch, bindingSet);

			bindingSet.Apply();
		}

		protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<SettingsSwitchCell, ISettingsSwitchVm> set)
		{
            set.Bind(title).To(vm => vm.Title);
		}

        protected virtual void BindSwitch(UISwitch uiSwitch, MvxFluentBindingDescriptionSet<SettingsSwitchCell, ISettingsSwitchVm> set)
		{
			set.Bind(uiSwitch).To(vm => vm.Enabled);
		}

		#endregion
	}
}