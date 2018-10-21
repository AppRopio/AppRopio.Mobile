//
//  Copyright 2018  AppRopio
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using Firebase.Messaging;
using Android.App;
using Android.Runtime;
using Android.Util;
using System.Text;
using System.Linq;
using Android.Content;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;
using Android.Support.V4.App;
using Android.Graphics;
using Android.Media;
using Android.OS;

namespace AppRopio.Base.Droid.FCM
{
    [Service(Name = "appropio.base.droid.fcm.arfirebasemessagingservice", Label = "Firebase Cloud Messaging receiver")]
    [IntentFilter(new string[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class ARFirebaseMessagingService : FirebaseMessagingService
    {
        public ARFirebaseMessagingService()
        {
        }

        protected ARFirebaseMessagingService(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);

            Log.Verbose(this.PackageName, "On push message received");

            System.Diagnostics.Debug.WriteLine(message: $"On push message received", category: this.PackageName);

            ParseRemoteMessage(message);
        }

        private void ParseRemoteMessage(RemoteMessage message)
        {
            var data = message.Data;
            var notification = message.GetNotification();

            var deeplink = string.Empty;
            data.TryGetValue(PushConstants.PUSH_DEEPLINK_KEY, out deeplink);

            SheduleNotification(notification.Title, notification.Body, message.MessageId, deeplink);
        }

        private void SheduleNotification(string title, string message, string id, string deeplink)
        {
            var activityType = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity.GetType();

            var notificationIntent = new Intent(Application.Context, activityType);
            notificationIntent.SetFlags(ActivityFlags.SingleTop);
            notificationIntent.SetAction(Guid.NewGuid().ToString());

            if (!deeplink.IsNullOrEmpty())
                notificationIntent.PutExtra(PushConstants.PUSH_DEEPLINK_KEY, deeplink);

            var notificationPendingIntent = PendingIntent.GetActivity(Application.Context, 2, notificationIntent, PendingIntentFlags.UpdateCurrent);

            //var iconDrawable = Resource.Drawable.ic_push_icon;
            //if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.Lollipop)
                //iconDrawable = Resource.Drawable.ic_push_silhouette;

            var notificationBuilder = new NotificationCompat.Builder(Application.Context)
                .SetAutoCancel(true)
                .SetContentIntent(notificationPendingIntent)
                .SetContentTitle(title ?? PackageName)
                .SetContentText(message)
                //.SetSmallIcon(iconDrawable)
                .SetSmallIcon(Android.Resource.Drawable.IcInputAdd)
                .SetChannelId(PackageName)
                .SetPriority((int)NotificationPriority.High)
                //.SetColor(Color.ParseColor(MAIN_THEME_COLOR).ToArgb())
                .SetColor(Color.Yellow.ToArgb())
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(message).SetBigContentTitle(title))
                .SetDefaults((int)NotificationDefaults.All)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetVibrate(new long[] { 300 });

            var notification = notificationBuilder.Build();

            notification.Flags = NotificationFlags.AutoCancel;

            var notificationManager = (NotificationManager)Application.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(PackageName, " ", NotificationImportance.High);
                notificationManager.CreateNotificationChannel(channel);
            }

            notificationManager.Notify(id.GetHashCode(), notification);
        }
    }
}
