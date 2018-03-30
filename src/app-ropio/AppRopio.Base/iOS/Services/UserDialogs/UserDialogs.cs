using System;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.UserDialogs;
using AppRopio.Base.iOS.Controls;
using AppRopio.Base.iOS.Models.Notification;
using UIKit;

namespace AppRopio.Base.iOS.Services.UserDialogs
{
    public class UserDialogs : IUserDialogs
    {
        private readonly object _lockObject = new object();

        private bool _notificationViewHidden = true;
        private ARNotificationView _notificationView;

        private void CheckNotificationExist()
        {
            lock (_lockObject)
            {
                if (!_notificationViewHidden && _notificationView != null)
                    UIApplication.SharedApplication.InvokeOnMainThread(_notificationView.RemoveFromSuperview);
            }
        }

        public async Task Alert(string message)
        {
            CheckNotificationExist();

            var tcs = new TaskCompletionSource<bool>();

            _notificationViewHidden = false;

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                _notificationView = new ARNotificationView(message, ARNotificationType.Alert);
                _notificationView.Show(() => tcs.TrySetResult(true));
            });

            _notificationViewHidden = await tcs.Task;
        }

        public async Task<bool> Confirm(string message, string buttonTitle, bool autoHide)
        {
            CheckNotificationExist();

            var tcs = new TaskCompletionSource<bool>();

            _notificationViewHidden = false;

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                _notificationView = new ARNotificationView(message, ARNotificationType.Confirm, buttonTitle, autoHide);
                _notificationView.Show(() => tcs.TrySetResult(false), () => tcs.SetResult(true));
            });

            var result = await tcs.Task;

            _notificationViewHidden = true;

            return result;
        }

        public async Task Error(string message)
        {
            CheckNotificationExist();

            var tcs = new TaskCompletionSource<bool>();

            _notificationViewHidden = false;

            UIApplication.SharedApplication.InvokeOnMainThread(() =>
            {
                _notificationView = new ARNotificationView(message, ARNotificationType.Error);
                _notificationView.Show(() => tcs.TrySetResult(true));            
            });

            _notificationViewHidden = await tcs.Task;
        }
    }
}
