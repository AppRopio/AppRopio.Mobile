using System;
using System.Collections.ObjectModel;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.Settings.Core.ViewModels.Regions.Items;
using AppRopio.Base.Settings.iOS.Models;
using AppRopio.Base.Settings.iOS.Services;
using AppRopio.Base.Settings.iOS.Views.Regions.Cell;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Settings.iOS.Views.Regions
{
    public class RegionsTableViewSource : MvxStandardTableViewSource
    {
        protected Collection<IRegionGroupItemVm> GroupItemsSource => ItemsSource as Collection<IRegionGroupItemVm>;

		protected SettingsThemeConfig ThemeConfig { get { return Mvx.Resolve<ISettingsThemeConfigService>().ThemeConfig; } }

		#region Constructor

		public RegionsTableViewSource(UITableView tableView, NSString cellIdentifier)
            : base(tableView, cellIdentifier)
        {
		}

        #endregion

        public override nint NumberOfSections(UITableView tableView)
        {
            return GroupItemsSource?.Count ?? 0;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return GroupItemsSource?[(int)section].Items?.Count ?? 0;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            return GroupItemsSource[indexPath.Section].Items[indexPath.Row];
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            return GroupItemsSource[(int)section].Title;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (string.IsNullOrWhiteSpace(GroupItemsSource[(int)section].Title))
                return 0;
            
            return (nfloat)ThemeConfig.Regions.RegionHeaderCell.Size.Height;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var headerView = tableView.DequeueReusableHeaderFooterView(RegionSectionHeader.Key) as RegionSectionHeader;

            headerView.BackgroundView = new UIView(headerView.Bounds)
                .WithBackground(ThemeConfig.Regions.RegionHeaderCell.Background.ToUIColor() ?? Theme.ColorPalette.Background.ToUIColor());
            headerView.TintColor = Theme.ColorPalette.Background.ToUIColor();
            headerView.BackgroundColor = ThemeConfig.Regions.RegionHeaderCell.Background.ToUIColor() ?? Theme.ColorPalette.Background.ToUIColor();

            return headerView;
        }
    }
}