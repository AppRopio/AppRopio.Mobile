using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductTextContent;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductTextContent
{
    public partial class ProductTextContentViewController : CommonViewController<IProductTextContentViewModel>
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public ProductTextContentViewController()
            : base("ProductTextContentViewController", null)
        {
        }

        #region Protected

        #region InitializationControls

        protected virtual void SetupTextView(UITextView textView)
        {
            textView.SetupStyle(ThemeConfig.ProductDetails.TextContent);
        }

        #endregion

        #region BindingControls

        protected virtual void BindTextView(UITextView textView, MvxFluentBindingDescriptionSet<ProductTextContentViewController, IProductTextContentViewModel> set)
        {
            set.Bind(textView).To(vm => vm.Text);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            SetupTextView(_textView);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<ProductTextContentViewController, IProductTextContentViewModel>();

            set.Bind(this).For(vc => vc.Title).To(vm => vm.Title);

            BindTextView(_textView, set);

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

