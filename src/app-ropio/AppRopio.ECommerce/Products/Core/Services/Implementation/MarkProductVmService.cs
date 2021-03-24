using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Products.API.Services;
using AppRopio.ECommerce.Products.Core.Messages;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Core.Services.Implementation
{
    public class MarkProductVmService : BaseVmService, IMarkProductVmService
    {
        #region Fields

        private bool _previuosTryWasFailure;

        private bool _markedMessagesDequeuingStarted;

        private Queue<ProductMarkChangedMessage> _markedMessagesQueue;

        #endregion

        #region Services

        protected IProductService ProductService { get { return Mvx.Resolve<IProductService>(); } }

        #endregion

        #region Constructor

        public MarkProductVmService()
        {
            _markedMessagesQueue = new Queue<ProductMarkChangedMessage>();
        }

        #endregion

        #region Public

        public delegate void MarkedDelegate(string groupId, string productId, bool isMarked);

        #region IMarkProductService implementation

        private async Task StartDequeuingMarkedItems(MarkedDelegate onSuccess = null, MarkedDelegate onFailure = null)
        {
            try
            {
                if (_markedMessagesQueue.Count > 0)
                {
                    ProductMarkChangedMessage msg = null;

                    lock (_markedMessagesQueue)
                    {
                        msg = _markedMessagesQueue.Dequeue();
                    }

                    var succeeded = await MarkProductAsFavorite(msg.Model.GroupId, msg.Model.Id, msg.Marked);

                    if (succeeded)
                    {
                        _previuosTryWasFailure = false;

                        onSuccess?.Invoke(msg.Model.GroupId, msg.Model.Id, msg.Marked);
                    }
                    else
                    {
                        onFailure?.Invoke(msg.Model.GroupId, msg.Model.Id, msg.Marked);

                        if (_previuosTryWasFailure)
                        {
                            foreach (var queuedMsg in _markedMessagesQueue)
                                onFailure?.Invoke(queuedMsg.Model.GroupId, queuedMsg.Model.Id, queuedMsg.Marked);

                            _markedMessagesQueue = new Queue<ProductMarkChangedMessage>();

                            _markedMessagesDequeuingStarted = false;
                        }

                        _previuosTryWasFailure = true;
                    }

                    await StartDequeuingMarkedItems(onSuccess, onFailure);
                }
                else
                    _markedMessagesDequeuingStarted = false;
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        public async Task<bool> MarkProductAsFavorite(string groupId, string productId, bool isMarked)
        {
            try
            {
                await ProductService.MarkProduct(groupId, productId, isMarked);
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
                return false;
            }
            catch (Exception ex)
            {
                OnException(ex);
                return false;
            }

            return true;
        }

        public void MarkProductAsFavorite(ProductMarkChangedMessage msg, MarkedDelegate onSuccess = null, MarkedDelegate onFailure = null)
        {
            lock (_markedMessagesQueue)
            {
                if (!_markedMessagesQueue.Any(x => (!x.Model.GroupId.IsNullOrEmpty() && x.Model.GroupId == msg.Model.GroupId) || x.Model.Id == msg.Model.Id))
                    _markedMessagesQueue.Enqueue(msg);
            }

            if (!_markedMessagesDequeuingStarted)
            {
                _markedMessagesDequeuingStarted = true;

                Task.Run(() => StartDequeuingMarkedItems(onSuccess, onFailure));
            }
        }

        #endregion

        #endregion
    }
}
