using System;
using UIKit;
namespace AppRopio.Base.iOS.UIExtentions
{
    public static class UINavigationControllerExtensions
    {
        public static void SetTranparentNavBar(this UINavigationController self, bool isTransparent, bool withAnimation = false)
        {
            var action = new Action(() =>
            {
                self.NavigationBarHidden = false;
                if (isTransparent)
                {
                    self.NavigationBar.BackgroundColor = null;
                    self.NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
                    self.NavigationBar.ShadowImage = new UIImage();
                    self.NavigationBar.Translucent = true;
                }
                else
                {
                    self.NavigationBar.SetupStyle(Theme.ControlPalette.NavigationBar);
                }
            });

            if (withAnimation)
                UIView.Animate(0.22, action);
            else
                action();
        }
    }
}
