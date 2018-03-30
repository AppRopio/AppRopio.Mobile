using System;
using MvvmCross.Platform.Plugins;

namespace AppRopio_Test.Bootstrap
{
	public class AppRopio_CloudPaymentsPluginBootstrap
		: MvxLoaderPluginBootstrapAction<AppRopio.Payments.CloudPayments.Core.PluginLoader, AppRopio.Payments.CloudPayments.iOS.Plugin>
	{

	}
}