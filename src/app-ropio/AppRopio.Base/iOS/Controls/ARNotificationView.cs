using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using AppRopio.Base.iOS.Models.Notification;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
using CoreGraphics;
using UIKit;

namespace AppRopio.Base.iOS.Controls
{
    public enum ARNotificationScrollOrientation
    {
        Unknown,
        FromLeftToRight,
        FromRightToLeft,
        FromBottomToTop,
        FromTopToBottom
    }

    public class ARNotificationView : UIView
    {
        private readonly ARNotificationType _alertType;
        private readonly string _message;
        private readonly bool _autoHide;

        private bool _hiddenHintWasShown;

        private bool _textSelected;

        private Action _onHideCallback;

        private int _touchCancels;

        private Timer _hiddenTimer;
        private Timer _hintTimer;

        private UIButton _button;
        private UITextView _textView;

        private const int MAXIMUM_CANCELS = 1;
        private const int MILLISECONDS_FOR_HIDE = 3000;
        private const int MILLISECONDS_FOR_HINT = 3000;
        private float HEIGHT = 65;
        private float TEXT_VIEW_OFFSET;
        private float TEXT_VIEW_HEIGHT;

        private CGSize _realTextSize;

        public const int TAG = 2062237;

        public ARNotificationView(string message, ARNotificationType alertType, string buttonTitle = null, bool autoHide = false)
        {
            _message = message;
            _autoHide = autoHide;

            _alertType = alertType;

            HEIGHT = 68.If_iPhoneX(88);
            TEXT_VIEW_OFFSET = 0.If_iPhoneX(23);

            TEXT_VIEW_HEIGHT = HEIGHT - TEXT_VIEW_OFFSET;

            Frame = new CGRect(0, -HEIGHT, DeviceInfo.ScreenWidth, HEIGHT);
            Tag = TAG;
            UserInteractionEnabled = true;

            InitializeSubviews(message, alertType, buttonTitle);
        }

        private void InitializeSubviews(string message, ARNotificationType alertType, string buttonTitle)
        {
            _textView = new UITextView(new CGRect(0, TEXT_VIEW_OFFSET, Frame.Width - 86, 0))
            {
                Editable = false,
                ScrollEnabled = false,
                IndicatorStyle = UIScrollViewIndicatorStyle.Black,
                DataDetectorTypes = UIDataDetectorType.None,
                Text = message,
                TextColor = GetTexViewTextColor(alertType),
                Font = GetTexViewFont(alertType),
                BackgroundColor = UIColor.Clear,
                Opaque = true,
                UserInteractionEnabled = false
            };

            if (alertType == ARNotificationType.Error)
            {
                _textView.DraggingStarted += OnDraggingStarted;
                _textView.DraggingEnded += OnDraggingEnded;
                _textView.DecelerationEnded += OnDecelerationEnded;
                _textView.SelectionChanged += OnSelectionChanged;
            }

            _textView.TextContainer.LineBreakMode = UILineBreakMode.TailTruncation;

            _textView.SizeToFit();

            _realTextSize = new CGSize(_textView.Frame.Size);

            switch (alertType)
            {
                case ARNotificationType.Alert:
                    InitializeWithImagePath(_textView, alertType, Theme.ControlPalette.Notifications.Alert.Icon);
                    break;
                case ARNotificationType.Confirm:
                    InitializeWithButtonTitle(_textView, buttonTitle);
                    break;
                case ARNotificationType.Error:
                    InitializeWithImagePath(_textView, alertType, Theme.ControlPalette.Notifications.Error.Icon);
                    break;
            }
        }

        private UIFont GetTexViewFont(ARNotificationType alertType)
        {
            switch (alertType)
            {
                case ARNotificationType.Error:
                    return (UIFont)Theme.ControlPalette.Notifications.Error.Text.Font;
                case ARNotificationType.Confirm:
                    return (UIFont)Theme.ControlPalette.Notifications.Confirm.Text.Font;
                default:
                    return (UIFont)Theme.ControlPalette.Notifications.Alert.Text.Font;
            }
        }

        private UIColor GetTexViewTextColor(ARNotificationType alertType)
        {
            switch (alertType)
            {
                case ARNotificationType.Error:
                    return (UIColor)Theme.ControlPalette.Notifications.Error.Text.TextColor;
                case ARNotificationType.Confirm:
                    return (UIColor)Theme.ControlPalette.Notifications.Confirm.Text.TextColor;
                default:
                    return (UIColor)Theme.ControlPalette.Notifications.Alert.Text.TextColor;
            }
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            if (_textView != null && _textView.SelectedTextRange != null && _textView.SelectedTextRange.IsEmpty)
                StartHiddenTimer();
            else
                _hiddenTimer.Stop();

            _textSelected = _textView != null && _textView.SelectedTextRange != null && !_textView.SelectedTextRange.IsEmpty;
        }

