using System;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Vertical;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Picker;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Horizontal;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Vertical;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.MinMax;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.MinMax.Date;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.MinMax.Number;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Picker;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Selection.MultiSelection;
using AppRopio.Base.Filters.iOS.Views.Filters.Cells.Selection.OneSelection;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;
using Foundation;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.OneSelection;
using AppRopio.Base.Filters.iOS.Views.FIlters.Cells.Switch;
using MvvmCross.Binding.Extensions;
using System.Collections.Generic;

namespace AppRopio.Base.Filters.iOS.Views.Filters
{
    public class FiltersTableViewSource : MvxStandardTableViewSource
    {
        protected Dictionary<NSIndexPath, float> CachedHeight { get; }

        public NSIndexPath IndexPathForSelectedRow { get; private set; }

        #region Constructor

        public FiltersTableViewSource(UITableView tableView)
            : base(tableView)
        {
            CachedHeight = new Dictionary<NSIndexPath, float>();
        }

        #endregion

        #region Protected

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, Foundation.NSIndexPath indexPath, object item)
        {
            var itemVm = item as IFiltersItemVM;

            switch (itemVm.WidgetType)
            {
                case FilterWidgetType.HorizontalCollection:
                    return tableView.DequeueReusableCell(HorizontalCollectionCell.Key, indexPath);
                case FilterWidgetType.VerticalCollection:
                    return tableView.DequeueReusableCell(VerticalCollectionCell.Key, indexPath);
                case FilterWidgetType.MinMax:
                    return itemVm.DataType == FilterDataType.Date ?
                        tableView.DequeueReusableCell(DateMinMaxCell.Key, indexPath) :
                        tableView.DequeueReusableCell(NumberMinMaxCell.Key, indexPath);
                case FilterWidgetType.Picker:
                    return tableView.DequeueReusableCell(PickerCell.Key, indexPath);
                case FilterWidgetType.OneSelection:
                    return tableView.DequeueReusableCell(OneSelectionCell.Key, indexPath);
                case FilterWidgetType.MultiSelection:
                    return tableView.DequeueReusableCell(MultiSelectionCell.Key, indexPath);
                case FilterWidgetType.Switch:
                    return tableView.DequeueReusableCell(SwitchCell.Key, indexPath);
            }

            return null;
        }

        #endregion

        #region Public

        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            float itemHeight = 0;

            if (!CachedHeight.TryGetValue(indexPath, out itemHeight))
            {
                var itemVm = GetItemAt(indexPath) as IFiltersItemVM;

                if (itemVm != null)
                {
                    switch (itemVm.WidgetType)
                    {
                        case FilterWidgetType.HorizontalCollection:
                            {
                                itemHeight = HorizontalCollectionCell.HORIZONTAL_COLLECTION_HEIGHT;

                                break;
                            }
                        case FilterWidgetType.VerticalCollection:
                            {
                                float itemsHeight = 0;

                                var verticalItemVm = itemVm as IVerticalCollectionFiVm;
                                if (verticalItemVm != null)
                                {
                                    itemsHeight = verticalItemVm.Values.IsNullOrEmpty() ?
                                                                0 :
                                                                ((verticalItemVm.Values.Count / 4) + (verticalItemVm.Values.Count % 4 == 0 ? 0 : 1)) * VerticalCollectionCell.VERTICAL_COLLECTION_ITEM_HEIGHT;
                                    itemsHeight += VerticalCollectionCell.VERTICAL_COLLECTION_BOTTOM_INSET;
                                }

                                itemHeight = VerticalCollectionCell.VERTICAL_COLLECTION_TITLE_HEIGHT + itemsHeight;

                                break;
                            }
                        case FilterWidgetType.MinMax:
                            {
                                itemHeight = BaseMinMaxCell.MIN_MAX_HEIGHT;

                                break;
                            }
                        case FilterWidgetType.Picker:
                            {
                                itemHeight = PickerCell.PICKER_TITLE_HEIGHT;

                                var pickerItemVm = itemVm as IPickerFiVm;
                                if (pickerItemVm != null && pickerItemVm.Selected)
                                    itemHeight += PickerCell.PICKER_CONTENT_HEIGHT;

                                break;
                            }
                        case FilterWidgetType.OneSelection:
                            {
                                itemHeight = OneSelectionCell.ONE_SELECTION_HEIGHT;

                                var oneItemVm = itemVm as IOneSelectionFiVm;
                                if (!oneItemVm.ValueName.IsNullOrEmtpy())
                                    itemHeight += OneSelectionCell.ONE_SELECTION_CONTENT_HEIGHT;

                                break;
                            }
                        case FilterWidgetType.MultiSelection:
                            {
                                itemHeight = MultiSelectionCell.MULTY_SELECTION_TITLE_HEIGHT;

                                var multiItemVm = itemVm as IMultiSelectionFiVm;
                                if (multiItemVm != null && !multiItemVm.Items.IsNullOrEmpty())
                                    itemHeight += MultiSelectionCell.MULTY_SELECTION_CONTENT_HEIGHT;

                                break;
                            }
                        case FilterWidgetType.Switch:
                            {
                                itemHeight = SwitchCell.SWITCH_HEIGHT;

                                break;
                            }
                    }
                }
            }

            CachedHeight[indexPath] = itemHeight;

            return itemHeight;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            if (item is IPickerFiVm)
                IndexPathForSelectedRow = indexPath;
            else
                IndexPathForSelectedRow = null;

            base.RowSelected(tableView, indexPath);
        }

        public override void ReloadTableData()
        {
            if (ItemsSource != null && ItemsSource.Count() > 0)
            {
                CachedHeight.Clear();

                for (int i = 0; i < ItemsSource.Count(); i++)
                {
                    var indexPath = NSIndexPath.FromRowSection(i, 0);
                    GetHeightForRow(TableView, indexPath);
                }

                base.ReloadTableData();

                for (int i = 0; i < ItemsSource.Count(); i++)
                {
                    var indexPath = NSIndexPath.FromRowSection(i, 0);
                    GetCell(TableView, indexPath);
                }
            }
        }

        public void ClearCachedHeights()
        {
            CachedHeight.Clear();
        }

        #endregion
    }
}