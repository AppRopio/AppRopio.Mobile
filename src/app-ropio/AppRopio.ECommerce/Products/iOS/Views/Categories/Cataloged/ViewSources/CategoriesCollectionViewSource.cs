using System;
using System.Collections;
using System.Linq;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged.ViewSources
{
    public class CategoriesCollectionViewSource : MvxCollectionViewSource
    {
        private UIView _tabIndicator;
        private bool _manuallySelected;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                if (_selectedIndex != value)
                {
                    _selectedIndex = value;

                    CollectionView.SelectItem(NSIndexPath.FromRowSection(_selectedIndex, 0), true, UICollectionViewScrollPosition.CenteredHorizontally);
                }
            }
        }

        public CategoriesCollectionViewSource(UICollectionView categories, NSString key)
            : base(categories, key)
        {
            categories.AddSubview(_tabIndicator = new UIView()
                                  .WithFrame(0, 0, 0, 2)
                                  .WithBackground(ThemeConfig.Categories.TabCell.Title.HighlightedTextColor.ToUIColor()));
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            base.ItemSelected(collectionView, indexPath);

            _manuallySelected = true;
            SelectedIndex = indexPath.Row;

            UIView.AnimateNotify(0.3f, () =>
            {
                var cell = CollectionView.CellForItem(indexPath);
                _tabIndicator.Frame = new CGRect(cell.Frame.Left, 0, cell.Frame.Width, 2);
            }, (finished) =>
            {
                _manuallySelected = false;
            });
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:")]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var item = GetItemAt(indexPath) as ICategoriesItemVM;

            var size = new CGSize((nfloat)ThemeConfig.Categories.TabCell.Size.Width, (nfloat)ThemeConfig.Categories.TabCell.Size.Height);

            if (item != null)
            {
                var label = new AppRopio.Base.iOS.Controls.ARLabel()
                    .WithFrame(0, 0, 0, (nfloat)ThemeConfig.Categories.TabCell.Size.Height)
                    .WithTune(tune =>
                    {
                        tune.SetupStyle(ThemeConfig.Categories.TabCell.Title);
                        tune.Text = item.Name;
                    });

                label.SetupStyle(ThemeConfig.Categories.TabCell.Title);
                label.SizeToFit();

                var width = label.Frame.Width + 24;
                size = new CGSize(width, (nfloat)ThemeConfig.Categories.TabCell.Size.Height);
            }

            return size;
        }

        public override void ReloadData()
        {
            if (ItemsSource != null && (ItemsSource as IList).Count > 0 && ((ItemsSource as IList).Count != CollectionView.NumberOfItemsInSection(0) || !CollectionView.VisibleCells.Any()))
            {
                CollectionView.PerformBatchUpdates(() =>
                {
                    var newItems = ItemsSource as IList;
                    if (newItems != null)
                    {
                        var indexes = new NSIndexPath[newItems.Count];
                        var startIndex = ItemsSource == null ? 0 : ItemsSource.Count() - newItems.Count;
                        for (var i = 0; i < indexes.Length; i++)
                            indexes[i] = NSIndexPath.FromRowSection(startIndex + i, 0);

                        if (CollectionView.VisibleCells.Any())
                            CollectionView.ReloadItems(indexes);
                        else
                            CollectionView.InsertItems(indexes);
                    }
                    else
                        ReloadData();
                }, ok =>
                {
                    var path = NSIndexPath.FromRowSection(_selectedIndex, 0);

                    CollectionView.SelectItem(path, false, UICollectionViewScrollPosition.CenteredHorizontally);

                    var cell = CollectionView.CellForItem(path);
                    if (cell != null)
                        _tabIndicator.Frame = new CGRect(cell.Frame.Left, 0, cell.Frame.Width, 2);
                });
            }
        }

        public virtual void OnExternalScroll(UIScrollView scrollView)
        {
            if (_manuallySelected)
                return;

            var contentOffset = scrollView.ContentOffset;

            var delta = (contentOffset.X - scrollView.Bounds.Width) / scrollView.Bounds.Width;

            var currentCell = CollectionView.CellForItem(NSIndexPath.FromRowSection(SelectedIndex, 0));

            if (currentCell != null)
            {
                var currentIndicatorX = currentCell.Frame.Left;

                var nextCellIndex = SelectedIndex + (delta > 0 ? 1 : -1);
                if (nextCellIndex >= 0 && nextCellIndex < (ItemsSource as IList).Count)
                {
                    var nextCell = CollectionView.CellForItem(NSIndexPath.FromRowSection(nextCellIndex, 0));

                    if (nextCell != null)
                    {
                        var nextIndicatorX = nextCell.Frame.Left;

                        var distance = (nextIndicatorX - currentIndicatorX) * delta;

                        var widthDifference = nextCell.Frame.Width - currentCell.Frame.Width;

                        _tabIndicator.Frame = new CGRect(
                            currentCell.Frame.Left + (delta > 0 ? distance : -distance),
                            0,
                            currentCell.Frame.Width + widthDifference * Math.Abs(delta),
                            2
                        );
                    }
                }
            }
        }
    }
}
