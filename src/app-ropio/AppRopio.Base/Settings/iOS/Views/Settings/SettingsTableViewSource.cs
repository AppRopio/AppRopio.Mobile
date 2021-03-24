using System;
using AppRopio.Base.Settings.Core.Models;
using AppRopio.Base.Settings.Core.ViewModels.Items;
using AppRopio.Base.Settings.iOS.Views.Cells.Picker;
using AppRopio.Base.Settings.iOS.Views.Cells.Switch;
using MvvmCross.Platforms.Ios.Binding;
using UIKit;

namespace AppRopio.Base.Settings.iOS.Views.Settings
{
    public class SettingsTableViewSource : MvxStandardTableViewSource
    {
        #region Constructor

        public SettingsTableViewSource(UITableView tableView)
            : base(tableView)
        {
        }

        #endregion

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, Foundation.NSIndexPath indexPath, object item)
        {
            var itemVm = item as ISettingsItemVm;

            switch (itemVm.ElementType)
			{
                case SettingsElementType.Region:
                case SettingsElementType.Language:
					return tableView.DequeueReusableCell(SettingsPickerCell.Key, indexPath);

                case SettingsElementType.Geolocation:
				case SettingsElementType.Notifications:
                    return tableView.DequeueReusableCell(SettingsSwitchCell.Key, indexPath);
			}

			return null;
        }
    }
}