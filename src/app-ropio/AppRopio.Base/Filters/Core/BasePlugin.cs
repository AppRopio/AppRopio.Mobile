using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin;

namespace AppRopio.Base.Filters.Core
{
    public abstract class BasePlugin : IMvxPlugin
    {
        public virtual void Load()
        {
            new App().Initialize();

            Mvx.IoCProvider.Resolve<IMvxLog>().Info("Filters plugin is loaded");
        }
    }
}
