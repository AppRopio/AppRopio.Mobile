using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Products.Core
{
    public abstract class BasePlugin : IMvxPlugin
    {
        public virtual void Load()
        {
            new App().Initialize();

            Mvx.IoCProvider.Resolve<IMvxLog>().Info("Products plugin is loaded");
        }
    }
}

