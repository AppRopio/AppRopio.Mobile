using System;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.Messages;

namespace AppRopio.ECommerce.Products.Core.Services
{
    public interface IMarkProductVmService
    {
        Task<bool> MarkProductAsFavorite(string groupId, string productId, bool isMarked);

        void MarkProductAsFavorite(ProductMarkChangedMessage msg, Implementation.MarkProductVmService.MarkedDelegate onSuccess = null, Implementation.MarkProductVmService.MarkedDelegate onFailure = null);
    }
}
