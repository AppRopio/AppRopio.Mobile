using System;
using AppRopio.ECommerce.HistoryOrders.Core.Models;

namespace AppRopio.ECommerce.HistoryOrders.Core.Services
{
    public interface IHistoryOrdersConfigService
    {
		HistoryOrdersConfig Config { get; }
    }
}