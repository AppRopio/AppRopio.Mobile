using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Extensions;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.WeakSubscription;

#pragma warning disable CS0618
namespace AppRopio.Base.Droid.Adapters
{
	public interface IARFlatGroupTemplateSelector : IMvxTemplateSelector
    {
        int GetHeaderViewType(object forItemObject);

        int GetFooterViewType(object forItemObject);
    }

    public delegate void TuneViewHolderOnCreateDelegate(RecyclerView.ViewHolder viewHolder, int viewType);

    public delegate void TuneViewHolderOnBindDelegate(bool first, bool last, RecyclerView.ViewHolder viewHolder);

    public delegate void TuneViewHolderOnRecycledDelegate(RecyclerView.ViewHolder viewHolder);

    public class ARFlatGroupAdapter : RecyclerView.Adapter, IMvxRecyclerAdapter
    {
        #region Fields

        private IDisposable _subscription;
        private List<int> _headersPostions;
        private List<int> _footerPostions;
        private List<int> _headersViewTypes;
        private List<int> _footerViewTypes;
        private List<object> _flatItems;
        private ICommand _itemClick, _itemLongClick, _sectionClick, _sectionLongClick;
        private InnerItemsProviderDelegate _innerItemsProvider;
        private int _headerLayout = -1;
        private int _footerLayout = -1;

        protected readonly IMvxAndroidBindingContext _bindingContext;

        #endregion

        #region Properties

        public int HeadersCount => _headersPostions?.Count ?? 0;

        public int FootersCount => _footerPostions?.Count ?? 0;

        public bool ReloadOnAllItemsSourceSets { get; set; }

        public delegate IEnumerable InnerItemsProviderDelegate(object item);

        public Func<object, bool> HasHeader { get; set; }

        public Func<object, bool> HasFooter { get; set; }

        public TuneViewHolderOnBindDelegate TuneSectionHeaderOnBind { get; set; }

        public TuneViewHolderOnBindDelegate TuneSectionItemOnBind { get; set; }

        public TuneViewHolderOnBindDelegate TuneSectionFooterOnBind { get; set; }

        public TuneViewHolderOnCreateDelegate TuneViewHolderOnCreate { get; set; }

        public TuneViewHolderOnRecycledDelegate TuneViewHolderOnRecycled { get; set; }

        public ICommand SectionClick
        {
            get
            {
                return _sectionClick;
            }
            set
            {
                if (ReferenceEquals(_sectionClick, value))
                {
                    return;
                }

                if (_sectionClick != null)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Warn("Changing SectionClick may cause inconsistencies where some items still call the old command.");
                }

                _sectionClick = value;
            }
        }

