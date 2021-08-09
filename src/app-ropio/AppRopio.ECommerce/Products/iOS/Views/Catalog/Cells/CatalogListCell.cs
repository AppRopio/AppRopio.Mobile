using System;
using FFImageLoading.Cross;
using Foundation;
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

            if (image is MvxCachedImageView imageView)
            {
                imageView.LoadingPlaceholderImagePath = $"res:{ThemeConfig.Products.ProductCell.Image.Path}";
                imageView.ErrorPlaceholderImagePath = $"res:{ThemeConfig.Products.ProductCell.Image.Path}";

                set.Bind(imageView).For(i => i.ImagePath).To(vm => vm.ImageUrl);
            }
        }
    }
}
