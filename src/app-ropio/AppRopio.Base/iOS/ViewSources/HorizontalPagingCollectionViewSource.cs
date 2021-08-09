using System;
using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace AppRopio.Base.iOS.ViewSources
{
    public class HorizontalPagingCollectionViewSource : MvxCollectionViewSource
    {
        public bool AutoScrollAfterChangingPage { get; set; }

        private int _page;
        public int Page
        {
            get => _page;
            set
            {
                if (value != _page)
                {
                    _page = value;

                    if (AutoScrollAfterChangingPage)
                        ScrollTo(value);
                }
            }
        }

        public event EventHandler PageChanged;

        public HorizontalPagingCollectionViewSource(UICollectionView collectionView, NSString cellIdentifier)
            : base(collectionView, cellIdentifier)
        {
        }

        protected virtual void ScrollTo(int page)
        {
            CollectionView.ScrollToItem(NSIndexPath.FromItemSection(page, 0), UICollectionViewScrollPosition.CenteredHorizontally, true);
        }

        public override void DecelerationEnded(UIScrollView scrollView)
        {
            var currentPage = (int)Math.Round(scrollView.ContentOffset.X / (CollectionView.Bounds.Width - CollectionView.ContentInset.Left - CollectionView.ContentInset.Right));

            _page = currentPage;

            PageChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
