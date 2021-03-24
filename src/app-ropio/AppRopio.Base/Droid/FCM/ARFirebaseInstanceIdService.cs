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
using Android.Runtime;
using Android.Util;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Push;
using Firebase.Iid;
using MvvmCross;
using Firebase.Messaging;

namespace AppRopio.Base.Droid.FCM
{
    [Preserve(AllMembers = true)]
    [Service]
    [IntentFilter(new string[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class ARFirebaseInstanceIdService : FirebaseInstanceIdService
    {
        private const string GLOBAL_TOPIC_KEY = "global";

        public ARFirebaseInstanceIdService()
        {
        }

        protected ARFirebaseInstanceIdService(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
        }

        public override void OnTokenRefresh()
        {
            base.OnTokenRefresh();

            FirebaseMessaging.Instance.SubscribeToTopic(GLOBAL_TOPIC_KEY);

            var token = FirebaseInstanceId.Instance.Token;

            Log.Verbose(PackageName, $"Push token refresh: {token}");

            System.Diagnostics.Debug.WriteLine(message: $"Push token refresh: {token}", category: PackageName);

            Mvx.CallbackWhenRegistered<IPushNotificationsService>(async service =>
            {
                try
                {
                    Log.Verbose(PackageName, $"Sending push token");

                    System.Diagnostics.Debug.WriteLine(message: $"Sending push token", category: PackageName);

                    AppSettings.PushToken = token;

                    await service.RegisterDeviceForPushNotificatons(token);

                    Log.Verbose(PackageName, $"Push token sent");

                    System.Diagnostics.Debug.WriteLine(message: $"Push token sent", category: PackageName);
                }
                catch (Exception ex) 
                {
                    System.Diagnostics.Debug.WriteLine(message: $"Exception when sending push token {ex.BuildAllMessagesAndStackTrace()}", category: PackageName);
                }
            });
        }
    }
}
