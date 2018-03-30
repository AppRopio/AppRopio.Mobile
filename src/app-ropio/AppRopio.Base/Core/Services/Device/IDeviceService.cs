using System;
using AppRopio.Base.Core.Models.Device;

namespace AppRopio.Base.Core.Services.Device
{
    public interface IDeviceService
    {
        PlatformType Platform { get; }

        string Token { get; }

        string PackageName { get; }

        string OSVersion { get; }

        string AppVersion { get; }

        string DeviceInfo { get; }
    }
}
