using System;
using Android.Views;
using Android.Widget;

namespace AppRopio.Base.Droid.Views
{
    public interface ICommonActivity
    {
        Android.Support.V4.App.FragmentManager SupportFragmentManager { get; }
		
        View View { get; }

        bool IsActive { get; }
		
        void HideKeyboard();
		
        void ShowKeyboard(View view);

		void ShowToast(string message, ToastLength length = ToastLength.Short);
    }
}
