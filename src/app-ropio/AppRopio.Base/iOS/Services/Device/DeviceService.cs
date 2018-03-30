using System;
using AppRopio.Base.Core.Models.Device;
using AppRopio.Base.Core.Services.Device;
using Foundation;
using UIKit;
using Newtonsoft.Json;
using System.Globalization;

namespace AppRopio.Base.iOS.Services.Device
{
    public class DeviceService : IDeviceService
    {
        #region IDeviceService implementation

        public PlatformType Platform
        {
            get
            {
                return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone
                                                                   ? PlatformType.iPhone
                                                                   : PlatformType.iPad;
            }
        }

        public string Token
        {
            get
            {
                return UIDevice.CurrentDevice.IdentifierForVendor.AsString();
            }
        }

        public string OSVersion
        {
            get
            {
                return UIDevice.CurrentDevice.SystemVersion;
            }
        }

        public string AppVersion
        {
            get
            {
                return NSBundle.MainBundle.InfoDictionary["CFBundleShortVersionString"]?.ToString();
            }
        }

        public string PackageName
        {
            get
            {
                return NSBundle.MainBundle.InfoDictionary["CFBundleIdentifier"]?.ToString();
            }
        }

        public string DeviceInfo
        {
            get
            {
                return JsonConvert.SerializeObject(new { Platform = Platform.ToString(), Token, OSVersion, AppVersion, PackageName, CurrentCulture = CultureInfo.CurrentCulture.ToString() });
            }
        }

        #endregion
    }
}
