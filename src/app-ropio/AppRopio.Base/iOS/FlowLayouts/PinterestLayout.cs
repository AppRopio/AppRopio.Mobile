using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AppRopio.Base.iOS.FlowLayouts
{
    public interface IPinterestLayoutDelegate
    {
        CGSize SizeForCellAtIndexPath(UICollectionView collectionView, NSIndexPath indexPath);
    }

    public class PinterestLayoutAttributes : UICollectionViewLayoutAttributes
    {
        public nfloat AnnotationHeight { get; set; } = 0;

        public nfloat AnnotationWidth { get; set; } = 0;

        public override NSObject Copy(NSZone zone)
        {
            var copy = base.Copy(zone) as PinterestLayoutAttributes;
            copy.AnnotationHeight = AnnotationHeight;
            copy.AnnotationWidth = AnnotationWidth;
            return copy;
        }

        public override bool IsEqual(NSObject anObject)
        {
            var attributes = anObject as PinterestLayoutAttributes;
            if (AnnotationHeight == attributes.AnnotationHeight && AnnotationWidth == attributes.AnnotationWidth)
                return base.IsEqual(anObject);
            return false;
        }
    }

    [Register("PinterestLayout")]
    public class PinterestLayout : UICollectionViewFlowLayout
    {
        private List<UICollectionViewLayoutAttributes> _cache = new List<UICollectionViewLayoutAttributes>();

        private nfloat _contentHeight = 0;

        private nfloat ContentWidth
        {
            get
            {
                var insets = CollectionView.ContentInset;
                return CollectionView.Bounds.Width - (insets.Left + insets.Right);
            }
        }

        public IPinterestLayoutDelegate Delegate { get; set; }

        public int NumberOfColumns { get; set; } = 2;

        public override CGSize CollectionViewContentSize
        {
            get
            {
                return new CGSize(ContentWidth, _contentHeight);
            }
        }

        public PinterestLayout()
        {

        }

        public PinterestLayout(IntPtr handle)
            : base(handle)
        {

        }

        public override void InvalidateLayout()
        {
            _contentHeight = 0;
            _cache = new List<UICollectionViewLayoutAttributes>();

            base.InvalidateLayout();
        }

        public override void PrepareLayout()
        {
            if (_cache.IsNullOrEmpty())
            {
                var cellSizes = new CGSize[CollectionView.NumberOfItemsInSection(0)];

                for (int index = 0; index < cellSizes.Length; index++)
                {
                    var indexPath = NSIndexPath.FromRowSection(index, 0);

                    var cellSize = Delegate?.SizeForCellAtIndexPath(CollectionView, indexPath) ?? CGSize.Empty;

                    cellSizes[index] = cellSize;
                }

                var column = 0;
                var yOffset = new nfloat[NumberOfColumns];

                var supplementaryAttr = UICollectionViewLayoutAttributes.CreateForSupplementaryView(UICollectionElementKindSection.Header, NSIndexPath.FromRowSection(0, 0));
                supplementaryAttr.Frame = new CGRect(CGPoint.Empty, HeaderReferenceSize);
                _cache.Add(supplementaryAttr);

                //add begin Y offset to each column
                for (int i = 0; i < NumberOfColumns; i++)
                {
                    yOffset[i] = HeaderReferenceSize.Height == 0 ? 0 : HeaderReferenceSize.Height + MinimumLineSpacing / 2;
                }


                var previousCellPoint = CGPoint.Empty;

                for (int index = 0; index < CollectionView.NumberOfItemsInSection(0); index++)
                {
                    CGSize previousCellSize = index == 0 ? CGSize.Empty : cellSizes[index - 1];
                    CGSize currentCellSize = cellSizes[index];

                    var width = currentCellSize.Width + (MinimumInteritemSpacing * 2);
                    var height = currentCellSize.Height + MinimumLineSpacing;

                    //check that width of next cell in column [1; NumberOfColumns) does not go beyond the borders of content width
                    var previuosCellsWidth = previousCellPoint.X + previousCellSize.Width + MinimumInteritemSpacing;
                    if (column != 0 && previuosCellsWidth + width > ContentWidth)
                    {
                        //add Y offset to current column
                        yOffset[column] = yOffset[column] + height;

                        //switch column number to zero
                        column = 0;
                        previousCellPoint = CGPoint.Empty;
                    }

                    //get cell real frame
                    var frame = new CGRect(column == 0 ? 0 : previuosCellsWidth, yOffset[column], width, height + MinimumLineSpacing);
                    var insetFrame = frame.Inset(MinimumInteritemSpacing, MinimumLineSpacing);

                    //get cell atribute for frame
                    var indexPath = NSIndexPath.FromRowSection(index, 0);
                    var attributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
                    attributes.Frame = insetFrame;

                    //add attribute to cache
                    _cache.Add(attributes);

                    //update content height
                    _contentHeight = (nfloat)Math.Max(_contentHeight, frame.Bottom);

                    //add Y offset to column
                    yOffset[column] = yOffset[column] + height;

                    //save previous cell point
                    previousCellPoint = column >= (NumberOfColumns - 1) ? CGPoint.Empty : new CGPoint(insetFrame.X, insetFrame.Y);

                    //increment column number
                    column = column >= (NumberOfColumns - 1) ? 0 : ++column;
                }
            }
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
        {
            var layoutAttributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);

            var cachedAttrs = _cache.FirstOrDefault(x => x.IndexPath == indexPath && x.RepresentedElementCategory == UICollectionElementCategory.Cell);
            if (cachedAttrs != null)
                layoutAttributes = cachedAttrs;

            return layoutAttributes;
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForSupplementaryView(NSString kind, NSIndexPath indexPath)
        {
            var layoutAttributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);

            var cachedAttrs = _cache.FirstOrDefault(x => x.IndexPath == indexPath && x.RepresentedElementCategory == UICollectionElementCategory.SupplementaryView);
            if (cachedAttrs != null)
                layoutAttributes = cachedAttrs;

            return layoutAttributes;
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var layoutAttributes = new List<UICollectionViewLayoutAttributes>();

            foreach (var attributes in _cache)
            {
                if (attributes.Frame.IntersectsWith(rect))
                {
                    layoutAttributes.Add(attributes);
                }
            }

            return layoutAttributes.ToArray();
        }
    }
}
