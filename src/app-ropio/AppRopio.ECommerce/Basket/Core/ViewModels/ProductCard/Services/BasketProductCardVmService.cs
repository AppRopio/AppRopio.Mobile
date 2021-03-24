﻿using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard.Services
{
    public class BasketProductCardVmService : BaseVmService, IBasketProductCardVmService
    {
        #region Services

        protected API.Services.IBasketService ApiService => Mvx.Resolve<API.Services.IBasketService>();

        #endregion

        #region IBasketProductCardVmService implementation

        public async Task<bool> AddProductToBasket(string groupId, string productId)
        {
            try
            {
                await ApiService.AddProductToBasket(groupId, productId);

                return true;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return false;
        }

        public async Task<float?> BasketProductQuantity(string productId)
        {
            float? quantity = null;

            try
            {
                var result = await ApiService.BasketProductQuantity(productId);
                quantity = result?.Quantity;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return quantity;
        }

        #endregion
    }
}
