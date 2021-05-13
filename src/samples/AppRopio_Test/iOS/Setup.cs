//
//  Copyright 2021  
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
using System.Collections.Generic;
using AppRopio.Navigation.Menu.iOS;

namespace AppRopio.Test.iOS
{
    public class Setup : MenuSetup
    {
        protected override IEnumerable<Type> GetPluginTypes()
        {
            return new Type[]
            {
                typeof(AppRopio.Analytics.AppsFlyer.iOS.Plugin),
                typeof(AppRopio.Analytics.Firebase.iOS.Plugin),
                typeof(AppRopio.Analytics.GoogleAnalytics.iOS.Plugin),
                typeof(AppRopio.Analytics.MobileCenter.iOS.Plugin),
                typeof(AppRopio.Base.Auth.iOS.Plugin),
                typeof(AppRopio.Base.Contacts.iOS.Plugin),
                typeof(AppRopio.Base.Filters.iOS.Plugin),
                typeof(AppRopio.Base.Information.iOS.Plugin),
                typeof(AppRopio.Base.Map.iOS.Plugin),
                typeof(AppRopio.Base.Profile.iOS.Plugin),
                typeof(AppRopio.Base.Settings.iOS.Plugin),
                typeof(AppRopio.Beacons.iOS.Plugin),
                typeof(AppRopio.Geofencing.iOS.Plugin),
                typeof(AppRopio.Feedback.iOS.Plugin),
                typeof(AppRopio.ECommerce.Basket.iOS.Plugin),
                typeof(AppRopio.ECommerce.Products.iOS.Plugin),
                typeof(AppRopio.ECommerce.Marked.iOS.Plugin),
                typeof(AppRopio.ECommerce.Loyalty.iOS.Plugin),
                typeof(AppRopio.ECommerce.HistoryOrders.iOS.Plugin),
                typeof(AppRopio.Payments.CloudPayments.iOS.Plugin),
                typeof(AppRopio.Payments.ApplePay.Plugin),
                typeof(AppRopio.Payments.iOS.Plugin),
                typeof(AppRopio.Payments.YandexKassa.iOS.Plugin),
                typeof(AppRopio.Payments.Best2Pay.iOS.Plugin),

                typeof(MvvmCross.Plugin.File.Platforms.Ios.Plugin),
                typeof(MvvmCross.Plugin.Json.Plugin),
                typeof(MvvmCross.Plugin.Messenger.Plugin),
                typeof(MvvmCross.Plugin.Network.Platforms.Ios.Plugin),
                typeof(MvvmCross.Plugin.Share.Platforms.Ios.Plugin),
                typeof(MvvmCross.Plugin.Visibility.Platforms.Ios.Plugin)
            };
        }
    }
}
