using System;
using System.Threading.Tasks;
namespace AppRopio.ECommerce.Products.Core.Services
{
    public interface IProductsShareVmService
    {
        Task ShareProduct(string groupId, string productId);
    }
}
