using System;
using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using AppRopio.Base.Droid.Adapters;
using MvvmCross.Platforms.Android.Binding.Views;

namespace AppRopio.Base.Droid.Controls
{
    [Register("appropio.base.droid.controls.ARViewPager")]
    public class ARViewPager
        : ViewPager
    {
        protected ARViewPager(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        public ARViewPager(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            ItemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId(context, attrs);
        }

        public bool DisposeAdapterOnDetached { get; set; } = false;

        public bool RestoreCurrentPageInstance { get; set; } = false;

        public new IARPagerAdapter Adapter
        {
            get { return base.Adapter as IARPagerAdapter; }
            set
            {
                var existing = Adapter as IARPagerAdapter;

                if (existing == value)
                    return;

                if (value != null)
                {
                    if (existing != null)
                    {
                        value.ItemTemplateId = existing.ItemTemplateId > 0 ? existing.ItemTemplateId : _itemTemplateId;
                        value.ItemClick = existing.ItemClick;
						value.ItemsSource = existing.ItemsSource;
                    }
                    else
                    {
                        value.ItemTemplateId = _itemTemplateId;
                        value.ItemClick = _itemClick;
                        value.ItemsSource = _itemsSource;
                    }
                }

                base.Adapter = value as PagerAdapter;

                if (existing != null)
                {
                    existing.ItemTemplateId = -1;
                    existing.ItemClick = null;
                    existing.ItemsSource = null;
                }

                if (_pendingSelectedItem != null)
                    HandlePendingSelectedItem();
                else if (value != null)
                    this.SetCurrentItem(_currentPageIndex, false);
            }
        }

        private bool invokeOnPageSelect = true;

        public event EventHandler CurrentPageIndexChanged;

        private int _currentPageIndex;
        public int CurrentPageIndex
        {
            get
            {
                return _currentPageIndex;
            }
            set
            {
                _currentPageIndex = value;
                var pastState = invokeOnPageSelect;
                invokeOnPageSelect = false;
                this.SetCurrentItem(value, true);
                invokeOnPageSelect = pastState;
                _pendingSelectedItem = null;
                _selectedItem = Adapter?.GetRawItem(value);
                SelectedItemChanged?.Invoke(this, null);
            }
        }

        public event EventHandler SelectedItemChanged;
        private object _pendingSelectedItem = null;
        private object _selectedItem;
        public object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (ReferenceEquals(_selectedItem, value) || value == null)
                    return;

                var index = Adapter?.GetDataContextPosition(value);

                if (index.HasValue && index >= 0)
                {
                    _selectedItem = value;
                    _pendingSelectedItem = null;

                    var pastState = invokeOnPageSelect;
                    invokeOnPageSelect = false;
                    this.SetCurrentItem(index.Value, true);
                    invokeOnPageSelect = pastState;
                    _currentPageIndex = index.Value;
                    CurrentPageIndexChanged?.Invoke(this, null);
                }
                else
                {
                    _pendingSelectedItem = value;
                }
            }
        }

        protected IEnumerable _itemsSource;
        public virtual IEnumerable ItemsSource
        {
            get { return Adapter?.ItemsSource ?? _itemsSource; }
            set
            {
                _itemsSource = value;

                if (Adapter != null)
                    Adapter.ItemsSource = value;

                HandlePendingSelectedItem();
            }
        }
        private int _itemTemplateId;
        public int ItemTemplateId
        {
            get
            {
                return Adapter?.ItemTemplateId ?? _itemTemplateId;
            }
            set
            {
                _itemTemplateId = value;
                if (Adapter != null)
                    Adapter.ItemTemplateId = value;
            }
        }

        private ICommand _itemClick;
        public ICommand ItemClick
        {
            get { return this.Adapter.ItemClick ?? _itemClick; }
            set
            {
                _itemClick = value;
                if (Adapter != null)
                    this.Adapter.ItemClick = value;
            }
        }

        private ICommand _pageSelectedCommand;
        public ICommand PageSelectedCommand
        {
            get { return _pageSelectedCommand; }
            set { _pageSelectedCommand = value; }
        }

        private ICommand _itemSelectedCommand;
        public ICommand ItemSelectedCommand
        {
            get { return _itemSelectedCommand; }
            set { _itemSelectedCommand = value; }
        }

        private void HandlePendingSelectedItem()
        {
            if (_pendingSelectedItem == null)
                return;

            SelectedItem = _pendingSelectedItem;
        }

        protected virtual void OnPageSelected(object sender, PageSelectedEventArgs e)
        {
            if (invokeOnPageSelect)
            {
                ExecutePageChagedCommand(PageSelectedCommand, e.Position);

                ExecuteItemChagedCommand(ItemSelectedCommand, Adapter?.GetRawItem(e.Position));
            }
        }

        protected virtual void ExecutePageChagedCommand(ICommand command, int toPage)
        {
            _currentPageIndex = toPage;
            CurrentPageIndexChanged?.Invoke(this, null);
            ExecuteCommand(command, toPage);
        }

        protected virtual void ExecuteItemChagedCommand(ICommand command, object item)
        {
            _selectedItem = item;
            SelectedItemChanged?.Invoke(this, null);
            ExecuteCommand(command, item);
        }

        protected virtual void ExecuteCommand(ICommand command, object parameter)
        {
            if (command == null)
                return;

            if (!command.CanExecute(parameter))
                return;

            command.Execute(parameter);
        }

        protected override void OnAttachedToWindow()
        {
            base.PageSelected += OnPageSelected;

            base.OnAttachedToWindow();
        }

        protected override void OnDetachedFromWindow()
        {
            base.PageSelected -= OnPageSelected;

            if (DisposeAdapterOnDetached)
            {
                Adapter?.Dispose();
                Adapter = null;

                _itemsSource = null;
                _itemClick = null;
            }

            base.OnDetachedFromWindow();
        }

        public override void OnRestoreInstanceState(Android.OS.IParcelable state)
        {
            bool pastState = invokeOnPageSelect;
            if (!RestoreCurrentPageInstance)
            {
                invokeOnPageSelect = false;
            }
            base.OnRestoreInstanceState(state);

            if (!RestoreCurrentPageInstance)
            {
                this.SetCurrentItem(_currentPageIndex, false);

                invokeOnPageSelect = pastState;
            }
        }
    }
}
