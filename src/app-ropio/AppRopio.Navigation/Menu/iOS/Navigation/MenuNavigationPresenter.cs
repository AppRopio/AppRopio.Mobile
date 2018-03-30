using System;
using System.Collections.Generic;
using System.Linq;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.PresentationHints;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.Navigation.Menu.Core.ViewModels.Services;
using AppRopio.Navigation.Menu.iOS.Models;
using AppRopio.Navigation.Menu.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.iOS.Views;
using UIKit;
using MvvmCross.Core.Navigation;
using AppRopio.Base.Core.Models.Bundle;

namespace AppRopio.Navigation.Menu.iOS.Navigation
{
    public class MenuNavigationPresenter : MvxIosViewPresenter
    {
        private bool _navigationInProgress;

        protected List<MvxViewModelRequest> _scheduledViewControllers;

        protected MenuThemeConfig ThemeConfig { get { return Mvx.Resolve<IMenuThemeConfigService>().ThemeConfig; } }

        public MenuNavigationController MenuNavigationController { get; set; }

        public MenuNavigationPresenter(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            _scheduledViewControllers = new List<MvxViewModelRequest>();

            MenuNavigationController = new MenuNavigationController();
        }

        #region Private

        #region Navigation bar buttons

        private void OnMenuTouch(object sender, EventArgs args)
        {
            MenuNavigationController.ShowLeftMenu();
        }

        private void SetNavBarButtons(UIViewController viewController, string title = null, bool root = false, bool presentModal = false)
        {
            var topItem = viewController.NavigationItem;

            var navigationController = viewController.NavigationController ?? MenuNavigationController.TopNavigationController;

            navigationController.NavigationBar.SetupStyle(Theme.ControlPalette.NavigationBar);

            if (Theme.ControlPalette.NavigationBar.UseCustomView)
            {
                var width = DeviceInfo.ScreenWidth - 88;
                UIView customTitle = null;

                if (Theme.ControlPalette.NavigationBar.Logo?.Path.IsNullOrEmtpy() ?? false)
                {
                    customTitle = new UIView()
                        .WithFrame(0, 0, width, 44)
                        .WithSubviews(new UIImageView()
                                     .WithFrame(root ? -10 : -12, 0, width, 44)
                                     .WithTune(img =>
                                     {
                                         img.SetupStyle(Theme.ControlPalette.NavigationBar.Logo);
                                     }));
                }
                else
                {
                    customTitle = new UIView()
                       .WithFrame(0, 0, width, 44)
                       .WithSubviews(new Base.iOS.Controls.ARLabel()
                                     .WithFrame(root ? -10 : -12, 0, width, 44)
                                     .WithTune(label =>
                                     {
                                         label.SetupStyle(Theme.ControlPalette.NavigationBar.Title);
                                         label.Text = title;
                                     }));
                }

                topItem.TitleView = customTitle;
            }

            if (!presentModal && root)
                viewController.NavigationItem.SetLeftBarButtonItem(
                    new UIBarButtonItem(
                        ImageCache.GetImage(ThemeConfig.NavBarMenuImage.Path).CreateResizableImage(new UIEdgeInsets(1, 27, 1, 0)),
                        UIBarButtonItemStyle.Plain,
                        OnMenuTouch),
                    false);

            var backImg = ImageCache.GetImage(Theme.ControlPalette.NavigationBar.BackImage.Path);
            if (backImg != null)
            {
                var backButton = new UIBarButtonItem(" ", UIBarButtonItemStyle.Plain, (s, e) => navigationController.PopViewController(true));

                topItem.BackBarButtonItem = backButton;
                topItem.LeftItemsSupplementBackButton = false;

                navigationController.NavigationBar.BackIndicatorImage = backImg.CreateResizableImage(new UIEdgeInsets(1, 27, 1, 0));
                navigationController.NavigationBar.BackIndicatorTransitionMaskImage = backImg.CreateResizableImage(new UIEdgeInsets(1, 27, 1, 0));
            }
            else
            {
                var backButton = new UIBarButtonItem("", UIBarButtonItemStyle.Plain, (s, e) => navigationController.PopViewController(true));
                topItem.BackBarButtonItem = backButton;
                topItem.LeftItemsSupplementBackButton = false;
            }
        }

