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
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Firebase.Messaging;
using Android.Support.V4.Content;
using System.Reflection;

namespace AppRopio.Base.Droid.FCM
{
    [Preserve(AllMembers = true)]
    [Service]
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
            LogMessage("On push message received");

            base.OnMessageReceived(message);

            ParseRemoteMessage(message);
        }

        private void LogMessage(string msg)
        {
            Log.Verbose(this.PackageName, msg);

            System.Diagnostics.Debug.WriteLine(message: msg, category: this.PackageName);
        }

        private void ParseRemoteMessage(RemoteMessage message)
        {
            var data = message.Data;
            var notification = message.GetNotification();

            data.TryGetValue(PushConstants.PUSH_DEEPLINK_KEY, out var deeplink);
            data.TryGetValue(PushConstants.PUSH_TITLE_KEY, out var title);
            data.TryGetValue(PushConstants.PUSH_BODY_KEY, out var body);

            SheduleNotification(notification?.Title ?? title, notification?.Body ?? body, message.MessageId, deeplink);
        }

        private void SheduleNotification(string title, string message, string id, string deeplink)
        {
            var iconResourceId = EnsureIconResourceSet();

            var argbColor = EnsureColorSet();

            var activityType = EnsureActivityTypeSet();

            var notificationIntent = new Intent(Application.Context, activityType); 

            notificationIntent.SetFlags(ActivityFlags.SingleTop);
            notificationIntent.SetAction(Guid.NewGuid().ToString());

            if (!deeplink.IsNullOrEmpty())
                notificationIntent.PutExtra(PushConstants.PUSH_DEEPLINK_KEY, deeplink);

            var notificationPendingIntent = PendingIntent.GetActivity(Application.Context, 2, notificationIntent, PendingIntentFlags.UpdateCurrent);

            var notificationBuilder = new NotificationCompat.Builder(Application.Context)
                .SetAutoCancel(true)
                .SetContentIntent(notificationPendingIntent)
                .SetContentTitle(title ?? PackageName)
                .SetContentText(message)
                .SetSmallIcon(iconResourceId)
                .SetChannelId(PackageName)
                .SetPriority((int)NotificationPriority.High)
                .SetColor(argbColor)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(message).SetBigContentTitle(title))
                .SetDefaults((int)NotificationDefaults.All)
                .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
                .SetVibrate(new long[] { 300 });

            var notification = notificationBuilder.Build();

            notification.Flags = NotificationFlags.AutoCancel;

            var notificationManager = (NotificationManager)Application.Context.GetSystemService(NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(PackageName, "Main channel", NotificationImportance.Default);
                notificationManager.CreateNotificationChannel(channel);
            }

            notificationManager.Notify(id.GetHashCode(), notification);
        }

        private int EnsureIconResourceSet()
        {
            int iconResourceId;

            LogMessage("EnsureIconResourceSet");

            if ((FcmSettings.Instance?.IconResourceId ?? 0) == 0)
            {
                LogMessage("Icon resource is null");

                var metadata = GetMetadata();

                LogMessage("Getting metadata");

                iconResourceId = metadata.GetInt(PushConstants.METADATA_ICON_KEY);
            }
            else
                iconResourceId = FcmSettings.Instance.IconResourceId;

            LogMessage($"Icon resource id {iconResourceId}");

            return iconResourceId;
        }

        private int EnsureColorSet()
        {
            int argbColor;

            LogMessage("EnsureColorSet");

            if ((FcmSettings.Instance?.ColorHex ?? string.Empty).IsNullOrEmtpy())
            {
                var metadata = GetMetadata();
                var resourceId = metadata.GetInt(PushConstants.METADATA_COLOR_KEY);

                argbColor = ContextCompat.GetColor(Application.Context, resourceId);
            }
            else
                argbColor = Color.ParseColor(FcmSettings.Instance.ColorHex).ToArgb();

            LogMessage($"Color argb {argbColor}");

            return argbColor;
        }

        private Type EnsureActivityTypeSet()
        {
            Type activityType = null;

            LogMessage("EnsureActivityTypeSet");

            if (FcmSettings.Instance?.ActivityType == null)
            {
                var metadata = GetMetadata();
                var assemblyInfo = metadata.GetString(PushConstants.METADATA_ACTIVITY_TYPE_KEY);

                var parts = assemblyInfo.Split(';');

                if (parts.Length == 2)
                {
                    var assemblyName = parts[0];
                    var typeName = parts[1];

                    var assembly = Assembly.Load(new AssemblyName(assemblyName));

                    activityType = assembly.GetType(typeName);
                }
            }
            else
                activityType = FcmSettings.Instance.ActivityType;

            LogMessage($"Activity type {activityType.FullName}");

            return activityType;
        }

        private Bundle GetMetadata()
        {
            LogMessage("GetMetadata");

            var applicationInfo = Application.Context.PackageManager.GetApplicationInfo(Application.Context.PackageName, Android.Content.PM.PackageInfoFlags.MetaData);
            var metadata = applicationInfo?.MetaData;

            LogMessage($"Metadata is null: {metadata == null}");

            return metadata;
        }
    }
}