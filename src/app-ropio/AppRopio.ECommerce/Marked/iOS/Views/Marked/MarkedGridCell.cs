using System;
using AppRopio.ECommerce.Marked.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells;
using Foundation;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Marked.iOS.Views.Marked
{
    public partial class MarkedGridCell : CatalogGridCell
    {
        public new static readonly NSString Key = new NSString("MarkedGridCell");
        public new static readonly UINib Nib;

        protected override ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IMarkedThemeConfigService>().ThemeConfig; } }

        protected override AppRopio.Base.iOS.Controls.ARLabel Name => _name;
        protected override AppRopio.Base.iOS.Controls.ARLabel Price => _price;
        protected override AppRopio.Base.iOS.Controls.ARLabel OldPrice => _oldPrice;
        protected override UIImageView Image => _image;
        protected override UICollectionView Badges => _badges;
        protected override NSLayoutConstraint BadgesWidthConstraint => _badgesWidthContraint;
        protected override UIButton MarkButton => _markButton;

		static MarkedGridCell()
        {
            Nib = UINib.FromName("MarkedGridCell", NSBundle.MainBundle);
        }

        public MarkedGridCell(IntPtr handle)
            : base(handle)
        {
		}
	}
}
