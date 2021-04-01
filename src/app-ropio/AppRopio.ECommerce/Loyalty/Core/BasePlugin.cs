using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin;

namespace AppRopio.ECommerce.Loyalty.Core
{
    public abstract class BasePlugin : IMvxPlugin
    {
        public virtual void Load()
        {
            new App().Initialize();

            Mvx.IoCProvider.Resolve<IMvxLog>().Info("Loyalty plugin is loaded");
        }
    }
}