        public ICommand SectionLongClick
        {
            get
            {
                return _sectionLongClick;
            }
            set
            {
                if (ReferenceEquals(_sectionLongClick, value))
                {
                    return;
                }

                if (_sectionLongClick != null)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Warn("Changing SectionLongClick may cause inconsistencies where some items still call the old command.");
                }

                _sectionLongClick = value;
            }
        }

        private IARFlatGroupTemplateSelector _flatGroupTemplateSelector;
        public virtual IARFlatGroupTemplateSelector FlatGroupTemplateSelector
        {
            get
            {
                return _flatGroupTemplateSelector;
            }
            set
            {
                if (ReferenceEquals(_itemTemplateSelector, value))
                    return;

                _flatGroupTemplateSelector = value;

                if (_flatItems != null)
                    NotifyDataSetChanged();
            }
        }

        #region IMvxRecyclerAdapter Implementation 

        private IEnumerable _itemsSource;
        public virtual IEnumerable ItemsSource
        {
            get
            {
                return _itemsSource;
            }
            set
            {
                SetItemsSource(value);
            }
        }


        private IMvxTemplateSelector _itemTemplateSelector;

        [System.Obsolete("Use FlatGroupTemplateSelector instead")]
        public virtual IMvxTemplateSelector ItemTemplateSelector
        {
            get
            {
                return _itemTemplateSelector;
            }
            set
            {
                if (ReferenceEquals(_itemTemplateSelector, value))
                    return;

                _itemTemplateSelector = value;

                if (_flatItems != null)
                    NotifyDataSetChanged();
            }
        }

        public ICommand ItemClick
        {
            get
            {
                return _itemClick;
            }
            set
            {
                if (ReferenceEquals(_itemClick, value))
                {
                    return;
                }

                if (_itemClick != null)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Warn("Changing ItemClick may cause inconsistencies where some items still call the old command.");
                }

                _itemClick = value;
            }
        }

        public ICommand ItemLongClick
        {
            get
            {
                return _itemLongClick;
            }
            set
            {
                if (ReferenceEquals(_itemLongClick, value))
                {
                    return;
                }

                if (_itemLongClick != null)
                {
                    Mvx.IoCProvider.Resolve<IMvxLog>().Warn("Changing ItemLongClick may cause inconsistencies where some items still call the old command.");
                }

                _itemLongClick = value;
            }
        }

        public int ItemTemplateId { get; set; }

        public override int ItemCount => _flatItems?.Count ?? 0;


        #endregion


        #endregion

        #region Constructor

        public ARFlatGroupAdapter(InnerItemsProviderDelegate innerItemsProvider, IARFlatGroupTemplateSelector flatGroupTemplateSelector, IMvxBindingContext bindingContext)
            : this(innerItemsProvider, (IMvxAndroidBindingContext)bindingContext)
        {
            _flatGroupTemplateSelector = flatGroupTemplateSelector;
        }

        public ARFlatGroupAdapter(InnerItemsProviderDelegate innerItemsProvider, IMvxBindingContext bindingContext, int sectionHeaderLayout = -1, int sectionFooterLayout = -1)
            : this(innerItemsProvider, (IMvxAndroidBindingContext)bindingContext)
        {
            _headerLayout = sectionHeaderLayout;
            _footerLayout = sectionFooterLayout;
        }

        protected ARFlatGroupAdapter(InnerItemsProviderDelegate innerItemsProvider, IMvxAndroidBindingContext bindingContext)
        {
            _bindingContext = bindingContext;
            _innerItemsProvider = innerItemsProvider;
        }

        public ARFlatGroupAdapter(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        #endregion

        #region Private

        private void SetInnerItems()
        {
            if (ItemsSource == null)
            {
                _flatItems = null;
                _headersPostions = null;
                _footerPostions = null;
                _headersViewTypes = null;
                _footerViewTypes = null;
                return;
            }

            _flatItems = new List<object>();
            _headersPostions = new List<int>();
            _footerPostions = new List<int>();
            _footerViewTypes = new List<int>();
            _headersViewTypes = new List<int>();

            int currentPosition = 0;

            foreach (var item in ItemsSource)
            {
                var hasHeader = HasHeader?.Invoke(item) ?? false;
                var hasFooter = HasFooter?.Invoke(item) ?? false;

                if (hasHeader)
                {
                    _flatItems.Add(item);
                    _headersPostions.Add(currentPosition);
                    currentPosition++;
                }


                if (_innerItemsProvider != null)
                {
                    var innerItems = _innerItemsProvider(item);
                    foreach (var innerItem in innerItems)
                        _flatItems.Add(innerItem);

                    currentPosition += innerItems.Count();
                }
                else
                {
                    _flatItems.Add(item);
                    currentPosition++;
                }

                if (hasFooter)
                {
                    _flatItems.Add(item);
                    _footerPostions.Add(currentPosition);
                    currentPosition++;
                }
            }
        }

        #endregion

        #region Protected

        protected virtual void SetItemsSource(IEnumerable value)
        {
            if (ReferenceEquals(_itemsSource, value) && !ReloadOnAllItemsSourceSets)
            {
                return;
            }

            _subscription?.Dispose();
            _subscription = null;

            _itemsSource = value;

            SetInnerItems();

            if (_itemsSource != null && !(_itemsSource is IList))
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn("Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");
            }

            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);

            NotifyDataSetChanged();
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetInnerItems();

            if (Looper.MainLooper == Looper.MyLooper())
            {
                NotifyDataSetChanged(e);
            }
            else
            {
                var h = new Handler(Looper.MainLooper);
                h.Post(() => NotifyDataSetChanged(e));
            }
        }

        public virtual void NotifyDataSetChanged(NotifyCollectionChangedEventArgs e)
        {
            try
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        NotifyItemRangeInserted(GetViewPosition(e.NewStartingIndex), e.NewItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Move:
                        for (int i = 0; i < e.NewItems.Count; i++)
                        {
                            var oldItem = e.OldItems[i];
                            var newItem = e.NewItems[i];

                            NotifyItemMoved(GetViewPosition(oldItem), GetViewPosition(newItem));
                        }
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        NotifyItemRangeChanged(GetViewPosition(e.NewStartingIndex), e.NewItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        NotifyItemRangeRemoved(GetViewPosition(e.OldStartingIndex), e.OldItems.Count);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        NotifyDataSetChanged();
                        break;
                }
            }
            catch (Exception exception)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn($"Exception masked during Adapter RealNotifyDataSetChanged {exception.BuildAllMessagesAndStackTrace()}. Are you trying to update your collection from a background task? See http://goo.gl/0nW0L6");
            }
        }

        protected virtual bool IsSectionHeaderPosition(int position)
        {
            return _headersPostions == null ? false : _headersPostions.Any(t => t == position);
        }

        protected virtual bool IsSectionFooterPosition(int position)
        {
            return _footerPostions == null ? false : _footerPostions.Any(t => t == position);
        }

        protected virtual bool IsSectionHeaderViewType(int viewType)
        {
            return (_headerLayout == viewType) || (_headersViewTypes?.Any(t => t == viewType) ?? false);
        }

        protected virtual bool IsSectionFooterViewType(int viewType)
        {
            return (_footerLayout == viewType) || (_footerViewTypes?.Any(t => t == viewType) ?? false);
        }

        protected virtual View InflateViewForHolder(ViewGroup parent, int viewType, IMvxAndroidBindingContext bindingContext)
        {
            int layoutId;

            if (FlatGroupTemplateSelector == null)
                layoutId = viewType == _headerLayout
                   ? _headerLayout
                   :
                   (
                       viewType == _footerLayout
                       ? _footerLayout
                       : ItemTemplateSelector.GetItemLayoutId(viewType)
                    );
            else
                layoutId = FlatGroupTemplateSelector.GetItemLayoutId(viewType);

             return bindingContext.BindingInflate(layoutId, parent, false);
        }

        #endregion

        #region Public

        public virtual object GetItem(int position)
        {
            if (_flatItems != null && position >= 0 && position < _flatItems.Count)
            {
                return _flatItems[position];
            }
            return null;
        }

        protected virtual int GetViewPosition(object item)
        {
            var itemsSourcePosition = _itemsSource.GetPosition(item);
            return GetViewPosition(itemsSourcePosition);
        }

        protected virtual int GetViewPosition(int itemsSourcePosition)
        {
            return itemsSourcePosition;
        }

        protected virtual int GetItemsSourcePosition(int viewPosition)
        {
            return viewPosition;
        }

        public override int GetItemViewType(int position)
        {
            var itemAtPosition = GetItem(position);

            if (FlatGroupTemplateSelector == null)
            {
                return IsSectionHeaderPosition(position)
                    ? _headerLayout
                    :
                    (
                        IsSectionFooterPosition(position)
                        ? _footerLayout
                        : ItemTemplateSelector.GetItemViewType(itemAtPosition)
                    );
            }
            else
            {
                int viewType = 0;

                if (IsSectionHeaderPosition(position))
                {
                    viewType = FlatGroupTemplateSelector.GetHeaderViewType(itemAtPosition);
                    _headersViewTypes.Add(viewType);
                }
                else
                if (IsSectionFooterPosition(position))
                {
                    viewType = FlatGroupTemplateSelector.GetFooterViewType(itemAtPosition);
                    _footerViewTypes.Add(viewType);
                }
                else
                    viewType = FlatGroupTemplateSelector.GetItemViewType(itemAtPosition);

                return viewType;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var dataContext = GetItem(position);

            bool sectionHeader = IsSectionHeaderPosition(position);
            bool sectionFooter = IsSectionFooterPosition(position);

            if (holder is IMvxRecyclerViewHolder viewHolder)
            {
                viewHolder.DataContext = dataContext;

                AttachClickListeners(viewHolder, sectionHeader || sectionFooter);
            }

            if (sectionHeader && TuneSectionHeaderOnBind != null)
            {
                bool first = _headersPostions.First() == position;
                bool last = _headersPostions.Last() == position;

                TuneSectionHeaderOnBind(first, last, holder);
            }

            if (sectionFooter && TuneSectionFooterOnBind != null)
            {
                bool first = _footerPostions.First() == position;
                bool last = _footerPostions.Last() == position;

                TuneSectionFooterOnBind(first, last, holder);
            }

            if (!sectionHeader && !sectionFooter && TuneSectionItemOnBind != null)
            {
                bool first = (position == 0) || (_headersPostions?.Any(t => t == position - 1) ?? false) || (_footerPostions?.Any(t => t == position - 1) ?? false);

                bool last = (position == ItemCount - 1) || (_headersPostions?.Any(t => t == position + 1) ?? false) || (_footerPostions?.Any(t => t == position + 1) ?? false);

                TuneSectionItemOnBind(first, last, holder);
            }
        }

        private void AttachClickListeners(IMvxRecyclerViewHolder viewHolder, bool isSection)
        {
            if (!isSection)
            {
                viewHolder.Click -= OnItemViewClick;
                viewHolder.LongClick -= OnItemViewLongClick;
                viewHolder.Click += OnItemViewClick;
                viewHolder.LongClick += OnItemViewLongClick;
			}
            else
            {
                viewHolder.Click -= OnSectionViewClick;
                viewHolder.LongClick -= OnSectionViewLongClick;
                viewHolder.Click += OnSectionViewClick;
                viewHolder.LongClick += OnSectionViewLongClick;
			}
        }

        protected virtual void OnSectionViewClick(object sender, EventArgs ev)
        {
            if (sender is IMvxRecyclerViewHolder holder)
            {
                ExecuteCommandOnItem(SectionClick, holder.DataContext);
            }
        }

        protected virtual void OnSectionViewLongClick(object sender, EventArgs ev)
        {
            if (sender is IMvxRecyclerViewHolder holder)
            {
                ExecuteCommandOnItem(SectionLongClick, holder.DataContext);
            }
        }

        protected virtual void OnItemViewClick(object sender, EventArgs ev)
        {
            if (sender is IMvxRecyclerViewHolder holder)
            {
                ExecuteCommandOnItem(ItemClick, holder.DataContext);
            }
        }

        protected virtual void OnItemViewLongClick(object sender, EventArgs ev)
        {
            if (sender is IMvxRecyclerViewHolder holder)
            {
                ExecuteCommandOnItem(ItemLongClick, holder.DataContext);
            }
        }

        protected virtual void ExecuteCommandOnItem(ICommand command, object itemDataContext)
        {
            if (command?.CanExecute(itemDataContext) == true && itemDataContext != null)
            {
                command.Execute(itemDataContext);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemBindingContext = new MvxAndroidBindingContext(parent.Context, _bindingContext.LayoutInflaterHolder);

            var vh = new MvxRecyclerViewHolder(InflateViewForHolder(parent, viewType, itemBindingContext), itemBindingContext);

            TuneViewHolderOnCreate?.Invoke(vh, viewType);

            return vh;
        }

        public void ReloadData()
        {
            SetInnerItems();
            NotifyDataSetChanged();
        }

        #region IMvxRecyclerViewHolder

        public override void OnViewAttachedToWindow(Java.Lang.Object holder)
        {
            base.OnViewAttachedToWindow(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnAttachedToWindow();
        }

        public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
        {
            base.OnViewDetachedFromWindow(holder);

            var viewHolder = (IMvxRecyclerViewHolder)holder;
            viewHolder.OnDetachedFromWindow();
        }

        public override void OnViewRecycled(Java.Lang.Object holder)
        {
            base.OnViewRecycled(holder);

            if (holder is IMvxRecyclerViewHolder mvxViewHolder)
            {
                mvxViewHolder.Click -= OnSectionViewClick;
                mvxViewHolder.LongClick -= OnSectionViewLongClick;
                mvxViewHolder.Click -= OnItemViewClick;
                mvxViewHolder.LongClick -= OnItemViewLongClick;
                mvxViewHolder.OnViewRecycled();
            }

            if (holder is RecyclerView.ViewHolder viewHolder)
            {
                TuneViewHolderOnRecycled?.Invoke(viewHolder);
            }
        }

        #endregion

        #endregion

    }
}
