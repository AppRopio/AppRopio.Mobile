using Android.App;
using Android.OS;
using AppRopio.Base.Core.Models.Device;
using AppRopio.Base.Core.Services.Device;

namespace AppRopio.Base.Droid.Services.Device
{
    public class DeviceService : IDeviceService
    {
        public PlatformType Platform => PlatformType.DroidPhone;

        public string Token => DeviceSettings.DeviceToken;

        public string PackageName => Application.Context.PackageName;

        public string OSVersion => Build.VERSION.Release;

        public string AppVersion => $"{GetVersionName()} {GetVersionCode()}";

        public string DeviceInfo => $"{Build.Device} {Build.Model}";

        private static string GetVersionName()
        {
            var context = Application.Context;
            var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);
            return info.VersionName;
        }

        private static string GetVersionCode()
        {
            var context = Application.Context;
            var info = context.PackageManager.GetPackageInfo(context.PackageName, 0);
            return info.VersionCode.ToString();
        }
    }
}
