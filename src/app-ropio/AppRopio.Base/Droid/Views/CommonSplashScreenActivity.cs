using System.Threading.Tasks;
using Android.Graphics;
using Android.Util;
using Android.Views;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;

namespace AppRopio.Base.Droid.Views {
	public class CommonSplashScreenActivity : MvxSplashScreenAppCompatActivity
    {
        public CommonSplashScreenActivity(int resourceId = 0)
            : base(resourceId)
        {
        }

        protected override void OnCreate(Android.OS.Bundle bundle)
        {
#if DEBUG
            Mvx.IoCProvider.Resolve<IMvxLog>().Info($"{this.GetType().FullName}: Ellapsed milliseconds after Application OnCreate {Base.Core.ServicesDebug.StartupTimerService.Instance.EllapsedMilliseconds()}");
#endif
            base.OnCreate(bundle);

            var statusBarColor = new TypedValue();
            Theme.ResolveAttribute(Android.Resource.Attribute.StatusBarColor, statusBarColor, true);
            int color = statusBarColor.Data;

            if (color == Color.Transparent.ToArgb())
                Window.SetFlags(WindowManagerFlags.TranslucentStatus, WindowManagerFlags.TranslucentStatus);
        }

        protected override Task RunAppStartAsync(Android.OS.Bundle bundle)
        {
#if DEBUG
            Mvx.IoCProvider.Resolve<IMvxLog>().Info($"{this.GetType().FullName}: Ellapsed milliseconds after MvvmCross started {Base.Core.ServicesDebug.StartupTimerService.Instance.EllapsedMilliseconds()}");
            Core.ServicesDebug.StartupTimerService.Instance.StopTimer();
#endif
            return base.RunAppStartAsync(bundle);
        }
    }
}
