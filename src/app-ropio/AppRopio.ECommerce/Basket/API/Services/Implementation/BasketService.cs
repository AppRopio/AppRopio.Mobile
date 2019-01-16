using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Basket.Responses.Basket;
using System.Globalization;
using AppRopio.Models.Products.Requests;
using AppRopio.Models.Products.Responses;
using AppRopio.Models.Basket.Requests;

namespace AppRopio.ECommerce.Basket.API.Services.Implementation
{
    public class BasketService : BaseService, IBasketService
    {
        protected string NEED_TO_LOAD = "basket/needToLoad";
        protected string VALIDATE = "basket/validate";

        protected string BASKET = "basket";
        protected string BASKET_DELETE_ALL = "basket/deleteAll";
        protected string BASKET_PRODUCTS_QUANTITY = "basket/quantity"; 
        protected string BASKET_ADD = "basket/add";
        protected string BASKET_PRODUCT_QUANTITY = "basket/productQuantity";
        protected string BASKET_PRODUCT_CHANGE_QUANTITY = "basket/changeQuantity";
        protected string BASKET_PRODUCT_DELETE = "basket/delete";
        protected string BASKET_SUMMARY_AMOUNT = "basket/amount";

        public Task DeleteAllItems()
        {
            return Post(BASKET_DELETE_ALL, postData: null, postDataCanBeNull: true);
        }

        public async Task<BasketModel> GetBasket(CancellationToken cancellationToken)
        {
            return await Get<BasketModel>(BASKET, cancellationToken: cancellationToken);
        }

        public async Task<int> GetQuantity()
        {
            return await Get(BASKET_PRODUCTS_QUANTITY, (string arg) => Convert.ToInt32(arg, NumberFormatInfo.InvariantInfo));
        }

        public async Task<bool> IsNeedToLoad(string versionId, CancellationToken cancellationToken)
        {
            return await Post(NEED_TO_LOAD, ToStringContent(new NeedToLoadRequest { Id = versionId }), (string arg) => Convert.ToBoolean(arg), cancellationToken: cancellationToken);
        }

        public async Task AddProductToBasket(string groupId, string productId)
        {
            await Post(BASKET_ADD, ToStringContent(new ProductRequest { GroupId = groupId, ProductId = productId }));
        }

        public async Task<ProductQuantity> ChangeQuantity(string productId, float quantity, CancellationToken cancellationToken)
        {
            return await Post<ProductQuantity>(BASKET_PRODUCT_CHANGE_QUANTITY, ToStringContent(new ChangeQuantityRequest { Id = productId, Quantity = quantity }), cancellationToken: cancellationToken);
        }

        public async Task<ProductQuantity> BasketProductQuantity(string productId)
        {
            return await Get<ProductQuantity>($"{BASKET_PRODUCT_QUANTITY}?productId={productId}");
        }

        public Task DeleteBasketProduct(string productId)
        {
            return Post(BASKET_PRODUCT_DELETE, ToStringContent(new { id = productId }));
        }

        public async Task<BasketValidity> CheckBasketValidity(string versionId, CancellationToken cancellationToken)
        {
            return await Post<BasketValidity>(VALIDATE, ToStringContent(new { id = versionId }));
        }

        public async Task<BasketAmount> GetBasketSummaryAmount(CancellationToken token)
        {
            return await Get<BasketAmount>(BASKET_SUMMARY_AMOUNT, cancellationToken: token);
        }
    }
}
