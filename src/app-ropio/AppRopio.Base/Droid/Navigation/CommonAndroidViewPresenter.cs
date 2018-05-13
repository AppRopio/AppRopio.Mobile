using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.View;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Droid.Views;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Droid.Views;
using MvvmCross.Droid.Views.Attributes;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Platform;

namespace AppRopio.Base.Droid.Navigation
{
    public class CommonAndroidViewPresenter : MvxAppCompatViewPresenter, ICommonAndroidViewPresenter
    {
        #region Fields

        private int _presentedModalFragments = 0;

        #endregion

        #region Properties

        protected Java.Util.Date LastBackClick { get; set; } = new Java.Util.Date();

        protected List<MvxFragmentPresentationAttribute> PresentationAttributesCache { get; set; } = new List<MvxFragmentPresentationAttribute>();

        protected List<ViewModelBundleCache> FragmentsBackStack { get; set; } = new List<ViewModelBundleCache>();

        protected int ContentLayoutId { get; }
        protected int ContentModalsLayoutId { get; }

        protected bool WasPresentedModalFragment => _presentedModalFragments > 0;

        #endregion

        #region Constructor

        public CommonAndroidViewPresenter(int contentLayoutId, IEnumerable<System.Reflection.Assembly> androidViewAssemblies, int contentModalsLayoutId = -1)
            : base(androidViewAssemblies)
        {
            ContentModalsLayoutId = contentModalsLayoutId;
            ContentLayoutId = contentLayoutId;
        }

        #endregion

        #region Protected

        protected virtual MvxFragmentPresentationAttribute DeqeueFragmentAttributeIfExist(Type viewType)
        {
            var attribute = PresentationAttributesCache.LastOrDefault(x => x.ViewType == viewType);

            if (attribute != null)
                PresentationAttributesCache.Remove(attribute);

            return attribute;
        }

        protected virtual MvxFragmentPresentationAttribute DeqeueFragmentAttributeIfExist(string fragmentName)
        {
            var attribute = PresentationAttributesCache.LastOrDefault(x => FragmentJavaName(x.ViewType) == fragmentName);

            if (attribute != null)
                PresentationAttributesCache.Remove(attribute);

            return attribute;
        }

        protected virtual void AddFragmentAttributeToCache(MvxFragmentPresentationAttribute attribute)
        {
            PresentationAttributesCache.Add(attribute);
        }

        protected virtual ViewModelBundleCache CreateCacheBundle(string fragmentName, IMvxViewModel viewModel)
        {
            return new ViewModelBundleCache(fragmentName, viewModel);
        }

        protected virtual void AddFragmentToBackStack(ViewModelBundleCache bundle)
        {
            FragmentsBackStack.Add(bundle);
        }

        protected virtual void RemoveFragmentFromBackStack(MvxFragmentPresentationAttribute fragmentAttribute)
        {
            var fragmentName = FragmentJavaName(fragmentAttribute.ViewType);
            var existCache = FragmentsBackStack.FirstOrDefault(x => x.Key == fragmentName);
            if (existCache != null)
            {
                UnbindCycle(existCache.ViewModel);
                FragmentsBackStack.Remove(existCache);
            }
        }

        protected virtual void RemoveFragmentFromBackStack(string fragmentName)
        {
            var existCache = FragmentsBackStack.FirstOrDefault(x => x.Key == fragmentName);
            if (existCache != null)
            {
                UnbindCycle(existCache.ViewModel);
                FragmentsBackStack.Remove(existCache);
            }
        }

        protected virtual void RemoveFragmentsFromBackStack()
        {
            FragmentsBackStack.ForEach(x =>
            {
                UnbindCycle(x.ViewModel);
            });
            FragmentsBackStack.Clear();
        }

