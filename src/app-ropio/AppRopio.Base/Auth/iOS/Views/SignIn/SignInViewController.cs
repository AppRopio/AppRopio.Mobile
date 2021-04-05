using System;
using AppRopio.Base.Auth.Core;
using AppRopio.Base.Auth.Core.ViewModels.SignIn;
using AppRopio.Base.Auth.iOS.Views._base;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace AppRopio.Base.Auth.iOS.Views.SignIn
{
    public partial class SignInViewController : AuthBaseViewController<ISignInViewModel>
    {
        #region Properties

        protected override string AccessoryButtonTitle
        {
            get
            {
                return LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignIn_SignIn");
            }
        }

        #endregion

        #region Constructor

        public SignInViewController()
            : base("SignInViewController", null)
        {

        }

        #endregion

        #region Private

        private void DismissKeyboard()
        {
            _identityField?.ResignFirstResponder();
            _passwordField?.ResignFirstResponder();
            SetViewsState(false);
        }

        #endregion 

        #region Protected

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
                _signInButton.Alpha = 0;
                UIView.Animate(ShortAnimationDuration, () =>
                {
                    _iconImage.Alpha = 0;
                    _headerView.Alpha = 0;
                });

                UIView.Animate(StandardAnimationDuration, () =>
                {
                    _headerView.Hidden = true;
                    _signInButton.Hidden = true;
                });
            }
            else
            {
                UIView.Animate(StandardAnimationDuration, () =>
                 {
                     _iconImage.Alpha = 1;
                     _headerView.Alpha = 1;
                     _signInButton.Alpha = 1;
                     _headerView.Hidden = false;
                     _signInButton.Hidden = false;
                 });
            }
        }

        #region InitializationControls

        protected virtual void SetupImage(UIImageView imageView)
        {
            imageView.SetupStyle(ThemeConfig.SignInImage);
            _imageWidth.Constant = imageView.Image.Size.Width;
            _imageHeight.Constant = imageView.Image.Size.Height;
        }

        protected virtual void SetupTitleLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.Title);
            label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignIn_Join");
        }

        protected virtual void SetupDescriptionLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.Description);
            label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignIn_Motivation");
        }

        protected virtual void SetupIdentityField(UITextField field, UITextField nextField, bool identifyUserByEmail)
        {
            field.SetupStyle(ThemeConfig.TextField);

            if (identifyUserByEmail)
            {
                field.Placeholder = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignIn_EmailPlaceholder");;
                field.KeyboardType = UIKeyboardType.EmailAddress;
            }
            else
            {
                field.Placeholder = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignIn_PhonePlaceholder");;
                field.KeyboardType = UIKeyboardType.NumberPad;
                field.AutocorrectionType = UITextAutocorrectionType.No;
            }

            field.ShouldBeginEditing += (textField) =>
            {
                SetViewsState(true);
                return true;
            };

            field.ShouldEndEditing += (textField) =>
            {
                return true;
            };

            field.InputAccessoryView = _accessoryButton;

            field.ShouldReturn += (textField) =>
            {
                nextField?.BecomeFirstResponder();
                return true;
            };
        }

        protected virtual void SetupPasswordField(UITextField field)
        {
            field.SetupStyle(ThemeConfig.TextField);

            field.Placeholder = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignIn_PasswordPlaceholder");

            field.ShouldReturn += (textField) =>
            {
                _passwordField?.ResignFirstResponder();
                SetViewsState(false);
                return true;
            };

            field.ShouldBeginEditing += (textField) =>
            {
                SetViewsState(true);
                return true;
            };

            field.InputAccessoryView = _accessoryButton;

            field.SetupSecurity();
        }

        protected virtual void SetupSignInBtn(UIButton button)
        {
            button.SetupStyle(ThemeConfig.Button);
            button.TouchUpInside += OnNextButtonClick;
            button.WithTitleForAllStates(LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignIn_SignIn"));
        }

        protected virtual void SetupResetBtn(UIButton button)
        {
            button.SetupStyle(ThemeConfig.TextButton);
            button.WithTitleForAllStates(LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignIn_RestorePassword"));
            button.TouchUpInside += (sender, e) => DismissKeyboard();
        }

        #endregion

        #region BindingControls

        protected virtual void BindIdentityField(UITextField field, MvxFluentBindingDescriptionSet<SignInViewController, ISignInViewModel> set, bool identifyUserByEmail)
        {
            if (identifyUserByEmail)
                set.Bind(field).To(vm => vm.Identity);
            else
                set.Bind(field).For("PhoneBinding").To(vm => vm.Identity);
        }

        protected virtual void BindPasswordField(UITextField field, MvxFluentBindingDescriptionSet<SignInViewController, ISignInViewModel> set)
        {
            set.Bind(field).To(vm => vm.Password);
        }

        protected virtual void BindResetPasswordBtn(UIButton button, MvxFluentBindingDescriptionSet<SignInViewController, ISignInViewModel> set)
        {
            set.Bind(button).To(vm => vm.RecoveryPassCommand);
        }

        protected virtual void BindSignInBtn(UIButton button, MvxFluentBindingDescriptionSet<SignInViewController, ISignInViewModel> set)
        {
            set.Bind(button).To(vm => vm.SignInCommand);
            set.Bind(button).For(p => p.Enabled).To(vm => vm.PropertiesValid);
        }

        protected virtual void BindAccessoryInBtn(UIButton button, MvxFluentBindingDescriptionSet<SignInViewController, ISignInViewModel> set)
        {
            set.Bind(button).To(vm => vm.SignInCommand);
            set.Bind(button).For(p => p.Enabled).To(vm => vm.PropertiesValid);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            base.InitializeControls();

            Title = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "SignIn_Title");
            SetupImage(_iconImage);
            SetupTitleLabel(_titleLabel);
            SetupDescriptionLabel(_descriptionLabel);
            SetupIdentityField(_identityField, _passwordField, Config.IdentifyUserByEmail);
            SetupPasswordField(_passwordField);
            SetupSignInBtn(_signInButton);
            SetupResetBtn(_resetPasswordButton);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<SignInViewController, ISignInViewModel>();

            BindIdentityField(_identityField, set, Config.IdentifyUserByEmail);

            BindPasswordField(_passwordField, set);

            BindResetPasswordBtn(_resetPasswordButton, set);

            BindSignInBtn(_signInButton, set);

            BindAccessoryInBtn(_accessoryButton, set);

            set.Apply();
        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();
        }

        #endregion

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

