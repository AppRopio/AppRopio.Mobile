using AppRopio.Base.Auth.Core.ViewModels.Auth;
using AppRopio.Base.Auth.iOS.Models;
using AppRopio.Base.Auth.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Auth.iOS.Views.Auth
{
    public partial class AuthViewController : CommonViewController<IAuthViewModel>
	{
		public AuthViewController()
			: base("AuthViewController", null)
		{
		}

		protected AuthThemeConfig ThemeConfig
		{ get { return Mvx.Resolve<IAuthThemeConfigService>().ThemeConfig; } }

		#region Protected

		#region InitializationControls

		protected virtual void SetupSignInBtn(UIButton button)
		{
			var theme = ThemeConfig;
			button.SetupStyle(ThemeConfig.Button);
		}

		protected virtual void SetupSignUpBtn(UIButton button)
		{
			button.SetupStyle(ThemeConfig.Button);
		}

		protected virtual void SetupSkipBtn(UIButton button)
		{
			button.SetupStyle(ThemeConfig.TextButton);
		}

		protected virtual void SetupVkBtn(UIButton button)
		{
			button.SetupStyle(ThemeConfig.VkButton);
		}

		protected virtual void SetupFacebookBtn(UIButton button)
		{
			button.SetupStyle(ThemeConfig.FacebookButton);
		}

		protected virtual void SetupImage(UIImageView imageView)
		{
			imageView.SetupStyle(ThemeConfig.AuthLogoImage);
			_logoImgWidth.Constant = imageView.Image.Size.Width;
			_logoImgHeight.Constant = imageView.Image.Size.Height;
		}

		#endregion

		#region BindingControls

		protected virtual void BindSignInBtn(UIButton button, MvxFluentBindingDescriptionSet<AuthViewController, IAuthViewModel> set)
		{
			set.Bind(button).To(vm => vm.NavigateToSignInCommand);
            set.Bind(button).For("Title").To(vm => vm.SignInText);
		}

		protected virtual void BindSignUpBtn(UIButton button, MvxFluentBindingDescriptionSet<AuthViewController, IAuthViewModel> set)
		{
			set.Bind(button).To(vm => vm.NavigateToSignUpCommand);
            set.Bind(button).For("Title").To(vm => vm.SignUpText);
		}

		protected virtual void BindSkipBtn(UIButton button, MvxFluentBindingDescriptionSet<AuthViewController, IAuthViewModel> set)
		{
			set.Bind(button).To(vm => vm.SkipAuthCommand);
            set.Bind(button).For("Title").To(vm => vm.SkipText);
		}

		protected virtual void BindVkBtn(UIButton button, MvxFluentBindingDescriptionSet<AuthViewController, IAuthViewModel> set)
		{
			set.Bind(button).To(vm => vm.VkCommand);
		}

		protected virtual void BindFacebookBtn(UIButton button, MvxFluentBindingDescriptionSet<AuthViewController, IAuthViewModel> set)
		{
			set.Bind(button).To(vm => vm.FacebookCommand);
		}

		#endregion

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
			SetupImage(_logoImg);
			SetupSignInBtn(_signInBtn);
			SetupSignUpBtn(_signUpBtn);
			SetupSkipBtn(_skipBtn);
			SetupVkBtn(_vkBtn);
			SetupFacebookBtn(_facebookBtn);
		}

		protected override void BindControls()
		{
			var set = this.CreateBindingSet<AuthViewController, IAuthViewModel>();

			BindSignInBtn(_signInBtn, set);
			BindSignUpBtn(_signUpBtn, set);
			BindSkipBtn(_skipBtn, set);
			BindVkBtn(_vkBtn, set);
			BindFacebookBtn(_facebookBtn, set);

			set.Apply();
		}

		protected override void CleanUp()
		{
			base.CleanUp();
			ReleaseDesignerOutlets();
		}

		#endregion

		#endregion

		#region Public

		public override void ViewWillAppear(bool animated)
		{
			NavigationController?.SetNavigationBarHidden(true, true);
			base.ViewWillAppear(animated);
		}

		#endregion

	}
}

