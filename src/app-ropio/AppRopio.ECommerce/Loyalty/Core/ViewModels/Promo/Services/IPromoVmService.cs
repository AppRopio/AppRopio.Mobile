using System.Threading.Tasks;

namespace AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo.Services
{
    public interface IPromoVmService
    {
        Task<bool> ApplyPromoCode(string code);
    }
}
