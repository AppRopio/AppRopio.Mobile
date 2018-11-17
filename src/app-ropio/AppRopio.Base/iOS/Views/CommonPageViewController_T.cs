using System;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS.Views.PageViewController;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.iOS.Views
{
    public abstract class CommonPageViewController<T> : MvxPageViewController<T>, IUnbindable
        where T : class, IMvxViewModel
    {
        protected ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();

        protected CommonPageViewController(
            UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll,
            UIPageViewControllerNavigationOrientation orientation = UIPageViewControllerNavigationOrientation.Horizontal,
            UIPageViewControllerSpineLocation spine = UIPageViewControllerSpineLocation.None)
            : base(style, orientation, spine)
        {

        }

        protected CommonPageViewController(IntPtr handle)
            : base(handle)
        {

        }

        protected CommonPageViewController(string nibName, Foundation.NSBundle bundle)
            : base(nibName, bundle)
        {

        }

        protected abstract void BindControls();

        protected virtual void InitializeControls()
        {

        }

        protected virtual void CleanUp()
        {

        }

        public override void LoadView()
        {
            base.LoadView();

            View.BackgroundColor = (UIColor)Theme.ColorPalette.Background;

            //this.EdgesForExtendedLayout = UIRectEdge.None;

            InitializeControls();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //NOTE: code below enabled synchronously invoking binding actions when binding context was changed. 
            //That may slow screen opening or closing, but exempt of exceptions when target binding is null
            //var bindingContext = this.BindingContext as MvxTaskBasedBindingContext;
            //if (bindingContext != null)
            //    bindingContext.RunSynchronously = true;

            if (ViewModel != null)
                BindControls();
        }

        public virtual void Pause()
        {

        }

        public void Unbind()
        {
            CleanUp();
        }
    }
}