        #endregion

        #region Unbind

        private static void UnbindCycle(ref IUnbindable controller)
        {
            controller.Unbind();

            var viewModel = (controller.ViewModel as IBaseViewModel);
            if (viewModel != null)
            {
                viewModel.Unbind();

                viewModel.DisposeIfDisposable();
                viewModel = null;
            }

            controller.ViewModel.DisposeIfDisposable();
            controller.ViewModel = null;

            try
            {
                var bindingContextOwner = controller as IMvxBindingContextOwner;
                if (bindingContextOwner != null)
                    bindingContextOwner.ClearAllBindings();
            }
            catch
            {

            }
        }

        #endregion

        #region Navigation methods

        private UIViewController GetLastPresentedController(UIViewController lastNotNullPresentedController)
        {
            return lastNotNullPresentedController.PresentedViewController != null ? GetLastPresentedController(lastNotNullPresentedController.PresentedViewController) : lastNotNullPresentedController;
        }

        private UIViewController GetLastPresentedControllerForViewModel(UINavigationController lastNotNullPresentedController, IMvxViewModel viewModel)
        {
            var mvxViewController = lastNotNullPresentedController.TopViewController as IMvxIosView;

            return mvxViewController != null && mvxViewController.ViewModel.GetType() == viewModel.GetType() ?
                                                 lastNotNullPresentedController
                                                 :
                                                 GetLastPresentedController(lastNotNullPresentedController.PresentedViewController);
        }

        private void ShowViewController(MvxViewModelRequest request)
        {
            MenuNavigationController.HideMenu();

            IBaseViewModel viewModel = null;

            viewModel = request is MvxViewModelInstanceRequest instanceRequest ?
                (IBaseViewModel)instanceRequest.ViewModelInstance
                            :
                        (IBaseViewModel)Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(request, null);

            var view = this.CreateViewControllerFor(request);
            view.ViewModel = viewModel;

            var viewController = view as UIViewController;

            var navigationType = viewModel?.VmNavigationType;

            if (navigationType == null || _navigationInProgress)
                return;

            _navigationInProgress = true;

            if (navigationType == NavigationType.ClearAndPush || navigationType == NavigationType.DoubleClearAndPush)
            {
                if (PushRootController(viewController, navigationType == NavigationType.DoubleClearAndPush))
                    SetNavBarButtons(viewController, viewModel?.Title, true);
            }
            else if (navigationType == NavigationType.Push || navigationType == NavigationType.DoublePush)
            {
                var controller = MenuNavigationController.TopNavigationController.PresentedViewController != null ?
                                        GetLastPresentedController(MenuNavigationController.TopNavigationController.PresentedViewController) :
                                        MenuNavigationController.TopNavigationController;

                if (controller is UINavigationController navigationController)
                {
                    navigationController.PushViewController(viewController, true);

                    SetNavBarButtons(viewController, viewModel?.Title);
                }
                else if (controller != null)
                    SetupPresentationOptionsAndPresent(viewController, controller);
            }
            else if (navigationType == NavigationType.PresentModal)
            {
                PresentModalViCo(viewController);

                SetNavBarButtons(viewController, viewModel?.Title, true, true);
            }

            _navigationInProgress = false;
        }

        private void PresentModalViCo(UIViewController viewController)
        {
            var controller = MenuNavigationController.TopNavigationController.PresentedViewController != null ?
                                        GetLastPresentedController(MenuNavigationController.TopNavigationController.PresentedViewController) :
                                        MenuNavigationController.TopNavigationController;

            SetupPresentationOptionsAndPresent(viewController, controller);
        }

