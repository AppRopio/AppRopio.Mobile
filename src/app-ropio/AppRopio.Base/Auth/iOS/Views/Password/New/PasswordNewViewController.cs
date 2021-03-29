using System;
using AppRopio.Base.Auth.Core.ViewModels.Password.New;
using AppRopio.Base.Auth.iOS.Views._base;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;
using AppRopio.Base.Auth.Core.Models.Config;
using AppRopio.Base.Auth.Core;

namespace AppRopio.Base.Auth.iOS.Views.Password.New
{
	public partial class PasswordNewViewController : AuthBaseViewController<IPasswordNewViewModel>
	{
		#region Properties

		protected override string AccessoryButtonTitle
		{
			get
			{
                return LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_New_Done");
			}
		}

		#endregion

        protected ILocalizationService LocalizationService => Mvx.IoCProvider.Resolve<ILocalizationService>();

		#region Constructor

		public PasswordNewViewController() : base("PasswordNewViewController", null)
		{
		}

		#endregion

		#region Private

		private void DismissKeyboard()
		{
			_passwordField.ResignFirstResponder();
			_passwordConfirmField.ResignFirstResponder();
            SetViewsState(false);
		}

		#endregion

		#region Protected

		#region InitializationControls

		protected virtual void SetupImage(UIImageView imageView)
		{
			imageView.SetupStyle(ThemeConfig.NewPasswordImage);
			_imageWidth.Constant = imageView.Image.Size.Width;
			_imageHeight.Constant = imageView.Image.Size.Height;
		}

		protected virtual void SetupDoneBtn(UIButton button)
		{
			button.SetupStyle(ThemeConfig.Button);
            button.WithTitleForAllStates(LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_New_Done"));
			button.TouchUpInside += OnNextButtonClick;
		}

		protected virtual void SetupTitleLabel(UILabel label)
		{
			label.SetupStyle(ThemeConfig.Title);
            label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_New_NewPassword");
		}

		protected virtual void SetupDescriptionLabel(UILabel label)
		{
			label.SetupStyle(ThemeConfig.Description);
            label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_New_Motivation");
		}

		protected virtual void SetupPasswordField(UITextField field, UITextField nextField = null)
		{
			field.SetupStyle(ThemeConfig.TextField);

            field.Placeholder = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_New_NewPasswordPlaceholder");

			field.InputAccessoryView = _accessoryButton;

			field.SetupSecurity();

			field.ShouldReturn += (textField) =>
			 {
				 nextField?.BecomeFirstResponder();
				 return true;
			 };
		}

		protected virtual void SetupPasswordConfirmField(UITextField field)
		{
			field.SetupStyle(ThemeConfig.TextField);

            field.Placeholder = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_New_ConfirmPasswordPlaceholder");

			field.InputAccessoryView = _accessoryButton;

			field.SetupSecurity();

			field.ShouldReturn += (textField) =>
			 {
				 _passwordConfirmField?.ResignFirstResponder();
				 SetViewsState(false);
				 return true;
			 };
		}

		#endregion

		#region BindingControls

		protected virtual void BindDoneBtn(UIButton button, MvxFluentBindingDescriptionSet<PasswordNewViewController, IPasswordNewViewModel> set)
		{
			set.Bind(button).To(vm => vm.DoneCommand);
			set.Bind(button).For(s => s.Enabled).To(vm => vm.PropertiesValid);
		}

		protected virtual void BindAccessoryButtonBtn(UIButton button, MvxFluentBindingDescriptionSet<PasswordNewViewController, IPasswordNewViewModel> set)
		{
			set.Bind(button).To(vm => vm.DoneCommand);
			set.Bind(button).For(s => s.Enabled).To(vm => vm.PropertiesValid);
		}

		protected virtual void BindPasswordField(UITextField field, MvxFluentBindingDescriptionSet<PasswordNewViewController, IPasswordNewViewModel> set)
		{
			set.Bind(field).To(vm => vm.Password);
		}

		protected virtual void BindPasswordConfirmField(UITextField field, MvxFluentBindingDescriptionSet<PasswordNewViewController, IPasswordNewViewModel> set)
		{
			set.Bind(field).To(vm => vm.PasswordConfirm);
		}

		#endregion

		#region CommonViewController implementation

		protected override void BindControls()
		{
			var set = this.CreateBindingSet<PasswordNewViewController, IPasswordNewViewModel>();

			BindDoneBtn(_doneButton, set);
			BindAccessoryButtonBtn(_accessoryButton, set);
			BindPasswordField(_passwordField, set);
			BindPasswordConfirmField(_passwordConfirmField, set);

			set.Apply();
		}

		protected override void InitializeControls()
		{
			base.InitializeControls();

            Title = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_New_Title");

			SetupImage(_iconImage);
			SetupDoneBtn(_doneButton);
			SetupTitleLabel(_titleLabel);
			SetupDescriptionLabel(_descriptionLabel);
			SetupPasswordField(_passwordField, _passwordConfirmField);
			SetupPasswordConfirmField(_passwordConfirmField);
		}

		protected override void CleanUp()
		{
			base.CleanUp();
			ReleaseDesignerOutlets();
		}

		#endregion

		protected override void OnNextButtonClick(object sender, EventArgs e)
		{
			DismissKeyboard();
		}

		protected virtual void SetViewsState(bool keyboardShown)
		{
			if (_lastKeyboardShownState == keyboardShown)
				return;
			_lastKeyboardShownState = keyboardShown;
			if (keyboardShown)
			{
				_bottomView.Alpha = 0;
				UIView.Animate(ShortAnimationDuration, () =>
				{
					_headerView.Alpha = 0;
				});

				UIView.Animate(StandardAnimationDuration, () =>
				{
					_headerView.Hidden = true;
					_bottomView.Hidden = true;
				});
			}
			else
			{
				_headerView.Hidden = false;
				_bottomView.Hidden = false;
				_headerView.Alpha = 0;
				_bottomView.Alpha = 0;
				UIView.Animate(StandardAnimationDuration, () =>
				 {
					 _headerView.Alpha = 1;
					 _bottomView.Alpha = 1;
				 });
			}
		}

		#endregion

		#region Public

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			NavigationController?.SetNavigationBarHidden(false, true);
		}

		#endregion

	}
}

