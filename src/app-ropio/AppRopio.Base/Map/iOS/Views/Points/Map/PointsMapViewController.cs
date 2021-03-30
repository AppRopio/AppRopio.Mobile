using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.Map.Core.ViewModels.Points.Map;
using AppRopio.Base.Map.iOS.Controls.Map;
using AppRopio.Base.Map.iOS.Services;
using CoreAnimation;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;
using AppRopio.Base.Map.Core;

namespace AppRopio.Base.Map.iOS.Views.Points.Map
{
    public partial class PointsMapViewController : CommonViewController<IPointsMapViewModel>
    {
        private const float MIN_INFO_BLOCK_HEIGHT = 74;
        private float IPHONE_X_OFFSET;

        private nfloat _minInfoBlockTop;
        private nfloat _maxInfoBlockTop;

        protected Models.PointsMap PointsMapTheme { get { return Mvx.IoCProvider.Resolve<IMapThemeConfigService>().ThemeConfig.Points.Map; } }
        protected Models.PointInfo InfoTheme { get { return PointsMapTheme.Info ?? Mvx.IoCProvider.Resolve<IMapThemeConfigService>().ThemeConfig.Points.BasePointInfo; } }

        public PointsMapViewController() 
            : base("PointsMapViewController", null)
        {
            IPHONE_X_OFFSET = 0.If_iPhoneX(30);
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(MapConstants.RESX_NAME, "Map_Title");

            SetupMapView(_mapView);
            SetupInfoBlockView(_infoBlockView);
            SetupInfoTopLineView(_infoTopLineView);

            SetupCallButton(_callButton);
            SetupRouteButton(_routeButton);

            SetupTitleLabel(_titleLabel);
            SetupAddressLabel(_addressLabel);

            SetupDistanceImageView(_distanceImageView);
            SetupDistanceLabel(_distanceLabel);

            SetupWorkTimeLabel(_workTimeLabel);

            SetupInfoLabel(_infoLabel);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<PointsMapViewController, IPointsMapViewModel>();

            BindMapView(_mapView, set);

            BindInfoBlockView(_infoBlockView, set);

            BindCallButton(_callButton, set);
            BindRouteButton(_routeButton, set);

            BindTitleLabel(_titleLabel, set);
            BindAddressLabel(_addressLabel, set);

            BindDistanceView(_distanceView, set);
            BindDistanceLabel(_distanceLabel, set);

            BindWorkTimeLabel(_workTimeLabel, set);

            BindInfoLabel(_infoLabel, set);

            set.Apply();
        }

        #endregion

        #region InitializationControls

        protected virtual void SetupMapView(IBindableMapView mapView)
        {
            
        }

        protected virtual void SetupTitleLabel(UILabel titleLabel)
        {
            titleLabel.SetupStyle(InfoTheme.TitleLabel);
        }

        protected virtual void SetupInfoBlockView(UIView infoBlockView)
        {
            infoBlockView.SetupStyle(InfoTheme);

            var mask = UIBezierPath.FromRoundedRect(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height), UIRectCorner.TopLeft | UIRectCorner.TopRight, new CGSize(10, 10));
            var maskLayer = new CAShapeLayer { Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height), Path = mask.CGPath, StrokeColor = infoBlockView.BackgroundColor.CGColor };

            infoBlockView.Layer.Mask = maskLayer;

