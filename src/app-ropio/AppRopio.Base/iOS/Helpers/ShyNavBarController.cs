using System;
using Foundation;
using UIKit;

namespace AppRopio.Base.iOS.Helpers
{
    public class ShyNavBarController
    {
        #region Fields

        private nfloat _previousScrollViewYOffset;

        private const float STATUS_BAR_HEIGHT = 20f;

        private float _topOffsetOfNavBar;

        private UINavigationBar _navigationBar;

        public const string NAVIGATION_BAR_FRAME_CHANGED = "NAVIGATION_BAR_FRAME_CHANGED";

        #endregion

        #region Private

        private void AnimateNavBarTo(nfloat y)
        {
            UIView.Animate(0.2,
            () =>
            {
                var navBarFrame = _navigationBar.Frame;
                navBarFrame.Y = y;

                _navigationBar.Frame = navBarFrame;

                NSNotificationCenter.DefaultCenter.PostNotificationName(NAVIGATION_BAR_FRAME_CHANGED, NSValue.FromCGRect(navBarFrame));
            });
        }

        private void StoppedScrolling()
        {
            var frame = _navigationBar.Frame;
            if (frame.Y < -STATUS_BAR_HEIGHT) //hide
                AnimateNavBarTo(-(frame.Size.Height + STATUS_BAR_HEIGHT + _topOffsetOfNavBar));
            else if (frame.Y <= STATUS_BAR_HEIGHT) //show
                AnimateNavBarTo(STATUS_BAR_HEIGHT);
        }

        #endregion

        #region Public

        public void Scrolled(UIScrollView scrollView)
        {
            if (_navigationBar != null && scrollView.ScrollEnabled)
            {
                var navBarFrame = _navigationBar.Frame;
                var navBarMinY = -(navBarFrame.Height + STATUS_BAR_HEIGHT + _topOffsetOfNavBar);

                var scrollOffset = scrollView.ContentOffset.Y;
                var scrollDiff = scrollOffset - _previousScrollViewYOffset;
                var scrollHeight = scrollView.Frame.Height;
                var scrollContentSizeHeight = scrollView.ContentSize.Height + scrollView.ContentInset.Bottom;

                if (scrollOffset <= -(scrollView.ContentInset.Top)) // bounces top
                    navBarFrame.Y = STATUS_BAR_HEIGHT;
                else if ((scrollOffset + scrollHeight) >= scrollContentSizeHeight) // bounces bottom
                    navBarFrame.Y = navBarMinY;
                else
                    navBarFrame.Y = (nfloat)Math.Min(STATUS_BAR_HEIGHT, Math.Max(navBarMinY, navBarFrame.Y - scrollDiff));

                _navigationBar.Frame = navBarFrame;

                NSNotificationCenter.DefaultCenter.PostNotificationName(NAVIGATION_BAR_FRAME_CHANGED, NSValue.FromCGRect(navBarFrame));

                _previousScrollViewYOffset = scrollOffset;
            }
        }

        public void DecelerationEnded()
        {
            if (_navigationBar != null)
                StoppedScrolling();
        }

        public void DraggingEnded(bool willDecelerate)
        {
            if (_navigationBar != null && !willDecelerate)
                StoppedScrolling();
        }

        /// <summary>
        /// Sets the top offset scrollView of navigaiton bar. You must invoke this method before EnableHideNavBarOnSwype
        /// </summary>
        public void SetTopOffsetOfNavBar(float yOffset) => _topOffsetOfNavBar = yOffset;

        public void EnableHideNavBarOnSwype(UINavigationBar navigationBar, UIScrollView scrollView, bool setupContentInset = true)
        {
            _navigationBar = navigationBar;

            if (setupContentInset)
            {
                var offset = _navigationBar.Frame.Height + _topOffsetOfNavBar;
                scrollView.ContentInset = new UIEdgeInsets(offset, 0, offset, 0);
                scrollView.ScrollIndicatorInsets = new UIEdgeInsets(offset, 0, offset, 0);
            }
        }

        public void DisableHideNavBarOnSwype()
        {
            _navigationBar = null;
        }

        #endregion
    }
}