        protected virtual void UnbindCycle(IMvxViewModel vm)
        {
            Task.Run(() =>
            {
                try
                {
                    if (vm is IBaseViewModel baseVM)
                        baseVM.Unbind();

                    vm?.DisposeIfDisposable();
                }
                catch (Exception ex)
                {
#pragma warning disable CS0618 // Type or member is obsolete
                    MvxTrace.Trace(() => ex.BuildAllMessagesAndStackTrace());
#pragma warning restore CS0618 // Type or member is obsolete
                }
            });
        }

        protected virtual void SetupAttribute(MvxFragmentPresentationAttribute fragmentAttribute, NavigationType result, bool closing = false)
        {
            switch (result)
            {
                case NavigationType.InsideScreen:
                case NavigationType.None:
                    fragmentAttribute.AddToBackStack = false;
                    break;
                default:
                    fragmentAttribute.AddToBackStack = true;
                    break;
            }

            fragmentAttribute.IsCacheableFragment = result != NavigationType.InsideScreen && result != NavigationType.None;

            if (ContentModalsLayoutId != -1 && (result == NavigationType.PresentModal || WasPresentedModalFragment))
            {
                fragmentAttribute.FragmentContentId = ContentModalsLayoutId;
            }

            if (result == NavigationType.PresentModal)
            {
                fragmentAttribute.EnterAnimation = Resource.Animation.abc_slide_in_bottom;
                fragmentAttribute.ExitAnimation = Resource.Animation.abc_slide_out_bottom;
                fragmentAttribute.PopEnterAnimation = Resource.Animation.abc_slide_out_bottom;
                fragmentAttribute.PopExitAnimation = Resource.Animation.abc_slide_in_bottom;
            }
            else
            {
                fragmentAttribute.EnterAnimation = Resource.Animation.abc_fade_in;
                fragmentAttribute.ExitAnimation = Resource.Animation.abc_fade_out;
                fragmentAttribute.PopEnterAnimation = Resource.Animation.abc_fade_out;
                fragmentAttribute.PopExitAnimation = Resource.Animation.abc_fade_in;
            }

            AddFragmentAttributeToCache(fragmentAttribute);
        }

