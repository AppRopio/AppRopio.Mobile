using System;
using System.Threading.Tasks;

namespace AppRopio.ECommerce.Basket.Core.Services
{
    public interface IProductDeleteVmService
    {
        Task<bool> DeleteProduct(string productId);
    }
}
