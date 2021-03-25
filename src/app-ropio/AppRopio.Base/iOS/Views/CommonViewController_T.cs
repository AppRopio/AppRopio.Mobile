using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;
using MvvmCross.ViewModels;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Binding.BindingContext;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.Core.Services.Localization;
using MvvmCross;

namespace AppRopio.Base.iOS.Views
{
    public abstract class CommonViewController<T> : MvxViewController<T>, IUnbindable
        where T : class, IMvxViewModel
    {
        private bool _isKeyboardOpened;

        private NSObject _keyboardObserverWillShow;
        private NSObject _keyboardObserverWillHide;

        public bool RegisterKeyboardActions { get; protected set; }

        public float RegisterKeyboardHeight { get; protected set; }

        public bool AutomaticallyLargeTitleDisplayMode { get; protected set; } = true;

        private bool _bindLoading = true;
        public bool BindLoading { get { return _bindLoading; } set { _bindLoading = value; } }

        protected UIActivityIndicatorView Loading { get; set; }
        protected UIView LoadingView { get; set; }

        public override string Title
        {
            get
            {
                return base.Title;
            }
            set
            {
                base.Title = value;

                if (Theme.ControlPalette.NavigationBar.UseCustomView && NavigationItem.TitleView != null && NavigationItem.TitleView.Subviews.Any())
                {
                    var titleLabel = NavigationItem.TitleView.Subviews[0] as UILabel;
                    if (titleLabel != null)
                        titleLabel.Text = value;
                }
            }
        }

        protected ILocalizationService LocalizationService => Mvx.IoCProvider.Resolve<ILocalizationService>();

        protected CommonViewController()
        {

        }

        protected CommonViewController(IntPtr handle)
            : base(handle)
        {

        }

        protected CommonViewController(string nibName, Foundation.NSBundle bundle)
            : base(nibName, bundle)
        {

        }

        protected virtual void SetupLoading()
        {
            nfloat r = 0, g = 0, b = 0, tmpAlpha = 0;

            var themeBackground = Theme.ColorPalette.Background.ToUIColor();
            themeBackground.GetRGBA(out r, out g, out b, out tmpAlpha);

            LoadingView = new UIView()
                .WithFrame(0, 0, DeviceInfo.ScreenWidth, DeviceInfo.ScreenHeight - (NavigationController != null && !NavigationController.NavigationBarHidden && !NavigationController.NavigationBar.Translucent ? 64 : 0))
                .WithBackground(UIColor.FromRGBA(r, g, b, (nfloat)0.4))
                .WithSubviews(
                    Loading = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge) { Color = (UIColor)Theme.ColorPalette.Accent }
                );
            Loading.StartAnimating();

            LoadingView.AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(Loading, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, LoadingView, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(Loading, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, LoadingView, NSLayoutAttribute.CenterY, 1, 0)
            });
            Loading.TranslatesAutoresizingMaskIntoConstraints = false;