            var gr = new UIPanGestureRecognizer(InfoPanGesture)
            {
                MaximumNumberOfTouches = 1,
                MinimumNumberOfTouches = 1
            };
            infoBlockView.AddGestureRecognizer(gr);
        }

        protected virtual void SetupInfoTopLineView(UIView infoTopLineView)
        {
            infoTopLineView.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
            infoTopLineView.Layer.CornerRadius = 3;
        }

        protected virtual void SetupCallButton(UIButton callButton)
        {
            callButton.SetupStyle(InfoTheme.CallButton);
        }

        protected virtual void SetupRouteButton(UIButton routeButton)
        {
            routeButton.SetupStyle(InfoTheme.RouteButton);
        }

        protected virtual void SetupAddressLabel(UILabel addressLabel)
        {
            addressLabel.SetupStyle(InfoTheme.AddressLabel);
        }

        protected virtual void SetupDistanceImageView(UIImageView distanceImageView)
        {
            distanceImageView.SetupStyle(InfoTheme.DistanceImage);
        }

        protected virtual void SetupDistanceLabel(UILabel distanceLabel)
        {
            distanceLabel.SetupStyle(InfoTheme.DistanceLabel);
        }

        protected virtual void SetupWorkTimeLabel(UILabel workTimeLabel)
        {
            workTimeLabel.SetupStyle(InfoTheme.WorkTimeLabel);
        }

        protected virtual void SetupInfoLabel(UILabel infoLabel)
        {
            infoLabel.SetupStyle(InfoTheme.InfoLabel);
        }

        #endregion

        #region BindingControls

        protected virtual void BindMapView(IBindableMapView mapView, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(mapView).For(s => s.Items).To(vm => vm.Items);
            set.Bind(mapView).For(s => s.SelectedItem).To(vm => vm.SelectedItem);
            set.Bind(mapView).For(s => s.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
        }

        protected virtual void BindInfoBlockView(UIView infoBlockView, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(infoBlockView).For("AnimatedVisibility").To(vm => vm.IsShowInfoBlock).WithConversion("Visibility");
        }

        protected virtual void BindCallButton(UIButton callButton, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(callButton).To(vm => vm.SelectedItem.CallCommand);
            set.Bind(callButton).For("Visibility").To(vm => vm.SelectedItem.Phone).WithConversion("Visibility");
        }

        protected virtual void BindRouteButton(UIButton routeButton, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(routeButton).To(vm => vm.SelectedItem.RouteCommand);
        }

        protected virtual void BindTitleLabel(UILabel titleLabel, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(titleLabel).To(vm => vm.SelectedItem.Name);
            set.Bind(titleLabel).For("Visibility").To(vm => vm.SelectedItem.Name).WithConversion("Visibility");
        }

        protected virtual void BindAddressLabel(UILabel addressLabel, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(addressLabel).To(vm => vm.SelectedItem.Address);
            set.Bind(addressLabel).For("Visibility").To(vm => vm.SelectedItem.Address).WithConversion("Visibility");
        }

        protected virtual void BindDistanceView(UIView distanceView, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(distanceView).For("Visibility").To(vm => vm.SelectedItem.Distance).WithConversion("Visibility");
        }

        protected virtual void BindDistanceLabel(UILabel distanceLabel, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(distanceLabel).To(vm => vm.SelectedItem.Distance);
        }

        protected virtual void BindWorkTimeLabel(UILabel workTimeLabel, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(workTimeLabel).To(vm => vm.SelectedItem.WorkTime);
            set.Bind(workTimeLabel).For("Visibility").To(vm => vm.SelectedItem.WorkTime).WithConversion("Visibility");
        }

        protected virtual void BindInfoLabel(UILabel infoLabel, MvxFluentBindingDescriptionSet<PointsMapViewController, IPointsMapViewModel> set)
        {
            set.Bind(infoLabel).To(vm => vm.SelectedItem.AdditionalInfo);
            set.Bind(infoLabel).For("Visibility").To(vm => vm.SelectedItem.AdditionalInfo).WithConversion("Visibility");
        }

        #endregion

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _minInfoBlockTop = View.Bounds.Height - MIN_INFO_BLOCK_HEIGHT - IPHONE_X_OFFSET;
        }

        private nfloat _height = -1;
        private void InfoPanGesture(UIPanGestureRecognizer recognizer)
        {
            if (_height < 0)
                _height = _infoBlockView.Bounds.Height + IPHONE_X_OFFSET;

            _maxInfoBlockTop = base.View.Bounds.Height - _height;

            var translation = recognizer.TranslationInView(_infoBlockView);
            var velocity = recognizer.VelocityInView(_infoBlockView);

            if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
                return;

            var y = _infoBlockView.Frame.GetMinY();

            if (recognizer.State == UIGestureRecognizerState.Changed)
            {
                if ((y + translation.Y >= _maxInfoBlockTop) && (y + translation.Y <= _minInfoBlockTop))
                {
                    _infoBlockView.Frame = new CGRect(0, y + translation.Y, _infoBlockView.Bounds.Width, _height);
                }
                else
                {
                    _infoBlockView.Frame = new CGRect(0, (translation.Y >= 0) ? _minInfoBlockTop : _maxInfoBlockTop,
                                                      _infoBlockView.Bounds.Width, _height);
                    _titleLabel.Alpha = (translation.Y >= 0) ? 0 : 1;
                    _addressLabel.Alpha = (translation.Y >= 0) ? 0 : 1;
                    _distanceView.Alpha = (translation.Y >= 0) ? 0 : 1;
                    _workTimeLabel.Alpha = (translation.Y >= 0) ? 0 : 1;
                    _infoLabel.Alpha = (translation.Y >= 0) ? 0 : 1;
                }

                recognizer.SetTranslation(CGPoint.Empty, _infoBlockView);
            }
            else if (recognizer.State == UIGestureRecognizerState.Ended)
            {
                var duration =  velocity.Y < 0 ? (y - _maxInfoBlockTop) / -velocity.Y : (_minInfoBlockTop - y) / velocity.Y;
                duration = duration > 1 ? (nfloat)0.8 : duration;

                UIView.Animate(duration, 0, UIViewAnimationOptions.AllowUserInteraction, () =>
                {
                    _infoBlockView.Frame = new CGRect(0, (velocity.Y >= 0) ? _minInfoBlockTop : _maxInfoBlockTop,
                                                      _infoBlockView.Bounds.Width, _height);

                    _titleLabel.Alpha = (velocity.Y >= 0) ? 0 : 1;
                    _addressLabel.Alpha = (velocity.Y >= 0) ? 0 : 1;
                    _distanceView.Alpha = (velocity.Y >= 0) ? 0 : 1;
                    _workTimeLabel.Alpha = (velocity.Y >= 0) ? 0 : 1;
                    _infoLabel.Alpha = (velocity.Y >= 0) ? 0 : 1;

                }, null);
            }
        }
    }
}

