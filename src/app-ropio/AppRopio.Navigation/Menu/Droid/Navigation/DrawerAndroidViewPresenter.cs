using System;
using System.Collections.Generic;
using Android.Animation;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views.Animations;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.PresentationHints;
using AppRopio.Base.Droid.Navigation;
using AppRopio.Navigation.Menu.Core.ViewModels.Services;
using AppRopio.Navigation.Menu.Droid.Views;
using MvvmCross.Core.Navigation;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Navigation.Menu.Droid.Navigation
{
    public class DrawerAndroidViewPresenter : CommonAndroidViewPresenter
    {
        protected const int DRAWER_CLOSED = 0;
        protected const int DRAWER_OPENED = 1;
        
        private MenuActivity MainActivity => (CurrentActivity as MenuActivity);

        public DrawerAndroidViewPresenter(IEnumerable<System.Reflection.Assembly> androidViewAssemblies)
            : base(Resource.Id.content_frame, androidViewAssemblies, Resource.Id.content_modals_frame)
        {
        }

        private void AnimateDrawerArrowToggle(int @from, int @to, Action onStartCallback = null, Action onEndCallback = null)
        {
            var anim = ValueAnimator.OfFloat(@from, @to);

            var listener = new AnimatorUpdateListener(MainActivity.Toggle, MainActivity.Drawer, onStartCallback, onEndCallback);

            anim.AddUpdateListener(listener);
            anim.AddListener(listener);

            anim.SetInterpolator(new DecelerateInterpolator());
            anim.SetDuration(400);
            anim.Start();
        }

        protected override void OnBeforeNavigation(Android.Support.V4.App.FragmentManager fragmentManager, MvvmCross.Platforms.Android.Presenters.Attributes.MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
            base.OnBeforeNavigation(fragmentManager, attribute, request);

            MainActivity?.RunOnUiThread(MainActivity.CloseDrawers);
        }

        protected override void OnBeforeFragmentChanging(Android.Support.V4.App.FragmentTransaction ft, Android.Support.V4.App.Fragment fragment, MvvmCross.Platforms.Android.Presenters.Attributes.MvxFragmentPresentationAttribute attribute)
        {
            base.OnBeforeFragmentChanging(ft, fragment, attribute);

            if (CanPop())
                AnimateDrawerArrowToggle(DRAWER_CLOSED, DRAWER_OPENED, onEndCallback: () => MainActivity.Toggle.DrawerIndicatorEnabled = false);
        }

        protected override void OnFragmentPopped(Android.Support.V4.App.FragmentTransaction ft, Android.Support.V4.App.Fragment fragment, MvvmCross.Platforms.Android.Presenters.Attributes.MvxFragmentPresentationAttribute attribute)
        {
            base.OnFragmentPopped(ft, fragment, attribute);

            if (!CanPop())
                AnimateDrawerArrowToggle(DRAWER_OPENED, DRAWER_CLOSED, onStartCallback: () => MainActivity.Toggle.DrawerIndicatorEnabled = true);
        }

        protected override void OnFragmentChanged(Android.Support.V4.App.FragmentTransaction ft, Android.Support.V4.App.Fragment fragment, MvvmCross.Platforms.Android.Presenters.Attributes.MvxFragmentPresentationAttribute attribute)
        {
            base.OnFragmentChanged(ft, fragment, attribute);

            if (CanPop())
                AnimateDrawerArrowToggle(DRAWER_CLOSED, DRAWER_OPENED, onEndCallback: () => MainActivity.Toggle.DrawerIndicatorEnabled = false);
            else
                AnimateDrawerArrowToggle(DRAWER_OPENED, DRAWER_CLOSED, onStartCallback: () => MainActivity.Toggle.DrawerIndicatorEnabled = true);
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

        protected class AnimatorUpdateListener : Java.Lang.Object, ValueAnimator.IAnimatorListener, ValueAnimator.IAnimatorUpdateListener
        {
            private ActionBarDrawerToggle toggle;
            private DrawerLayout drawer;

            public Action OnEndCallback { get; }
            public Action OnStartCallback { get; }

            public AnimatorUpdateListener(ActionBarDrawerToggle toggle, DrawerLayout drawer, Action onStartCallback, Action onEndCallback)
            {
                OnStartCallback = onStartCallback;
                OnEndCallback = onEndCallback;

                this.toggle = toggle;
                this.drawer = drawer;
            }

            public void OnAnimationCancel(Animator animation)
            {
                
            }

            public void OnAnimationEnd(Animator animation)
            {
                OnEndCallback?.Invoke();
            }

            public void OnAnimationRepeat(Animator animation)
            {
                
            }

            public void OnAnimationStart(Animator animation)
            {
                OnStartCallback?.Invoke();
            }

            public void OnAnimationUpdate(ValueAnimator valueAnimator)
            {
                float slideOffset = (float)valueAnimator.AnimatedValue;
                toggle.OnDrawerSlide(drawer, slideOffset);
            }
        }
    }
}