        protected virtual void OnBeforeNavigation(FragmentManager fragmentManager, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {

        }

        protected virtual Fragment SaveViewModelRequestInFragmentArguments(Fragment fragmentView, MvxViewModelRequest request)
        {
            // save MvxViewModelRequest in the Fragment's Arguments
            var bundle = new Bundle();
            var serializedRequest = NavigationSerializer.Serializer.SerializeObject(request);
            bundle.PutString(ViewModelRequestBundleKey, serializedRequest);

            if (fragmentView != null)
            {
                if (fragmentView.Arguments == null)
                {
                    fragmentView.Arguments = bundle;
                }
                else
                {
                    fragmentView.Arguments.Clear();
                    fragmentView.Arguments.PutAll(bundle);
                }
            }

            return fragmentView;
        }

        protected virtual void PushFragment(FragmentManager fragmentManager, FragmentTransaction fragmentTransaction, MvxFragmentPresentationAttribute fragmentAttribute, MvxViewModelRequest request, IMvxViewModel viewModel, string fragmentName, string fragmentTag = null)
        {
            if (string.IsNullOrEmpty(fragmentTag))
                fragmentTag = fragmentName;

            var newFragment = fragmentManager.FindFragmentByTag(fragmentTag);
            if (newFragment != null)
            {
                OnBeforeFragmentChanging(fragmentTransaction, newFragment, fragmentAttribute);

                if (newFragment.IsDetached)
                {
                    if (newFragment is IMvxFragmentView mvxFragment)
                        mvxFragment.ViewModel = viewModel;

                    if (request != null)
                        newFragment = SaveViewModelRequestInFragmentArguments(newFragment, request);

                    fragmentTransaction.Attach(newFragment);

                    OnFragmentChanging(fragmentTransaction, newFragment, fragmentAttribute);
                }
            }
            else if (fragmentAttribute != null)
            {
                var fragment = CreateFragment(fragmentAttribute, fragmentName);
                fragment.ViewModel = viewModel;

                Fragment fragmentView = null;

                if (request != null)
                    fragmentView = SaveViewModelRequestInFragmentArguments(fragment.ToFragment(), request);

                if (fragmentView != null)
                {
                    OnBeforeFragmentChanging(fragmentTransaction, fragmentView, fragmentAttribute);

                    fragmentTransaction.Add(fragmentAttribute.FragmentContentId, fragmentView, fragmentTag);

                    OnFragmentChanging(fragmentTransaction, fragmentView, fragmentAttribute);
                }
            }
        }

        protected virtual void PopFragment(FragmentManager fragmentManager, FragmentTransaction fragmentTransaction, MvxFragmentPresentationAttribute fragmentAttribute, string fragmentName, bool removeIsNeeded = false, bool forward = false)
        {
            fragmentAttribute = fragmentAttribute ?? DeqeueFragmentAttributeIfExist(fragmentName);

            if (fragmentAttribute != null)
            {
                if (!fragmentAttribute.EnterAnimation.Equals(int.MinValue) && !fragmentAttribute.ExitAnimation.Equals(int.MinValue))
                {
                    if (!fragmentAttribute.PopEnterAnimation.Equals(int.MinValue) && !fragmentAttribute.PopExitAnimation.Equals(int.MinValue))
                        fragmentTransaction.SetCustomAnimations(fragmentAttribute.EnterAnimation, fragmentAttribute.ExitAnimation, fragmentAttribute.PopEnterAnimation, fragmentAttribute.PopExitAnimation);
                    else
                        fragmentTransaction.SetCustomAnimations(fragmentAttribute.EnterAnimation, fragmentAttribute.ExitAnimation);
                }

                if (fragmentAttribute.TransitionStyle != int.MinValue)
                    fragmentTransaction.SetTransitionStyle(fragmentAttribute.TransitionStyle);
            }

            if ((FragmentsBackStack.LastOrDefault(x => x.Key == fragmentName)?.ViewModel as IBaseViewModel)?.VmNavigationType == NavigationType.PresentModal)
            {
                lock (this)
                {
                    _presentedModalFragments--;
                }
            }

            var oldFragment = fragmentManager.FindFragmentByTag(fragmentName);
            if (oldFragment != null)
            {
                if (!oldFragment.IsDetached)
                {
                    if (removeIsNeeded)
                    {
                        RemoveFragmentFromBackStack(fragmentName);

                        fragmentTransaction.Remove(oldFragment);
                    }
                    else
                        fragmentTransaction.Detach(oldFragment);
                }
                else if (removeIsNeeded)
                {
                    RemoveFragmentFromBackStack(fragmentName);

                    fragmentTransaction.Remove(oldFragment);
                }

                if (!forward)
                    OnFragmentPopped(fragmentTransaction, oldFragment, null);
            }
        }

        protected override void PerformShowFragmentTransaction(FragmentManager fragmentManager, MvxFragmentPresentationAttribute attribute, MvxViewModelRequest request)
        {
            var navigationType = NavigationType.None;

            if (request.ParameterValues != null && request.ParameterValues.TryGetValue("NavigationType", out string typeStr) && Enum.TryParse(typeStr, out navigationType))
                SetupAttribute(attribute, navigationType);
            else if (request is MvxViewModelInstanceRequest instanceRequest)
            {
                navigationType = (instanceRequest.ViewModelInstance as IBaseViewModel)?.VmNavigationType ?? NavigationType.None;
                SetupAttribute(attribute, navigationType);
            }

            if (navigationType == NavigationType.PresentModal)
            {
                lock (this)
                {
                    _presentedModalFragments++;
                }
            }

            var fragmentName = FragmentJavaName(attribute.ViewType);

            IMvxViewModel viewModel = null;
            // MvxNavigationService provides an already instantiated ViewModel here
            if (request is MvxViewModelInstanceRequest)
            {
                viewModel = (request as MvxViewModelInstanceRequest).ViewModelInstance;
            }

            OnBeforeNavigation(fragmentManager, attribute, request);

            var fragmentTransaction = fragmentManager.BeginTransaction();

            fragmentTransaction.DisallowAddToBackStack();

            var oldCachedFragments = new List<ViewModelBundleCache>(FragmentsBackStack);

            var isSameFragmentInBackStack = oldCachedFragments.Any(x => x.Key == fragmentName);

            //check if user has requested transition to single ViewModel added in cache earlier
            var isNavigationIgnored = (navigationType == NavigationType.ClearAndPush && oldCachedFragments.Count == 1 && isSameFragmentInBackStack);
            if (!isNavigationIgnored)
            {
                var firstOldFragment = oldCachedFragments.FirstOrDefault();
                var isMovingToRoot = navigationType == NavigationType.ClearAndPush && firstOldFragment?.Key == fragmentName;

                if (isMovingToRoot)
                    oldCachedFragments.Remove(firstOldFragment);

                //remove or detach current fragment(s) if navigationType not equal PresentModal and there are no modal fragments in BackStack
                if (navigationType != NavigationType.PresentModal && !WasPresentedModalFragment)
                {
                    foreach (var oldCachedFragment in oldCachedFragments)
                    {
                        var oldFragmentsRemoved = isMovingToRoot || navigationType == NavigationType.ClearAndPush || navigationType == NavigationType.DoubleClearAndPush;
                        PopFragment(fragmentManager, fragmentTransaction, null, oldCachedFragment.Key, oldFragmentsRemoved, true);
                    }
                }

                if (isMovingToRoot)
                {
                    PushFragment(fragmentManager, fragmentTransaction, null, null, firstOldFragment.ViewModel, firstOldFragment.Key);
                }
                else
                {
                    //add or attach new fragment
                    var tag = fragmentName;
                    if (navigationType == NavigationType.DoublePush && isSameFragmentInBackStack)
                        tag = GetFragmentTag(fragmentName, viewModel);

                    if (attribute.AddToBackStack == true)
                        AddFragmentToBackStack(CreateCacheBundle(tag, viewModel));
                    
                    PushFragment(fragmentManager, fragmentTransaction, attribute, request, viewModel, fragmentName, tag);
                }

                if (!fragmentTransaction.IsEmpty)
                    fragmentTransaction.CommitNow();

                OnFragmentChanged(fragmentTransaction, null, attribute);
            }
        }

        protected virtual string GetFragmentTag(string fragmentName, IMvxViewModel viewModel)
        {
            return $"{fragmentName}.{Guid.NewGuid().ToString()}";
        }

        protected override bool TryPerformCloseFragmentTransaction(FragmentManager fragmentManager, MvxFragmentPresentationAttribute fragmentAttribute)
        {
            var fragmentName = FragmentJavaName(fragmentAttribute.ViewType);

            var fragmentTransaction = fragmentManager.BeginTransaction();

            fragmentTransaction.DisallowAddToBackStack();

            PopFragment(fragmentManager, fragmentTransaction, fragmentAttribute, fragmentName, true);

            var newLastCachedFragment = FragmentsBackStack?.LastOrDefault();
            if (newLastCachedFragment != null)
                PushFragment(fragmentManager, fragmentTransaction, null, null, newLastCachedFragment.ViewModel, newLastCachedFragment.Key);
            
            fragmentTransaction.CommitNow();

            return true;
        }

        protected override void OnBeforeFragmentChanging(FragmentTransaction ft, Fragment fragment, MvxFragmentPresentationAttribute attribute)
        {
            if (attribute != null)
            {
                if (attribute.SharedElements != null)
                {
                    foreach (var item in attribute.SharedElements)
                    {
                        string name = item.Key;
                        if (string.IsNullOrEmpty(name))
                            name = ViewCompat.GetTransitionName(item.Value);
                        ft.AddSharedElement(item.Value, name);
                    }
                }
                if (!attribute.EnterAnimation.Equals(int.MinValue) && !attribute.ExitAnimation.Equals(int.MinValue))
                {
                    if (!attribute.PopEnterAnimation.Equals(int.MinValue) && !attribute.PopExitAnimation.Equals(int.MinValue))
                        ft.SetCustomAnimations(attribute.EnterAnimation, attribute.ExitAnimation, attribute.PopEnterAnimation, attribute.PopExitAnimation);
                    else
                        ft.SetCustomAnimations(attribute.EnterAnimation, attribute.ExitAnimation);
                }
                if (attribute.TransitionStyle != int.MinValue)
                    ft.SetTransitionStyle(attribute.TransitionStyle);
            }
        }

        #endregion

        #region Public

        public override MvvmCross.Core.Views.MvxBasePresentationAttribute GetPresentationAttribute(Type viewModelType)
        {
            return base.GetPresentationAttribute(viewModelType);
        }

        public override MvvmCross.Core.Views.MvxBasePresentationAttribute CreatePresentationAttribute(Type viewModelType, Type viewType)
        {
            if (viewType.IsSubclassOf(typeof(Fragment)))
            {
#pragma warning disable CS0618 // Type or member is obsolete
                MvxTrace.Trace("PresentationAttribute not found for {0}. Assuming Fragment presentation", viewType.Name);
#pragma warning restore CS0618 // Type or member is obsolete
                return DeqeueFragmentAttributeIfExist(viewType) ?? new MvxFragmentPresentationAttribute(GetCurrentActivityViewModelType(), ContentLayoutId) { ViewType = viewType, ViewModelType = viewModelType };
            }

            return base.CreatePresentationAttribute(viewModelType, viewType);
        }

        public virtual bool CanPop()
        {
            return FragmentsBackStack.Count > 1;
        }

        public virtual void MoveBack()
        {
            if (CanPop())
            {
                var cachedFragment = FragmentsBackStack?.LastOrDefault();
                if (cachedFragment != null)
                {
                    var fragmentManager = CurrentFragmentManager;

                    var fragmentTransaction = fragmentManager.BeginTransaction();

                    fragmentTransaction.DisallowAddToBackStack();

                    PopFragment(fragmentManager, fragmentTransaction, DeqeueFragmentAttributeIfExist(cachedFragment.Key), cachedFragment.Key, true);

                    var newLastCachedFragment = FragmentsBackStack?.LastOrDefault();
                    if (newLastCachedFragment != null)
                        PushFragment(fragmentManager, fragmentTransaction, null, null, newLastCachedFragment.ViewModel, newLastCachedFragment.Key);

                    fragmentTransaction.CommitNow();
                }
            }
            else
            {
                if ((new Java.Util.Date().Time - LastBackClick.Time) / 1000 < 3)
                {
                    CurrentActivity.FinishAffinity();
                    Process.KillProcess(Process.MyPid());
                }
                else
                {
                    LastBackClick = new Java.Util.Date();
                    (CurrentActivity as ICommonActivity)?.ShowToast("Для выхода из приложения нажмите кнопку повторно");
                }
            }
        }

        #region IMvxMultipleViewModelCache implementation

        public virtual void Cache(IMvxViewModel toCache, string viewModelTag = "singleInstanceCache")
        {
            //nothing
        }

        public virtual IMvxViewModel GetAndClear(Type viewModelType, string viewModelTag = "singleInstanceCache")
        {
            var existCache = FragmentsBackStack.LastOrDefault(x => x.Key == viewModelTag);
            return existCache?.ViewModel;
        }

        public virtual T GetAndClear<T>(string viewModelTag = "singleInstanceCache") where T : IMvxViewModel
        {
            var existCache = FragmentsBackStack.LastOrDefault(x => x.Key == viewModelTag);
            return (T)existCache?.ViewModel;
        }

        #endregion

        #endregion
    }
}
