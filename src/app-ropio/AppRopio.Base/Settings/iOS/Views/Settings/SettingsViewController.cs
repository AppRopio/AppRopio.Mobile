using System;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.Settings.Core.ViewModels.Settings;
using AppRopio.Base.Settings.iOS.Models;
using AppRopio.Base.Settings.iOS.Services;
using AppRopio.Base.Settings.iOS.Views.Cells.Picker;
using AppRopio.Base.Settings.iOS.Views.Cells.Switch;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Settings.iOS.Views.Settings
{
    public partial class SettingsViewController : CommonViewController<ISettingsViewModel>
    {
		protected SettingsThemeConfig ThemeConfig { get { return Mvx.Resolve<ISettingsThemeConfigService>().ThemeConfig; } }

		public SettingsViewController() : base("SettingsViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = "Настройки";

            SetupTableView(TableView);
        }

		protected override void BindControls()
		{
			var bindingSet = this.CreateBindingSet<SettingsViewController, ISettingsViewModel>();

			BindTable(TableView, bindingSet);

			bindingSet.Apply();
		}

		#endregion

		protected virtual void RegisterCells(UITableView tableView)
        {
			tableView.RegisterNibForCellReuse(SettingsSwitchCell.Nib, SettingsSwitchCell.Key);
			tableView.RegisterNibForCellReuse(SettingsPickerCell.Nib, SettingsPickerCell.Key);
        }

		protected virtual void SetupTableView(UITableView tableView)
		{
            RegisterCells(tableView);

            tableView.RowHeight = (nfloat)ThemeConfig.SettingsCell.Size.Height;
			tableView.TableFooterView = new UIView();
		}

		#region BingingControls

        protected virtual void BindTable(UITableView tableView, MvxFluentBindingDescriptionSet<SettingsViewController, ISettingsViewModel> set)
		{
			var dataSource = SetupTableViewDataSource(tableView);

			tableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
		}

		protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
		{
            return new SettingsTableViewSource(tableView)
			{
				DeselectAutomatically = true
			};
		}

        #endregion
    }
}