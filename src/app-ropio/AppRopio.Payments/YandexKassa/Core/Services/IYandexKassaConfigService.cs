using System;
using AppRopio.Payments.YandexKassa.Core.Models;

namespace AppRopio.Payments.YandexKassa.Core.Services
{
    public interface IYandexKassaConfigService
    {
        YandexKassaConfig Config { get; }
    }
}