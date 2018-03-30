using System;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views.PageViewController;
using AppRopio.ECommerce.Products.iOS.Views.Catalog;
using Foundation;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged
{
    public class CCategoriesPagedViewController : CatalogViewController, IMvxPageViewController
    {
        private NSObject _navBarFrameChanged;

        public int PageIndex
        {
            get; set;
        }

        #region Protected

        protected override void SetupCollectionView(UIKit.UICollectionView collectionView)
        {
            base.SetupCollectionView(collectionView);

            if (ViewModel == null || ViewModel.VmNavigationType == Base.Core.Models.Navigation.NavigationType.None)
            {
                CollectionViewBottomConstraint.Constant = ThemeConfig.Categories?.TabCell?.Size.Height ?? (nfloat)0;

                if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                    collectionView.ContentInsetAdjustmentBehavior = UIScrollViewContentInsetAdjustmentBehavior.Never;
            }
        }

        protected override void SetupLoading()
        {
            base.SetupLoading();

            LoadingView.ChangeFrame(y: 0, h: LoadingView.Frame.Height);
        }

        #endregion

        #region Public

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (CollectionView != null && (ViewModel == null || ViewModel.VmNavigationType == Base.Core.Models.Navigation.NavigationType.None))
            {
                CollectionView.ContentInset = new UIEdgeInsets(64, 0, 0, 0);
                CollectionView.ScrollIndicatorInsets = new UIEdgeInsets(64, 0, 0, 0);
            }
        }

        #endregion
    }
}
