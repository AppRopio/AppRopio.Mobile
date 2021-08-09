using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;
using MvvmCross;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.Logging;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.WeakSubscription;
using UIKit;

namespace AppRopio.Base.iOS.Views.PageViewController.ViewSources
{
    public abstract class MvxPageViewSource : MvxBasePageViewSource
    {
        private IEnumerable _itemSource;
        private IDisposable _subscription;

        private int _currentPage;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                if (_currentPage != value)
                {
                    if (ItemSource != null && value < ItemSource.Count())
                    {
                        try
                        {
                            var controller = GetViewControllerAtIndex(value);
                            if (controller != null)
                            {
                                var list = new UIViewController[] { controller };
                                // Bug found here: http://stackoverflow.com/questions/15325891/refresh-uipageviewcontroller-reorder-pages-and-add-new-pages
                                // has been partially fixed in iOS10 but it is not perfect yet. Let the developer decide what to do.
                                SetViewControllers(list, value >= _currentPage ? UIPageViewControllerNavigationDirection.Forward : UIPageViewControllerNavigationDirection.Reverse);
                            }
                        }
                        catch (Exception exception)
                        {
                            Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{this.GetType().FullName}: {exception.BuildAllMessagesAndStackTrace()}");
                        }
                    }

                    _currentPage = value;
                }
            }
        }

        public ICommand PageChangedCommand { get; set; }

        protected MvxPageViewSource(UIPageViewController pageView) : base(pageView)
        {
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }
            }

            base.Dispose(disposing);
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemSource
        {
            get { return _itemSource; }
            set
            {
                if (ReferenceEquals(_itemSource, value))
                    return;

                if (_subscription != null)
                {
                    _subscription.Dispose();
                    _subscription = null;
                }

                _itemSource = value;

                var collectionChanged = _itemSource as INotifyCollectionChanged;
                if (collectionChanged != null)
                {
                    _subscription = collectionChanged.WeakSubscribe(CollectionChangedOnCollectionChanged);
                }

                ReloadData();
            }
        }

        public virtual bool CanLoop { get; set; }

        public virtual bool PageIndicatorEnabled { get; set; }

        protected virtual void CollectionChangedOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            ReloadData();
        }

        public virtual int GetPageIndexForController(UIViewController referenceViewController)
        {
            var mvxView = referenceViewController as IMvxView;
            if (mvxView != null)
            {
                var vm = mvxView.ViewModel as Core.ViewModels.IMvxPageViewModel;
                if (vm != null)
                    return vm.PageIndex;
            }

            var mvxPageView = referenceViewController as IMvxPageViewController;
            if (mvxPageView != null)
                return mvxPageView.PageIndex;

            return -1;
        }

        public override UIViewController GetNextViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            if (ItemSource.Count() == 1)
                return null;

            var page = GetPageIndexForController(referenceViewController) + 1;
            if (page != 0)
            {
                var index = (page == ItemSource.Count()) ?
                    (CanLoop ? 0 : -1) :
                    (page);

                return index == -1 ? null : GetViewControllerAtIndex(index);
            }

            return null;
        }

        public override UIViewController GetPreviousViewController(UIPageViewController pageViewController, UIViewController referenceViewController)
        {
            if (ItemSource.Count() == 1)
                return null;

            var page = GetPageIndexForController(referenceViewController);
            if (page != -1)
            {
                var index = (page == 0) ?
                    (CanLoop ? ItemSource.Count() : 0) - 1 :
                    (page - 1);

                return index == -1 ? null : GetViewControllerAtIndex(index);
            }

            return null;
        }

        public override nint GetPresentationCount(UIPageViewController pageViewController)
        {
            var count = ItemSource.Count();

            return PageIndicatorEnabled ?
                                        (count == 1 ? 0 : count) :
                                        0;
        }

        public override nint GetPresentationIndex(UIPageViewController pageViewController)
        {
            return 0;
        }

        public void SetCurrentPageWithoutNPC(int index)
        {
            _currentPage = index;
        }
    }
}
