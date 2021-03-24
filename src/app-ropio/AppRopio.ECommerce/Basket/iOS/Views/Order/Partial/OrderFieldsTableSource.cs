using System;
using System.Collections.ObjectModel;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Cells;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Partial
{
    public class OrderFieldsTableSource : MvxTableViewSource
    {
        #region Fields

        protected const float HEADER_HEIGHT = 40;
        protected const float FOOTER_HEIGHT = 10;

        #endregion

        #region Commands

        #endregion

        #region Properties

        protected Models.Order OrderTheme => Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order;
        protected Models.OrderFieldCell UserCellTheme => OrderTheme.UserInfo.Cell ?? Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.BaseOrderFieldCell;

        protected Collection<IOrderFieldsGroupVM> GroupItemsSource => ItemsSource as Collection<IOrderFieldsGroupVM>;
        protected Collection<IOrderFieldItemVM> FieldItemsSource => ItemsSource as Collection<IOrderFieldItemVM>;

        public UIView FieldInputAccessoryView { get; set; }

        #endregion

        #region Constructor

        public OrderFieldsTableSource(UITableView tableView)
            : base(tableView)
        {
            tableView.RegisterNibForHeaderFooterViewReuse(SectionHeader.Nib, SectionHeader.Key);
            tableView.RegisterNibForCellReuse(OrderFieldCell.Nib, OrderFieldCell.Key);
            tableView.RegisterNibForCellReuse(OrderFieldCounterCell.Nib, OrderFieldCounterCell.Key);
            tableView.RegisterNibForCellReuse(OrderFieldOptionalCell.Nib, OrderFieldOptionalCell.Key);
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            if (GroupItemsSource == null)
                return FieldItemsSource?.ElementAt(indexPath.Row);

            return GroupItemsSource[indexPath.Section].Items?.ElementAt(indexPath.Row);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            if (IsOptional(indexPath))
            {
                var optionalCell = tableView.DequeueReusableCell(OrderFieldOptionalCell.Key, indexPath) as OrderFieldOptionalCell;

                optionalCell.OnSwitchChanged = (on) =>
                {
                    TableView.BeginUpdates();
                    TableView.EndUpdates();
                };
                optionalCell.FieldInputAccessoryView = FieldInputAccessoryView;

                return optionalCell;
            }

            if (IsCounter(indexPath))
            {
                var counterCell = tableView.DequeueReusableCell(OrderFieldCounterCell.Key, indexPath) as OrderFieldCounterCell;

                counterCell.OnExpanded = (on) =>
                {
                    TableView.BeginUpdates();
                    TableView.EndUpdates();
                };

                return counterCell;
            }

            var cell = tableView.DequeueReusableCell(OrderFieldCell.Key, indexPath) as OrderFieldCell;

            cell.FieldInputAccessoryView = FieldInputAccessoryView;

            return cell;
        }

        protected virtual bool IsCounter(NSIndexPath indexPath)
        {
            return (GetItemAt(indexPath) as IOrderFieldItemVM)?.Type == AppRopio.Models.Basket.Responses.Enums.OrderFieldType.Counter;
        }

        protected virtual bool IsOptional(NSIndexPath indexPath)
        {
            return (GetItemAt(indexPath) as IOrderFieldItemVM)?.IsOptional ?? false;
        }

        #endregion

        #region Public

        public override nint NumberOfSections(UITableView tableView)
        {
            if ((ItemsSource?.Count() ?? 0) == 0)
                return 0;

            return GroupItemsSource?.Count ?? 1;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (NumberOfSections(tableView) == 1)
                return 0;
            
            var groupItem = section < GroupItemsSource?.Count ? GroupItemsSource[(int)section] as IOrderFieldsGroupVM : null;

            var height = groupItem == null || groupItem.Name.IsNullOrEmpty() ? 1f : HEADER_HEIGHT;
            return height;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            if (GroupItemsSource == null)
                return new UIView();

            var headerView = tableView.DequeueReusableHeaderFooterView(SectionHeader.Key) as SectionHeader;
            headerView.Title = GroupItemsSource[(int)section].Name;

            return headerView;
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return section == NumberOfSections(tableView) - 1 ? 1f : FOOTER_HEIGHT;
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            var view = new UIView()
                .WithFrame(0, 0, tableView.Frame.Width, section == NumberOfSections(tableView) - 1 ? 0 : FOOTER_HEIGHT)
                .WithBackground(section == NumberOfSections(tableView) - 1 ? UIColor.Clear : Theme.ColorPalette.Separator.ToUIColor());
            return view;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (GroupItemsSource == null)
                return ItemsSource?.Count() ?? 0;

            return GroupItemsSource[(int)section]?.Items?.Count() ?? 0;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsOptional(indexPath))
                return UITableView.AutomaticDimension;

            if (IsCounter(indexPath))
                return !(GetItemAt(indexPath) as IOrderFieldItemVM).Expanded ? (nfloat)UserCellTheme.Size.Height : (nfloat)UserCellTheme.Size.Height + 142;

            return (nfloat)UserCellTheme.Size.Height;
        }

        public override nfloat EstimatedHeight(UITableView tableView, NSIndexPath indexPath)
        {
            if (IsOptional(indexPath))
                return (nfloat)(UserCellTheme.Size.Height + UserCellTheme.OptionalTextViewSize.Height) / 2;
            
            return GetHeightForRow(tableView, indexPath);
        }

        #endregion

    }
}
