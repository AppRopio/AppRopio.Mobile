using System;
using AppRopio.Payments.ApplePay.Models;

namespace AppRopio.Payments.ApplePay.Services
{
    public interface IApplePayConfigService
    {
        ApplePayConfig Config { get; }
    }
}