        private void InitializeWithImagePath(UITextView textView, ARNotificationType alertType, Image image)
        {
            BackgroundColor = alertType == ARNotificationType.Error ? Theme.ControlPalette.Notifications.Error.Background.ToUIColor() : Theme.ControlPalette.Notifications.Alert.Background.ToUIColor();

            var imageView = new UIImageView();
            imageView.SetupStyle(image);

            textView.Frame = imageView.Image.Size == CGSize.Empty ?
                new CGRect(16, TEXT_VIEW_OFFSET, Frame.Width - 32, TEXT_VIEW_HEIGHT) :
                new CGRect(16 + imageView.Image.Size.Width + 10, TEXT_VIEW_OFFSET, Frame.Width - 86, TEXT_VIEW_HEIGHT);

            AddSubviews(
                imageView
                .WithFrame(16, (TEXT_VIEW_OFFSET / 2) + ((Frame.Height - imageView.Image.Size.Height) / 2), imageView.Image.Size.Width, imageView.Image.Size.Height)
                .WithTune(tune => tune.ActionOnTap(() =>
                {
                    _hintTimer?.Stop();
                    _hiddenTimer?.Stop();

                    Hide();
                })),
                textView
            );
        }

        private void InitializeWithButtonTitle(UITextView textView, string buttonTitle)
        {
            BackgroundColor = Theme.ControlPalette.Notifications.Confirm.Background.ToUIColor();

            _button = new UIButton(UIButtonType.Custom);

            _button.SetupStyle(Theme.ControlPalette.Notifications.Confirm.Button);

            _button.SetTitle(buttonTitle.ToUpperInvariant(), UIControlState.Normal);

            _button.TouchDown += (sender, e) =>
            {
                _hiddenTimer?.Stop();
                _hintTimer?.Stop();
            };
            _button.TouchCancel += (sender, e) => OnTouchCancel();

            _button.SizeToFit();

            var buttonWidth = Math.Max(_button.Frame.Width, 44);
            _button.Frame = new CGRect(Frame.Width - buttonWidth - 16, TEXT_VIEW_OFFSET, buttonWidth, TEXT_VIEW_HEIGHT);

            textView.Frame = new CGRect(16, TEXT_VIEW_OFFSET, Frame.Width - 32 - buttonWidth, TEXT_VIEW_HEIGHT);

            AddSubviews(textView, _button);
        }

        private void OnTouchCancel()
        {
            _touchCancels++;

            if (!_hiddenHintWasShown)
                StartAnimatedHiddenHint();
            else if (_touchCancels > MAXIMUM_CANCELS)
                StartHiddenTimer();
        }

        private void StartHiddenTimer()
        {
            _hiddenTimer = new Timer(MILLISECONDS_FOR_HIDE)
            {
                AutoReset = false
            };
            _hiddenTimer.Elapsed += OnHiddenTimerElapsed;

            _hiddenTimer.Start();
        }

        private void StartAnimatedHiddenHint()
        {
            _hintTimer = new Timer(MILLISECONDS_FOR_HINT)
            {
                AutoReset = false
            };
            _hintTimer.Elapsed += OnHintTimerElapsed;

            _hintTimer.Start();
        }

        private void OnHiddenTimerElapsed(object sender, ElapsedEventArgs e)
        {
            Hide();

            _onHideCallback?.Invoke();
        }

        private void OnHintTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _hiddenHintWasShown = true;

            InvokeOnMainThread(() =>
            {
                AnimateNotify(0.2f, 0, UIViewAnimationOptions.CurveEaseOut, () =>
                {
                    this.ChangeFrame(x: -(DeviceInfo.ScreenWidth * 0.1f));
                }, (finished) =>
                {
                    AnimateNotify(0.15, 0.35, UIViewAnimationOptions.CurveEaseIn, () =>
                    {
                        this.ChangeFrame(x: 0);
                    }, null);
                });
            });
        }

        private void OnDraggingStarted(object sender, EventArgs e)
        {
            _hiddenTimer?.Stop();
        }

        private void OnDecelerationEnded(object sender, EventArgs e)
        {
            if (!_textSelected)
                StartHiddenTimer();
        }

        private void OnDraggingEnded(object sender, DraggingEventArgs e)
        {
            if (!e.Decelerate && !_textSelected)
                StartHiddenTimer();
        }

