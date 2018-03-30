using System;
using AppRopio.Payments.YandexKassa.API.Services;
using AppRopio.Payments.YandexKassa.API.Services.Implementation;
using MvvmCross.Platform;

namespace AppRopio.Payments.YandexKassa.API
{
	public class App : MvvmCross.Core.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
			Mvx.RegisterType<IYandexKassaService>(() => new YandexKassaService());
		}
	}
}