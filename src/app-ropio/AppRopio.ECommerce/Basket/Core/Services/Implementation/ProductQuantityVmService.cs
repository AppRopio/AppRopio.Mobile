using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Services.TasksQueue;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Basket.API.Services;
using AppRopio.Models.Products.Responses;
using MvvmCross;

namespace AppRopio.ECommerce.Basket.Core.Services.Implementation
{
    public class ProductQuantityVmService : BaseVmService, IProductQuantityVmService
    {
        #region Fields

        private Dictionary<string, CancellationTokenSource> _quantityCTSs = new Dictionary<string, CancellationTokenSource>();

        #endregion

        #region Services

        private IBasketService _apiService;
        public IBasketService ApiService => _apiService ?? (_apiService = Mvx.IoCProvider.Resolve<IBasketService>());

        #endregion

        public async Task<ProductQuantity> ChangeQuantityTo(string id, float quantity)
        {
            var quantityResponse = new ProductQuantity();

            try
            {
                if (_quantityCTSs.ContainsKey(id) && _quantityCTSs[id] != null)
                    _quantityCTSs[id].Cancel();

                _quantityCTSs[id] = new CancellationTokenSource();

                quantityResponse = await Mvx.IoCProvider.Resolve<ITasksQueueService>()
                                            .Append(() => ApiService.ChangeQuantity(id, quantity, _quantityCTSs[id].Token), _quantityCTSs[id].Token);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);

                if (ex.RequestResult.RequestCancelReason == Base.API.Models.RequestCancelReason.Manual)
                    throw new OperationCanceledException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return quantityResponse;
        }
    }
}
