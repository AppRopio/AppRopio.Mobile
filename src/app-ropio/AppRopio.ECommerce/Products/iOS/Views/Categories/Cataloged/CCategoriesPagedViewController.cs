using System;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views.PageViewController;
using AppRopio.ECommerce.Products.Core.Models;
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

        protected override void SetupSearchView(UIView searchView)
        {
            base.SetupSearchView(searchView);

            if (ViewModel == null || ViewModel.VmNavigationType == Base.Core.Models.Navigation.NavigationType.None)
            {
                searchView.ChangeFrame(y: 44.0f);
            }
        }

        protected override void SetupCollectionView(UIKit.UICollectionView collectionView)
        {
            base.SetupCollectionView(collectionView);

            if (ViewModel == null || ViewModel.VmNavigationType == Base.Core.Models.Navigation.NavigationType.None)
            {
                CollectionViewBottomConstraint.Constant = ThemeConfig.Categories?.TabCell?.Size.Height ?? 0.0f;
                if (CollectionViewBottomConstraint.Constant > 0.0f)
                    CollectionViewBottomConstraint.Constant += DeviceInfo.SafeAreaInsets.Bottom;

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
                CollectionView.ContentInset = new UIEdgeInsets(44.0f, 0.0f, ThemeConfig.Categories?.TabCell?.Size.Height ?? 0.0f, 0.0f);
                CollectionView.ScrollIndicatorInsets = new UIEdgeInsets(44.0f.If_iPhoneX(0.0f), 0.0f, (ThemeConfig.Categories?.TabCell?.Size.Height ?? 0.0f) - DeviceInfo.SafeAreaInsets.Bottom, 0.0f);
            }
            if (Config.SearchType == SearchType.Bar)
            {
                var insets = CollectionView.ContentInset;
                CollectionView.ContentInset = insets.ChangeInsets(t: insets.Top + 56.0f);
            }
        }

        #endregion
    }
}
