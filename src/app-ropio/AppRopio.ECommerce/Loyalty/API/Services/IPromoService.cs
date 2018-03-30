using System.Threading.Tasks;
using AppRopio.Models.Loyalty.Responses;

namespace AppRopio.ECommerce.Loyalty.API.Services
{
    public interface IPromoService
    {
        Task<PromocodeApplied> ApplyPromoCode(string code);
    }
}
