using System;
using System.Linq;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Controls;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Loyalty.Core;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo;
using AppRopio.ECommerce.Loyalty.iOS.Services;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
using ObjCRuntime;
using UIKit;

namespace AppRopio.ECommerce.Loyalty.iOS.Views.Promo
{
    public partial class PromoCodeView : MvxView, IMvxIosView<IPromoCodeViewModel>
    {
        private ARTextField _accessoryTextField;

        public static readonly NSString Key = new NSString("PromoCodeView");
        public static readonly UINib Nib = UINib.FromName("PromoCodeView", NSBundle.MainBundle);

        protected Models.Promo PromoTheme { get { return Mvx.Resolve<ILoyaltyThemeConfigService>().ThemeConfig.Promo; } }

        #region Properties

        IMvxViewModel IMvxView.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (IPromoCodeViewModel)value; }
        }

        private MvxViewModelRequest _request;
        public MvxViewModelRequest Request
        {
            get { return _request; }
            set
            {
                _request = value;
                ViewModel = (IPromoCodeViewModel)((value as MvxViewModelInstanceRequest).ViewModelInstance);
                DataContext = ViewModel;
            }
        }

        public IPromoCodeViewModel ViewModel { get; set; }

        #endregion

        protected ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();

        #region Constructor

        public PromoCodeView()
        {
            InitializeControls();
            this.DelayBind(() => BindControls());
        }

        public PromoCodeView(IntPtr handle) 
            : base(handle)
        {
            
        }

        #endregion

        #region CommonViewController implementation

        protected virtual void InitializeControls()
        {
            Frame = new CGRect(0, 0, (nfloat)PromoTheme.CodeFieldSize.Width, (nfloat)PromoTheme.CodeFieldSize.Height);

            var arr = NSBundle.MainBundle.LoadNib(nameof(PromoCodeView), null, null);
            var viewFromNib = Runtime.GetNSObject<PromoCodeView>(arr.ValueAt(0));

            if (viewFromNib != null)
            {
                _textField = viewFromNib._textField;
                _textField.Placeholder = LocalizationService.GetLocalizableString(LoyaltyConstants.RESX_NAME, "Promocode_Placeholder");

                if (_textField != null)
                    SetupCodeField(_textField, _accessoryTextField = new ARTextField() { Frame = _textField.Frame, Placeholder = LocalizationService.GetLocalizableString(LoyaltyConstants.RESX_NAME, "Promocode_Placeholder") });

                viewFromNib.Frame = Bounds;
                viewFromNib.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

                AddSubview(viewFromNib);
            }
        }

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PromoCodeView, IPromoCodeViewModel>();

            BindCodeField(_textField, _accessoryTextField, set);

            set.Apply();
        }

        #endregion

        #region InitializationControls

        protected virtual void SetupCodeField(UITextField textField, UITextField accessoryTextField)
        {
            textField.SetupStyle(PromoTheme.CodeField);
            textField.ClearButtonMode = UITextFieldViewMode.WhileEditing;

            textField.ReturnKeyType = UIReturnKeyType.Done;
            textField.ShouldReturn = (sender) => 
            {
                this.EndEditing(true);
                return true;
            };

            accessoryTextField.SetupStyle(PromoTheme.CodeField);
            accessoryTextField.ClearButtonMode = UITextFieldViewMode.Always;

            accessoryTextField.ReturnKeyType = UIReturnKeyType.Done;
            accessoryTextField.ShouldReturn = (sender) => 
            {
                accessoryTextField.ResignFirstResponder();
                this.EndEditing(true);
                return true;
            };

            var heightConstr = textField.Constraints.FirstOrDefault(x => x.FirstAttribute == NSLayoutAttribute.Height);
            if (heightConstr != null)
                heightConstr.Constant = (nfloat)PromoTheme.CodeFieldSize.Height;
            else
                textField.HeightAnchor.ConstraintEqualTo((nfloat)PromoTheme.CodeFieldSize.Height).Active = true;
            
            textField.InputAccessoryView = new UIView()
                .WithFrame(new CGRect(0, 0, DeviceInfo.ScreenWidth, accessoryTextField.Bounds.Height))
                .WithBackground(Theme.ColorPalette.Background.ToUIColor())
                .WithSubviews(accessoryTextField);
        }

        #endregion

        #region BindingControls

        protected void BindCodeField(UITextField textField, UITextField accessoryTextField, MvxFluentBindingDescriptionSet<PromoCodeView, IPromoCodeViewModel> set)
        {
            set.Bind(textField).To(vm => vm.Code);
            set.Bind(textField).For("EditingDidEnd").To(vm => vm.ApplyCommand);

            set.Bind(accessoryTextField).To(vm => vm.Code);
            set.Bind(accessoryTextField).For("EditingDidEnd").To(vm => vm.ApplyCommand);
        }

        #endregion
    }
}
