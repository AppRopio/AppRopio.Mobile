using System;
using AppRopio.Payments.YandexKassa.API.Services;
using AppRopio.Payments.YandexKassa.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.Payments.YandexKassa.API
{
	public class App : MvvmCross.ViewModels.MvxApplication
	{
		public override void Initialize()
		{
			Mvx.IoCProvider.RegisterType<IYandexKassaService>(() => new YandexKassaService());
		}
	}
}