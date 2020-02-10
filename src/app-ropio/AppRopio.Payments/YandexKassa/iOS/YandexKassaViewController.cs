using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.Payments.YandexKassa.Core;
using AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa;
using MvvmCross.Binding.BindingContext;

namespace AppRopio.Payments.YandexKassa.iOS
{
    public partial class YandexKassaViewController : CommonViewController<IYandexKassaViewModel>
    {
        public YandexKassaViewController() : base("YandexKassaViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(YandexKassaConstants.RESX_NAME, "Card_Payment");
        }

        protected override void BindControls()
        {
            var bindingSet = this.CreateBindingSet<YandexKassaViewController, IYandexKassaViewModel>();

            BindWebView(WebView, bindingSet);

            bindingSet.Apply();
        }

        #endregion

        #region BindingControls

        protected virtual void BindWebView(BindableWebView webView, MvxFluentBindingDescriptionSet<YandexKassaViewController, IYandexKassaViewModel> set)
        {
            set.Bind(webView).To(vm => vm.PaymentParams).For(v => v.HttpContent);
            set.Bind(webView).To(vm => vm.PaymentUrl).For(v => v.Url);
            set.Bind(webView).To(vm => vm.ShouldLoadCommand).For(v => v.ShouldLoadCommand);
        }

        #endregion
    }
}