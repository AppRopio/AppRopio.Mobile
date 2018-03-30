using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace AppRopio.Payments.Best2Pay.iOS.Views
{
    public partial class Best2PayViewController : CommonViewController<IBest2PayViewModel>
    {
        public Best2PayViewController() : base("Best2PayViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = "Оплата картой";
        }

        protected override void BindControls()
        {
            var bindingSet = this.CreateBindingSet<Best2PayViewController, IBest2PayViewModel>();

            BindWebView(WebView, bindingSet);

            bindingSet.Apply();
        }

        #endregion

        #region BindingControls

        protected virtual void BindWebView(BindableWebView webView, MvxFluentBindingDescriptionSet<Best2PayViewController, IBest2PayViewModel> set)
        {
			set.Bind(webView).For(w => w.Url).To(vm => vm.PaymentUrl);
            set.Bind(webView).For(w => w.ShouldLoadCommand).To(vm => vm.ShouldLoadCommand);
        }

        #endregion
    }
}