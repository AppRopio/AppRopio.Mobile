﻿using AppRopio.Base.Map.API.Services;
using MvvmCross;
using AppRopio.Base.Map.API.Services.Implementation;
using AppRopio.Base.Map.API.Services.Fakes;
using AppRopio.Base.API;

namespace AppRopio.Base.Map.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.IoCProvider.RegisterType<IPointsService>(() => new PointsFakeService());
            else
                Mvx.IoCProvider.RegisterType<IPointsService>(() => new PointsService());
        }
    }
}
