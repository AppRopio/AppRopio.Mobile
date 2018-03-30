using System;
using AppRopio.Payments.CloudPayments.Core.Models;

namespace AppRopio.Payments.CloudPayments.Core.Services
{
    public interface ICloudPaymentsConfigService
    {
        CloudPaymentsConfig Config { get; }
    }
}