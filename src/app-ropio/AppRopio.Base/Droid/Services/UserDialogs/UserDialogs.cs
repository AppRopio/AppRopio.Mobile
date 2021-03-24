using System.Threading.Tasks;
using Android.App;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using AppRopio.Base.Core.Services.UserDialogs;
using MvvmCross;
using MvvmCross.Platforms.Android;
using AppRopio.Base.Droid.Views;

namespace AppRopio.Base.Droid.Services.UserDialogs
{
    public class UserDialogs : IUserDialogs
    {
        protected Activity TopActivity => Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

        private Task<Snackbar> CreateSnackbar(AppCompatActivity activity, string msg, bool autoHide = false)
        {
            var tcs = new TaskCompletionSource<Snackbar>();

            activity.RunOnUiThread(() =>
            {
                var view = (activity as ICommonActivity)?.View ?? activity.Window.DecorView.RootView.FindViewById(Android.Resource.Id.Content);

                try
                {
                    var snackBar = Snackbar.Make(
                        view,
                        msg,
                        autoHide ? Snackbar.LengthShort : Snackbar.LengthLong
                    );

                    tcs.TrySetResult(snackBar);
                }
                catch
                {
                    tcs.TrySetResult(null);
                }
            });

            return tcs.Task;
        }

        public async Task Alert(string message)
        {
            var compat = TopActivity as AppCompatActivity;

            if (compat == null)
                return;

            var snackBar = await CreateSnackbar(compat, message);
            if (snackBar != null)
                compat.RunOnUiThread(snackBar.Show);
        }

        public Task<bool> Confirm(string message, string button, bool autoHide = false)
        {
            var compat = TopActivity as AppCompatActivity;

            if (compat == null)
                return Task.FromResult(false);

            var tcs = new TaskCompletionSource<bool>();

            Task.Run(async () =>
            {
                var snackBar = await CreateSnackbar(compat, message);

                if (snackBar != null)
                    compat.RunOnUiThread(() =>
                    {
                        if (!string.IsNullOrEmpty(button))
                        {
                            snackBar.SetAction(button.ToUpperInvariant(), x =>
                            {
                                tcs.TrySetResult(true);
                                snackBar.Dismiss();
                            });

                        //snackBar.SetActionTextColor(color.Value.ToNative());
                    }

                        snackBar.Show();
                    });
            });

            return tcs.Task;
        }

        public async Task Error(string message)
        {
            var compat = TopActivity as AppCompatActivity;

            if (compat == null)
                return;

            var snackBar = await CreateSnackbar(compat, message);
            if (snackBar != null)
                compat.RunOnUiThread(snackBar.Show);
        }
    }
}
