using System.Runtime.CompilerServices;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.MultiSelection.Cells;
using CoreGraphics;
using Foundation;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.MultiSelection
{
    public class PDMultiSelectionCollectionViewSource : BaseCollectionViewSource
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public PDMultiSelectionCollectionViewSource(UICollectionView collectionView, NSString defaultIdentifier)
            : base(collectionView, defaultIdentifier)
        {
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var itemVm = GetItemAt(indexPath) as MultiCollectionItemVM;

            if (itemVm != null)
            {
                var label = new AppRopio.Base.iOS.Controls.ARLabel { Frame = new CGRect(0, 0, 0, PDMultiSelectionTextCell.LABEL_HEIGHT), Text = itemVm.ValueName };

                label.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MultiSelection.MultiSelectionCell.Value);
                label.SizeToFit();

                return new CGSize(label.Bounds.Width + PDMultiSelectionTextCell.HORIZONTAL_MARGINS, collectionView.Bounds.Height);
            }

            return new CGSize(74, collectionView.Bounds.Height);
        }
    }
}