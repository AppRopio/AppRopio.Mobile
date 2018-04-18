using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Loyalty.Responses;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Loyalty.API.Services.Fake
{
    public class PromoFakeService : IPromoService
    {
        public bool IsRussianCulture => Mvx.Resolve<IConnectionService>().Headers.ContainsValue("ru-RU");

        public async Task<PromocodeApplied> ApplyPromoCode(string code)
        {
            await Task.Delay(700);
            return new PromocodeApplied { IsApplied = true, Message = IsRussianCulture ? "Промокод на сумму 200 \u20BD применен" : "$200 promotional code applied" };
        }
    }
}
