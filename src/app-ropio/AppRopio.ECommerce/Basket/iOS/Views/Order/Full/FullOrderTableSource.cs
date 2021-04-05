using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Cells;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.WeakSubscription;
using UIKit;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.ECommerce.Basket.Core;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Full
{
    public class FullOrderTableSource : MvxTableViewSource
    {
        #region Fields

        protected const float HEADER_HEIGHT = 40;

        protected const float FOOTER_HEIGHT = 10;

        private IEnumerable _deliveryItemsSource;

        private MvxNotifyCollectionChangedEventSubscription _subscription;

        #endregion

        #region Commands

        #endregion

        #region Properties

        protected Models.Order OrderTheme => Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Order;

        protected Models.OrderFieldCell UserCellTheme => OrderTheme.UserInfo.Cell ?? Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.BaseOrderFieldCell;

        protected Models.DeliveryTypeCell DeliveryCellTheme => OrderTheme.DeliveryInfo.Cell;

        protected int UserSections { get; set; }

        protected int DeliverySections { get; set; }

        protected int Sections => UserSections + DeliverySections;

        public UIView FieldInputAccessoryView { get; set; }

        #endregion

        #region Constructor

        public FullOrderTableSource(UITableView tableView)
            : base(tableView)
        {

        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected virtual bool IsCounter(NSIndexPath indexPath)
        {
            return (GetItemAt(indexPath) as IOrderFieldItemVM)?.Type == AppRopio.Models.Basket.Responses.Enums.OrderFieldType.Counter;
        }

        protected virtual bool IsOptional(NSIndexPath indexPath)
        {
            return (GetItemAt(indexPath) as IOrderFieldItemVM)?.IsOptional ?? false;
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            object item = null;

            if (indexPath.Section < UserSections)
            {
                var sectionItem = ItemsSource.ElementAt(indexPath.Section);

                if (sectionItem is IOrderFieldsGroupVM groupFields)
                    item = groupFields.Items[indexPath.Row];
            }
            else
                item = ItemsSource.ElementAt(indexPath.Section + indexPath.Row);

            return item;
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
        {
            UITableViewCell cell = null;

            if (item is IOrderFieldItemVM orderItem)
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

                cell = tableView.DequeueReusableCell(OrderFieldCell.Key, indexPath);

                (cell as OrderFieldCell).FieldInputAccessoryView = FieldInputAccessoryView;
            }
            else if (item is IDeliveryTypeItemVM delivery)
            {
                cell = tableView.DequeueReusableCell(DeliveryTypeCell.Key, indexPath);
            }

            return cell;
        }

        #endregion

        #region Public

        public override nint NumberOfSections(UITableView tableView)
        {
            if ((ItemsSource?.Count() ?? 0) == 0)
                return 0;

            var items = (ItemsSource as IEnumerable<IMvxViewModel>).ToList();

            UserSections = items?.Where(x => x is IOrderFieldsGroupVM)?.Count() ?? 0;
            DeliverySections = (items?.Any(x => x is IDeliveryTypeItemVM) ?? false) ? 1 : 0;

            return Sections;
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            nfloat height = 0;

            if (section == Sections - 1)
                height = HEADER_HEIGHT;
            else
            {
                var groupItem = ItemsSource.ElementAt((int)section) as IOrderFieldsGroupVM;

                height = groupItem == null || groupItem.Name.IsNullOrEmpty() ? 1f : HEADER_HEIGHT;
            }

            return height;
        }

        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            var headerView = tableView.DequeueReusableHeaderFooterView(SectionHeader.Key) as SectionHeader;

            headerView.Title = (section == (Sections - 1) ? Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(BasketConstants.RESX_NAME, "Order_DeliveryTypeHeader") : (ItemsSource.ElementAt((int)section) as IOrderFieldsGroupVM).Name);

            return headerView;
        }

        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            return section == (Sections - 1) ? 1f : FOOTER_HEIGHT;
        }

        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            var view = new UIView()
                .WithFrame(0, 0, tableView.Frame.Width, section == (Sections - 1) ? 0 : FOOTER_HEIGHT)
                .WithBackground(section == NumberOfSections(tableView) - 1 ? UIColor.Clear : Theme.ColorPalette.Separator.ToUIColor());
            return view;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            var items = (ItemsSource as IEnumerable<IMvxViewModel>).ToList();

            return section == (Sections - 1) ?
                items.Where(x => x is IDeliveryTypeItemVM).Count()
                         :
                (items.ElementAt((int)section) as IOrderFieldsGroupVM).Items.Count;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == Sections - 1)
                return (nfloat)DeliveryCellTheme.Size.Height;

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
