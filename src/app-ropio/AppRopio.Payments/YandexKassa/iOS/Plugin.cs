using System;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.YandexKassa.Core.ViewModels.YandexKassa;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Payments.YandexKassa.iOS
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            var viewLookupService = Mvx.Resolve<IViewLookupService>();

            viewLookupService.Register<IYandexKassaViewModel, YandexKassaViewController>();
        }
    }
}