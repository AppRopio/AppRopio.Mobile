using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Basket.iOS.Models;
using AppRopio.ECommerce.Basket.iOS.Services;
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
using AppRopio.Base.Core.Services.Localization;
using AppRopio.ECommerce.Basket.Core;

namespace AppRopio.ECommerce.Basket.iOS.Views.ProductCard
{
    public partial class BasketProductCardView : MvxView, IMvxIosView<IBasketProductCardViewModel>
    {
        #region Fields

        private AppRopio.Base.iOS.Controls.ARLabel _unitName;

        private UIButton _accessoryDecrementBtn;
        private UIButton _accessoryIncrementBtn;
        private UITextField _accessoryQuantityTextField;
        private UIView _accessoryStepperView;
        private AppRopio.Base.iOS.Controls.ARLabel _accessoryUnitName;
        private UIButton _accessoryQuantityChangedBtn;

        #endregion

        #region Properties

        protected BasketThemeConfig ThemeConfig { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig; } }

        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (IBasketProductCardViewModel)value;
            }
        }

        private MvxViewModelRequest _request;
        public MvxViewModelRequest Request
        {
            get
            {
                return _request;
            }
            set
            {
                _request = value;
                ViewModel = (IBasketProductCardViewModel)((value as MvxViewModelInstanceRequest).ViewModelInstance);
                DataContext = ViewModel;
            }
        }

        public IBasketProductCardViewModel ViewModel { get; set; }

        protected ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();

        #endregion

        #region Constructor

        public BasketProductCardView()
        {
            InitializeControls();
            this.DelayBind(BindControls);
        }

        public BasketProductCardView(IntPtr handle)
            : base(handle)
        {

        }

        #endregion

        #region Protected

        #region InitializationControls

        private void InitializeControls()
        {
            Frame = GetFrame();

            var arr = NSBundle.MainBundle.LoadNib(nameof(BasketProductCardView), null, null);
            var viewFromNib = Runtime.GetNSObject<BasketProductCardView>(arr.ValueAt(0));

            _buyButton = viewFromNib._buyButton;
            _stepperView = viewFromNib._stepperView;
            _quantityTextField = viewFromNib._quantityTextField;
            _unitName = viewFromNib._unitName ?? new AppRopio.Base.iOS.Controls.ARLabel(new CGRect(0, 0, 44, 44));
            _decrementBtn = viewFromNib._decrementBtn;
            _incrementBtn = viewFromNib._incrementBtn;

            SetupBuyButton(_buyButton);
            SetupStepperView(_stepperView, _quantityTextField, _unitName, _decrementBtn, _incrementBtn);
            SetupStepperAccessoryView(
                _quantityTextField,
                _accessoryStepperView = new UIView(new CGRect(0, 0, DeviceInfo.ScreenWidth, 120)),
                _accessoryQuantityTextField = new UITextField(new CGRect(16, 16, DeviceInfo.ScreenWidth - 150, 44)),
                _accessoryUnitName = new AppRopio.Base.iOS.Controls.ARLabel(new CGRect(0, 0, 44, 44)),
                _accessoryDecrementBtn = new UIButton(UIButtonType.Custom).WithFrame(DeviceInfo.ScreenWidth - 114, 16, 44, 44),
                _accessoryIncrementBtn = new UIButton(UIButtonType.Custom).WithFrame(DeviceInfo.ScreenWidth - 60, 16, 44, 44),
                _accessoryQuantityChangedBtn = new UIButton(UIButtonType.Custom).WithFrame(0, 76, DeviceInfo.ScreenWidth, 44)
            );

            viewFromNib.Frame = Bounds;
            viewFromNib.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            AddSubview(viewFromNib);

            this.SetupStyle(ThemeConfig.ProductCard);
        }

        protected virtual CGRect GetFrame()
        {
            return new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 80.If_iPhoneX(100));
        }

        protected virtual void SetupBuyButton(UIButton buyButton)
        {
            buyButton.SetupStyle(ThemeConfig.ProductCard.BuyButton);
            buyButton.WithTitleForAllStates(LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "BasketProductCard_AddToBasket"));
        }

        protected virtual void SetupStepperView(UIView stepperView, UITextField quantityTextField, UILabel unitName, UIButton decrementBtn, UIButton incrementBtn)
        {
            stepperView.BackgroundColor = UIColor.Clear;

            quantityTextField.SetupStyle(ThemeConfig.ProductCard.QuantityField);
            unitName.SetupStyle(ThemeConfig.ProductCard.UnitName);
            decrementBtn.SetupStyle(ThemeConfig.ProductCard.DecrementButton);
            incrementBtn.SetupStyle(ThemeConfig.ProductCard.IncrementButton);

            quantityTextField.TextAlignment = UITextAlignment.Center;
            quantityTextField.RightView = new UIView().WithFrame(unitName.Frame).WithSubviews(unitName);
            quantityTextField.RightViewMode = UITextFieldViewMode.Always;
            quantityTextField.LeftView = new UIView().WithFrame(0, 0, 22, 44);
            quantityTextField.LeftViewMode = UITextFieldViewMode.Always;
            quantityTextField.KeyboardType = UIKeyboardType.NumberPad;
            quantityTextField.AutocorrectionType = UITextAutocorrectionType.No;
        }

        protected virtual void SetupStepperAccessoryView(UITextField textField, UIView stepperView, UITextField quantityTextField, UILabel unitName, UIButton decrementBtn, UIButton incrementBtn, UIButton quantityChangedBtn)
        {
            stepperView.AddSubviews(
                quantityTextField, decrementBtn, incrementBtn, quantityChangedBtn
            );

            SetupStepperView(stepperView, quantityTextField, unitName, decrementBtn, incrementBtn);

            quantityTextField.UserInteractionEnabled = false;

            quantityChangedBtn.SetTitle(LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "BasketProductCard_Done"), UIControlState.Normal);
            quantityChangedBtn.SetupStyle(ThemeConfig.ProductCard.QuantityButton);

            textField.InputAccessoryView = stepperView;
        }

        #endregion

        #region BindingControls

        private void BindControls()
        {
            var set = this.CreateBindingSet<BasketProductCardView, IBasketProductCardViewModel>();

            BindBuyButton(_buyButton, set);
            BindStepperView(_stepperView, _quantityTextField, _unitName, _decrementBtn, _incrementBtn, set);
            BindStepperAccessoryView(_accessoryStepperView, _accessoryQuantityTextField, _accessoryUnitName, _accessoryDecrementBtn, _accessoryIncrementBtn, _accessoryQuantityChangedBtn, set);

            set.Apply();
        }

        protected virtual void BindBuyButton(UIButton buyButton, MvxFluentBindingDescriptionSet<BasketProductCardView, IBasketProductCardViewModel> set)
        {
            set.Bind(buyButton).To(vm => vm.BuyCommand);
            set.Bind(buyButton).For("Visibility").To(vm => vm.BasketVisible).WithConversion("Visibility");
        }

        protected virtual void BindStepperView(UIView stepperView, UITextField quantityTextField, UILabel unitName, UIButton decrementBtn, UIButton incrementBtn, MvxFluentBindingDescriptionSet<BasketProductCardView, IBasketProductCardViewModel> set)
        {
            set.Bind(stepperView).For("Visibility").To(vm => vm.UnitStepVisible).WithConversion("Visibility");
            set.Bind(quantityTextField).To(vm => vm.QuantityString);
            set.Bind(unitName).To(vm => vm.UnitName);
            set.Bind(decrementBtn).To(vm => vm.DecrementCommand);
            set.Bind(incrementBtn).To(vm => vm.IncrementCommand);
        }

        protected virtual void BindStepperAccessoryView(UIView stepperView, UITextField quantityTextField, UILabel unitName, UIButton decrementBtn, UIButton incrementBtn, UIButton quantityChangedBtn, MvxFluentBindingDescriptionSet<BasketProductCardView, IBasketProductCardViewModel> set)
        {
            BindStepperView(stepperView, quantityTextField, unitName, decrementBtn, incrementBtn, set);

            quantityChangedBtn.TouchUpInside += (sender, e) => 
            {
                quantityTextField.EndEditing(true);

                this.EndEditing(true);

                ViewModel.QuantityChangedCommand.Execute();
            };
        }

        #endregion

        #endregion
    }
}
