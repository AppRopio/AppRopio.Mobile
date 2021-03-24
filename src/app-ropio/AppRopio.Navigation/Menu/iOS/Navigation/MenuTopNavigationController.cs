using System.Linq;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.iOS.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platform.Core;
using MvvmCross.Platforms.Ios.Views.Base;
using UIKit;

namespace AppRopio.Navigation.Menu.iOS.Navigation
{
    public class MenuTopNavigationController : UINavigationController
    {
        public MenuTopNavigationController()
        {
            
        }

        public MenuTopNavigationController(UIViewController controller)
            : base(controller)
        {
        }

        #region Private

        private void UnbindCycle(ref IUnbindable controller)
        {
            try
            {
                var viewModel = (controller.ViewModel as IBaseViewModel);
                if (viewModel != null)
                {
                    viewModel.Unbind();
                    viewModel.DisposeIfDisposable();
                    viewModel = null;
                }

                controller.Unbind();

                controller.ViewModel.DisposeIfDisposable();
                controller.ViewModel = null;

                var bindingContextOwner = controller as IMvxBindingContextOwner;
                if (bindingContextOwner != null)
                    bindingContextOwner?.ClearAllBindings();

                controller.DisposeIfDisposable();
                controller = null;
            }
            catch {}
        }

        private void OnDidDisappearCalled(object sender, MvxValueEventArgs<bool> e)
        {
            var controller = sender as IUnbindable;
            if (controller != null)
                UnbindCycle(ref controller);

            sender.DisposeIfDisposable();
            sender = null;
        }

        private void OnWillAppearCalled(object sender, MvxValueEventArgs<bool> e)
        {
            var topController = sender as IMvxEventSourceViewController;

            topController.ViewDidDisappearCalled -= OnDidDisappearCalled;
            topController.ViewWillAppearCalled -= OnWillAppearCalled;
        }

        private void ScheduleUnbind(UIViewController viCo)
        {
            var controller = viCo as IMvxEventSourceViewController;
            if (controller != null)
            {
                controller.ViewDidDisappearCalled += OnDidDisappearCalled;
                controller.ViewWillAppearCalled += OnWillAppearCalled;
            }
        }

        private void PauseTopViewController(UIViewController viewController)
        {
            var unbindableViCo = viewController as IUnbindable;
            if (unbindableViCo != null)
            {
                var baseVm = (unbindableViCo.ViewModel as IBaseViewModel);
                if (baseVm != null)
                    baseVm.Pause();

                unbindableViCo.Pause();
            }
        }

        #endregion

        #region Public

        public override void PushViewController(UIViewController viewController, bool animated)
        {
            PauseTopViewController(this.TopViewController);

            base.PushViewController(viewController, animated);
        }

        public override void PresentViewController(UIViewController viewControllerToPresent, bool animated, System.Action completionHandler)
        {
            PauseTopViewController(this.TopViewController);

            base.PresentViewController(viewControllerToPresent, animated, completionHandler);
        }

        public override System.Threading.Tasks.Task PresentViewControllerAsync(UIViewController viewControllerToPresent, bool animated)
        {
            PauseTopViewController(this.TopViewController);

            return base.PresentViewControllerAsync(viewControllerToPresent, animated);
        }

        public override UIViewController PopViewController(bool animated)
        {
            ScheduleUnbind(this.TopViewController);

            return base.PopViewController(animated);
        }

        public override System.Threading.Tasks.Task DismissViewControllerAsync(bool animated)
        {
            ScheduleUnbind(this.TopViewController);

            return base.DismissViewControllerAsync(animated);
        }

        public override UIViewController[] PopToRootViewController(bool animated)
        {
            var removedViewControllers = base.PopToRootViewController(animated);

            if (removedViewControllers != null && removedViewControllers.Any())
            {
                foreach (var viCo in removedViewControllers)
                {
                    ScheduleUnbind(viCo);
                }
            }

            return removedViewControllers;
        }

        #endregion
    }
}

