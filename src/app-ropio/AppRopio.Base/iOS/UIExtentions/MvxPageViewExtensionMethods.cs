using System;
using System.Collections.Generic;
using AppRopio.Base.iOS.Views.PageViewController;
using AppRopio.Base.iOS.Views.PageViewController.ViewSources;
using MvvmCross.Core;
using MvvmCross.ViewModels;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.iOS.UIExtentions
{
    public static class MvxPageViewExtensions
    {

        public static UIViewController CreateViewController<T>(this MvxPageViewSource self, object parameterValuesObject, int pageIndex = -1)
        {
            return CreateViewController<T>(self, parameterValuesObject.ToSimplePropertyDictionary(), pageIndex);
        }

        public static UIViewController CreateViewController<T>(this MvxPageViewSource self, IDictionary<string, string> parameterValues, int pageIndex = -1)
        {
            return CreateViewController<T>(self, new MvxBundle(parameterValues), pageIndex);
        }

        public static UIViewController CreateViewController<T>(this MvxPageViewSource self, IMvxBundle parameterBundle, int pageIndex = -1)
        {
            return CreateViewControllerImpl(self, typeof(T), parameterBundle, pageIndex);
        }

        private static UIViewController CreateViewControllerImpl(this MvxPageViewSource self, Type viewModelType, IMvxBundle parameterBundle, int pageIndex)
        {
            var request = new MvxViewModelRequest(viewModelType, parameterBundle, null);
            var viewController = (self.PageView as IMvxIosView)?.CreateViewControllerFor(request) as UIViewController;

            if (pageIndex >= 0)
                SetPageIndexForController(viewController, pageIndex);
            return viewController;
        }

        private static void SetPageIndexForController(UIViewController referenceViewController, int index)
        {
            var mvxPageView = referenceViewController as IMvxPageViewController;
            if (mvxPageView != null)
            {
                var prop = mvxPageView.GetType().GetProperty("PageIndex", BindingFlags.Public | BindingFlags.Instance);
                if (prop?.GetSetMethod() != null)
                    prop.SetValue(mvxPageView, index);
            }
        }
    }
}
