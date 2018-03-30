using System;
using System.Collections;
using System.Linq;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using MvvmCross.Binding.ExtensionMethods;

namespace AppRopio.Base.Droid.Controls
{
    [Register("appropio.base.droid.controls.ARNumberPicker")]
    public class ARNumberPicker : NumberPicker
    {
        private IEnumerable _itemsSource;

        public virtual IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { SetItemsSource(value); }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (value != null)
                {
                    _selectedItem = value;

                    ChangeValueToSelectedItem();

                    OnSelectedItemChanged(EventArgs.Empty);
                }
            }
        }

        private EventHandler _SelectedItemChanged;
        public event EventHandler SelectedItemChanged
        {
            add
            {
                lock (this)
                {
                    _SelectedItemChanged += value;

                    if (SelectedItem != null)
                        OnSelectedItemChanged(EventArgs.Empty);
                }
            }
            remove
            {
                lock (this)
                {
                    _SelectedItemChanged -= value;
                }
            }
        }

        public ARNumberPicker(Context context)
            : base(context)
        {
        }

        public ARNumberPicker(Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            DisableDefaultPrivateFields(context);
        }

        public ARNumberPicker(Context context, IAttributeSet attrs, int defStyleAttr)
            : base(context, attrs, defStyleAttr)
        {
        }

        public ARNumberPicker(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes)
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected ARNumberPicker(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        private void DisableDefaultPrivateFields(Context context)
        {
            try
            {
                var numberPickerClass = Java.Lang.Class.ForName("android.widget.NumberPicker");

                var selectionDivider = numberPickerClass.GetDeclaredField("mSelectionDivider");
                if (selectionDivider != null)
                {
                    selectionDivider.Accessible = true;
                    selectionDivider.Set(this, null);
                }

                var incrementButton = numberPickerClass.GetDeclaredField("mIncrementButton");
                if (incrementButton != null)
                {
                    incrementButton.Accessible = true;
                    incrementButton.Set(this, new ImageButton(context));
                }

                var decrementButton = numberPickerClass.GetDeclaredField("mDecrementButton");
                if (decrementButton != null)
                {
                    decrementButton.Accessible = true;
                    decrementButton.Set(this, new ImageButton(context));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e.Message);
            }
        }

        private void ChangeValueToSelectedItem()
        {
            if (ItemsSource != null)
            {
                var index = (ItemsSource as IList).IndexOf(SelectedItem);
                Value = index > 0 ? index : 0;
            }
            else
                Value = 0;
        }

        protected virtual void SetItemsSource(IEnumerable value)
        {
            if (ReferenceEquals(_itemsSource, value))
                return;

            _itemsSource = value;

            int itemsCount = value?.Count() ?? 0;

            if (value != null && itemsCount > 0)
            {
                SetDisplayedValues(null);

                var strings = new string[itemsCount];

                for (int i = 0; i < strings.Length; i++)
                    strings[i] = value.ElementAt(i).ToString();

                MaxValue = itemsCount - 1;
                MinValue = 0;

                SetDisplayedValues(strings);

                SetOnValueChangedListener(new OnValueChangeListener((picker, oldPosition, currentPosition) => SelectItemInPosition(currentPosition)));

                if (SelectedItem == null)
                    SelectItemInPosition(0);
                else if (SelectedItem != null)
                {
                    ChangeValueToSelectedItem();
                    SelectItemInPosition(Value);
                }
            }
            else
            {
                SetDisplayedValues(null);

                SetDisplayedValues(new[] { " " });

                MaxValue = 0;
                MinValue = 0;
                Value = 0;
            }
        }

        protected virtual void SelectItemInPosition(int currentPosition)
        {
            if (ItemsSource != null && ItemsSource.Count() > 0)
                SelectedItem = ItemsSource.ElementAt(currentPosition);
        }

        protected virtual void OnSelectedItemChanged(EventArgs e)
        {
            EventHandler handler;
            lock (this)
            {
                handler = _SelectedItemChanged;
            }
            handler?.Invoke(this, e);
        }

        //protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        //{
        //    base.OnLayout(changed, left, top, right, bottom);

        //    this.VerticalFadingEdgeEnabled = false;
        //    SetFadingEdgeLength(0);
        //}

        public class OnValueChangeListener : Java.Lang.Object, IOnValueChangeListener
        {
            public delegate void OnValueChangedDelegate(NumberPicker picker, int oldPosition, int currentPosition);

            private OnValueChangedDelegate _onValueChanged;

            public OnValueChangeListener(OnValueChangedDelegate onValueChanged)
            {
                _onValueChanged = onValueChanged;
            }

            public void OnValueChange(NumberPicker picker, int oldVal, int newVal)
            {
                _onValueChanged?.Invoke(picker, oldVal, newVal);
            }
        }
    }
}
