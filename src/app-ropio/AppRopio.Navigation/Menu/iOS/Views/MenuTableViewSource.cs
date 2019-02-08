using Foundation;
using UIKit;
using MvvmCross.Binding.iOS.Views;
using AppRopio.Navigation.Menu.Core.Services;
using MvvmCross.Platform;
using AppRopio.Navigation.Menu.Core.Models;
using AppRopio.Navigation.Menu.iOS.Models;
using AppRopio.Navigation.Menu.iOS.Services;
using System;
using System.Linq;
using AppRopio.Navigation.Menu.Core.ViewModels.Items;
using MvvmCross.Binding.ExtensionMethods;
using System.Collections.ObjectModel;

namespace AppRopio.Navigation.Menu.iOS.Views
{
    public class MenuTableViewSource : MvxSimpleTableViewSource
    {
        protected MenuConfig Config { get; set; }
        protected MenuThemeConfig ThemeConfig { get; set; }

        protected Collection<IMenuItemVM> MenuItemsSource => ItemsSource as Collection<IMenuItemVM>;

        public MenuTableViewSource(UITableView tableView, NSString nibName, NSString cellIdentifier)
            : base(tableView, nibName, cellIdentifier)
        {
            Config = Mvx.Resolve<IMenuConfigService>().Config;
            ThemeConfig = Mvx.Resolve<IMenuThemeConfigService>().ThemeConfig;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (MenuItemsSource.IsNullOrEmpty())
                return null;

            var itemsCountBeforeThis = 0;

            for (int i = 0; i < indexPath.Section; i++)
                itemsCountBeforeThis += Config.Sections[i].Items?.Count ?? 0;

            var itemIndex = itemsCountBeforeThis + Config.Sections[indexPath.Section].Items?.IndexOf(Config.Sections[indexPath.Section].Items?.ElementAt(indexPath.Row)) ?? 0;

            indexPath = NSIndexPath.FromRowSection(itemIndex, 0);

            return base.GetItemAt(indexPath);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            var cell = base.GetOrCreateCellFor(tableView, indexPath, item);

            if (cell is MenuCell menuCell)
            {
                var overlays = ThemeConfig.LeftViewController.MenuTable.OverlayCellThemes;
                if (!overlays.IsNullOrEmpty())
                {
                    var menuItem = item as IMenuItemVM;
                    var overlay = overlays.FirstOrDefault(x =>
                                                          (x.IndexPath != null && x.IndexPath.Row == indexPath.Row && x.IndexPath.Section == indexPath.Section) ||
                                                          (x.ViewModelType == menuItem.Type)
                                                         );
                    if (overlay != null)
                        menuCell.OverrideExistingTheme(overlay.MenuCell);
                    else
                        menuCell.OverrideExistingTheme(ThemeConfig.LeftViewController.MenuTable.MenuCell);
                }
            }

            return cell;
        }

        public override UIView GetViewForHeader(UITableView tableView, System.nint section)
        {
            return new UIView { BackgroundColor = UIColor.Clear };
        }

        public override System.nfloat GetHeightForHeader(UITableView tableView, System.nint section)
        {
            return section == 0 ? 0 : ThemeConfig.LeftViewController.MenuTable.SectionHeaderHeight;
        }

        public override System.nint RowsInSection(UITableView tableview, System.nint section)
        {
            if (MenuItemsSource.IsNullOrEmpty())
                return 0;

            return Config.Sections[(int)section].Items.Count;
        }

        public override System.nint NumberOfSections(UITableView tableView)
        {
            if (MenuItemsSource.IsNullOrEmpty())
                return 0;

            return Config.Sections.Count(x => !x.Items.IsNullOrEmpty());
        }
    }
}