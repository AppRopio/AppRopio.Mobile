using System.Collections.Generic;
using System.Linq;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using UIKit;
using System;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged
{
    public class CCategoriesPageViCoWrapper : ProductsViewController<ICCategoriesViewModel>
    {
        #region Fields

        private CCategoriesPageViewController _pageViewController;

        #endregion

        #region Protected

        protected void SetupBasketCartIndicator()
        {
            var config = Mvx.Resolve<IProductConfigService>().Config;
            if (config.Basket?.CartIndicator != null && Mvx.Resolve<IViewLookupService>().IsRegistered(config.Basket?.CartIndicator.TypeName))
            {
                var cartIndicatorView = ViewModel.CartIndicatorVM == null ? null : Mvx.Resolve<IMvxIosViewCreator>().CreateView(ViewModel.CartIndicatorVM) as UIView;

                if (cartIndicatorView != null)
                {
                    var list = new List<UIBarButtonItem>();

                    var cartIndicatorBarButton = new UIBarButtonItem(cartIndicatorView);

                    if (!NavigationItem.RightBarButtonItems.IsNullOrEmpty())
                        list.AddRange(NavigationItem.RightBarButtonItems);

                    list.Add(cartIndicatorBarButton);

                    NavigationItem.SetRightBarButtonItems(list.ToArray(), false);
                }
            }
        }

        #endregion

        #region CommonViewController implementation

        protected override void SetupRightBarButtonItems()
        {
            base.SetupRightBarButtonItems();

            SetupBasketCartIndicator();
        }

        protected override void BindControls()
        {
            _pageViewController = new CCategoriesPageViewController { ViewModel = ViewModel };

            //NOTE: it's very important to add page view controller as child before invoke any View methods and properties
            AddChildViewController(_pageViewController);

            _pageViewController.View.Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
            _pageViewController.ViewModel = ViewModel;

            View.Frame = new CoreGraphics.CGRect(0, -64, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height + 64);
            View.AddSubview(_pageViewController.View);
        }

        protected override void InitializeControls()
        {
            Title = "Каталог";
        }

        protected override void CleanUp()
        {
            _pageViewController.Unbind();
        }

        public override void ViewWillAppear(bool animated)
        {
            if (NavigationController.NavigationBarHidden)
                NavigationController.SetNavigationBarHidden(false, true);

            base.ViewWillAppear(animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _pageViewController.View.ChangeFrame(y: 64);
        }

        public override void ViewWillDisappear(bool animated)
        {
            _pageViewController.ViewWillDisappear(animated);

            base.ViewWillDisappear(animated);
        }

        public override void ViewWillLayoutSubviews()
        {
            if (View.Constraints.Any())
            {
                var topConstraint = View.Constraints.FirstOrDefault(x => (long)x.FirstAttribute == 33);
                var midYConstraint = View.Constraints.FirstOrDefault(x => (long)x.FirstAttribute == 35);
                var heightContraint = View.Constraints.FirstOrDefault(x => x.FirstAttribute == NSLayoutAttribute.Height);

                if (topConstraint != null)
                    topConstraint.Constant = -64;

                if (midYConstraint != null)
                    midYConstraint.Constant = (UIScreen.MainScreen.Bounds.Height / 2) - 64;

                if (heightContraint != null)
                    heightContraint.Constant = UIScreen.MainScreen.Bounds.Height;
            }

            base.ViewWillLayoutSubviews();

            View.Frame = new CoreGraphics.CGRect(0, -64, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height + 64);
        }

        #endregion
    }
}
