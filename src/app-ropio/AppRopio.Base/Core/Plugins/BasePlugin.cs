using MvvmCross;
using MvvmCross.Logging;
using MvvmCross.Plugin;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Core.Plugins
{
    public abstract class BasePlugin<TApplication> : IMvxPlugin where TApplication : IMvxApplication, new()
    {
        protected abstract string Name { get; }

        public virtual void Load()
        {
            new TApplication().Initialize();

            Mvx.IoCProvider.Resolve<IMvxLog>().Info($"{Name} plugin is loaded");
        }
    }
}
