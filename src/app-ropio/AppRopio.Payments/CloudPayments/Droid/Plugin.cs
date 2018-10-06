using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Payments.CloudPayments.Core.Services;
using AppRopio.Payments.CloudPayments.Droid.Services.Implementation;
using AppRopio.Payments.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Plugins;

namespace AppRopio.Payments.CloudPayments.Droid
{
    public class Plugin : IMvxPlugin
    {
        public void Load()
        {
            //Mvx.RegisterSingleton<INativePaymentService>(() => new NativePaymentService());
            Mvx.RegisterSingleton<IPayment3DSService>(() => new CloudPayments3DSService());
        }
    }
}
