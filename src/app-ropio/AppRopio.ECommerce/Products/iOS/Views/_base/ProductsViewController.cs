using System;
using System.Linq;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views
{
    public abstract class ProductsViewController<T> : CommonViewController<T>
        where T : class, IProductsViewModel
    {
        protected virtual ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }
        protected virtual ProductsConfig Config { get { return Mvx.Resolve<IProductConfigService>().Config; } }

        protected ProductsViewController()
        {

        }

        protected ProductsViewController(IntPtr handle)
            : base(handle)
        {

        }

        protected ProductsViewController(string nibName, Foundation.NSBundle bundle)
            : base(nibName, bundle)
        {

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupRightBarButtonItems();

            if (Theme.ControlPalette.NavigationBar.UseCustomView && NavigationItem.TitleView != null && NavigationItem.TitleView.Subviews.Any())
            {
                NavigationItem.TitleView.Subviews.ForEach(subview =>
                {
                    subview.ChangeFrame(w: DeviceInfo.ScreenWidth - 44 - (NavigationItem.RightBarButtonItems?.Length ?? 0) * 52);
                });
            }

            Title = ViewModel?.Title;
        }

        protected virtual void SetupRightBarButtonItems()
        {
            if (ViewModel != null && ViewModel.SearchEnabled && ThemeConfig.NavBarSearch.Enabled
                && !ViewModel.SearchBar && Config.SearchType != SearchType.Disabled)
            {
                var searchButton = new UIBarButtonItem(
                    ImageCache.GetImage(ThemeConfig.NavBarSearch.Image.Path),
                    UIBarButtonItemStyle.Plain,
                    (sender, e) => ViewModel.ShowSearchCommand.Execute(null)
                );

                searchButton.AccessibilityIdentifier = nameof(searchButton);

                NavigationItem.SetRightBarButtonItem(searchButton, false);
            }
        }
    }
}
