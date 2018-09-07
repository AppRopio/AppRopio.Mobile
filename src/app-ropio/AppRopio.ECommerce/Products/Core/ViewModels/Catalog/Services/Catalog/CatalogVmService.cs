using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Products.API.Services;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using AppRopio.Models.Filters.Responses;
using AppRopio.Models.Products.Responses;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Logging;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Services
{
    public class CatalogVmService : BaseVmService, ICatalogVmService
    {
        #region Services

        protected IProductService ProductService { get { return Mvx.Resolve<IProductService>(); } }

        #endregion

        #region Protected

        protected virtual ICatalogItemVM SetupItem(Product model)
        {
            return new CatalogItemVM(model);
        }

        #endregion

        #region ICatalogVmService implementation

        public async Task<MvxObservableCollection<ICatalogItemVM>> LoadProductsInCategory(string categoryId,
                                                   int offset = 0,
                                                   int count = 10,
                                                   string searchText = null,
                                                   List<ApplyedFilter> filters = null,
                                                   SortType sort = null)
        {
            MvxObservableCollection<ICatalogItemVM> dataSource = null;

            try
            {
                var products = await ProductService.LoadProductsInCategory(
                    categoryId,
                    offset,
                    count,
                    searchText,
                    filters,
                    sort
                );
                dataSource = new MvxObservableCollection<ICatalogItemVM>(products.Select(SetupItem));
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return dataSource;
        }

        public IMvxViewModel LoadHeaderVm()
        {
            var config = Mvx.Resolve<IProductConfigService>().Config;

            if (config.Header != null)
            {
                var assembly = Assembly.Load(new AssemblyName(config.Header.AssemblyName));

                var headerType = assembly.GetType(config.Header.TypeName);

                if (headerType != null)
                {
                    var headerInstance = Activator.CreateInstance(headerType);

                    return headerInstance as IMvxViewModel;
                }

                Mvx.Resolve<IMvxLog>().Warn("CatalogVmService LoadHeaderVm headerType is null");
            }

            Mvx.Resolve<IMvxLog>().Warn("CatalogVmService LoadHeaderVm return null");

            return null;
        }

        public IMvxViewModel LoadItemBasketVm()
        {
            var config = Mvx.Resolve<IProductConfigService>().Config;

            try
            {
                if (config.Basket?.ItemAddToCart != null)
                {
                    var assembly = Assembly.Load(new AssemblyName(config.Basket.ItemAddToCart.AssemblyName));

                    var basketType = assembly.GetType(config.Basket.ItemAddToCart.TypeName);

                    object basketInstance = null;

                    if (basketType.GetTypeInfo().IsInterface)
                    {
                        var viewModelType = Mvx.Resolve<IViewModelLookupService>().Resolve(basketType);
                        basketInstance = Activator.CreateInstance(viewModelType);
                    }
                    else
                        basketInstance = Activator.CreateInstance(basketType);

                    return basketInstance as IMvxViewModel;
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
