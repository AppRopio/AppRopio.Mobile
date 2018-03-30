using System;
using System.Threading.Tasks;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard.Services
{
    public interface IBasketProductCardVmService
    {
        Task<bool> AddProductToBasket(string groupId, string productId);

        Task<float?> BasketProductQuantity(string productId);
    }
}
