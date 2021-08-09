using AppRopio.Base.Core.Plugins;
using AppRopio.Geofencing.Core;
using AppRopio.Geofencing.Core.Service;
using AppRopio.Geofencing.Droid.Services;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Geofencing.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Geofencing";

        public override void Load()
        {
            base.Load();

            Mvx.IoCProvider.RegisterSingleton<IGeofencingService>(() => new GeofencingService());

            Mvx.IoCProvider.Resolve<IGeofencingService>().Start();
        }
    }
}
