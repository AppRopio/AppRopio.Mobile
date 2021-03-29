using System;
using AppRopio.Base.Auth.Core.ViewModels.Password.Reset.Sms;
using AppRopio.Base.Auth.iOS.Views._base;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;
using AppRopio.Base.Auth.Core;

namespace AppRopio.Base.Auth.iOS.Views.Password.Reset.Sms
{
    public partial class ResetSmsViewController : AuthBaseViewController<IResetPasswordSmsViewModel>
    {
        protected override string AccessoryButtonTitle
        {
            get
            {
                return LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Sms_ConfirmCode");
            }
        }

        protected ILocalizationService LocalizationService => Mvx.IoCProvider.Resolve<ILocalizationService>();

        public ResetSmsViewController() 
            : base("ResetSmsViewController", null)
        {
        }

        #region Protected

        protected override void OnNextButtonClick(object sender, EventArgs e)
        {
            _verificationCodeField?.ResignFirstResponder();
            SetViewsState(false);
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
                    _iconImage.Hidden = true;
                    _bottomView.Hidden = true;
                    _spacingView.Hidden = true;
                });
            }
            else
            {
                UIView.Animate(StandardAnimationDuration, () =>
                 {
                     _iconImage.Hidden = false;
                     _bottomView.Hidden = false;
                     _iconImage.Alpha = 1;
                     _bottomView.Alpha = 1;
                     _spacingView.Hidden = false;
                 });
            }
        }

        #region InitializationControls

        protected virtual void SetupImage(UIImageView imageView)
        {
            imageView.SetupStyle(ThemeConfig.ResetPasswordSendedImage);
            _imageWidth.Constant = imageView.Image.Size.Width;
            _imageHeight.Constant = imageView.Image.Size.Height;
        }

        protected virtual void SetupTitleLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.Title);
            label.Text = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Sms_CodeText");
        }

        protected virtual void SetupDescriptionLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.Description); //text биндится
        }

        protected virtual void SetupVerificationCodeField(UITextField field)
        {
            field.SetupStyle(ThemeConfig.TextField);

            field.Placeholder = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Sms_CodePlaceholder");

            field.InputAccessoryView = _accessoryButton;

            field.ShouldReturn += (textField) =>
            {
                field?.ResignFirstResponder();
                SetViewsState(false);
                return true;
            };

            field.ShouldBeginEditing += (textField) =>
             {
                 SetViewsState(true);
                 return true;
             };
        }

        protected virtual void SetupResendCodeButton(UIButton button)
        {
            button.SetupStyle(ThemeConfig.TextButton);

            var paragraphStyle = new NSMutableParagraphStyle();
            paragraphStyle.Alignment = UITextAlignment.Center;

            var resendTitle = new NSAttributedString(LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Sms_SentAgain"),
                                                     font: (UIFont)ThemeConfig.TextButton.Font,
                                                     foregroundColor: button.TitleLabel.TextColor,
                                                     paragraphStyle: paragraphStyle);

            button.WithAttributedTitleForAllStates(resendTitle);
        }

        protected virtual void SetupValidateCodeButton(UIButton button)
        {
            button.TouchUpInside += OnNextButtonClick;
            button.SetupStyle(ThemeConfig.Button);
            button.WithTitleForAllStates(AccessoryButtonTitle);
        }

        #endregion

        #region BindingControls

        protected virtual void BindVerificationCodeField(UITextField field, MvxFluentBindingDescriptionSet<ResetSmsViewController, IResetPasswordSmsViewModel> set)
        {
            set.Bind(field).To(vm => vm.VerifyCode);
        }

        protected virtual void BindDescriptionLabel(UILabel label, MvxFluentBindingDescriptionSet<ResetSmsViewController, IResetPasswordSmsViewModel> set)
        {
            set.Bind(label).To(vm => vm.DescriptionWithPhone);
        }


        protected virtual void BindValidateCodeBtn(UIButton button, MvxFluentBindingDescriptionSet<ResetSmsViewController, IResetPasswordSmsViewModel> set)
        {
            set.Bind(button).To(vm => vm.ValidateCodeCmd);
            set.Bind(button).For(s => s.Enabled).To(vm => vm.PropertiesValid);
        }

        protected virtual void BindAccessoryBtn(UIButton button, MvxFluentBindingDescriptionSet<ResetSmsViewController, IResetPasswordSmsViewModel> set)
        {
            set.Bind(button).To(vm => vm.ValidateCodeCmd);
            set.Bind(button).For(s => s.Enabled).To(vm => vm.PropertiesValid);
        }

        protected virtual void BindResendBtn(UIButton button, MvxFluentBindingDescriptionSet<ResetSmsViewController, IResetPasswordSmsViewModel> set)
        {
            set.Bind(button).To(vm => vm.ResendCodeCmd);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            base.InitializeControls();

            Title = LocalizationService.GetLocalizableString(AuthConst.RESX_NAME, "Password_Sms_Title");

            SetupImage(_iconImage);

            SetupTitleLabel(_titleLabel);

            SetupDescriptionLabel(_descriptionLabel);

            SetupResendCodeButton(_resendCodeButton);

            SetupVerificationCodeField(_verificationCodeField);

            SetupValidateCodeButton(_validateCodeButton);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<ResetSmsViewController, IResetPasswordSmsViewModel>();

            BindVerificationCodeField(_verificationCodeField, set);
            BindDescriptionLabel(_descriptionLabel, set);

            BindAccessoryBtn(_accessoryButton, set);
            BindValidateCodeBtn(_validateCodeButton, set);
            BindResendBtn(_resendCodeButton, set);

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
            base.ViewWillAppear(animated);
            NavigationController?.SetNavigationBarHidden(false, true);
            SetViewsState(false);
        }

        #endregion
    }
}

