using System;
using AppRopio.ECommerce.Products.Core.Models;
namespace AppRopio.ECommerce.Products.Core.Services
{
    public interface IProductConfigService
    {
        ProductsConfig Config { get; }
    }
}

