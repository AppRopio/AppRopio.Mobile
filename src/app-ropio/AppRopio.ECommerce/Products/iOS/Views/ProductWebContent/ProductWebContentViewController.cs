using System;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductWebContent;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductWebContent
{
    public partial class ProductWebContentViewController : CommonViewController<IProductWebContentViewModel>
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public ProductWebContentViewController()
            : base("ProductWebContentViewController", null)
        {
        }

        #region Protected

        #region InitializationControls

        protected virtual void SetupWebView(BindableWebView webView)
        {

        }

        #endregion

        #region BindingControls

        protected virtual void BindWebView(BindableWebView webView, MvxFluentBindingDescriptionSet<ProductWebContentViewController, IProductWebContentViewModel> set)
        {
            set.Bind(_webView).For(v => v.Text).To(vm => vm.HtmlContent);
            set.Bind(_webView).For(v => v.Url).To(vm => vm.Url);
            set.Bind(_webView).For(v => v.LoadFinishedCommand).To(vm => vm.LoadFinishedCommand);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            SetupWebView(_webView);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<ProductWebContentViewController, IProductWebContentViewModel>();

            set.Bind(this).For(vc => vc.Title).To(vm => vm.Title);

            BindWebView(_webView, set);

            set.Apply();
        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();

            base.CleanUp();
        }

        #endregion

        #endregion
    }
}
