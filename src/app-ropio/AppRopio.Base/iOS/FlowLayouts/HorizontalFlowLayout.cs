using System;
using CoreGraphics;
using UIKit;

namespace AppRopio.Base.iOS.FlowLayouts
{
    /// <summary>
    /// Для реализации пейджинга на коллекциях с отступами слева и справа
    /// </summary>
    public class HorizontalFlowLayout : UICollectionViewFlowLayout
    {
        nfloat MinContentOffset
        {
            get { return -CollectionView.ContentInset.Left; }
        }

        nfloat MaxContentOffset
        {
            get { return MinContentOffset + CollectionView.ContentSize.Width - ItemSize.Width; }
        }

        nfloat SnapStep
        {
            get { return ItemSize.Width + MinimumLineSpacing; }
        }

        bool IsValidOffset(nfloat offset)
        {
            return (offset >= MinContentOffset && offset <= MaxContentOffset);
        }

        public override CGPoint TargetContentOffset(CGPoint proposedContentOffset, CGPoint scrollingVelocity)
        {
            var offSetAdjustment = nfloat.MaxValue;
            var horizontalCenter = (nfloat)(proposedContentOffset.X + (this.CollectionView.Bounds.Size.Width / 2.0));

            var targetRect = new CGRect(proposedContentOffset.X, 0.0f, this.CollectionView.Bounds.Size.Width, this.CollectionView.Bounds.Size.Height);
            var array = base.LayoutAttributesForElementsInRect(targetRect);

            foreach (var layoutAttributes in array)
            {
                var itemHorizontalCenter = layoutAttributes.Center.X;
                if (Math.Abs(itemHorizontalCenter - horizontalCenter) < Math.Abs(offSetAdjustment))
                {
                    offSetAdjustment = itemHorizontalCenter - horizontalCenter;
                }
            }

            var nextOffset = proposedContentOffset.X + offSetAdjustment;

            do
            {
                proposedContentOffset.X = nextOffset;

                var deltaX = proposedContentOffset.X - CollectionView.ContentOffset.X;
                var velX = scrollingVelocity.X;

                if (Math.Sign(deltaX) * Math.Sign(velX) != -1)
                    break;

                nextOffset += Math.Sign(scrollingVelocity.X) * SnapStep;
            } while (IsValidOffset(nextOffset));

            return proposedContentOffset;
        }
    }
}
