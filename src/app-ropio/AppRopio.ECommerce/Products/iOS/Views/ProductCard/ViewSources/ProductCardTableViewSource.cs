using System;
using System.Collections.Generic;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Images;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.ShortInfo;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Products;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Shops;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Vertical;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Images;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MinMax.Date;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MinMax.Number;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MultilineText;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Picker;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.MultiSelection;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.OneSelection;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.ShortInfo;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Switch;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Transition;
using AppRopio.Models.Products.Responses;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.ViewSources
{
    public class ProductCardTableViewSource : ScrolledEventsTableViewSource
    {
        protected Dictionary<NSIndexPath, float> CachedHeight { get; }
        protected Dictionary<NSIndexPath, UITableViewCell> CachedCells { get; }

        public NSIndexPath IndexPathForSelectedRow { get; private set; }

        public delegate float WidgetHeightForDataType(object item, ProductWidgetType? widgetType, ProductDataType? dataType);
        public WidgetHeightForDataType WidgetHeight { get; set; }

        #region Constructor

        public ProductCardTableViewSource(UITableView tableView)
            : base(tableView)
        {
            CachedHeight = new Dictionary<NSIndexPath, float>();
            CachedCells = new Dictionary<NSIndexPath, UITableViewCell>();
        }

        #endregion

        #region Protected

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, Foundation.NSIndexPath indexPath, object item)
        {
            var detailsItemVm = item as IProductDetailsItemVM;

            if (detailsItemVm != null)
            {
                switch (detailsItemVm.WidgetType)
                {
                    case ProductWidgetType.HorizontalCollection:
                        {
                            switch (detailsItemVm.DataType)
                            {
                                case ProductDataType.Products:
                                    return tableView.DequeueReusableCell(PDHorizontalProductsCollectionCell.Key, indexPath);
                                case ProductDataType.ShopsAvailability_Count:
                                case ProductDataType.ShopsAvailability_Indicator:
                                    return tableView.DequeueReusableCell(PDHorizontalShopsCollectionCell.Key, indexPath);
                                default:
                                    return tableView.DequeueReusableCell(PDHorizontalCollectionCell.Key, indexPath);
                            }

                        }
                    case ProductWidgetType.VerticalCollection:
                        return tableView.DequeueReusableCell(PDVerticalCollectionCell.Key, indexPath);
                    case ProductWidgetType.MinMax:
                        return detailsItemVm.DataType == ProductDataType.Date ?
                            tableView.DequeueReusableCell(PDDateMinMaxCell.Key, indexPath) :
                            tableView.DequeueReusableCell(PDNumberMinMaxCell.Key, indexPath);
                    case ProductWidgetType.Picker:
                        return tableView.DequeueReusableCell(PDPickerCell.Key, indexPath);
                    case ProductWidgetType.OneSelection:
                        return tableView.DequeueReusableCell(PDOneSelectionCell.Key, indexPath);
                    case ProductWidgetType.MultiSelection:
                        return tableView.DequeueReusableCell(PDMultiSelectionCell.Key, indexPath);
                    case ProductWidgetType.Switch:
                        return tableView.DequeueReusableCell(PDSwitchCell.Key, indexPath);
                    case ProductWidgetType.MultilineText:
                        return tableView.DequeueReusableCell(MultilineTextCell.Key, indexPath);
                    case ProductWidgetType.Transition:
                        return tableView.DequeueReusableCell(TransitionCell.Key, indexPath);
                }
            }

            var imagesItem = item as IImagesProductsPciVm;
            if (imagesItem != null && !imagesItem.ImagesUrls.IsNullOrEmpty())
                return tableView.DequeueReusableCell(ImagesCell.Key, indexPath);

            var shortInfoItem = item as IShortInfoProductsPciVm;
            if (shortInfoItem != null)
                return tableView.DequeueReusableCell(ShortInfoCell.Key, indexPath);

            return null;
        }

        #endregion

        #region Public

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (!CachedCells.TryGetValue(indexPath, out UITableViewCell cell))
            {
                var item = GetItemAt(indexPath);
                cell = GetOrCreateCellFor(tableView, indexPath, item); 

                var bindable = cell as IMvxBindable;

                if (bindable != null)
                {
                    var bindingContext = bindable.BindingContext as MvxTaskBasedBindingContext;
                    if (bindingContext != null && tableView.RowHeight == UITableView.AutomaticDimension)
                        bindingContext.RunSynchronously = true;

                    InvokeOnMainThread(() => bindable.DataContext = GetItemAt(indexPath));
                }
            }

            return cell;
        }

        public override nfloat GetHeightForRow(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            if (!CachedHeight.TryGetValue(indexPath, out float itemHeight))
            {
                var itemVm = GetItemAt(indexPath);

                if (itemVm is IProductDetailsItemVM detailsItemVm)
                    itemHeight = WidgetHeight.Invoke(detailsItemVm, detailsItemVm.WidgetType, detailsItemVm.DataType);
                else
                    itemHeight = WidgetHeight.Invoke(itemVm, null, null);

                CachedHeight[indexPath] = itemHeight;
            }

            return itemHeight;
        }

        public override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath);
            if (item is IPickerPciVm)
                IndexPathForSelectedRow = indexPath;
            else
                IndexPathForSelectedRow = null;

            base.RowSelected(tableView, indexPath);
        }

        public override void ReloadTableData()
        {
            if (ReloadOnAllItemsSourceSets)
                ClearCache();

            if (ItemsSource != null && ItemsSource.Count() > 0)
            {
                for (int i = 0; i < ItemsSource.Count(); i++)
                {
                    var indexPath = NSIndexPath.FromRowSection(i, 0);
                    GetHeightForRow(TableView, indexPath);
                }

                base.ReloadTableData();

                for (int i = 0; i < ItemsSource.Count(); i++)
                {
                    var indexPath = NSIndexPath.FromRowSection(i, 0);

                    CachedCells[indexPath] = GetCell(TableView, indexPath);
                }
            }
            else
                ClearCache();
        }

        protected override void CollectionChangedOnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset ||
                args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
                ClearCache();

            base.CollectionChangedOnCollectionChanged(sender, args);
        }

        public void ClearCache()
        {
            CachedHeight.Clear();
            CachedCells.Clear();
        }

        #endregion
    }
}
