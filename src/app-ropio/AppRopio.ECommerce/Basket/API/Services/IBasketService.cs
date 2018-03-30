using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Models.Basket.Responses.Basket;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Basket.API.Services
{
    public interface IBasketService
    {
        /// <summary>
        /// Проверяет актуальность локальной корзины, относительно серверной.
        /// </summary>
        /// <param name="versionId">Версия локальной корзины.</param>
        Task<bool> IsNeedToLoad(string versionId, CancellationToken cancellationToken);

        Task<BasketValidity> CheckBasketValidity(string versionId, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает актуальную корзину с сервера.
        /// </summary>
        Task<BasketModel> GetBasket(CancellationToken cancellationToken);

        /// <summary>
        /// Очищает корзину на сервере.
        /// </summary>
        Task DeleteAllItems();

        /// <summary>
        /// Возвращает количество товара в корзине.
        /// </summary>
        Task<int> GetQuantity();

        Task AddProductToBasket(string groupId, string productId);

        Task<ProductQuantity> ChangeQuantity(string id, float quantity, CancellationToken token);

        Task<ProductQuantity> BasketProductQuantity(string productId);

        Task DeleteBasketProduct(string productId);

        Task<BasketAmount> GetBasketSummaryAmount(CancellationToken token);
    }
}
