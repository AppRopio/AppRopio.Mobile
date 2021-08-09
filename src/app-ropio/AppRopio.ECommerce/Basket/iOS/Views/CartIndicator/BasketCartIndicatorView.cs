using System;
using AppRopio.ECommerce.Basket.Core.ViewModels.CartIndicator;
using AppRopio.ECommerce.Basket.iOS.Models;
using AppRopio.ECommerce.Basket.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS;
using System.Linq;
using CoreGraphics;

namespace AppRopio.ECommerce.Basket.iOS.Views.CartIndicator
{
    public class BasketCartIndicatorView : MvxView, IMvxIosView<IBasketCartIndicatorViewModel>
    {
        #region Fields

        private AppRopio.Base.iOS.Controls.ARLabel _badge;
        private UIImageView _imageView;

        #endregion

        #region Properties

        protected BasketThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig; } }

        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (IBasketCartIndicatorViewModel)value;
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
                ViewModel = (IBasketCartIndicatorViewModel)((value as MvxViewModelInstanceRequest).ViewModelInstance);
                DataContext = ViewModel;
            }
        }

        public IBasketCartIndicatorViewModel ViewModel { get; set; }

        #endregion

        #region Constructor

        public BasketCartIndicatorView()
        {
            InitializeControls();
            this.DelayBind(BindControls);
        }

        public BasketCartIndicatorView(IntPtr handle)
            : base(handle)
        {
        }

        #endregion

        #region Protected

        #region InitializeControls

        private void InitializeControls()
        {
            Frame = new CoreGraphics.CGRect(0, 0, 44, 44);

            _imageView = new UIImageView()
                .WithFrame(8, 8, 28, 28)
                .WithTune(tune =>
            {
                tune.ClipsToBounds = true;
            });
            _imageView.AddConstraint(NSLayoutConstraint.Create(_imageView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 28));
            _imageView.AddConstraint(NSLayoutConstraint.Create(_imageView, NSLayoutAttribute.Width, NSLayoutRelation.GreaterThanOrEqual, 1, 28));

            _badge = new AppRopio.Base.iOS.Controls.ARLabel();
            _badge.WithFrame(24, 4, 16, 16);
            _badge.WithTextAlignment(UITextAlignment.Center);
            _badge.TranslatesAutoresizingMaskIntoConstraints = false;
            _badge.AddConstraint(NSLayoutConstraint.Create(_badge, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 16));
            _badge.AddConstraint(NSLayoutConstraint.Create(_badge, NSLayoutAttribute.Width, NSLayoutRelation.GreaterThanOrEqual, 1, 16));

            SetupImage(_imageView);
            SetupLabel(_badge);

            AddSubviews(_imageView, _badge);

            AddConstraints(new[]
            {
                NSLayoutConstraint.Create(_badge, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, 4),
                NSLayoutConstraint.Create(_badge, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, this, NSLayoutAttribute.Trailing, 1, -4),
                NSLayoutConstraint.Create(_imageView, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1, 0),
                NSLayoutConstraint.Create(_imageView, NSLayoutAttribute.CenterY, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterY, 1, 0)
            });
        }

        private void SetupImage(UIImageView imageView)
        {
            imageView.SetupStyle(ThemeConfig.CartIndicator.Image);
        }

        private void SetupLabel(UILabel badge)
        {
            badge.SetupStyle(ThemeConfig.CartIndicator.Badge);
        }

        #endregion

        #region BindControls

        private void BindControls()
        {
            var set = this.CreateBindingSet<BasketCartIndicatorView, IBasketCartIndicatorViewModel>();
            set.Bind().For("Tap").To(vm => vm.BasketCommand);
            set.Bind(_badge).To(vm => vm.Quantity);
            set.Bind(_badge).For("Visibility").To(vm => vm.Quantity).WithConversion("Visibility");
            set.Apply();
        }

        #endregion

        #endregion

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            if (UIDevice.CurrentDevice.CheckSystemVersion(11, 0))
                Frame = new CoreGraphics.CGRect(0, 0, 44, 44);
        }

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            Animate(0.2f, () => _imageView.Alpha = 0.2f);
        }

        public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            Animate(0.2f, () => _imageView.Alpha = 1);
        }

        public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            var touch = touches.FirstOrDefault() as UITouch;
            if (touch != null)
            {
                var location = touch.LocationInView(this);
                if (location.X > Frame.Width || location.X < 0 || location.Y > Frame.Height || location.Y < 0)
                    Animate(0.2f, () => _imageView.Alpha = 1);
                else
                    Animate(0.2f, () => _imageView.Alpha = 0.2f);
            }
            else
                Animate(0.2f, () => _imageView.Alpha = 1);
        }

        public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            Animate(0.2f, () => _imageView.Alpha = 1);
        }
    }
}