        private static void SetupPresentationOptionsAndPresent(UIViewController viewController, UIViewController presentedViewController)
        {
            UINavigationController navigationController = null;

            if (viewController.ModalPresentationStyle != UIModalPresentationStyle.None &&
                            viewController.ModalPresentationStyle != UIModalPresentationStyle.Custom)
            {
                navigationController = new MenuTopNavigationController(viewController)
                {
                    NavigationBarHidden = true,
                    ModalPresentationStyle = viewController.ModalPresentationStyle,
                    TransitioningDelegate = viewController.TransitioningDelegate
                };
            }
            else if (viewController.ModalPresentationStyle == UIModalPresentationStyle.None)
            {
                navigationController = new MenuTopNavigationController(viewController) { NavigationBarHidden = true };
            }

            navigationController?.NavigationBar?.SetupStyle(Theme.ControlPalette.NavigationBar);

            presentedViewController.PresentViewControllerAsync(navigationController ?? viewController, true);
        }

        private bool PushRootController(UIViewController newRootViewController, bool canShowTheSameController = false)
        {
            var topNavigationController = MenuNavigationController.TopNavigationController;

            topNavigationController.PopToRootViewController(false);

            var successful = true;
            var oldViewController = topNavigationController.ChildViewControllers.IsNullOrEmpty() ? null : topNavigationController.ChildViewControllers.First();

            if (topNavigationController.PresentedViewController != null)
                topNavigationController.DismissViewController(false, null);

            if (canShowTheSameController || (oldViewController == null) || (oldViewController != null && newRootViewController.GetType() != oldViewController.GetType()))
            {
                if (oldViewController != null)
                {
                    if (oldViewController is MvxViewController) //баг в iOS – ViewDidDisappear не вызывается при вызове RemoveFromParentViewController()
                    {
                        var unbindableVC = oldViewController as IUnbindable;
                        if (unbindableVC != null)
                            UnbindCycle(ref unbindableVC);
                    }
                    else
                    {
                        var eventSourceViCo = (oldViewController as IMvxEventSourceViewController);
                        if (eventSourceViCo != null)
                            eventSourceViCo.ViewDidDisappearCalled += (sender, e) =>
                            {
                                var unbindableVC = sender as IUnbindable;
                                if (unbindableVC != null)
                                    UnbindCycle(ref unbindableVC);
                            };
                    }

                    oldViewController.RemoveFromParentViewController();
                }

                if (topNavigationController.NavigationBar.TopItem != null)
                    topNavigationController.NavigationBar.TopItem.Title = newRootViewController.Title ?? string.Empty;

                topNavigationController.PushViewController(newRootViewController, false);
            }
            else
            {
                var unbindableVC = newRootViewController as IUnbindable;
                if (unbindableVC != null)
                {
                    UnbindCycle(ref unbindableVC);

                    unbindableVC.DisposeIfDisposable();
                    unbindableVC = null;
                }

                newRootViewController.Dispose();
                newRootViewController = null;

                successful = false;
            }

            if (MenuNavigationController.LeftMenuVisible || MenuNavigationController.RightMenuVisible)
                MenuNavigationController.HideMenu(true);

            return successful;
        }

        #endregion

        #endregion

        #region Protected

        protected virtual void SetupFlyoutNavigationController(MenuNavigationController flyoutController)
        {
            flyoutController.View.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();
            flyoutController.LayerShadowing = true;

            flyoutController.LeftMenuNavigationController.View.BackgroundColor = Theme.ColorPalette.BackgroundMenu.ToUIColor();
            if (!flyoutController.LeftMenuNavigationController.View.Subviews.IsNullOrEmpty())
                flyoutController.LeftMenuNavigationController.View.Subviews[0].BackgroundColor = Theme.ColorPalette.BackgroundMenu.ToUIColor();

            flyoutController.RightMenuNavigationController.View.BackgroundColor = Theme.ColorPalette.BackgroundMenu.ToUIColor();
            if (!flyoutController.RightMenuNavigationController.View.Subviews.IsNullOrEmpty())
                flyoutController.RightMenuNavigationController.View.Subviews[0].BackgroundColor = Theme.ColorPalette.BackgroundMenu.ToUIColor();
            flyoutController.RightMenuIgnoreOpenSwype = true;

            flyoutController.TopNavigationController.View.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();
            if (!flyoutController.TopNavigationController.View.Subviews.IsNullOrEmpty())
                flyoutController.TopNavigationController.View.Subviews[0].BackgroundColor = Theme.ColorPalette.Background.ToUIColor();
            flyoutController.TopNavigationController.NavigationBar.SetupStyle(Theme.ControlPalette.NavigationBar);

            flyoutController.MenuSlidesWithTopView = ThemeConfig.FlyoutController.MenuSlidesWithTopView;

            if (flyoutController.TopNavigationController.NavigationBar.TopItem != null)
            {
                flyoutController.TopNavigationController.NavigationBar.TopItem.SetLeftBarButtonItem(
                   new UIBarButtonItem(
                       ImageCache.GetImage(ThemeConfig.NavBarMenuImage.Path),
                       UIBarButtonItemStyle.Done,
                       (s, e) => MenuNavigationController.ShowLeftMenu()),
                   false);
            }
        }

