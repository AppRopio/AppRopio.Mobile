using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Loyalty.Requests;
using AppRopio.Models.Loyalty.Responses;

namespace AppRopio.ECommerce.Loyalty.API.Services.Implementation
{
    public class PromoService : BaseService, IPromoService
    {
        protected string PROMO_CODE = "promo/code";

        public async Task<PromocodeApplied> ApplyPromoCode(string code)
        {
            return await Post<PromocodeApplied>(PROMO_CODE, ToStringContent(new PromoRequest { Code = code }));
        }
    }
}
