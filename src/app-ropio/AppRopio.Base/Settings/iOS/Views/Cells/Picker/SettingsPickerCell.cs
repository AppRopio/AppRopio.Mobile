using System;
using AppRopio.Base.iOS;
using AppRopio.Base.Settings.Core.ViewModels.Items.Picker;
using AppRopio.Base.Settings.iOS.Models;
using AppRopio.Base.Settings.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Settings.iOS.Views.Cells.Picker
{
    public partial class SettingsPickerCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("SettingsPickerCell");
        public static readonly UINib Nib;

		protected SettingsThemeConfig ThemeConfig { get { return Mvx.Resolve<ISettingsThemeConfigService>().ThemeConfig; } }

		static SettingsPickerCell()
        {
            Nib = UINib.FromName("SettingsPickerCell", NSBundle.MainBundle);
        }

		protected SettingsPickerCell(IntPtr handle) : base(handle)
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
            SetupValue(ValueLabel);

            this.SetupStyle(ThemeConfig.SettingsCell);
		}

		protected virtual void SetupTitle(UILabel title)
		{
			title.SetupStyle(ThemeConfig.SettingsCell.Title);
		}

		protected virtual void SetupValue(UILabel value)
		{
			value.SetupStyle(ThemeConfig.SettingsCell.Value);
		}

		#endregion

		#region BindingControls

		protected virtual void BindControls()
		{
			var bindingSet = this.CreateBindingSet<SettingsPickerCell, ISettingsPickerVm>();

			BindTitle(TitleLabel, bindingSet);
            BindValue(ValueLabel, bindingSet);

			bindingSet.Apply();
		}

		protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<SettingsPickerCell, ISettingsPickerVm> set)
		{
			set.Bind(title).To(vm => vm.Title);
		}

		protected virtual void BindValue(UILabel value, MvxFluentBindingDescriptionSet<SettingsPickerCell, ISettingsPickerVm> set)
		{
            set.Bind(value).To(vm => vm.SelectedValueTitle);
		}

		#endregion
	}
}
