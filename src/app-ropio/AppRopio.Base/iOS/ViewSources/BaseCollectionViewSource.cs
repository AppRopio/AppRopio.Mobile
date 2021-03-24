using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Input;
using AppRopio.Base.iOS.Helpers;
using Foundation;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding;
using UIKit;
using System.Linq;

namespace AppRopio.Base.iOS.ViewSources
{
    public class BaseCollectionViewSource : MvxCollectionViewSource
    {
        #region Fields

        private readonly ShyNavBarController _shyNavBarController;

        #endregion

        #region Commands

        public ICommand MoreCommand { get; set; }

        #endregion

        #region Properties

        protected int FromBottomCellStartLoadingIndex { get; set; } = 4;

        public bool DeselectAutomatically { get; set; }

        #endregion

        #region Constructor

        public BaseCollectionViewSource(UICollectionView collectionView)
            : base(collectionView)
        {
            _shyNavBarController = new ShyNavBarController();
        }

        public BaseCollectionViewSource(UICollectionView collectionView, NSString cellIdentifier)
            : base(collectionView, cellIdentifier)
        {
            _shyNavBarController = new ShyNavBarController();
        }

        #endregion

        #region Protected

        protected override void CollectionChangedOnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            if (args.Action == NotifyCollectionChangedAction.Remove)
            {
                try
                {
                    CollectionView.PerformBatchUpdates(() =>
                    {
                        var startIndex = args.OldStartingIndex;
                        var indexes = new NSIndexPath[args.OldItems.Count];
                        for (var i = 0; i < indexes.Length; i++)
                            indexes[i] = NSIndexPath.FromRowSection(startIndex + i, 0);
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
                                    indexes[i] = NSIndexPath.FromRowSection(startIndex + i, 0);
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
                                indexes[i] = NSIndexPath.FromRowSection(startIndex + i, 0);
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
                        indexes.Add(NSIndexPath.FromRowSection(i, 0));

                    CollectionView.ReloadItems(indexes.ToArray());
                }, ok => { });
            }
            else
            {
                base.CollectionChangedOnCollectionChanged(sender, args);
            }
        }

        #endregion

        #region Public

        public override void WillDisplayCell(UICollectionView collectionView, UICollectionViewCell cell, NSIndexPath indexPath)
        {
            if ((indexPath.Row >= (ItemsSource.Count() - FromBottomCellStartLoadingIndex)) && MoreCommand != null && MoreCommand.CanExecute(null))
                MoreCommand.Execute(null);
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            base.ItemSelected(collectionView, indexPath);

            if (DeselectAutomatically)
                collectionView.DeselectItem(indexPath, true);
        }

        #region NavigationBar Hidden Magic

        public override void Scrolled(UIScrollView scrollView) =>
            _shyNavBarController.Scrolled(scrollView);

        public override void DecelerationEnded(UIScrollView scrollView) =>
            _shyNavBarController.DecelerationEnded();

        public override void DraggingEnded(UIScrollView scrollView, bool willDecelerate) =>
            _shyNavBarController.DraggingEnded(willDecelerate);

        /// <summary>
        /// Sets the top offset scrollView of navigaiton bar. You must invoke this method before EnableHideNavBarOnSwype
        /// </summary>
        public void SetTopOffsetOfNavBar(float yOffset) =>
            _shyNavBarController?.SetTopOffsetOfNavBar(yOffset);

        public void EnableHideNavBarOnSwype(UINavigationBar navigationBar, UICollectionView scrollView, bool setupContentInset = true) =>
            _shyNavBarController?.EnableHideNavBarOnSwype(navigationBar, scrollView, setupContentInset);

        public void DisableHideNavBarOnSwype() =>
            _shyNavBarController?.DisableHideNavBarOnSwype();

        #endregion

        #endregion
    }
}
