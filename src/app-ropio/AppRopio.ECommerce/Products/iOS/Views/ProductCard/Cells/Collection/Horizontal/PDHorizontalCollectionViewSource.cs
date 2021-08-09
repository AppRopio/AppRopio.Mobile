using System.Runtime.CompilerServices;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Cells;
using CoreGraphics;
using Foundation;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal
{
    public class PDHorizontalCollectionViewSource : BaseCollectionViewSource
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public PDHorizontalCollectionViewSource(UICollectionView collectionView)
            : base(collectionView)
        {

        }

        protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, Foundation.NSIndexPath indexPath, object item)
        {
            var itemVm = item as CollectionItemVM;

            if (itemVm != null)
            {
                switch (itemVm.DataType)
                {
                    case AppRopio.Models.Products.Responses.ProductDataType.Color:
                        return (UICollectionViewCell)collectionView.DequeueReusableCell(PDHorizontalColorCell.Key, indexPath);
                    default:
                        return (UICollectionViewCell)collectionView.DequeueReusableCell(PDHorizontalTextCell.Key, indexPath);
                }
            }

            return null;
        }

        [Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
        public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
        {
            var itemVm = GetItemAt(indexPath) as CollectionItemVM;

            if (itemVm != null)
            {
                if (itemVm.DataType == AppRopio.Models.Products.Responses.ProductDataType.Text)
                {
                    var label = new AppRopio.Base.iOS.Controls.ARLabel { Text = itemVm.ValueName };

                    label.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Collection.Value);
                    label.SizeToFit();

                    return new CGSize(label.Bounds.Width + PDHorizontalTextCell.HORIZONTAL_MARGINS, collectionView.Bounds.Height);
                }
            }

            return new CGSize(40, collectionView.Bounds.Height);
        }
    }
}