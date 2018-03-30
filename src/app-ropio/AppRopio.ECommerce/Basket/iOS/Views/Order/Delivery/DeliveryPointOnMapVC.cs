using System;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery.Map;
using CoreAnimation;
using CoreGraphics;
using CoreLocation;
using MapKit;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery
{
    public partial class DeliveryPointOnMapVC : CommonViewController<IDeliveryPointOnMapVM>
    {
        private PlaceAnnotationManager<IDeliveryPointItemVM> _annotationManager;
        private UIBarButtonItem _closeButton;

        private const float MIN_INFO_BLOCK_HEIGHT = 74;
        private nfloat _minInfoBlockTop;
        private nfloat _maxInfoBlockTop;

        protected Models.DeliveryOnPointMap MapTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.OnPoint.Map; } }
        protected Models.DeliveryPointInfo InfoTheme { get { return MapTheme.Info ?? Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.OnPoint.BaseDeliveryPointInfo; } }

        public DeliveryPointOnMapVC() : base("DeliveryPointOnMapVC", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = "На карте";

            if (ViewModel.VmNavigationType == NavigationType.PresentModal)
            {
                _closeButton = new UIBarButtonItem();
                NavigationItem.SetRightBarButtonItem(_closeButton, true);
                SetupCloseButton(_closeButton);
            }
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

            _stackViewBottomConstraint.Constant = 16.If_iPhoneX(46);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM>();

            BindCloseButton(_closeButton, set);
            BindMapView(_mapView, set);

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

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();
        }

        #endregion

        #region InitializationControls

        /// <summary>
        /// Используется (и отображается) только в модальном варианте экрана
        /// </summary>
        protected virtual void SetupCloseButton(UIBarButtonItem closeButton)
        {
            closeButton.SetupStyle(MapTheme.CloseButton);
        }

        protected virtual void SetupMapView(MKMapView mapView)
        {
            mapView.Delegate = new PlaceMapDelegate<IDeliveryPointItemVM>();
            _annotationManager = new PlaceAnnotationManager<IDeliveryPointItemVM>(mapView);
        }

        protected virtual void SetupTitleLabel(UILabel titleLabel)
        {
            titleLabel.SetupStyle(InfoTheme.TitleLabel);
        }

        protected virtual void SetupInfoBlockView(UIView infoBlockView)
        {
            infoBlockView.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();

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

        protected virtual void BindCloseButton(UIBarButtonItem closeButton, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(closeButton).To(vm => vm.CloseCommand);
        }

        protected virtual void BindMapView(MKMapView mapView, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(_annotationManager).For(s => s.Item).To(vm => vm.Item);
        }

        protected virtual void BindCallButton(UIButton callButton, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(callButton).To(vm => vm.Item.CallCommand);
            set.Bind(callButton).For("AnimatedVisibility").To(vm => vm.Item.Phone).WithConversion("Visibility");
        }

        protected virtual void BindRouteButton(UIButton routeButton, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(routeButton).To(vm => vm.Item.RouteCommand);
        }

        protected virtual void BindTitleLabel(UILabel titleLabel, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(titleLabel).To(vm => vm.Item.Name);
            set.Bind(titleLabel).For("AnimatedVisibility").To(vm => vm.Item.Name).WithConversion("Visibility");
        }

        protected virtual void BindAddressLabel(UILabel addressLabel, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(addressLabel).To(vm => vm.Item.Address);
            set.Bind(addressLabel).For("AnimatedVisibility").To(vm => vm.Item.Address).WithConversion("Visibility");
        }

        protected virtual void BindDistanceView(UIView distanceView, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(distanceView).For("AnimatedVisibility").To(vm => vm.Item.Distance).WithConversion("Visibility");
        }

        protected virtual void BindDistanceLabel(UILabel distanceLabel, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(distanceLabel).To(vm => vm.Item.Distance);
        }

        protected virtual void BindWorkTimeLabel(UILabel workTimeLabel, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(workTimeLabel).To(vm => vm.Item.WorkTime);
            set.Bind(workTimeLabel).For("AnimatedVisibility").To(vm => vm.Item.WorkTime).WithConversion("Visibility");
        }

        protected virtual void BindInfoLabel(UILabel infoLabel, MvxFluentBindingDescriptionSet<DeliveryPointOnMapVC, IDeliveryPointOnMapVM> set)
        {
            set.Bind(infoLabel).To(vm => vm.Item.AdditionalInfo);
            set.Bind(infoLabel).For("AnimatedVisibility").To(vm => vm.Item.AdditionalInfo).WithConversion("Visibility");
        }

        #endregion

        #region Map

        private void ZoomToPoint()
        {
            var point = ViewModel?.Item?.Coordinates;
            if (point == null)
                return;

            MKCoordinateRegion region;
            region.Center = new CLLocationCoordinate2D(point.Latitude, point.Longitude);
            region.Span.LatitudeDelta = 0.01;
            region.Span.LongitudeDelta = 0.01;
            _mapView.SetRegion(region, true);
        }

        #endregion

        #region Private

        private void InfoPanGesture(UIPanGestureRecognizer recognizer)
        {
            _maxInfoBlockTop = View.Bounds.Height - _infoBlockView.Bounds.Height;

            var translation = recognizer.TranslationInView(_infoBlockView);
            var velocity = recognizer.VelocityInView(_infoBlockView);

            if (Math.Abs(velocity.X) > Math.Abs(velocity.Y))
                return;

            var y = _infoBlockView.Frame.GetMinY();

            if (recognizer.State == UIGestureRecognizerState.Changed)
            {
                if ((y + translation.Y >= _maxInfoBlockTop) && (y + translation.Y <= _minInfoBlockTop))
                {
                    _infoBlockView.Frame = new CGRect(0, y + translation.Y, _infoBlockView.Bounds.Width, _infoBlockView.Bounds.Height);
                }
                else
                {
                    _infoBlockView.Frame = new CGRect(0, (translation.Y >= 0) ? _minInfoBlockTop : _maxInfoBlockTop,
                                                      _infoBlockView.Bounds.Width, _infoBlockView.Bounds.Height);
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
                                                      _infoBlockView.Frame.Width, _infoBlockView.Frame.Height);

                }, null);
            }
        }

        #endregion

        #region Public

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _minInfoBlockTop = View.Bounds.Height - MIN_INFO_BLOCK_HEIGHT;

            ZoomToPoint();
        }

        public override void ViewWillAppear(bool animated)
        {
            _mapViewTopConstraint.Constant = -64;
            UIView.Animate(0.22, () => 
            {
                NavigationController.SetTranparentNavBar(true);
                _mapView.LayoutIfNeeded();
            });

            base.ViewWillAppear(animated);
        }

        #endregion
    }
}

