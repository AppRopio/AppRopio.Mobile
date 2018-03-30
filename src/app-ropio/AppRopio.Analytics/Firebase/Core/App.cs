using System;
using AppRopio.Analytics.Firebase.Core.Services;
using AppRopio.Analytics.Firebase.Core.Services.Implementation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.Analytics.Firebase.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            Mvx.RegisterSingleton<INotificationsSubscriber>(new NotificationsSubscriber());
        }
    }
}