        #endregion

        #region MvxIosViewPresenter implementation

        public override void Show(MvxViewModelRequest request)
        {
            if (MasterNavigationController == null)
            {
                SetupFlyoutNavigationController(MenuNavigationController);

                _window.RootViewController = MenuNavigationController;
                MenuNavigationController.View.BackgroundColor = UIColor.Red;
                MasterNavigationController = MenuNavigationController.TopNavigationController;

                var viewModel = Mvx.Resolve<IMvxViewModelLoader>().LoadViewModel(request, null);
                var viewController = this.CreateViewControllerFor(request);
                viewController.ViewModel = viewModel;

                var navigationType = (viewModel as IBaseViewModel)?.VmNavigationType ?? NavigationType.None;

                if (navigationType == NavigationType.None)
                    MenuNavigationController.LeftMenuNavigationController.PushViewController((UIViewController)viewController, false);
                else
                    _scheduledViewControllers.Add(request);

                return;
            }

            if (MenuNavigationController.LeftMenuNavigationController.ChildViewControllers.IsNullOrEmpty())
            {
                _scheduledViewControllers.Add(request);

                return;
            }

            ShowViewController(request);

            if (_scheduledViewControllers.Any())
            {
                _scheduledViewControllers.ForEach(ShowViewController);
                _scheduledViewControllers = new List<MvxViewModelRequest>();
            }
        }

        public override void Close(IMvxViewModel viewModel)
        {
            var navigationType = ((IBaseViewModel)viewModel)?.VmNavigationType ?? NavigationType.None;

            if (navigationType == NavigationType.None)
            {
                MenuNavigationController.HideMenu();
                return;
            }

            if (navigationType == NavigationType.Push)
            {
                if (MenuNavigationController.TopNavigationController.PresentedViewController != null)
                {
                    var navigationController = (MenuNavigationController.TopNavigationController.PresentedViewController as UINavigationController);
                    if (navigationController != null)
                        navigationController.PopViewController(true);
                    else
                        MenuNavigationController.TopNavigationController.PresentedViewController.DismissViewControllerAsync(true);
                }
                else
                    MenuNavigationController.TopNavigationController.PopViewController(true);
            }
            if (navigationType == NavigationType.PresentModal)
            {
                var controller = GetLastPresentedController(MenuNavigationController.TopNavigationController.PresentedViewController ?? MenuNavigationController.TopNavigationController.TopViewController);
                controller?.DismissViewControllerAsync(true);
            }

            ((IBaseViewModel)viewModel)?.Unbind();
        }

        public override void ChangePresentation(MvxPresentationHint hint)
        {
            if (hint is NavigateToDefaultViewModelHint)
            {
                Mvx.CallbackWhenRegistered<IMvxNavigationService>(async service =>
                {
                    await service.Navigate(Mvx.Resolve<IMenuVmService>().DefaultViewModelType(), ((IMvxBundle)new BaseBundle(NavigationType.ClearAndPush)), null);
                });
                return;
            }

            base.ChangePresentation(hint);
        }

        #endregion
    }
}

