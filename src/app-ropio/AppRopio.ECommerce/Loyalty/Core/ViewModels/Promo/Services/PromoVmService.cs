using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross;
using AppRopio.ECommerce.Loyalty.API.Services;

namespace AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo.Services
{
    public class PromoVmService : BaseVmService, IPromoVmService
    {
        #region Services

        private IPromoService _apiService;
        public IPromoService ApiService => _apiService ?? (_apiService = Mvx.Resolve<IPromoService>());

        #endregion

        #region IPromoVmService implementation

        public async Task<bool> ApplyPromoCode(string code)
        {
            var result = false;
            try
            {
                var applied = await ApiService.ApplyPromoCode(code);

                result = applied.IsApplied;

                if (!applied.Message.IsNullOrEmpty())
                    UserDialogs.Confirm(applied.Message, "OK", !applied.IsApplied);
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
            return result;
        }

        #endregion
    }
}
