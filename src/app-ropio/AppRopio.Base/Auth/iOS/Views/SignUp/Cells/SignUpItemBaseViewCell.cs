using System;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;
using AppRopio.Base.Auth.iOS.Models;
using AppRopio.Base.Auth.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ValueConverters;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;
using AppRopio.Base.iOS.Controls;

namespace AppRopio.Base.Auth.iOS.Views.SignUp.Cells
{
    public partial class SignUpItemBaseViewCell : MvxTableViewCell
    {
        protected AuthThemeConfig ThemeConfig { get { return Mvx.Resolve<IAuthThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("SignUpItemBaseViewCell");
        public static readonly UINib Nib = UINib.FromName("SignUpItemBaseViewCell", NSBundle.MainBundle);

        public ARTextField TextField
        {
            get
            {
                return _textField;
            }
            set
            {
                _textField = value;
            }
        }

        protected SignUpItemBaseViewCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region Protected

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupTextField(_textField);
        }

        protected virtual void SetupTextField(UITextField textField)
        {
            textField.SetupStyle(ThemeConfig.TextField);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<SignUpItemBaseViewCell, ISignUpItemBaseViewModel>();

            BindTextField(_textField, set);

            set.Apply();
        }

        protected virtual void BindTextField(ARTextField textField, MvxFluentBindingDescriptionSet<SignUpItemBaseViewCell, ISignUpItemBaseViewModel> set)
        {
            set.Bind(textField).To(vm => vm.Value);
            set.Bind(textField)
               .For(p => p.Error)
               .To(vm => vm.Invalid);
        }

        #endregion

        #endregion
    }
}