        private void ReleaseDesignOutlets()
        {
            if (_textView != null)
            {
                _textView.Dispose();
                _textView = null;
            }
            if (_button != null)
            {
                _button.Dispose();
                _button = null;
            }
        }

        public void Hide()
        {
            InvokeOnMainThread(() =>
            {
                AnimateNotify(0.15f, () =>
                {
                    this.ChangeFrame(y: -HEIGHT);
                }, (finished) =>
                {
                    if (finished)
                    {
                        if (_textView != null && _alertType == ARNotificationType.Error)
                        {
                            _textView.DraggingStarted -= OnDraggingStarted;
                            _textView.DraggingEnded -= OnDraggingEnded;
                            _textView.DecelerationEnded -= OnDecelerationEnded;
                            _textView.SelectionChanged -= OnSelectionChanged;
                        }

                        RemoveFromSuperview();

                        ReleaseDesignOutlets();
                    }
                });
            });
        }

        public void Show(Action onHideCallback = null, Action onButtonCallback = null)
        {
            _onHideCallback = onHideCallback;

            if (_button != null)
            {
                _button.TouchUpInside += (sender, e) =>
                {
                    Hide();

                    onButtonCallback?.Invoke();
                };
            }

            UIApplication.SharedApplication.KeyWindow.AddSubview(this);

            //UIApplication.SharedApplication.SetStatusBarHidden(true, UIStatusBarAnimation.None);

            AnimateNotify(0.3f, () =>
            {
                this.ChangeFrame(y: 0);
            }, (finished) =>
            {
                if (finished && _alertType != ARNotificationType.Confirm)
                    StartHiddenTimer();
                else if (finished)
                {
                    if (_alertType == ARNotificationType.Confirm && _autoHide)
                        StartHiddenTimer();
                    else
                        StartAnimatedHiddenHint();
                }
            });
        }

        public override void RemoveFromSuperview()
        {
            //UIApplication.SharedApplication.SetStatusBarHidden(false, UIStatusBarAnimation.Fade);

            base.RemoveFromSuperview();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (_textView != null)
            {
                _textView.TextContainer.LineBreakMode = UILineBreakMode.TailTruncation;

                if (_realTextSize.Height < Frame.Height - TEXT_VIEW_OFFSET)
                    _textView.ContentOffset = new CGPoint(0, -(Frame.Height - TEXT_VIEW_OFFSET - _realTextSize.Height) / 2);
                else
                    _textView.ContentOffset = new CGPoint(0, -8);
            }
        }

        #region Touches

        private CGPoint _previousTouchPosition;
        private ARNotificationScrollOrientation _scrollOrientation;

        private void AnimateFrameChanges()
        {
            switch (_scrollOrientation)
            {
                case ARNotificationScrollOrientation.FromRightToLeft:
                    this.ChangeFrame(x: -DeviceInfo.ScreenWidth);
                    break;
                case ARNotificationScrollOrientation.FromLeftToRight:
                    this.ChangeFrame(x: DeviceInfo.ScreenWidth);
                    break;
                case ARNotificationScrollOrientation.FromBottomToTop:
                    this.ChangeFrame(y: -HEIGHT);
                    break;
                case ARNotificationScrollOrientation.FromTopToBottom:
                    var maxHeight = (nfloat)Math.Min(_realTextSize.Height + TEXT_VIEW_OFFSET, DeviceInfo.ScreenHeight);
                    this.ChangeFrame(h: maxHeight);
                    _textView.ChangeFrame(h: Frame.Height - TEXT_VIEW_OFFSET);
                    break;
            }
        }

        private double GetTouchDistance(double timestamp, nfloat end, nfloat start)
        {
            return (end - start);// / timestamp;
        }

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            _hiddenTimer?.Stop();
            _hintTimer?.Stop();

            if (touches.Count == 1)
            {
                var touch = touches.First() as UITouch;
                var point = touch.LocationInView(this);

                if (point.Y < HEIGHT)
                    _previousTouchPosition = point;
            }

