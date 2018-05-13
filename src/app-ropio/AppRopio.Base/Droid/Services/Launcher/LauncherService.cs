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
using System.Threading.Tasks;
using Android.Content;
using AppRopio.Base.Core.Services.Launcher;

namespace AppRopio.Base.Droid.Services.Launcher
{
    public class LauncherService : ILauncherService
    {
        public Task LaunchAddress(string address)
        {
            String map = "http://maps.google.co.in/maps?q=" + Uri.EscapeUriString(address); 
            Intent i = new Intent(Intent.ActionView, Android.Net.Uri.Parse(map));

            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity.StartActivity(i);

            return Task.CompletedTask;
        }

        public Task LaunchEmail(string email)
        {
            if (Plugin.Messaging.CrossMessaging.Current.EmailMessenger.CanSendEmail)
                Plugin.Messaging.CrossMessaging.Current.EmailMessenger.SendEmail(email);

            return Task.CompletedTask;
        }

        public Task LaunchPhone(string phoneNumber)
        {
            if (Plugin.Messaging.CrossMessaging.Current.PhoneDialer.CanMakePhoneCall)
                Plugin.Messaging.CrossMessaging.Current.PhoneDialer.MakePhoneCall(phoneNumber);

            return Task.CompletedTask;
        }

        public async Task LaunchUri(string uri)
        {
            await Plugin.Share.CrossShare.Current.OpenBrowser(uri);
        }
    }
}
