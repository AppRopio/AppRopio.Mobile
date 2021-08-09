using System;
using AppRopio.Base.iOS;
using AppRopio.Feedback.Core.ViewModels.Items.Text;
using AppRopio.Feedback.iOS.Models;
using AppRopio.Feedback.iOS.Services;
using Foundation;
using MvvmCross.Binding;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.Feedback.iOS.Views.ReviewPost.Cell.Text
{
    public partial class TextCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("TextCell");
        public static readonly UINib Nib;

		protected FeedbackThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFeedbackThemeConfigService>().ThemeConfig; } }

        public UIView FieldInputAccessoryView { get; set; }

		static TextCell()
        {
            Nib = UINib.FromName("TextCell", NSBundle.MainBundle);
        }

        protected TextCell(IntPtr handle) : base(handle)
        {
			this.DelayBind(() =>
			{
				InitializeControls();
				BindControls();
			});
        }

		#region InitializationControls

		protected virtual void InitializeControls()
		{
            SetupTextView(TextView);
		}

        protected virtual void SetupTextView(UITextView textView)
        {
            textView.SetupStyle(ThemeConfig.ReviewPost.Text);
            textView.InputAccessoryView = this.FieldInputAccessoryView;
        }

		#endregion

		#region BindingControls

		protected virtual void BindControls()
		{
			var bindingSet = this.CreateBindingSet<TextCell, ITextItemVm>();

            BindText(TextView, bindingSet);

			bindingSet.Apply();
		}

        protected virtual void BindText(UITextView text, MvxFluentBindingDescriptionSet<TextCell, ITextItemVm> set)
		{
            set.Bind(text).To(vm => vm.Text).TwoWay();
		}

		#endregion
	}
}