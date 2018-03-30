using AppRopio.Base.Information.Core.ViewModels.InformationTextContent;
using AppRopio.Base.iOS.Views;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace AppRopio.Base.Information.iOS.Views.InformationTextContent
{
    public partial class InformationTextContentViewController : CommonViewController<IInformationTextContentViewModel>
    {
        public InformationTextContentViewController() : base("InformationTextContentViewController", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
        }

        protected override void BindControls()
        {
            var bindingSet = this.CreateBindingSet<InformationTextContentViewController, IInformationTextContentViewModel>();

            BindTitle(bindingSet);
            BindText(TextView, bindingSet);

            bindingSet.Apply();
        }

        #endregion

        #region BindingControls

        protected virtual void BindTitle(MvxFluentBindingDescriptionSet<InformationTextContentViewController, IInformationTextContentViewModel> set)
        {
            set.Bind(this).To(vm => vm.Title).For(v => v.Title);
        }

        protected virtual void BindText(UITextView textView, MvxFluentBindingDescriptionSet<InformationTextContentViewController, IInformationTextContentViewModel> set)
		{
            set.Bind(textView).To(vm => vm.Text).For(v => v.Text);
		}

        #endregion
    }
}