using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells
{
    public partial class CatalogListCell : CatalogGridCell
    {
        public new static readonly NSString Key = new NSString("CatalogListCell");
        public new static readonly UINib Nib;

        protected override AppRopio.Base.iOS.Controls.ARLabel Name => _name;
        protected override AppRopio.Base.iOS.Controls.ARLabel Price => _price;
        protected override AppRopio.Base.iOS.Controls.ARLabel OldPrice => _oldPrice;
        protected override UIImageView Image => _image;
        protected override UICollectionView Badges => null;
        protected override NSLayoutConstraint BadgesWidthConstraint => null;
        protected override UIButton MarkButton => _markButton;

        static CatalogListCell()
        {
            Nib = UINib.FromName("CatalogListCell", NSBundle.MainBundle);
        }

        protected CatalogListCell(IntPtr handle) 
            : base(handle)
        {
            
        }

        protected override void BindImage(UIImageView image, MvvmCross.Binding.BindingContext.MvxFluentBindingDescriptionSet<CatalogGridCell, Core.ViewModels.Catalog.Items.ICatalogItemVM> set)
        {
            if (image == null)
                return;

            var imageLoader = new MvxImageViewLoader(() => image)
            {
                DefaultImagePath = $"res:{ThemeConfig.Products.ProductCell.Image.Path}",
                ErrorImagePath = $"res:{ThemeConfig.Products.ProductCell.Image.Path}"
            };

            set.Bind(imageLoader).To(vm => vm.ImageUrl);
        }
    }
}