            base.TouchesBegan(touches, evt);
        }

        public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
        {
            if (touches.Count == 1)
            {
                var touch = touches.First() as UITouch;
                var point = touch.LocationInView(this);


                var currentScrollOrientation = Math.Abs(GetTouchDistance(touch.Timestamp, point.X, _previousTouchPosition.X)) > Math.Abs(GetTouchDistance(touch.Timestamp, point.Y, _previousTouchPosition.Y)) ?
                                         ARNotificationScrollOrientation.FromRightToLeft :
                                         (point.Y < _previousTouchPosition.Y ? ARNotificationScrollOrientation.FromBottomToTop : ARNotificationScrollOrientation.FromTopToBottom);

                //if (_scrollOrientation == ARNotificationScrollOrientation.Unknown)
                //    _scrollOrientation = currentScrollOrientation;
                //else if ((_scrollOrientation == ARNotificationScrollOrientation.FromLeftToRight || _scrollOrientation == ARNotificationScrollOrientation.FromRightToLeft)
                //        && (currentScrollOrientation == ARNotificationScrollOrientation.FromLeftToRight || currentScrollOrientation == ARNotificationScrollOrientation.FromRightToLeft))
                //    _scrollOrientation = currentScrollOrientation;
                //else if ((_scrollOrientation == ARNotificationScrollOrientation.FromBottomToTop || _scrollOrientation == ARNotificationScrollOrientation.FromTopToBottom)
                    //    && (currentScrollOrientation == ARNotificationScrollOrientation.FromBottomToTop || currentScrollOrientation == ARNotificationScrollOrientation.FromTopToBottom))
                    //_scrollOrientation = currentScrollOrientation;

                Debug.WriteLine(point);
                Debug.WriteLine(_scrollOrientation);
                Debug.WriteLine(_previousTouchPosition);

                if (Frame.Y == 0 && point.X < _previousTouchPosition.X && currentScrollOrientation == ARNotificationScrollOrientation.FromRightToLeft)
                {
                    var delta = _previousTouchPosition.X - point.X;

                    this.ChangeFrame(x: Frame.X - delta);

                    _scrollOrientation = currentScrollOrientation;
                }
                else if (Frame.X == 0 && point.Y < _previousTouchPosition.Y && currentScrollOrientation == ARNotificationScrollOrientation.FromBottomToTop)
                {
                    var delta = _previousTouchPosition.Y - point.Y;

                    this.ChangeFrame(y: Frame.Y - delta);

                    _scrollOrientation = currentScrollOrientation;
                }
                else if (Frame.X == 0 && _alertType == ARNotificationType.Error && point.Y > _previousTouchPosition.Y && currentScrollOrientation == ARNotificationScrollOrientation.FromTopToBottom)
                {
                    var delta = point.Y - _previousTouchPosition.Y;

                    if (_realTextSize.Height > TEXT_VIEW_HEIGHT)
                    {
                        var newHeight = Frame.Height + delta;
                        var maxHeight = (nfloat)Math.Min(_realTextSize.Height + TEXT_VIEW_OFFSET, DeviceInfo.ScreenHeight);

                        this.ChangeFrame(h: newHeight > maxHeight ? maxHeight : newHeight);
                    }

                    _scrollOrientation = currentScrollOrientation;
                }

                _previousTouchPosition = point;
            }

            base.TouchesMoved(touches, evt);
        }

        public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
        {
            _scrollOrientation = ARNotificationScrollOrientation.Unknown;

            if (_alertType == ARNotificationType.Confirm)
                OnTouchCancel();
            else
                StartHiddenTimer();

            base.TouchesCancelled(touches, evt);
        }

        public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
        {
            var xOffset = (DeviceInfo.ScreenWidth * 0.1f);
            var yOffset = HEIGHT * -0.1f;

            if (Frame.X < -xOffset || Frame.X > xOffset || Frame.Y < yOffset)
            {
                AnimateNotify(0.3f, 0.0f, UIViewAnimationOptions.CurveEaseIn,
                              () => AnimateFrameChanges(),
                              (finished) =>
                              {
                                  RemoveFromSuperview();
                                  _onHideCallback?.Invoke();

                                  _previousTouchPosition = CGPoint.Empty;
                              });
            }
            else if (_alertType == ARNotificationType.Error && _scrollOrientation == ARNotificationScrollOrientation.FromTopToBottom && Frame.Height > HEIGHT)
            {
                _textView.ScrollEnabled = true;
                _textView.UserInteractionEnabled = true;

                AnimateNotify(0.3f, 0.0f, UIViewAnimationOptions.CurveEaseIn,
                              () => AnimateFrameChanges(),
                              (finished) =>
                              {
                                  _textView.SetContentOffset(CGPoint.Empty, true);
                                  StartHiddenTimer();
                              });
            }
            else
                AnimateNotify(0.3f, 0.0f, UIViewAnimationOptions.CurveEaseIn,
                              () => this.ChangeFrame(x: 0, y: 0),
                              (finished) =>
                              {
                                  TouchesCancelled(touches, evt);

                                  _previousTouchPosition = CGPoint.Empty;
                              });

            _scrollOrientation = ARNotificationScrollOrientation.Unknown;

            base.TouchesEnded(touches, evt);
        }

        #endregion
    }
}
