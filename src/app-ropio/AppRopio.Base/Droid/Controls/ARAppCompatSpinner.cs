using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using AppRopio.Base.Droid.Adapters;
using MvvmCross.Platforms.Android.Binding;

using MvvmCross.Droid.Support.V7.AppCompat.Widget;
using System.Windows.Input;

namespace AppRopio.Base.Droid.Controls
{
    /// <summary>
    /// Спиннер у которого выбранный эл-т (c id ItemTemplateId) биндится с основным DataContext, а не с эл-том из ItemsSource
    /// </summary>
    [Register("appropio.base.droid.controls.ARAppCompatSpinner")]
    public class ARAppCompatSpinner : MvxAppCompatSpinner
    {
        private object _selectedItem;
        public new object SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    if (Adapter != null)
                    {
                        var position = this.Adapter.GetPosition(value);
                        if (position != this.SelectedItemPosition)
                            this.SetSelection(position);
                    }
                }
            }
        }

        public ARAppCompatSpinner(Context context, IAttributeSet attrs)
            : this(
                context, attrs,
                new ARAppCompatSpinnerAdapter(context)
                {
                    ItemTemplateId = Android.Resource.Layout.SimpleSpinnerItem,
                    DropDownItemTemplateId = Android.Resource.Layout.SimpleSpinnerDropDownItem,
                })
        {
            (Adapter as ARAppCompatSpinnerAdapter).SetDropDownWidth = SetDropDownWidth;
        }

        public ARAppCompatSpinner(Context context, IAttributeSet attrs, IMvxAdapter adapter)
            : base(context, attrs, adapter)
        {
            
        }

        protected ARAppCompatSpinner(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void SetSelection(int position)
        {
            base.SetSelection(position);
        }

        public void SetDropDownWidth(float width)
        {
            this.DropDownWidth = (int)width;
        }

        protected override void HandleSelected(int position)
        {
            base.HandleSelected(position);
        }
    }
}
