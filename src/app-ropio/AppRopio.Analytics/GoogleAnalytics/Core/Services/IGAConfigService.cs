using System;
using AppRopio.Analytics.GoogleAnalytics.Core.Models.Config;
namespace AppRopio.Analytics.GoogleAnalytics.Core.Services
{
    public interface IGAConfigService
    {
        GAConfig Config { get; }
    }
}
