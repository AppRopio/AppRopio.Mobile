using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin;

namespace AppRopio.Payments.YandexKassa.Core
{
    public abstract class BasePlugin : IMvxPlugin
    {
        public virtual void Load()
        {
            new App().Initialize();

            Mvx.IoCProvider.Resolve<IMvxLog>().Info("YandexKassa plugin is loaded");
        }
    }
}