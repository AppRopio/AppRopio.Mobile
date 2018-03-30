using System;
using AppRopio.Analytics.AppsFlyer.Core.Models.Config;
namespace AppRopio.Analytics.AppsFlyer.Core.Services
{
    public interface IAFConfigService
    {
        AFConfig Config { get; }
    }
}
