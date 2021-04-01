using System;
using AppRopio.Payments.CloudPayments.API.Services;
using AppRopio.Payments.CloudPayments.API.Services.Implementation;
using MvvmCross;

namespace AppRopio.Payments.CloudPayments.API
{
    public class App : MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            Mvx.IoCProvider.RegisterType<ICloudPaymentsService>(() => new CloudPaymentsService());
        }
    }
}