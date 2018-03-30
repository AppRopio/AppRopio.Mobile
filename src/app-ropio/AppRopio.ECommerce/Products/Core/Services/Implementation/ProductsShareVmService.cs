using System;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using Plugin.Share;

namespace AppRopio.ECommerce.Products.Core.Services.Implementation
{
    public class ProductsShareVmService : BaseVmService, IProductsShareVmService
    {
        #region Services

        protected API.Services.IProductService ProductService { get { return Mvx.Resolve<API.Services.IProductService>(); } }

        #endregion

        #region IProductsShareVmService implementation

        public async Task ShareProduct(string groupId, string productId)
        {
            try
            {
                var shareInfo = await ProductService.GetProductForShare(groupId, productId);

                if (shareInfo != null)
                {
                    Mvx.Resolve<IMvxMainThreadDispatcher>().RequestMainThreadAction(() =>
                       CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage { Title = shareInfo.Title, Text = shareInfo.Text, Url = shareInfo.Url })
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
