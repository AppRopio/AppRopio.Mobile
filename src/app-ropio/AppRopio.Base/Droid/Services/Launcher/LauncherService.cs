﻿//
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
using MvvmCross;
using MvvmCross.Logging;
using Xamarin.Essentials;

namespace AppRopio.Base.Droid.Services.Launcher
{
    public class LauncherService : ILauncherService
    {
        public Task LaunchAddress(string address)
        {
            String map = "http://maps.google.co.in/maps?q=" + Uri.EscapeUriString(address); 
            Intent i = new Intent(Intent.ActionView, Android.Net.Uri.Parse(map));

            Platform.CurrentActivity.StartActivity(i);

            return Task.CompletedTask;
        }

        public async Task<bool> LaunchEmail(string email)
        {
            try
            {
                await Email.ComposeAsync(string.Empty, string.Empty, email);
                return true;
            }
            catch (Exception exception)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn(exception + "\n" + exception.StackTrace);
            }

            return false;
        }

        public Task<bool> LaunchPhone(string phoneNumber)
        {
            try
            {
                PhoneDialer.Open(phoneNumber);
                return Task.FromResult(true);
            }
            catch (Exception exception)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn(exception + "\n" + exception.StackTrace);
            }

            return Task.FromResult(false);
        }

        public async Task<bool> LaunchUri(string uri)
        {
            try
            {
                await Browser.OpenAsync(uri);
                return true;
            }
            catch (Exception exception)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn(exception + "\n" + exception.StackTrace);
            }

            return false;
        }
    }
}
