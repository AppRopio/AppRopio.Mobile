﻿using System;
using AppRopio.Analytics.Firebase.Core.Services;
using AppRopio.Analytics.Firebase.Core.Services.Implementation;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.Analytics.Firebase.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterSingleton<INotificationsSubscriber>(new NotificationsSubscriber());
        }
    }
}
