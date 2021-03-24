using System;
using System.Reflection;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Core.Services.Implementation
{
    public class ProductsVmService : BaseVmService, IProductsVmService
    {
        #region Properties

        protected virtual IMvxViewModel BasketCartIndicatorVm { get; private set; }

        #endregion

        #region IProductsVmService implementation

        public async Task<IMvxViewModel> LoadCartIndicatorViewModel()
        {
            if (BasketCartIndicatorVm != null)
                return BasketCartIndicatorVm;

            var config = Mvx.Resolve<IProductConfigService>().Config;

            try
            {
                if (config.Basket?.CartIndicator != null)
                {
                    var assembly = Assembly.Load(new AssemblyName(config.Basket.CartIndicator.AssemblyName));

                    var basketType = assembly.GetType(config.Basket.CartIndicator.TypeName);

                    object basketInstance = null;

                    if (basketType.GetTypeInfo().IsInterface)
                    {
                        var vmLookupService = Mvx.Resolve<IViewModelLookupService>();
                        if (vmLookupService.IsRegistered(basketType))
                        {
                            var viewModelType = vmLookupService.Resolve(basketType);

                            basketInstance = Activator.CreateInstance(viewModelType);
                        }
                        else
                        {
                            var tcs = new TaskCompletionSource<IMvxViewModel>();

                            vmLookupService.CallbackWhenRegistered(basketType, type =>
                            {
                                tcs.TrySetResult(Activator.CreateInstance(type) as IMvxViewModel);
                            });

                            return await tcs.Task;
                        }
                    }
                    else
                        basketInstance = Activator.CreateInstance(basketType);

                    return (BasketCartIndicatorVm = basketInstance as IMvxViewModel);
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return null;
        }

        #endregion
    }
}
