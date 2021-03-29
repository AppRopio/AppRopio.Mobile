using System;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Main;
using AppRopio.Base.Auth.iOS.Views._base;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using MvvmCross.Binding.BindingContext;
using UIKit;
using MvvmCross;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Auth.Core;

namespace AppRopio.Base.Auth.iOS.Views.Password.Reset.Main
{
    public partial class ResetPasswordViewController : AuthBaseViewController<IResetPasswordViewModel>
    {
        #region Properties

        private bool _showEmailSended;
        public bool ShowEmailSended
        {
            get
            {
                return _showEmailSended;
            }
            set
            {
                _showEmailSended = value;
                OnShowEmailSendedExecute(value);
            }
        }

        protected override string AccessoryButtonTitle
        {
            get
            {
                return LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_GetCode");
            }
        }

        #endregion

        protected ILocalizationService LocalizationService => Mvx.IoCProvider.Resolve<ILocalizationService>();

        #region Constructor

        public ResetPasswordViewController() : base("ResetPasswordViewController", null)
        {
        }

        #endregion

        #region Private

        private void OnShowEmailSendedExecute(bool show)
        {
            if (show)
            {
                if (NavigationController?.NavigationBar != null)
                    NavigationController.SetNavigationBarHidden(true, false);
                if (_emailSendedView != null && _mainStackView != null)
                {
                    _identityField?.ResignFirstResponder();
                    SetViewsState(false);
                    _emailSendedView.Alpha = 0;
                    _mainStackView.Alpha = 0.7f;
                    _emailSendedView.Hidden = false;

                    UIView.Animate(0.3f, () =>
                     {
                         _emailSendedView.Alpha = 1;
                         _mainStackView.Alpha = 0;

                     }, () =>
                     {
                         _mainStackView.Hidden = true;
                     });
                }
            }
            else
            {
                if (NavigationController?.NavigationBar != null)
                    NavigationController.SetNavigationBarHidden(false, false);
                if (_emailSendedView != null && _mainStackView != null)
                {
                    SetViewsState(false);
                    _emailSendedView.Hidden = true;
                    _mainStackView.Hidden = false;
                    _mainStackView.Alpha = 1;
                }
            }

            OnShowEmailSended(show);
        }

        private void DismissKeyboard()
        {
            _identityField?.ResignFirstResponder();
            SetViewsState(false);
        }

        #endregion

        #region Protected

