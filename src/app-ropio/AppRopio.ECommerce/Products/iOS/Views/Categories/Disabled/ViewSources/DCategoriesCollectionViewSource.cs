using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Disabled;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.ExtensionMethods;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Disabled.ViewSources
{
    public class DCategoriesCollectionViewSource : SupplementaryCollectionViewSource
    {
        protected IDCategoriesViewModel ViewModel { get; }

        public string BannersHeaderReuseID { get; set; }

        public DCategoriesCollectionViewSource(UICollectionView collectionView, NSString cellIdentifier, IDCategoriesViewModel viewModel, object headerVm = null)
            : base(collectionView, cellIdentifier, headerVm)
        {
            ViewModel = viewModel;
        }

        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
            if (elementKind == UICollectionElementKindSectionKey.Header && !HeaderReuseID.IsNullOrEmtpy() && indexPath.Section == 1)
            {
                var headerView = collectionView.DequeueReusableSupplementaryView(elementKind, HeaderReuseID, indexPath);

                if (_headerDataContext != null)
                    SetupReusableView(headerView, elementKind, indexPath, _headerDataContext);

                return headerView;
            }

            if (elementKind == UICollectionElementKindSectionKey.Header && !BannersHeaderReuseID.IsNullOrEmtpy() && indexPath.Section == 0)
            {
                var headerView = collectionView.DequeueReusableSupplementaryView(elementKind, BannersHeaderReuseID, indexPath);

                if (ViewModel != null)
                    SetupReusableView(headerView, elementKind, indexPath, ViewModel);

                return headerView;
            }

            if (elementKind == UICollectionElementKindSectionKey.Footer && !FooterReuseID.IsNullOrEmtpy() && indexPath.Section == 1)
            {
                var footerView = collectionView.DequeueReusableSupplementaryView(elementKind, FooterReuseID, indexPath);

                if (ViewModel != null)
                    SetupReusableView(footerView, elementKind, indexPath, ViewModel);

                return footerView;
            }

            return null;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 2;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return section == 0 ? 0 : (ItemsSource?.Count() ?? 0);
        }

        protected override void CollectionChangedOnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            var sectionNumber = 1;
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                try
                {
                    CollectionView.PerformBatchUpdates(() =>
                    {
                        var startIndex = args.OldStartingIndex;
                        var indexes = new NSIndexPath[args.OldItems.Count];
                        for (var i = 0; i < indexes.Length; i++)
                            indexes[i] = NSIndexPath.FromRowSection(startIndex + i, sectionNumber);
                        CollectionView.DeleteItems(indexes);
                    }, ok => { });
                }
                catch
                {
                    CollectionView.PerformBatchUpdates(() => { }, animationsCompleted =>
                    {
                        ReloadData();
                    });
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Add)
            {
                try
                {
                    if (args.NewItems.Count == 1 && (args.NewItems[0] is IList newItems))
                    {
                        CollectionView.PerformBatchUpdates(() =>
                        {
                            if (newItems != null)
                            {
                                var indexes = new NSIndexPath[newItems.Count];
                                var startIndex = ItemsSource == null ? 0 : ItemsSource.Count() - newItems.Count;
                                for (var i = 0; i < indexes.Length; i++)
                                    indexes[i] = NSIndexPath.FromRowSection(startIndex + i, sectionNumber);
                                CollectionView.InsertItems(indexes);
                            }
                        }, ok => { });
                    }
                    else if (args.NewItems.Count > 1 && !(args.NewItems[0] is IList))
                    {
                        CollectionView.PerformBatchUpdates(() =>
                        {
                            var indexes = new NSIndexPath[args.NewItems.Count];
                            var startIndex = ItemsSource == null ? 0 : ItemsSource.Count() - args.NewItems.Count;
                            for (var i = 0; i < indexes.Length; i++)
                                indexes[i] = NSIndexPath.FromRowSection(startIndex + i, sectionNumber);
                            CollectionView.InsertItems(indexes);
                        }, ok => { });
                    }
                    else
                        base.CollectionChangedOnCollectionChanged(sender, args);
                }
                catch
                {
                    CollectionView.PerformBatchUpdates(() => { }, animationsCompleted =>
                    {
                        ReloadData();
                    });
                }
            }
            else if (args.Action == NotifyCollectionChangedAction.Move)
            {
                CollectionView.PerformBatchUpdates(() =>
                {
                    var oldCount = args.OldItems.Count;
                    var newCount = args.NewItems.Count;
                    var indexes = new List<NSIndexPath>();

                    var startIndex = (int)Math.Min(args.OldStartingIndex, args.NewStartingIndex);
                    var endIndex = Math.Max(args.OldStartingIndex, args.NewStartingIndex);

                    for (var i = startIndex; i <= endIndex; i++)
                        indexes.Add(NSIndexPath.FromRowSection(i, sectionNumber));

                    CollectionView.ReloadItems(indexes.ToArray());
                }, ok => { });
            }
            else
            {
                base.CollectionChangedOnCollectionChanged(sender, args);
            }
        }

        [Export("collectionView:layout:referenceSizeForFooterInSection:")]
        public override CGSize GetReferenceSizeForFooter(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            var width = UIScreen.MainScreen.Bounds.Width;
            return section == 0 || FooterReuseID.IsNullOrEmpty() || ViewModel.BottomBanners == null || ViewModel.BottomBanners.Count() == 0 ?
                CGSize.Empty
                    :
                new CGSize(width, width * 9 / 16);
        }

        [Export("collectionView:layout:referenceSizeForHeaderInSection:")]
        public override CGSize GetReferenceSizeForHeader(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            var width = UIScreen.MainScreen.Bounds.Width;
            return section == 0 ? 
                ViewModel.TopBanners == null || BannersHeaderReuseID.IsNullOrEmpty() || ViewModel.TopBanners.Count() == 0 ?
                      CGSize.Empty
                          : 
                      new CGSize(width, width * 9 / 16)
                    :
                new CGSize(width, 44);
        }
    }
}
