using System;

using Foundation;
using UIKit;

namespace AppRopio.ECommerce.Marked.iOS.Views.Marked
{
    public partial class MarkedListCell : MarkedGridCell
    {
        public new static readonly NSString Key = new NSString("MarkedListCell");
        public new static readonly UINib Nib;

        protected override AppRopio.Base.iOS.Controls.ARLabel Name => _name;
        protected override AppRopio.Base.iOS.Controls.ARLabel Price => _price;
        protected override AppRopio.Base.iOS.Controls.ARLabel OldPrice => _oldPrice;
        protected override UIImageView Image => _image;
        protected override UICollectionView Badges => null;
        protected override NSLayoutConstraint BadgesWidthConstraint => null;
        protected override UIButton MarkButton => _markButton;

        static MarkedListCell()
        {
            Nib = UINib.FromName("MarkedListCell", NSBundle.MainBundle);
        }

        protected MarkedListCell(IntPtr handle) : base(handle)
        {
            
        }
    }
}
