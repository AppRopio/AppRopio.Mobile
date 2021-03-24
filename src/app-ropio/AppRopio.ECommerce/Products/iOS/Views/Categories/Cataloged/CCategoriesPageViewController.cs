using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.iOS.Views.PageViewController.Delegate;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged.Cells;
using AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged.ViewSources;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;
using System.Linq;
using AppRopio.ECommerce.Products.Core;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged
{
    public class CCategoriesPageViewController : CommonPageViewController<ICCategoriesViewModel>
    {
        private NSObject _navBarFrameChanged;

        protected UICollectionView _collectionView;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public CCategoriesPageViewController()
            : base(UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation.None)
        {
        }

        #region Protected

        #region InitializationControls

        protected virtual void SetupCategoriesCollection(UICollectionView collectionView)
        {
            collectionView.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();
            collectionView.RegisterNibForCell(CCategoryCell.Nib, CCategoryCell.Key);
            collectionView.ShowsHorizontalScrollIndicator = false;

            collectionView.Layer.SetupStyle(ThemeConfig.Categories.TabLayer);

            View.AddSubview(collectionView);
        }

        #endregion

        #region BindingControls

        protected virtual void BindCategoriesCollection(UICollectionView collectionView, MvxFluentBindingDescriptionSet<CCategoriesPageViewController, ICCategoriesViewModel> set)
        {
            var dataSource = new CategoriesCollectionViewSource(collectionView, CCategoryCell.Key);
            collectionView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
            set.Bind(dataSource).For(dS => dS.SelectedIndex).To(vm => vm.CurrentPage);

            collectionView.ReloadData();

            var scrollView = View.Subviews.FirstOrDefault(x => x is UIScrollView) as UIScrollView;
            if (scrollView != null)
            {
                scrollView.Delegate = new PageControlScrollViewDelegate
                {
                    OnScrolled = dataSource.OnExternalScroll
                };
            }
        }

        protected virtual void BindViewControlls(MvxFluentBindingDescriptionSet<CCategoriesPageViewController, ICCategoriesViewModel> set)
        {
            var dataSource = new CategoriesPageViewSource(this);
            var viewDelegate = new MvxPageViewControllerDelegate(dataSource);

            set.Bind(dataSource).For(dS => dS.ItemSource).To(vm => vm.Pages);
            set.Bind(dataSource).For(dS => dS.CurrentPage).To(vm => vm.CurrentPage);
            set.Bind(dataSource).For(dS => dS.PageChangedCommand).To(vm => vm.PageChangedCommand);

            this.DataSource = dataSource;
            this.Delegate = viewDelegate;
        }

        #endregion

        #region CommonPageViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "CategoriesEmptyTitle");

            this.AutomaticallyAdjustsScrollViewInsets = false;

            SetupCategoriesCollection(_collectionView = new UICollectionView(
                new CGRect(0, DeviceInfo.ScreenHeight - (nfloat)ThemeConfig.Categories.TabCell.Size.Height, DeviceInfo.ScreenWidth, (nfloat)ThemeConfig.Categories.TabCell.Size.Height),
                new UICollectionViewFlowLayout
                {
                    EstimatedItemSize = new CGSize((nfloat)ThemeConfig.Categories.TabCell.Size.Width, (nfloat)ThemeConfig.Categories.TabCell.Size.Height),
                    ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                    MinimumInteritemSpacing = 0,
                    MinimumLineSpacing = 0
                }
            ));


        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<CCategoriesPageViewController, ICCategoriesViewModel>();

            BindCategoriesCollection(_collectionView, set);

            BindViewControlls(set);

            set.Apply();
        }

        protected override void CleanUp()
        {
            if (_collectionView != null)
            {
                _collectionView.Source?.Dispose();
                _collectionView.Source = null;

                _collectionView.Dispose();
                _collectionView = null;
            }

            (DataSource as CategoriesPageViewSource)?.UnbindCachedViewControllers();
        }

        #endregion

        protected virtual void ShowNavBarOnDisapear()
        {
            if (NavigationController != null)
            {
                var navBarFrame = NavigationController.NavigationBar.Frame;
                if (navBarFrame.Y < 20)
                {
                    navBarFrame.Y = 20;
                    NavigationController.NavigationBar.Frame = navBarFrame;
                }
            }
        }

        #endregion

        #region Public

        public override void ViewWillDisappear(bool animated)
        {
            ShowNavBarOnDisapear();

            base.ViewWillDisappear(animated);
        }

        private class PageControlScrollViewDelegate : UIScrollViewDelegate
        {
            public Action<UIScrollView> OnScrolled { get; set; }
            
            public override void Scrolled(UIScrollView scrollView)
            {
                OnScrolled?.Invoke(scrollView);
            }
        }

        #endregion
    }
}

