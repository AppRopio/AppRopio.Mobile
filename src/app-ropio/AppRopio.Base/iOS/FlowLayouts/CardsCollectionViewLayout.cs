using System;
using CoreGraphics;
using UIKit;
using Foundation;
using System.Linq;
namespace AppRopio.Base.iOS.FlowLayouts
{
    public class CardsCollectionViewLayout : UICollectionViewFlowLayout
    {
        public override CoreGraphics.CGSize ItemSize
        {
            get
            {
                return base.ItemSize;
            }
            set
            {
                if (base.ItemSize != value)
                {
                    base.ItemSize = value;
                    InvalidateLayout();
                }
            }
        }

        public override nfloat MinimumInteritemSpacing
        {
            get
            {
                return base.MinimumInteritemSpacing;
            }
            set
            {
                if (base.MinimumInteritemSpacing != value)
                {
                    base.MinimumInteritemSpacing = value;
                    InvalidateLayout();
                }
            }
        }

        private int _maximumVisibleItems = 4;
        public int MaximumVisibleItems
        {
            get => _maximumVisibleItems;
            set
            {
                if (_maximumVisibleItems != value)
                {
                    _maximumVisibleItems = value;
                    InvalidateLayout();
                }
            }
        }

        public override CoreGraphics.CGSize CollectionViewContentSize
        {
            get
            {
                var itemsCount = CollectionView.NumberOfItemsInSection(0);
                return ScrollDirection == UICollectionViewScrollDirection.Horizontal ?
                    new CGSize(CollectionView.Bounds.Width * itemsCount, CollectionView.Bounds.Height)
                        :
                    new CGSize(CollectionView.Bounds.Width, CollectionView.Bounds.Height * itemsCount);
            }
        }

        public override void PrepareLayout()
        {
            base.PrepareLayout();

            if (CollectionView.NumberOfSections() != 1)
                Console.WriteLine("Multiple sections aren't supported!");
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var totalItemsCount = (int)CollectionView.NumberOfItemsInSection(0);

            var minVisibleIndex = Math.Max(0, (int)(CollectionView.ContentOffset.X / CollectionView.Bounds.Width));
            var maxVisibleIndex = Math.Min(minVisibleIndex + MaximumVisibleItems, totalItemsCount);

            var contentCenterX = (float)(CollectionView.ContentOffset.X + (CollectionView.Bounds.Width / 2.0f));
            var deltaOffset = (int)(CollectionView.ContentOffset.X % CollectionView.Bounds.Width);
            var percentageDeltaOffset = (float)((float)deltaOffset / CollectionView.Bounds.Width);

            var visibleIndexes = Enumerable.Range(minVisibleIndex, maxVisibleIndex - minVisibleIndex);

            var attributes = visibleIndexes.Select(index =>
            {
                var indexPath = NSIndexPath.FromItemSection(index, 0);
                return ComputeLayoutAttributesForItem(indexPath: indexPath,
                                     minVisibleIndex: minVisibleIndex,
                                     contentCenterX: contentCenterX,
                                     deltaOffset: deltaOffset,
                                     percentageDeltaOffset: percentageDeltaOffset);
            }).ToArray();

            return attributes;
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(Foundation.NSIndexPath indexPath)
        {
            var minVisibleIndex = (int)(CollectionView.ContentOffset.X / CollectionView.Bounds.Width);

            var contentCenterX = (float)(CollectionView.ContentOffset.X + (CollectionView.Bounds.Width / 2.0f));
            var deltaOffset = (int)(CollectionView.ContentOffset.X % CollectionView.Bounds.Width);
            var percentageDeltaOffset = (float)((float)deltaOffset / CollectionView.Bounds.Width);

            return ComputeLayoutAttributesForItem(indexPath: indexPath,
                                           minVisibleIndex: minVisibleIndex,
                                           contentCenterX: contentCenterX,
                                           deltaOffset: deltaOffset,
                                           percentageDeltaOffset: percentageDeltaOffset);
        }

        public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
        {
            return true;
        }

        #region Layout computations

        private float Scale(int index)
        {
            //var translatedCoefficient = index - MaximumVisibleItems / 2.0f;
            //return (float)Math.Pow(0.9, translatedCoefficient);
            var translatedCoefficient = (float)(index - MaximumVisibleItems) / (float)MaximumVisibleItems;
            return (float)Math.Pow(0.9, index);
        }

        private CGAffineTransform Transform(int visibleIndex, float percentageOffset)
        {
            var rawScale = visibleIndex < MaximumVisibleItems ? Scale(visibleIndex) : 1.0f;

            if (visibleIndex != 0)
            {
                var previousScale = Scale(visibleIndex - 1);
                var delta = (previousScale - rawScale) * percentageOffset;
                rawScale += delta;
            }

            return CGAffineTransform.MakeScale(rawScale, rawScale);
        }

        private UICollectionViewLayoutAttributes ComputeLayoutAttributesForItem(NSIndexPath indexPath, int minVisibleIndex, float contentCenterX, float deltaOffset, float percentageDeltaOffset)
        {
            var attributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
            var visibleIndex = indexPath.Row - minVisibleIndex;

            attributes.Size = ItemSize;

            var midY = CollectionView.Bounds.GetMidY();

            var center = new CGPoint(x: contentCenterX/* + MinimumInteritemSpacing * visibleIndex*/,
                                     y: midY - MinimumInteritemSpacing * visibleIndex * 3f);

            attributes.ZIndex = MaximumVisibleItems - visibleIndex;
            attributes.Transform = Transform(visibleIndex, percentageDeltaOffset);

            if (visibleIndex == 0)
                center.X -= deltaOffset;
            else if (visibleIndex > 0 && visibleIndex < MaximumVisibleItems - 1)
            {
                //center.X -= MinimumInteritemSpacing * percentageDeltaOffset;
                center.Y += MinimumInteritemSpacing * percentageDeltaOffset * 3;

                var maxAlpha = 1f - 0.15f * visibleIndex;
                attributes.Alpha = Math.Min(1f, Math.Max(percentageDeltaOffset, maxAlpha));
            }
            else
            {
                center.Y += MinimumInteritemSpacing * percentageDeltaOffset * 3;

                var maxAlpha = 1f - 0.15f * visibleIndex;
                attributes.Alpha = Math.Max(maxAlpha * percentageDeltaOffset, 0); 
            }

            attributes.Center = center;

            return attributes;
        }

        #endregion
    }
}
