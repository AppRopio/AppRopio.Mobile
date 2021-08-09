using System;
using Android.App;
using Android.Runtime;

namespace AppRopio.Test.Droid
{
	[Preserve(AllMembers = true)]
    [Application]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
        }
    }
}
