using AppRopio.Base.Information.Core.ViewModels.InformationWebContent;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using MvvmCross.Binding.BindingContext;

namespace AppRopio.Base.Information.iOS.Views.InformationWebContent
{
    public partial class InformationWebContentViewController : CommonViewController<IInformationWebContentViewModel>
    {
        public InformationWebContentViewController() : base("InformationWebContentViewController", null)
        {
        }

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
		}

		protected override void BindControls()
		{
			var bindingSet = this.CreateBindingSet<InformationWebContentViewController, IInformationWebContentViewModel>();

			BindTitle(bindingSet);
            BindWebView(WebView, bindingSet);

			bindingSet.Apply();
		}

		#endregion

		#region BindingControls

		protected virtual void BindTitle(MvxFluentBindingDescriptionSet<InformationWebContentViewController, IInformationWebContentViewModel> set)
		{
			set.Bind(this).To(vm => vm.Title).For(v => v.Title);
		}

		protected virtual void BindWebView(BindableWebView webView, MvxFluentBindingDescriptionSet<InformationWebContentViewController, IInformationWebContentViewModel> set)
		{
            set.Bind(webView).To(vm => vm.HtmlContent).For(v => v.Text);
		}

		#endregion
	}
}