using System;
using UIKit;
using Foundation;
using MvvmCross.Binding.ExtensionMethods;

namespace AppRopio.Base.iOS.ViewSources
{
    public class InfinityCollectionViewSource : HorizontalPagingCollectionViewSource
    {
        private const int REPEATS_COUNT = 100;
        private int _repeatsCounter;

        private int _minVisibleItems = 1;
        public int MinVisibleItems 
        {
            get => _minVisibleItems;
            set
            {
                _minVisibleItems = value;
                ReloadData();
            }
        }

        public InfinityCollectionViewSource(UICollectionView collectionView, NSString cellIdentifier)
            : base (collectionView, cellIdentifier)
        {
        }

        protected override object GetItemAt(NSIndexPath indexPath)
        {
            var itemsCount = ItemsSource.Count();
            var row = indexPath.Row;

            _repeatsCounter = indexPath.Row / itemsCount;
            
            return ItemsSource.ElementAt(indexPath.Row - itemsCount * _repeatsCounter);
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            var itemsCount = ItemsSource?.Count() ?? 0;
            return ItemsSource == null || itemsCount == 0 ? 0 : (itemsCount <= MinVisibleItems ? itemsCount : itemsCount * REPEATS_COUNT);
        }
    }
}
