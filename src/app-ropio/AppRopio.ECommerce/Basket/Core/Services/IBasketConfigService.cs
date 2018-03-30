using System;
using AppRopio.ECommerce.Basket.Core.Models;
namespace AppRopio.ECommerce.Basket.Core.Services
{
    public interface IBasketConfigService
    {
        BasketConfig Config { get; }
    }
}
