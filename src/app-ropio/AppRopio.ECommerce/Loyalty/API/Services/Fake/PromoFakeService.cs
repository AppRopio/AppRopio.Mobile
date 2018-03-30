using System.Threading.Tasks;
using AppRopio.Models.Loyalty.Responses;

namespace AppRopio.ECommerce.Loyalty.API.Services.Fake
{
    public class PromoFakeService : IPromoService
    {
        public async Task<PromocodeApplied> ApplyPromoCode(string code)
        {
            await Task.Delay(700);
            return new PromocodeApplied { IsApplied = true, Message = "Промокод на сумму 200 \u20BD применен" };
        }
    }
}
