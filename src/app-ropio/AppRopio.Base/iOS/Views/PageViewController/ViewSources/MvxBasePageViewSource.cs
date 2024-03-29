﻿using System;
using MvvmCross;
using MvvmCross.Exceptions;
using MvvmCross.Logging;
using UIKit;

namespace AppRopio.Base.iOS.Views.PageViewController.ViewSources
{
    public abstract class MvxBasePageViewSource : UIPageViewControllerDataSource
    {
        private readonly UIPageViewController _pageView;

        public UIPageViewController PageView => this._pageView;

        protected MvxBasePageViewSource(UIPageViewController pageView)
        {
            this._pageView = pageView;
        }

        public virtual void ReloadData()
        {
            try
            {
                var controller = GetInitialViewController();
                if (controller != null)
                {
                    var list = new UIViewController[] { controller };
                    // Bug found here: http://stackoverflow.com/questions/15325891/refresh-uipageviewcontroller-reorder-pages-and-add-new-pages
                    // has been partially fixed in iOS10 but it is not perfect yet. Let the developer decide what to do.
                    SetViewControllers(list);
                }
            }
            catch (Exception exception)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn($"Exception masked during PageView SetViewControllers {exception.ToLongString()}");
            }
        }

        protected virtual void SetViewControllers(UIViewController[] viewControllers, UIPageViewControllerNavigationDirection direction = UIPageViewControllerNavigationDirection.Forward)
        {
            PageView.SetViewControllers(viewControllers, direction, true, null);
        }

        protected abstract UIViewController GetViewControllerAtIndex(int index);

        protected virtual UIViewController GetInitialViewController()
        {
            return GetViewControllerAtIndex(0);
        }
    }
}
