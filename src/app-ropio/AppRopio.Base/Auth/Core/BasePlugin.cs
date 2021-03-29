using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin;

namespace AppRopio.Base.Auth.Core
{
    public abstract class BasePlugin : IMvxPlugin
    {
        public virtual void Load() {
            new App().Initialize();

            Mvx.IoCProvider.Resolve<IMvxLog>().Info("Auth plugin is loaded");
        }
    }
}
