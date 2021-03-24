﻿using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Services.TasksQueue;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross;
using AppRopio.ECommerce.Basket.API.Services;

namespace AppRopio.ECommerce.Basket.Core.Services.Implementation
{
    public class ProductDeleteVmService : BaseVmService, IProductDeleteVmService
    {
        #region Services

        private IBasketService _apiService;
        public IBasketService ApiService => _apiService ?? (_apiService = Mvx.Resolve<IBasketService>());

        #endregion

        #region IProductDeleteVmService implementation

        public async Task<bool> DeleteProduct(string productId)
        {
            var result = false;

            try
            {
                await Mvx.Resolve<ITasksQueueService>().Append(() => ApiService.DeleteBasketProduct(productId));
                result = true;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return result;
        }

        #endregion
    }
}
