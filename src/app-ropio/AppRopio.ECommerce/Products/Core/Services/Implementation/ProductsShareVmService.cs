using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross;
using MvvmCross.Base;
using Xamarin.Essentials;

namespace AppRopio.ECommerce.Products.Core.Services.Implementation
{
    public class ProductsShareVmService : BaseVmService, IProductsShareVmService
    {
        #region Services

        protected API.Services.IProductService ProductService { get { return Mvx.IoCProvider.Resolve<API.Services.IProductService>(); } }

        #endregion

        #region IProductsShareVmService implementation

        public async Task ShareProduct(string groupId, string productId)
        {
            try
            {
                var shareInfo = await ProductService.GetProductForShare(groupId, productId);

                if (shareInfo != null)
                {
                    Mvx.IoCProvider.Resolve<IMvxMainThreadAsyncDispatcher>().ExecuteOnMainThreadAsync(() =>
                       Share.RequestAsync(new ShareTextRequest() { Title = shareInfo.Title, Text = shareInfo.Text, Uri = shareInfo.Url })
                  );
                }
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }
        }

        #endregion
    }
}
