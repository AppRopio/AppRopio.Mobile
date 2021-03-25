using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using AppRopio.Base.Droid.Listeners;
using MvvmCross;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.WeakSubscription;

namespace AppRopio.Base.Droid.Adapters {
	public class ARViewPagerAdapter : PagerAdapter, IARPagerAdapter
    {
        #region Fields

        private IDisposable _subscription;

        #endregion

        #region Properties

        public override int Count => _itemsSource?.Count() ?? 0;

        private IEnumerable _itemsSource;
        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { SetItemsSource(value); }
        }

        protected ICommand _itemClick;
        [MvxSetToNullAfterBinding]
        public ICommand ItemClick
        {
            get { return _itemClick; }
            set
            {
                if (ReferenceEquals(_itemClick, value))
                    return;

                _itemClick = value;
            }
        }

        private int _itemTemplateId;
        public int ItemTemplateId
        {
            get { return _itemTemplateId; }
            set
            {
                if (_itemTemplateId == value)
                    return;

                _itemTemplateId = value;

                if (_itemTemplateId > 0)
                    NotifyDataSetChanged();
            }
        }

        protected Context Context { get; }

        protected IMvxAndroidBindingContext BindingContext { get; }

        protected LayoutInflater LayoutInflater { get; }

        #endregion

        #region Constructors

        public ARViewPagerAdapter()
            : this(Application.Context, MvxAndroidBindingContextHelpers.Current())
        {

        }

        public ARViewPagerAdapter(Context context)
            : this(context, MvxAndroidBindingContextHelpers.Current())
        {

        }

        public ARViewPagerAdapter(Context context, IMvxAndroidBindingContext bindingContext)
        {
            BindingContext = bindingContext;
            Context = context;
            LayoutInflater = bindingContext?.LayoutInflaterHolder?.LayoutInflater ?? LayoutInflater.From(context);
        }

        protected ARViewPagerAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        #endregion

        #region Protected

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
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn(
                    "Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);

            NotifyDataSetChanged();
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged();
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, object dataContext)
        {
            if (command == null)
                return;

            var item = dataContext;
            if (item == null)
                return;

            if (!command.CanExecute(item))
                return;

            command.Execute(item);
        }

        #endregion

        #region Public

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            var dataContext = GetRawItem(position);

            var itemBindingContext = new MvxAndroidBindingContext(Context, BindingContext?.LayoutInflaterHolder ?? new MvxSimpleLayoutInflaterHolder(LayoutInflater), dataContext);

            View itemView = null;

            itemView = itemBindingContext.BindingInflate(ItemTemplateId, container, false);

            itemView.SetOnClickListener(
                new ARViewPagerPageClickListener(
                    dataContext,
                    @object => ExecuteCommandOnItem(ItemClick, @object)
                )
            );

            container.AddView(itemView);

            return itemView;
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return (view == @object);
        }

        public int GetDataContextPosition(object dataContext)
        {
            return ItemsSource?.GetPosition(dataContext) ?? -1;
        }

        public object GetRawItem(int position)
        {
            return _itemsSource?.ElementAt(position);
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            var view = (View)@object;

            container.RemoveView(view);
        }

        #endregion
    }
}
