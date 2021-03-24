using System;
using System.Windows.Input;
using Android.Content;
using Android.Runtime;
using Android.Util;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.AttributeHelpers;
using MvvmCross.Platforms.Android.Binding;

namespace AppRopio.Base.Droid.Controls
{
    [Register("appropio.base.droid.controls.ARLinearLayout")]
    public class ARLinearLayout : MvxLinearLayout
    {
        private ICommand _itemClick;
        public ICommand ItemClick
        {
            get => _itemClick;
            set
            {
                _itemClick = value;

                if (Adapter is ARLinearLayoutAdapter arAdapter)
                {
                    arAdapter.ItemClick = value; 
                }
            }
        }

        public ARLinearLayout(Context context, IAttributeSet attrs) 
            : base(context, attrs, new ARLinearLayoutAdapter(context))
        {
            var itemTemplateSelector = ARLinearLayoutAttributeExtensions.BuildItemTemplateSelector(context, attrs);

            ItemTemplateId = ARLinearLayoutAttributeExtensions.ReadTemplateId(context, attrs);

            if (Adapter is ARLinearLayoutAdapter arAdapter)
            {
                arAdapter.ItemClick = ItemClick;
                arAdapter.ItemTemplateSelector = itemTemplateSelector;
            }

            Adapter.ItemTemplateId = ItemTemplateId;
        }

        public ARLinearLayout(Context context, IAttributeSet attrs, IMvxAdapterWithChangedEvent adapter) 
            : base(context, attrs, adapter)
        {
        }

        protected ARLinearLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
    }
}
