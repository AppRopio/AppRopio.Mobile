using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Products.API.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items.Banners;
using AppRopio.Models.Products.Responses;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services.Banners
{
    public class BannersVmService : BaseVmService, IBannersVmService
    {
        #region Fields

        //private Queue _queue;

        #endregion

        #region Services

        protected IBannersService BannersService { get { return Mvx.Resolve<IBannersService>(); } }

        #endregion

        #region Private

        private async Task<ObservableCollection<IBannerItemVM>> LoadBannersFor(string categoryId, BannerPosition position)
        {
            ObservableCollection<IBannerItemVM> dataSource = null;

            try
            {
                var banners = await BannersService.LoadBanners(categoryId, position) ?? new List<Banner>();

                dataSource = new ObservableCollection<IBannerItemVM>(banners.Select(c => SetupItem(c)));
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

        #region Protected

        protected virtual IBannerItemVM SetupItem(Banner model)
        {
            return new BannerItemVM(model);
        }

        #endregion

        #region IBannersVmService implementation

        public async Task<ObservableCollection<IBannerItemVM>> LoadBannersFor(string categoryId = null)
        {
            return await LoadBannersFor(categoryId, BannerPosition.Top | BannerPosition.Bottom);
        }

        public async Task<ObservableCollection<IBannerItemVM>> LoadTopBannersFor(string categoryId = null)
        {
            return await LoadBannersFor(categoryId, BannerPosition.Top);
        }

        public async Task<ObservableCollection<IBannerItemVM>> LoadBottomBannersFor(string categoryId = null)
        {
            return await LoadBannersFor(categoryId, BannerPosition.Bottom);
        }

        #endregion
    }
}
