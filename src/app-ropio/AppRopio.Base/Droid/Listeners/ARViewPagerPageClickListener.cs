using System;
using Android.Views;

namespace AppRopio.Base.Droid.Listeners
{
    public class ARViewPagerPageClickListener: Java.Lang.Object, View.IOnClickListener
    {
        protected WeakReference<object> _datacontextReference;

        protected Action<object> _onClick;

        public ARViewPagerPageClickListener(object dataContext,Action<object> onClick)
        {
            _datacontextReference = new WeakReference<object>(dataContext);
            _onClick = onClick;
        }

        public void OnClick(View v)
        {
            object context;
            if (_datacontextReference.TryGetTarget(out context))
            {
                _onClick?.Invoke(context);
            }
        }
    }
}
