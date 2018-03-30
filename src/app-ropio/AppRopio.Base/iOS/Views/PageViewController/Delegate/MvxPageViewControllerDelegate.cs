using AppRopio.Base.iOS.Views.PageViewController.ViewSources;
using UIKit;

namespace AppRopio.Base.iOS.Views.PageViewController.Delegate
{
    public class MvxPageViewControllerDelegate : UIPageViewControllerDelegate
    {
        private readonly MvxPageViewSource _dataSource;

        public MvxPageViewControllerDelegate(MvxPageViewSource dataSource)
        {
            _dataSource = dataSource;
        }

        public override void DidFinishAnimating(UIPageViewController pageViewController, bool finished, UIViewController[] previousViewControllers, bool completed)
        {
            if (!completed)
                return;

            var viCo = pageViewController.ViewControllers[0];

            var index = _dataSource.GetPageIndexForController(viCo);

            _dataSource.SetCurrentPageWithoutNPC(index);
            _dataSource.PageChangedCommand?.Execute(index);
        }
    }
}
