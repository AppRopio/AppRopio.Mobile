using System;
using Android.App;
using Android.Runtime;
using Android.Views;
using Android.Views.InputMethods;

namespace AppRopio.Base.Droid.Listeners
{
    public class AROnEnterKeyListener : Java.Lang.Object, View.IOnKeyListener
    {
        public Activity Activity { get; }
        public Action OnEnterCallback { get; }

        public AROnEnterKeyListener(Activity activity,  Action onEnterCallback)
        {
            OnEnterCallback = onEnterCallback;
            Activity = activity;
        }

        protected void HideKeyboard()
        {
            var imm = (InputMethodManager)Application.Context.GetSystemService(Android.Content.Context.InputMethodService);
            if (imm != null && Activity?.Window?.CurrentFocus?.WindowToken != null)
                imm.HideSoftInputFromWindow(Activity.Window.CurrentFocus.WindowToken, 0);
        }

        public bool OnKey(View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            var handled = false;

            if (e.Action == KeyEventActions.Up && keyCode == Keycode.Enter)
            {
                HideKeyboard();

                OnEnterCallback?.Invoke();

                handled = true;
            }

            return handled;
        }
    }
}
