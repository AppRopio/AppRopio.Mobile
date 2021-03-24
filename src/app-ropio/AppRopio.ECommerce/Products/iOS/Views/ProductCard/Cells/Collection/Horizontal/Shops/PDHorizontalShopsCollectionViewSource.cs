using System.Runtime.CompilerServices;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops.Items;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Shops.Cells;
using CoreGraphics;
using Foundation;
using MvvmCross.Platforms.Ios.Binding;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Shops
{
    public class PDHorizontalShopsCollectionViewSource : MvxCollectionViewSource
    {
        public PDHorizontalShopsCollectionViewSource(UICollectionView collectionView) 
            : base(collectionView)
        {
        }

        protected override UICollectionViewCell GetOrCreateCellFor(UICollectionView collectionView, NSIndexPath indexPath, object item)
        {
            var shopItem = item as IShopAvailabilityItemVM;
            return shopItem.DataType == AppRopio.Models.Products.Responses.ProductDataType.ShopsAvailability_Count ?
                           (UICollectionViewCell)collectionView.DequeueReusableCell(PDHShopsCountCell.Key, indexPath)
                               :
                           (UICollectionViewCell)collectionView.DequeueReusableCell(PDHShopIndicatorCell.Key, indexPath);
        }

		[Export("collectionView:layout:sizeForItemAtIndexPath:"), CompilerGenerated]
		public virtual CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
		{
            var item = GetItemAt(indexPath) as IShopAvailabilityItemVM;

			return new CoreGraphics.CGSize(
				PDHorizontalShopsCollectionCell.CELL_WIDTH,
				(item.DataType == AppRopio.Models.Products.Responses.ProductDataType.ShopsAvailability_Count ? PDHorizontalShopsCollectionCell.COUNT_HEIGHT : PDHorizontalShopsCollectionCell.INDICATOR_HEIGHT)
			);
		}
    }
}