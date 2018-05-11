using System.Linq;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using UIKit;

namespace AppRopio.Base.iOS
{
    public class AppRopioView : UIView
    {
        private UIImageView _appropioImage;
        private UILabel _appropioDevelopedLabel;
        private UILabel _appropioNameLabel;

        public AppRopioView()
        {
            Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 97);
            TranslatesAutoresizingMaskIntoConstraints = false;
            SetupAppropioView();
        }

        #region Private

        private void SetupAppropioView()
        {
            this.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 97.If_iPhoneX(127)));
            this.AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 1, UIScreen.MainScreen.Bounds.Width));

            var backgroundColor = Theme.ColorPalette.Background.ToUIColor();
            BackgroundColor = backgroundColor;

            var brightness = ((backgroundColor.CGColor.Components[0] * 299) + (backgroundColor.CGColor.Components[1] * 587) + (backgroundColor.CGColor.Components[2] * 114)) / 1000;

            var contentColor = brightness < 0.5f ? UIColor.White : UIColor.Black;

            AddSubviews(
                _appropioImage = new UIImageView(ImageCache.GetImage("Images/appropio_logo.png", contentColor))
                {
                    TranslatesAutoresizingMaskIntoConstraints = false
                },
                _appropioNameLabel = new UILabel()
                {
                    AttributedText = new NSMutableAttributedString("APPROPIO", new UIStringAttributes { KerningAdjustment = 5, ForegroundColor = contentColor, Font = (UIFont)Theme.FontsPalette.BoldOfSize(13) }),
                    TranslatesAutoresizingMaskIntoConstraints = false
                },
                _appropioDevelopedLabel = new UILabel()
                {
                    Font = (UIFont)Theme.FontsPalette.MediumOfSize(10),
                    TextColor = contentColor.ColorWithAlpha(0.3f),
                    Text = "designed by",
                    TranslatesAutoresizingMaskIntoConstraints = false
                }
            );

            _appropioImage.AddConstraint(NSLayoutConstraint.Create(_appropioImage, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 34));
            _appropioImage.AddConstraint(NSLayoutConstraint.Create(_appropioImage, NSLayoutAttribute.Width, NSLayoutRelation.Equal, 1, 27));

            this.AddConstraints(new NSLayoutConstraint[]
            {
                NSLayoutConstraint.Create(_appropioImage, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, 10),
                NSLayoutConstraint.Create(_appropioImage, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1, 0),

                NSLayoutConstraint.Create(_appropioNameLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, _appropioImage, NSLayoutAttribute.Bottom, 1, 12),
                NSLayoutConstraint.Create(_appropioNameLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1, 0),

                NSLayoutConstraint.Create(_appropioDevelopedLabel, NSLayoutAttribute.Top, NSLayoutRelation.Equal, _appropioNameLabel, NSLayoutAttribute.Bottom, 1, 4),
                NSLayoutConstraint.Create(_appropioDevelopedLabel, NSLayoutAttribute.CenterX, NSLayoutRelation.Equal, this, NSLayoutAttribute.CenterX, 1, 0),
            });
        }

        #endregion

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            Animate(0.2f, () => Alpha = 0.2f);
        }

        public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            var touch = touches.FirstOrDefault() as UITouch;
            if (touch != null)
            {
                var location = touch.LocationInView(this);
                if (location.X > Frame.Width || location.X < 0 || location.Y > Frame.Height || location.Y < 0)
                    return;

                UIApplication.SharedApplication.OpenUrl(NSUrl.FromString($"http://appropio.com?utm_source={NSBundle.MainBundle.InfoDictionary["CFBundleIdentifier"]?.ToString()}"), new UIApplicationOpenUrlOptions(), (obj) => { });
            }

            Animate(0.2f, () => Alpha = 1);
        }

        public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            var touch = touches.FirstOrDefault() as UITouch;
            if (touch != null)
            {
                var location = touch.LocationInView(this);
                if (location.X > Frame.Width || location.X < 0 || location.Y > Frame.Height || location.Y < 0)
                    Animate(0.2f, () => Alpha = 1);
                else
                    Animate(0.2f, () => Alpha = 0.2f);
            }
            else
                Animate(0.2f, () => Alpha = 1);
        }

        public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            Animate(0.2f, () => Alpha = 1);
        }
    }
}