using System;
using AppRopio.Base.Settings.Core.Models;

namespace AppRopio.Base.Settings.Core.Services
{
    public interface ISettingsConfigService
    {
        SettingsConfig Config { get; }
    }
}