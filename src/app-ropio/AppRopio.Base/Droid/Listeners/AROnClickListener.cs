using System;
using Android.Views;
namespace AppRopio.Base.Droid.Listeners
{
    public class AROnClickListener : Java.Lang.Object, View.IOnClickListener
    {
        public Action HandleClick { get; }

        public AROnClickListener(Action onClick)
        {
            HandleClick = onClick;
        }

        public void OnClick(View v)
        {
            HandleClick?.Invoke();
        }
    }
}
