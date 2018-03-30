using System;
using System.Threading.Tasks;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Basket.Core.Services
{
    public interface IProductQuantityVmService
    {
        Task<ProductQuantity> ChangeQuantityTo(string productId, float quantity);
    }
}
