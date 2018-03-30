using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Basket.Core.Messages.Basket;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.Models.Products.Responses;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items.Services
{
    public class BasketItemVmService : BaseVmService, IBasketItemVmService
    {
        #region Fields

        private Dictionary<string, CancellationTokenSource> _quantityCTSs = new Dictionary<string, CancellationTokenSource>();

        #endregion

        #region Services

        protected IProductQuantityVmService QuantityVmService => Mvx.Resolve<IProductQuantityVmService>();

        #endregion

        #region IBasketItemVmService implementation

        public Task<ProductQuantity> SetQuantity(string id, float quantity)
        {
            return QuantityVmService.ChangeQuantityTo(id, quantity);
        }

        public Task Delete(string id)
        {
            return Task.Run(() =>
            {
                try
                {
                    Mvx.Resolve<IMvxMessenger>().Publish(new ItemDeletedMessage(this) { Id = id });
                    //вызов метода API происходит при обработке сообщения,
                    //т.к. удаление ячейки происходит из VM экрана

                    if (_quantityCTSs.ContainsKey(id))
                        _quantityCTSs.Remove(id);
                }
                catch (ConnectionException ex)
                {
                    OnConnectionException(ex);
                }
                catch (Exception ex)
                {
                    OnException(ex);
                }
            });
        }

        #endregion
    }
}
