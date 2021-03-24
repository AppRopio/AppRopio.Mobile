using System;
using AppRopio.Base.API;
using AppRopio.Base.Settings.API.Services;
using AppRopio.Base.Settings.API.Services.Fakes;
using AppRopio.Base.Settings.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.Base.Settings.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.RegisterType<ISettingsService>(() => new FakeSettingsService());
            else
                Mvx.RegisterType<ISettingsService>(() => new SettingsService());
        }
    }
}