using System;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay;
using AppRopio.Payments.Best2Pay.iOS.Views;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Payments.Best2Pay.iOS
{
	public class Plugin : IMvxPlugin
	{
		public void Load()
		{
			var viewLookupService = Mvx.Resolve<IViewLookupService>();

			viewLookupService.Register<IBest2PayViewModel, Best2PayViewController>();
		}
	}
}