﻿using System;
using AppRopio.Base.API;
using AppRopio.Payments.API.Services;
using AppRopio.Payments.API.Services.Fake;
using AppRopio.Payments.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.Payments.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.IoCProvider.RegisterSingleton<IPaymentService>(() => new PaymentFakeService());
            else
                Mvx.IoCProvider.RegisterSingleton<IPaymentService>(() => new PaymentService());
        }
    }
}