        protected virtual void OnShowEmailSended(bool show)
        {

        }

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
                    _iconImage.Alpha = 0;
                });

                UIView.Animate(StandardAnimationDuration, () =>
                {
                    _headerIconImageView.Hidden = true;
                    _bottomView.Hidden = true;
                    _spacingView.Hidden = true;
                });
            }
            else
            {
                UIView.Animate(StandardAnimationDuration, () =>
                 {
                     _iconImage.Alpha = 1;
                     _bottomView.Alpha = 1;
                     _headerIconImageView.Hidden = false;
                     _bottomView.Hidden = false;
                     _spacingView.Hidden = false;
                 });
            }
        }

        #region InitializationControls

        protected virtual void SetupImage(UIImageView imageView)
        {
            imageView.SetupStyle(ThemeConfig.ResetPasswordImage);
            _imageWidth.Constant = imageView.Image.Size.Width;
            _imageHeight.Constant = imageView.Image.Size.Height;
        }

        protected virtual void SetupEmailImage(UIImageView imageView)
        {
            imageView.SetupStyle(ThemeConfig.ResetPasswordSendedImage);
            _emailImageWidth.Constant = imageView.Image.Size.Width;
            _emailImageHeight.Constant = imageView.Image.Size.Height;
        }

        protected virtual void SetupTitleLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.Title);
            label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_Forgot");
        }

        protected virtual void SetupDescriptionLabel(UILabel label, bool identifyUserByEmail)
        {
            label.SetupStyle(ThemeConfig.Description);

            if (identifyUserByEmail)
                label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_MotivationEmail");
            else
                label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_MotivationPhone");
        }

        protected virtual void SetupIdentityField(UITextField field, bool identifyUserByEmail)
        {
            field.SetupStyle(ThemeConfig.TextField);

            if (identifyUserByEmail)
            {
                field.Placeholder = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_EmailPlaceholder");
                field.KeyboardType = UIKeyboardType.EmailAddress;
            }
            else
            {
                field.Placeholder = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_PhonePlaceholder");
                field.KeyboardType = UIKeyboardType.NumberPad;
                field.AutocorrectionType = UITextAutocorrectionType.No;
            }

            field.InputAccessoryView = _accessoryButton;

            field.ShouldBeginEditing += (textField) =>
             {
                 SetViewsState(true);
                 return true;
             };

            field.ShouldReturn += (textField) =>
            {
                field?.ResignFirstResponder();
                SetViewsState(false);
                return true;
            };
        }

        protected virtual void SetupGetPasswordBtn(UIButton button, bool identifyUserByEmail)
        {
            button.SetupStyle(ThemeConfig.Button);
            button.TouchUpInside += OnNextButtonClick;
            if (!identifyUserByEmail)
                button = button.WithTitleForAllStates(LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_GetCode"));
            else
                button = button.WithTitleForAllStates(LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_GetPassword"));
        }

        protected virtual void SetupEmailTitleLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.Title);
            label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_PasswordSent");
        }

        protected virtual void SetupEmailDescriptionLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.Description);
            label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_EmailInsctruction");
        }

        protected virtual void SetupEmailDoneBtn(UIButton button)
        {
            button.SetupStyle(ThemeConfig.Button);
            button.WithTitleForAllStates(LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_Close"));
        }

        protected virtual void SetupEmailSendedView(UIView view)
        {
            view.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();
        }

        #endregion

        #region BindingControls

        protected virtual void BindIdentityBtn(UITextField field, MvxFluentBindingDescriptionSet<ResetPasswordViewController, IResetPasswordViewModel> set, bool identifyUserByEmail)
        {
            if (identifyUserByEmail)
                set.Bind(field).To(vm => vm.Identity);
            else
                set.Bind(field).For("PhoneBinding").To(vm => vm.Identity);
        }

        protected virtual void BindAccessoryBtn(UIButton button, MvxFluentBindingDescriptionSet<ResetPasswordViewController, IResetPasswordViewModel> set)
        {
            set.Bind(button).To(vm => vm.ForgotCmd);
            set.Bind(button).For(s => s.Enabled).To(vm => vm.PropertiesValid);
        }

        protected virtual void BindGetPasswordBtn(UIButton button, MvxFluentBindingDescriptionSet<ResetPasswordViewController, IResetPasswordViewModel> set)
        {
            set.Bind(button).To(vm => vm.ForgotCmd);
            set.Bind(button).For(s => s.Enabled).To(vm => vm.PropertiesValid);
        }

        protected virtual void BindEmailDoneBtn(UIButton button, MvxFluentBindingDescriptionSet<ResetPasswordViewController, IResetPasswordViewModel> set)
        {
            set.Bind(button).To(vm => vm.CloseCmd);
        }

        #endregion

        #region CommonViewController implementation

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<ResetPasswordViewController, IResetPasswordViewModel>();

            BindIdentityBtn(_identityField, set, Config.IdentifyUserByEmail);
            BindAccessoryBtn(_accessoryButton, set);
            BindGetPasswordBtn(_getPasswordButton, set);
            BindEmailDoneBtn(_emailDoneButton, set);

            set.Bind().For(p => p.ShowEmailSended).To(vm => vm.SuccessViewVisible);

            set.Apply();
        }

        protected override void InitializeControls()
        {
            base.InitializeControls();

            Title = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Reset_Title");

            SetupImage(_iconImage);
            SetupEmailImage(_emailIconImage);
            SetupTitleLabel(_titleLabel);
            SetupDescriptionLabel(_descriptionLabel, Config.IdentifyUserByEmail);
            SetupIdentityField(_identityField, Config.IdentifyUserByEmail);
            SetupGetPasswordBtn(_getPasswordButton, Config.IdentifyUserByEmail);

            SetupEmailTitleLabel(_emailTitleLabel);
            SetupEmailDescriptionLabel(_emailDescriptionLabel);
            SetupEmailDoneBtn(_emailDoneButton);
            SetupEmailSendedView(_emailSendedView); SetViewsState(false);
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
            base.ViewWillAppear(animated);
            NavigationController?.SetNavigationBarHidden(false, true);
        }

        #endregion

    }
}

