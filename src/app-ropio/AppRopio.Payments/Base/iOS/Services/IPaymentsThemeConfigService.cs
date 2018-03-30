using System;
using AppRopio.Payments.iOS.Models;

namespace AppRopio.Payments.iOS.Services
{
    public interface IPaymentsThemeConfigService
    {
        PaymentsThemeConfig ThemeConfig { get; }
    }
}