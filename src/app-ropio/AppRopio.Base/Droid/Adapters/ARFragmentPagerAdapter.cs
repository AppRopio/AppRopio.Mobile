using System;
using System.Collections;
using System.Collections.Specialized;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using AppRopio.Base.Droid.Listeners;
using MvvmCross.Binding;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Binding.ExtensionMethods;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platform.Core;
using MvvmCross.Platform.Droid.WeakSubscription;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.WeakSubscription;

namespace AppRopio.Base.Droid.Adapters
{
    public class ARFragmentPagerAdapter : ARSimpleFragmentPagerAdapter, IARPagerAdapter
    {
        #region Fields

        private IEnumerable _itemsSource;
        private IDisposable _subscription;

        #endregion

        #region Delegates

        public delegate string TitleForPositionDelegate(int itemPosition);

        public delegate string TitleForViewModelDelegate(IMvxViewModel itemVM);

        #endregion

        #region Properties

        public TitleForPositionDelegate TitleForPosition { get; set; }

        public TitleForViewModelDelegate TitleForViewModel { get; set; }

        public Func<IMvxViewModel, bool> IsVmStarted { get; set; }

        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { SetItemsSource(value); }
        }

        public override int Count
        {
            get { return _itemsSource.Count(); }
        }

        #endregion

        #region Constructor

        public ARFragmentPagerAdapter(Context context, IMvxAndroidBindingContext bindingContext, FragmentManager fm) 
            : base(context, bindingContext, fm, 0)
        {
        }

        protected ARFragmentPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        #endregion

        protected virtual void SetItemsSource(IEnumerable value)
        {
            if (ReferenceEquals(_itemsSource, value))
                return;

            if (_itemsSource == value)
                return;

            if (_subscription != null)
            {
                _subscription.Dispose();
                _subscription = null;
            }

            _itemsSource = value;
            if (_itemsSource != null && !(_itemsSource is IList))
                MvxBindingTrace.Trace(MvxTraceLevel.Warning,
                                      "Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);

            NotifyDataSetChanged();
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged(e);
        }

        protected override void SetOnClickListenerToFragment(MvxFragment fragment)
        {
            if (ItemClick != null)
            {
                var listener = new ARViewPagerPageClickListener(
                    fragment.DataContext,
                    (obj) =>
                    {
                        ExecuteCommandOnItem(ItemClick, obj);
                    });

                fragment.View.SetOnClickListener(listener);
            }
        }

        protected virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            base.NotifyDataSetChanged();
        }

        public override int GetItemPosition(Java.Lang.Object @object)
        {
            if (Count > 0)
            {
                var fragment = @object as MvxFragment;
                if (fragment != null)
                {
                    var index = ItemsSource.GetPosition(fragment.ViewModel);
                    if (index >= 0)
                        return index;

                    return PagerAdapter.PositionNone;
                }
                else
                {
                    var basePosition = base.GetItemPosition(@object);
                    return basePosition;
                }
            }
            else
                return PagerAdapter.PositionNone;
        }

        public int GetDataContextPosition(object dataContext)
        {
            return ItemsSource?.GetPosition(dataContext) ?? -1;
        }

        public System.Object GetRawItem(int position)
        {
            return _itemsSource?.ElementAt(position);
        }

        protected override object GetItemDataContext(int position)
        {
            return GetRawItem(position);
        }

        protected override void OnFragmentResume(MvxFragment mvxFragment)
        {
            base.OnFragmentResume(mvxFragment);

            var vm = mvxFragment?.ViewModel;
            if (vm != null)
                if (!IsVmStarted?.Invoke(vm) ?? true)
                    vm.Start();
        }

        protected override void Dispose(bool disposing)
        {
            if (_subscription != null)
            {
                _subscription.Dispose();
                _subscription = null;
            }
            if (_itemsSource != null)
            {
                _itemsSource.DisposeIfDisposable();
                _itemsSource = null;
            }
            base.Dispose(disposing);
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(TitleForPosition?.Invoke(position) ?? TitleForViewModel?.Invoke(GetRawItem(position) as IMvxViewModel) ?? string.Empty);
        }
    }
}
