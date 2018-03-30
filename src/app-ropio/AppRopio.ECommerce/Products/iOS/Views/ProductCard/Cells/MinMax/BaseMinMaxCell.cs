using System;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MinMax
{
    public abstract class BaseMinMaxCell : MvxTableViewCell
    {
        protected UIButton _hideFromButton;
        protected UIButton _hideToButton;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public const float MIN_MAX_HEIGHT = 82;

        protected BaseMinMaxCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeHidingButton();
                InitializeControls();
                BindControls();
            });
        }

        #region Private

        private UIButton CreateHideKeyboardButton()
        {
            return new UIButton(UIButtonType.Custom)
                .WithFrame(0, 162.5f.If_iPhone6Plus(169.3f), 104.5f.If_iPhone6(122.5f).If_iPhone6Plus(135), 60)
                .WithBackground(UIColor.Clear)
                .WithButtonImage(ImageCache.GetImage("Images/Main/hide_key.png", UIColor.Black), UIControlState.Normal)
                .WithButtonImage(ImageCache.GetImage("Images/Main/hide_key_state.png", UIColor.Black), UIControlState.Highlighted)
                .WithTune(tune =>
            {
                tune.SetBackgroundImage(UIColor.White.ToUIImage(), UIControlState.Highlighted);
            });
        }

        private void InitializeHidingButton()
        {
            _hideFromButton = CreateHideKeyboardButton();
            _hideToButton = CreateHideKeyboardButton();

            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWillShowNotification);
            NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardWillHideNotification);
        }

        private void KeyboardWillHideNotification(NSNotification notification)
        {
            if (_hideFromButton != null && _hideFromButton.Superview != null)
                _hideFromButton.RemoveFromSuperview();

            if (_hideToButton != null && _hideToButton.Superview != null)
                _hideToButton.RemoveFromSuperview();
        }

        #endregion

        #region Protected

        protected virtual void KeyboardWillShowNotification(NSNotification notification)
        {
        }

        protected abstract void InitializeControls();

        protected abstract void BindControls();

        #endregion
    }
}
