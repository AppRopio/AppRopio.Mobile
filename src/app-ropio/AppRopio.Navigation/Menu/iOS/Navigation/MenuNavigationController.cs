using System;
using System.Collections.Generic;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using MvvmCross.iOS.Views.Presenters.Attributes;
using UIKit;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Navigation.Menu.iOS.Navigation
{
    [MvxRootPresentation(WrapInNavigationController = false)]
    public class MenuNavigationController : UIViewController
    {
        private class MenuPanGestureRecognizerDelegate : UIGestureRecognizerDelegate
        {
            private UIView _view;

            public MenuPanGestureRecognizerDelegate(UIView view)
            {
                _view = view;
            }

            public override bool ShouldBegin(UIGestureRecognizer recognizer)
            {
                var panRecognizer = recognizer as UIPanGestureRecognizer;
                var velocity = panRecognizer.VelocityInView(_view);
                return Math.Abs(velocity.X) > Math.Abs(velocity.Y);// Horizontal panning
            }
        }

        public const string LEFT_MENU_VISIBILITY_CHANGED = "LEFT_MENU_VISIBILITY_CHANGED";
        public const string RIGHT_MENU_VISIBILITY_CHANGED = "RIGHT_MENU_VISIBILITY_CHANGED";

        private UINavigationController _internalLeftMenuNavigationController;
        private UINavigationController _internalRightMenuNavigationController;
        private UINavigationController _internalTopNavigationController;

        private readonly UIPanGestureRecognizer _panGesture;
        private readonly UITapGestureRecognizer _tapGesture;

        private bool _ignorePan;
        private bool _globalIgnorePan = false;
        private nfloat _panOriginX;
        private bool _menuEnabled = true;

        private CALayer _shadowLeftLayer;
        private CALayer _shadowRightLayer;

        private UINavigationControllerDelegate _topViewNavigationControllerDelegate;

        public UINavigationControllerDelegate TopViewNavigationControllerDelegate
        {
            get { return _topViewNavigationControllerDelegate; }
            set
            {
                if (value != _topViewNavigationControllerDelegate)
                {
                    _topViewNavigationControllerDelegate = value;
                    if (_internalTopNavigationController != null)
                        _internalTopNavigationController.Delegate = _topViewNavigationControllerDelegate;
                }
            }
        }

        public UINavigationController LeftMenuNavigationController
        {
            get { return _internalLeftMenuNavigationController; }
        }

        public UINavigationController RightMenuNavigationController
        {
            get { return _internalRightMenuNavigationController; }
        }

        public UINavigationController TopNavigationController
        {
            get { return _internalTopNavigationController; }
        }

        public bool RightMenuIgnoreOpenSwype { get; set; }

        public bool LeftMenuIgnoreOpenSwype { get; set; }

        public nfloat SlideHeight { get; set; }

        public bool MenuSlidesWithTopView { get; set; }

        public bool MenuEnabled
        {
            get { return _menuEnabled; }
            set
            {
                if (value == _menuEnabled)
                    return;

                if (!value)
                    HideMenu();

                _menuEnabled = value;
            }
        }

        public bool ConsiderNavigationBarHidden { get; set; } = true;

        private bool _showMenuTopBars = false;

        public bool ShowMenuTopBars
        {
            get { return _showMenuTopBars; }
            set
            {
                _showMenuTopBars = value;
                _internalLeftMenuNavigationController?.SetNavigationBarHidden(!_showMenuTopBars, false);
                _internalRightMenuNavigationController?.SetNavigationBarHidden(!_showMenuTopBars, false);
            }
        }

        private bool _layerShadowing = false;

        public bool LayerShadowing
        {
            get { return _layerShadowing; }
            set
            {
                if (_layerShadowing != value)
                {
                    _layerShadowing = value;
                    if (value)
                    {
                        _internalTopNavigationController.View.Layer.InsertSublayer(_shadowLeftLayer, 0);
                        _internalTopNavigationController.View.Layer.InsertSublayer(_shadowRightLayer, 0);
                    }
                    else
                    {
                        _shadowLeftLayer.RemoveFromSuperLayer();
                        _shadowRightLayer.RemoveFromSuperLayer();
                    }
                }
            }
        }

        public float SlideSpeed { get; set; }

        public bool LeftMenuVisible { get; private set; }

        public bool RightMenuVisible { get; private set; }

        public float SlideWidth { get; set; }

        public MenuNavigationController()
        {
            IgnoreZones = new List<UIView>();
            SlideSpeed = 0.3f;
            SlideWidth = 270f;
            SlideHeight = UIScreen.MainScreen.ApplicationFrame.Height;
            FlyoutGestureEnable = true;
            MenuSlidesWithTopView = false;

            _internalLeftMenuNavigationController = new UINavigationController
            {
                NavigationBarHidden = true
            };

            _internalRightMenuNavigationController = new UINavigationController
            {
                NavigationBarHidden = true
            };

            _internalTopNavigationController = new MenuTopNavigationController
            {
                View = { Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height), BackgroundColor = Theme.ColorPalette.Background.ToUIColor() },
                ToolbarHidden = true
            };

            _tapGesture = new UITapGestureRecognizer();
            _tapGesture.AddTarget(() => HideMenu());
            _tapGesture.NumberOfTapsRequired = 1;

            _panGesture = new UIPanGestureRecognizer
            {
                Delegate = new SlideoutPanDelegate(_internalTopNavigationController.View, this),
                MaximumNumberOfTouches = 1,
                MinimumNumberOfTouches = 1
            };

            _panGesture.AddTarget(() => Pan(_internalTopNavigationController.View));

            _internalTopNavigationController.View.AddGestureRecognizer(_panGesture);

            InitShadows();
        }

        private void InitShadows()
        {
            var layer = new CALayer();
            layer.ShadowOffset = new CGSize(-4, 0);

            layer.ShadowPath = UIBezierPath.FromRect(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height)).CGPath;

            layer.ShadowRadius = 4.0f;
            layer.ShadowOpacity = 0.11f;
            layer.ShadowColor = UIColor.Black.CGColor;
            //layer.WeakDelegate = TopNavigationController.View;

            _shadowLeftLayer = layer;

            layer = new CALayer();
            layer.ShadowOffset = new CGSize(4, 0);

            layer.ShadowPath = UIBezierPath.FromRect(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height)).CGPath;

            layer.ShadowRadius = 3.0f;
            layer.ShadowOpacity = 0.3f;
            layer.ShadowColor = UIColor.Black.CGColor;
            //layer.WeakDelegate = TopNavigationController.View;

            _shadowRightLayer = layer;
        }

        private void Pan(UIView view)
        {
            if (!MenuEnabled || (ConsiderNavigationBarHidden && TopNavigationController.NavigationBarHidden))
                return;

            if (_panGesture.State == UIGestureRecognizerState.Began)
            {
                _panOriginX = view.Frame.X;
                _ignorePan = false;

                var firsTouch = _panGesture.LocationInView(view);
                if (firsTouch.Y > SlideHeight || _internalTopNavigationController.PresentedViewController != null)
                    _ignorePan = true;
            }
            else if (!_globalIgnorePan && !_ignorePan && (_panGesture.State == UIGestureRecognizerState.Changed))
            {
                var tx = _panGesture.TranslationInView(view).X;
                var ty = _panGesture.TranslationInView(view).Y;

                if (Math.Abs(tx) <= Math.Abs(ty) && view.Frame.X == 0)
                {
                    _globalIgnorePan = true;
                    return;
                }

                SetEnabalingExternalRecognizers();

                if (_internalLeftMenuNavigationController != null && !LeftMenuIgnoreOpenSwype)
                {
                    if (tx < _panOriginX)
                    {
                        if (!LeftMenuVisible && !RightMenuIgnoreOpenSwype)
                        {
                            if (_internalRightMenuNavigationController != null)
                            {
                                _internalLeftMenuNavigationController.View.Hidden = true;

                                _internalRightMenuNavigationController.SetNavigationBarHidden(!_showMenuTopBars, false);
                                _internalRightMenuNavigationController.View.Hidden = false;
                            }
                        }
                    }
                }
                if (_internalRightMenuNavigationController != null && !RightMenuIgnoreOpenSwype)
                {
                    if (tx > _panOriginX)
                    {
                        if (!RightMenuVisible && !LeftMenuIgnoreOpenSwype)
                        {
                            if (_internalLeftMenuNavigationController != null)
                            {
                                _internalRightMenuNavigationController.View.Hidden = true;

                                _internalLeftMenuNavigationController.SetNavigationBarHidden(!_showMenuTopBars, false);
                                _internalLeftMenuNavigationController.View.Hidden = false;
                            }
                        }
                    }
                }

                if (_internalLeftMenuNavigationController != null && !LeftMenuIgnoreOpenSwype)
                {

                    if (tx >= 0 && LeftMenuVisible)
                        tx = 0;
                    else if (tx <= -_internalLeftMenuNavigationController.View.Bounds.Width && LeftMenuVisible)
                        tx = -_internalLeftMenuNavigationController.View.Bounds.Width;
                    else if (tx >= _internalLeftMenuNavigationController.View.Bounds.Width && !LeftMenuVisible)
                        tx = _internalLeftMenuNavigationController.View.Bounds.Width;

                }
                else if (tx > 0 && !RightMenuVisible)
                    tx = 0;

                if (_internalRightMenuNavigationController != null && !RightMenuIgnoreOpenSwype)
                {

                    if (tx <= 0 && RightMenuVisible)
                        tx = 0;
                    else if (tx >= _internalRightMenuNavigationController.View.Bounds.Width && RightMenuVisible)
                        tx = _internalRightMenuNavigationController.View.Bounds.Width;
                    else if (tx <= -_internalRightMenuNavigationController.View.Bounds.Width && !RightMenuVisible)
                        tx = -_internalRightMenuNavigationController.View.Bounds.Width;

                }
                else if (tx < 0 && !LeftMenuVisible)
                    tx = 0;

                if (MenuSlidesWithTopView)
                {
                    if (!RightMenuIgnoreOpenSwype)
                    {
                        var rightMenuView = _internalRightMenuNavigationController.View;
                        rightMenuView.Frame = new CGRect(SlideWidth + _panOriginX + tx, rightMenuView.Frame.Y, rightMenuView.Frame.Width, rightMenuView.Frame.Height);
                    }

                    if (!LeftMenuIgnoreOpenSwype)
                    {
                        var leftMenuView = _internalLeftMenuNavigationController.View;
                        leftMenuView.Frame = new CGRect(-SlideWidth + _panOriginX + tx, leftMenuView.Frame.Y, leftMenuView.Frame.Width, leftMenuView.Frame.Height);
                    }
                }

                view.Frame = new CGRect(_panOriginX + tx, view.Frame.Y, view.Frame.Width, view.Frame.Height);

            }
            else if (!_ignorePan &&
                     (_panGesture.State == UIGestureRecognizerState.Ended ||
                     _panGesture.State == UIGestureRecognizerState.Cancelled))
            {
                var velocity = _panGesture.VelocityInView(view).X;

                if (Math.Abs(velocity) > 800 && !_globalIgnorePan)
                {
                    if (velocity < 0)
                    {
                        if (LeftMenuVisible)
                            HideMenu();
                        else
                        {
                            if (!RightMenuIgnoreOpenSwype)
                                ShowRightMenu();
                            else
                                HideMenu();
                        }
                    }
                    else
                    {
                        if (RightMenuVisible)
                            HideMenu();
                        else
                        {
                            if (!LeftMenuIgnoreOpenSwype)
                                ShowLeftMenu();
                            else
                                HideMenu();
                        }
                    }


                }
                else
                {
                    if (RightMenuVisible)
                    {
                        if (view.Frame.X >= -_internalRightMenuNavigationController.View.Frame.Width / 2)
                            HideMenu();
                        else if (!RightMenuIgnoreOpenSwype)
                            ShowRightMenu();
                        else
                            HideMenu();

                    }
                    else if (LeftMenuVisible)
                    {
                        if (view.Frame.X <= _internalLeftMenuNavigationController.View.Frame.Width / 2)
                            HideMenu();
                        else if (!LeftMenuIgnoreOpenSwype)
                            ShowLeftMenu();
                        else
                            HideMenu();
                    }
                    else
                    {

                        if (view.Frame.X >= _internalLeftMenuNavigationController.View.Frame.Width / 4 && !LeftMenuIgnoreOpenSwype)
                            ShowLeftMenu();
                        else if (view.Frame.X >= 0)
                            HideMenu();
                        else if (view.Frame.X >= -_internalRightMenuNavigationController.View.Frame.Width / 2)
                            HideMenu();
                        else if (!RightMenuIgnoreOpenSwype)
                            ShowRightMenu();
                        else
                            HideMenu();
                    }
                }
                _globalIgnorePan = false;
            }

        }

        public void SetEnabalingExternalRecognizers()
        {
            var state = true;
            if (this._internalTopNavigationController.View.Frame.X != 0)
                state = false;

            if (!(View.GestureRecognizers == null))
            {
                foreach (var recognizer in this.View.GestureRecognizers)
                    recognizer.Enabled = state;
            }

        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _internalTopNavigationController.View.Frame = new CGRect(0, 0, View.Frame.Width, View.Frame.Height);

            _internalLeftMenuNavigationController.View.Frame = new CGRect(0, 0, SlideWidth, View.Frame.Height);

            AddChildViewController(_internalRightMenuNavigationController);
            View.AddSubview(_internalRightMenuNavigationController.View);

            //Add the list View
            AddChildViewController(_internalLeftMenuNavigationController);
            View.AddSubview(_internalLeftMenuNavigationController.View);

            //Add the parent view
            AddChildViewController(_internalTopNavigationController);
            View.AddSubview(_internalTopNavigationController.View);

        }

        bool _menuEnabledRotate;

        public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            HideMenu();
            _menuEnabledRotate = MenuEnabled;
            MenuEnabled = false;

            base.WillRotate(toInterfaceOrientation, duration);
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            MenuEnabled = _menuEnabledRotate;
        }

        public void ShowLeftMenu()
        {
            if (_internalLeftMenuNavigationController != null)
            {
                _internalLeftMenuNavigationController.NavigationBarHidden = !_showMenuTopBars;
                _internalLeftMenuNavigationController.View.Hidden = false;
            }

            if (_internalRightMenuNavigationController != null)
            {
                _internalRightMenuNavigationController.NavigationBarHidden = true;
                _internalRightMenuNavigationController.View.Hidden = true;
            }

            LeftMenuVisible = true;
            NSNotificationCenter.DefaultCenter.PostNotificationName(LEFT_MENU_VISIBILITY_CHANGED, NSNumber.FromBoolean(LeftMenuVisible));

            UIView view = _internalTopNavigationController.View;
            UIView leftView = _internalLeftMenuNavigationController.View;

            UIView.Animate(SlideSpeed, 0, UIViewAnimationOptions.CurveEaseInOut,
                () =>
                {
                    view.Frame = new CGRect(SlideWidth, 0, view.Frame.Width, view.Frame.Height);

                    if (MenuSlidesWithTopView)
                        leftView.Frame = new CGRect(0, 0, leftView.Frame.Width, leftView.Frame.Height);

                },
                () =>
                {
                    SetupUserInteractions(view);
                    view.AddGestureRecognizer(_tapGesture);
                });
        }

        public void ShowRightMenu()
        {
            if (_internalRightMenuNavigationController != null)
            {
                _internalRightMenuNavigationController.NavigationBarHidden = !_showMenuTopBars;
                _internalRightMenuNavigationController.View.Hidden = false;
            }

            if (_internalLeftMenuNavigationController != null)
            {
                _internalLeftMenuNavigationController.NavigationBarHidden = true;
                _internalLeftMenuNavigationController.View.Hidden = true;
            }

            RightMenuVisible = true;
            NSNotificationCenter.DefaultCenter.PostNotificationName(RIGHT_MENU_VISIBILITY_CHANGED, NSNumber.FromBoolean(RightMenuVisible));

            UIView view = _internalTopNavigationController.View;
            UIView rightView = _internalRightMenuNavigationController.View;
            UIView.Animate(SlideSpeed, 0, UIViewAnimationOptions.CurveEaseInOut,
                () =>
                {
                    view.Frame = new CGRect(-SlideWidth, 0, view.Frame.Width, view.Frame.Height);
                    if (MenuSlidesWithTopView)
                        rightView.Frame = new CGRect(rightView.Frame.Width - SlideWidth, 0, rightView.Frame.Width, rightView.Frame.Height);
                },
                () =>
                {
                    SetupUserInteractions(view);
                    view.AddGestureRecognizer(_tapGesture);
                });
        }

        private void SetupUserInteractions(UIView view, bool isHide = false)
        {
            if (view.Subviews == null)
                return;

            var count = view.Subviews.Length;

            _internalTopNavigationController.View.EndEditing(true);

            if (count > 0)
            {
                foreach (var item in view.Subviews)
                {
                    item.UserInteractionEnabled = isHide;
                }

                if (!isHide)
                {
                    if (RightMenuVisible)
                        _internalRightMenuNavigationController.View.UserInteractionEnabled = true;
                    else
                        _internalRightMenuNavigationController.View.UserInteractionEnabled = false;

                    if (LeftMenuVisible)
                        _internalLeftMenuNavigationController.View.UserInteractionEnabled = true;
                    else
                        _internalLeftMenuNavigationController.View.UserInteractionEnabled = false;
                }
                else
                {
                    SetEnabalingExternalRecognizers();
                    _internalRightMenuNavigationController.View.UserInteractionEnabled = false;
                    _internalLeftMenuNavigationController.View.UserInteractionEnabled = false;
                }
            }
        }

        public void HideMenu(bool animate = true, Action menuDidHide = null)
        {
            if (RightMenuVisible)
            {
                RightMenuVisible = false;
                NSNotificationCenter.DefaultCenter.PostNotificationName(RIGHT_MENU_VISIBILITY_CHANGED, NSNumber.FromBoolean(RightMenuVisible));
            }

            if (LeftMenuVisible)
            {
                LeftMenuVisible = false;
                NSNotificationCenter.DefaultCenter.PostNotificationName(LEFT_MENU_VISIBILITY_CHANGED, NSNumber.FromBoolean(LeftMenuVisible));
            }

            UIView view = _internalTopNavigationController.View;

            Action animation = () =>
            {
                view.Frame = new CGRect(0, 0, view.Frame.Width, view.Frame.Height);
            };
            Action finished = () =>
            {
                menuDidHide?.Invoke();

                SetupUserInteractions(view, true);
                view.RemoveGestureRecognizer(_tapGesture);
            };

            if (animate)
                UIView.Animate(SlideSpeed, 0, UIViewAnimationOptions.CurveEaseInOut, animation, finished);
            else
            {
                animation();
                finished();
            }
        }

        public void SetMenuNavigationBackgroundImage(UIImage image, UIBarMetrics metrics)
        {
            if (_internalLeftMenuNavigationController != null)
                _internalLeftMenuNavigationController.NavigationBar.SetBackgroundImage(image, metrics);
            if (_internalRightMenuNavigationController != null)
                _internalRightMenuNavigationController.NavigationBar.SetBackgroundImage(image, metrics);
        }

        public void SetTopNavigationBackgroundImage(UIImage image, UIBarMetrics metrics)
        {
            _internalTopNavigationController.NavigationBar.SetBackgroundImage(image, metrics);
        }

        public static List<UIView> IgnoreZones { get; private set; }

        public void AddIgnoreZone(UIView view)
        {
            if (IgnoreZones.Contains(view))
                return;

            IgnoreZones.Add(view);
        }

        public void ClearIgnoreZones()
        {
            IgnoreZones.Clear();
        }

        public bool FlyoutGestureEnable { get; set; }

        public bool ShoudIgnoreTouch(CGPoint point)
        {
            if (!FlyoutGestureEnable)
                return true;

            bool inZone = false;

            if (!LeftMenuVisible && !RightMenuVisible)
            {
                foreach (var view in IgnoreZones)
                {
                    var rect = view.GetAbsoluteFrame(TopNavigationController.View);
                    inZone = point.PointInRect(rect);
                    if (inZone)
                        return inZone;
                }
            }

            return inZone;
        }

        private class SlideoutPanDelegate : UIGestureRecognizerDelegate
        {
            private readonly MenuNavigationController _controller;
            private UIView _view;

            public SlideoutPanDelegate(UIView view, MenuNavigationController controller)
            {
                _view = view;
                _controller = controller;
            }

            public override bool ShouldBegin(UIGestureRecognizer recognizer)
            {
                var panRecognizer = recognizer as UIPanGestureRecognizer;
                var velocity = panRecognizer.VelocityInView(_view);

                return Math.Abs(velocity.X) > Math.Abs(velocity.Y) &&
                           (
                            (!_controller.LeftMenuIgnoreOpenSwype &&
                                 ((!_controller.LeftMenuVisible && velocity.X > 0) || (_controller.LeftMenuVisible && velocity.X < 0))) ||
                            (!_controller.RightMenuIgnoreOpenSwype &&
                                 ((!_controller.RightMenuIgnoreOpenSwype && velocity.X < 0) || (_controller.RightMenuIgnoreOpenSwype && velocity.X > 0)))
                           );
            }

            public override bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
            {
                var point = touch.LocationInView(_controller.TopNavigationController.View);

                if (_controller.ShoudIgnoreTouch(point))
                    return false;

                return (_controller.LeftMenuVisible || _controller.RightMenuVisible ||
                (touch.LocationInView(_controller._internalTopNavigationController.View).Y <= _controller.SlideHeight)) && _controller.MenuEnabled;
            }

            public override bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
            {
                var scrollView = otherGestureRecognizer.View as UIScrollView;
                if (scrollView != null && (scrollView.ContentOffset.X >= 0))
                    return false;

                if (_controller.LeftMenuVisible || _controller.RightMenuVisible || _controller._internalTopNavigationController.View.Frame.X != 0)
                    return false;

                return otherGestureRecognizer.State == UIGestureRecognizerState.Failed || otherGestureRecognizer.State == UIGestureRecognizerState.Ended;
            }
        }
    }

    internal static class FlyoutExtensions
    {
        public static bool PointInRect(this CGPoint point, CGRect frame)
        {
            return point.X >= frame.X && point.Y >= frame.Y && point.X <= frame.Right && point.Y <= frame.Bottom;
        }

        public static CGRect GetAbsoluteFrame(this UIView view, UIView wrapperView)
        {
            var x = view.Frame.X;
            var y = view.Frame.Y;

            var superView = view.Superview;
            while (superView != null && superView != wrapperView)
            {
                x += superView.Frame.X;
                y += superView.Frame.Y;

                if (superView is UIScrollView)
                {
                    var scroll = superView as UIScrollView;
                    x -= scroll.ContentOffset.X;
                    y -= scroll.ContentOffset.Y;
                }


                superView = superView.Superview;
            }

            return new CGRect(x, y, view.Frame.Width, view.Frame.Height);
        }
    }
}