using System;
using System.Collections.Generic;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.iOS.Views.PageViewController.ViewSources;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Views;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Cataloged.ViewSources
{
    public class CategoriesPageViewSource : MvxPageViewSource
    {
        private readonly Dictionary<int, UIViewController> _cachedVC;

        public CategoriesPageViewSource(UIPageViewController pageView)
            : base(pageView)
        {
            _cachedVC = new Dictionary<int, UIViewController>();
        }

        protected override UIViewController GetViewControllerAtIndex(int index)
        {
            if (ItemSource == null || ItemSource.Count() == 0)
                return null;

            UIViewController viewController;

            if (!_cachedVC.TryGetValue(index, out viewController))
            {
                var viewModel = ItemSource.ElementAt(index) as IMvxViewModel;
                viewModel.Initialize();

                viewController = (PageView as IMvxIosView)?.CreateViewControllerFor(viewModel) as UIViewController;

                _cachedVC[index] = viewController;
            }

            return viewController;
        }

        public void UnbindCachedViewControllers()
        {
            _cachedVC?.ForEach(x => (x.Value as IUnbindable)?.Unbind());
        }
    }
}
