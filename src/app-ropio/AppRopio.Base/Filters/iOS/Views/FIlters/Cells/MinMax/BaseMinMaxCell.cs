using System;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Helpers;
using Foundation;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.MinMax
{
    public abstract class BaseMinMaxCell : MvxTableViewCell
    {
        protected UIButton _hideFromButton;
        protected UIButton _hideToButton;

        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

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
