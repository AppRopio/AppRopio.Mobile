using System;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.StepByStep
{
    public class SSCategoriesCollectionViewSource : SupplementaryCollectionViewSource
    {
        public ISSCategoriesViewModel ViewModel { get; }

        public SSCategoriesCollectionViewSource(ISSCategoriesViewModel viewModel, UICollectionView collectionView, NSString cellIdentifier)
            : base(collectionView, cellIdentifier, viewModel, viewModel)
        {
            ViewModel = viewModel;
        }

        [Export("collectionView:layout:referenceSizeForFooterInSection:")]
        public override CGSize GetReferenceSizeForFooter(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            var width = UIScreen.MainScreen.Bounds.Width;
            return FooterReuseID.IsNullOrEmpty() || ViewModel.BottomBanners == null || ViewModel.BottomBanners.Count == 0 ?
                CGSize.Empty
                    :
                new CGSize(width, width * 9 / 16);
        }

        [Export("collectionView:layout:referenceSizeForHeaderInSection:")]
        public override CGSize GetReferenceSizeForHeader(UICollectionView collectionView, UICollectionViewLayout layout, nint section)
        {
            var width = UIScreen.MainScreen.Bounds.Width;
            return ViewModel.TopBanners == null || HeaderReuseID.IsNullOrEmpty() || ViewModel.TopBanners.Count == 0 ?
                CGSize.Empty
                    :
                new CGSize(width, width * 9 / 16);
        }
    }
}