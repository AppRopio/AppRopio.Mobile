using System;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Marked.API.Services;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked.Items;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.ECommerce.Marked.Core.ViewModels.Marked.Services
{
    public class MarkedVmService : BaseVmService, IMarkedVmService
    {
		#region Services

		protected IMarkedService ApiService { get { return Mvx.Resolve<IMarkedService>(); } }

        #endregion

        #region IMarkedVmService implementation

        public async Task<MvxObservableCollection<ICatalogItemVM>> LoadMarkedProducts(int count, int offset = 0)
        {
            MvxObservableCollection<ICatalogItemVM> dataSource = null;

            try
            {
                var marked = await ApiService.GetMarkedProducts(count, offset);

                //if (!marked.IsNullOrEmpty())
                    dataSource = new MvxObservableCollection<ICatalogItemVM>(marked.Select(m => new MarkedProductVM(m)));
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

        #endregion
    }
}