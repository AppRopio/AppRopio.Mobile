using AppRopio.Base.Auth.Core;
using AppRopio.Base.Auth.Core.ViewModels.Thanks;
using AppRopio.Base.Auth.iOS.Models;
using AppRopio.Base.Auth.iOS.Services;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Auth.iOS.Views.Thanks
{
    public partial class ThanksViewController : CommonViewController<IThanksViewModel>
	{
		protected AuthThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IAuthThemeConfigService>().ThemeConfig; } }

		public ThanksViewController() : base("ThanksViewController", null)
		{
		}

		#region Protected

		#region InitializationControls

		protected virtual void SetupImageView(UIImageView imageView)
		{
			imageView.SetupStyle(ThemeConfig.ThanksImage);
			_imageWidth.Constant = imageView.Image.Size.Width;
			_imageHeight.Constant = imageView.Image.Size.Height;

		}

		protected virtual void SetupTitleLabel(UILabel label)
		{
			label.SetupStyle(ThemeConfig.Title);
            label.Text = Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "Thanks_Thanks");
		}

		protected virtual void SetupDesriptionLabel(UILabel label)
		{
			label.SetupStyle(ThemeConfig.Description);
            label.Text = Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "Thanks_Motivation");
		}

		protected virtual void SetupDoneButton(UIButton button)
		{
			button.SetupStyle(ThemeConfig.Button);
            button.WithTitleForAllStates(Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(AuthConst.RESX_NAME, "Thanks_Start"));
		}

		#endregion

		#region BindingControls

		protected virtual void BindDoneBtn(UIButton button, MvxFluentBindingDescriptionSet<ThanksViewController, IThanksViewModel> set)
		{
			set.Bind(button).To(vm => vm.StartCommand);
		}

		#endregion

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
			SetupImageView(_iconImage);
			SetupTitleLabel(_titleLabel);
			SetupDesriptionLabel(_descriptionLabel);
			SetupDoneButton(_doneButton);
		}

		protected override void BindControls()
		{
			var set = this.CreateBindingSet<ThanksViewController, IThanksViewModel>();
			BindDoneBtn(_doneButton, set);
			set.Apply();
		}

		protected override void CleanUp()
		{
			base.CleanUp();
			ReleaseDesignerOutlets();
		}

		#endregion

		#endregion

	}
}