            if (BindLoading)
            {
                View.AddSubview(LoadingView);

                var set = this.CreateBindingSet<CommonViewController<T>, T>();
                set.Bind(LoadingView).For("Visibility").To(vm => ((IBaseViewModel)vm).Loading).WithConversion("Visibility");
                set.Apply();
            }
        }

        protected virtual void InitializeControls()
        {

        }

        protected abstract void BindControls();

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //NOTE: code below enabled synchronously invoking binding actions when binding context was changed. 
            //That may slow screen opening or closing, but exempt of exceptions when target binding is null
            //var bindingContext = this.BindingContext as MvxTaskBasedBindingContext;
            //if (bindingContext != null)
            //    bindingContext.RunSynchronously = true;

            if (ViewModel != null)
            {
                if (ViewModel is IBaseViewModel && UIDevice.CurrentDevice.CheckSystemVersion(11, 0) && AutomaticallyLargeTitleDisplayMode)
                {
                    NavigationItem.LargeTitleDisplayMode = (ViewModel as IBaseViewModel).VmNavigationType == Core.Models.Navigation.NavigationType.ClearAndPush ?
                       UINavigationItemLargeTitleDisplayMode.Always
                           :
                       UINavigationItemLargeTitleDisplayMode.Never;
                }

                BindControls();
            }
        }

        public override void LoadView()
        {
            base.LoadView();

            View.BackgroundColor = (UIColor)Theme.ColorPalette.Background;

            SetupLoading();

            InitializeControls();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (RegisterKeyboardActions)
            {
                RegisterHideKeyboardOnSwipe();
                RegisterForKeyboardNotifications();
            }
        }

        public override void ViewWillDisappear(bool animated)
        {
            UnregisterKeyboardNotifications();

            base.ViewWillDisappear(animated);
        }

        #region Keyboard

        private void RegisterHideKeyboardOnSwipe()
        {
            var scrollView = View.Subviews.OfType<UIScrollView>().FirstOrDefault();
            if (scrollView == null)
                return;

            scrollView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;
        }

        private void RegisterForKeyboardNotifications()
        {
            _keyboardObserverWillShow = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, KeyboardWillShowNotification);
            _keyboardObserverWillHide = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, KeyboardWillHideNotification);
        }

        protected float _keyboardHeight;
        protected UIEdgeInsets _beforeKeyboardContentInsets;
        protected UIEdgeInsets _beforeKeyboardScrollIndicatorInsets;

        protected virtual void KeyboardWillShowNotification(NSNotification notification)
        {
            var activeView = View.FindFirstResponder();

            if (activeView == null)
                return;

            var scrollView = View.Subviews.OfType<UIScrollView>().FirstOrDefault();
            if (scrollView == null)
                return;
            
            if (!_isKeyboardOpened)
            {
                _isKeyboardOpened = true;
                _beforeKeyboardContentInsets = scrollView.ContentInset;
                _beforeKeyboardScrollIndicatorInsets = scrollView.ScrollIndicatorInsets;
            }

            var keyboardFrame = notification.UserInfo == null ? new CGRect(0, 0, 0, 0) : ((NSValue)notification.UserInfo.ObjectForKey(UIKeyboard.BoundsUserInfoKey)).CGRectValue;
            _keyboardHeight = RegisterKeyboardHeight == 0f ? (float)keyboardFrame.Height : RegisterKeyboardHeight;

            var scrollViewDelta = scrollView.Frame.Top + (View.Bounds.Height - scrollView.Frame.Bottom);
            scrollView.ContentInset = new UIEdgeInsets(0, 0, _keyboardHeight - scrollViewDelta, 0);
            scrollView.ScrollIndicatorInsets = new UIEdgeInsets(0, 0, _keyboardHeight - scrollViewDelta, 0);

            if (activeView is UITextView)
            {
                var absolutePosition = activeView.GetAbsolutePositionIn(scrollView);
                scrollView.ScrollRectToVisible(
                    new CGRect(0, absolutePosition.Y, activeView.Superview.Frame.Width, activeView.Superview.Frame.Height),
                    true);
            }
        }

        protected virtual void KeyboardWillHideNotification(NSNotification notification)
        {
            var activeView = View.FindFirstResponder();
            if (activeView == null)
                return;

            var scrollView = View.Subviews.OfType<UIScrollView>().FirstOrDefault();
            if (scrollView == null)
                return;
            
            scrollView.ContentInset = _beforeKeyboardContentInsets;
            scrollView.ScrollIndicatorInsets = _beforeKeyboardScrollIndicatorInsets;

            _isKeyboardOpened = false;
        }

        private void UnregisterKeyboardNotifications()
        {
            if (_keyboardObserverWillShow != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillShow);
            if (_keyboardObserverWillHide != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_keyboardObserverWillHide);

            _keyboardObserverWillShow = null;
            _keyboardObserverWillHide = null;
        }

        protected void SetupInputAccessoryToolbar(UIColor tintColor)
        {
            var allFields = View.AllSubviews(new Type[] { typeof(UITextField), typeof(UITextView) });
            for (var i = 0; i < allFields.Count; i++)
            {
                var toolbar = new UIToolbar();
                toolbar.TintColor = tintColor;
                toolbar.Translucent = true;
                toolbar.SizeToFit();

                var doneButton = new UIBarButtonItem(Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString("Base", "Done"), UIBarButtonItemStyle.Done, null);
                toolbar.SetItems(new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton }, false);

                var textField = allFields[i] as UITextField;
                if (textField != null)
                {
                    if (textField.InputAccessoryView == null)
                    {
                        doneButton.Clicked += (s, e) => textField.ResignFirstResponder();
                        textField.InputAccessoryView = toolbar;
                    }
                    textField.ShouldReturn = (tF) =>
                    {
                        if (i + 1 >= allFields.Count)
                            textField.ResignFirstResponder();
                        else
                        {
                            var field = allFields[i + 1] as UITextField;
                            if (field != null && field.Enabled)
                                field.BecomeFirstResponder();

                            var view = allFields[i + 1] as UITextView;
                            if (view != null && view.Editable)
                                view.BecomeFirstResponder();
                        }
                        return false;
                    };
                }

                var textView = allFields[i] as UITextView;
                if (textView != null && textView.InputAccessoryView == null)
                {
                    doneButton.Clicked += (s, e) => textView.ResignFirstResponder();
                    textView.InputAccessoryView = toolbar;
                }
            }
        }

        #endregion

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        #region IUnbindable implementation

        protected virtual void CleanUp()
        {
            //nothing here
        }

        public virtual void Pause()
        {
            //nothing here
        }

        public void Unbind()
        {
            CleanUp();
        }

        #endregion

    }

}