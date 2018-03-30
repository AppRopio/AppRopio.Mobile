using System.Threading.Tasks;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items.Services
{
    public interface IBasketItemVmService
    {
        Task<ProductQuantity> SetQuantity(string id, float quantity);

        Task Delete(string id);
    }
}
