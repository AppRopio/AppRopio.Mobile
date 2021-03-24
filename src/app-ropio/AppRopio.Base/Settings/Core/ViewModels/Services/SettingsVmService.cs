using System;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.API.Exceptions;
using AppRopio.Base.Core;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Settings.API.Services;
using AppRopio.Base.Settings.Core.Models;
using AppRopio.Base.Settings.Core.ViewModels.Regions.Items;
using AppRopio.Base.Settings.Core.ViewModels.Messages;
using MvvmCross.ViewModels;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using AppRopio.Base.API.Services;
using System.Threading;
using AppRopio.Base.Settings.Core.Services;

namespace AppRopio.Base.Settings.Core.ViewModels.Services
{
    public class SettingsVmService : BaseVmService, ISettingsVmService
    {
		#region Services

		protected ISettingsService ApiService { get { return Mvx.Resolve<ISettingsService>(); } }

		protected IMvxMessenger MessengerService { get { return Mvx.Resolve<IMvxMessenger>(); } }

		#endregion

		public async Task<MvxObservableCollection<IRegionGroupItemVm>> LoadRegions()
		{
			MvxObservableCollection<IRegionGroupItemVm> dataSource = null;

			try
			{
                var regions = await ApiService.GetRegions();

                dataSource = new MvxObservableCollection<IRegionGroupItemVm>(regions.Select(r => new RegionGroupItemVm()
                {
                    Title = r.Title,
                    Items = r.Regions.Select(x => new RegionItemVm(x.Id, x.Title)).OfType<IRegionItemVm>().ToList()
                }));
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

        public async Task<IRegionItemVm> LoadRegion(string id)
		{
			IRegionItemVm item = null;

			try
			{
                var region = await ApiService.GetRegion(id);

                item = new RegionItemVm(region.Id, region.Title);
			}
			catch (ConnectionException ex)
			{
				OnConnectionException(ex);
			}
			catch (Exception ex)
			{
				OnException(ex);
			}

			return item;
		}

        public async Task<MvxObservableCollection<IRegionGroupItemVm>> SearchRegions(string query)
		{
			MvxObservableCollection<IRegionGroupItemVm> dataSource = null;

			try
			{
                var regions = await ApiService.SearchRegions(query);

				dataSource = new MvxObservableCollection<IRegionGroupItemVm>(regions.Select(r => new RegionGroupItemVm()
				{
					Title = r.Title,
					Items = r.Regions.Select(x => new RegionItemVm(x.Id, x.Title)).OfType<IRegionItemVm>().ToList()
				}));
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

        public void ChangeSelectedRegion(IRegionItemVm region)
		{
            Mvx.Resolve<IRegionService>().ChangeSelectedRegion(region.Id, region.Title);
		}

        public async Task ChangeNotifications(bool enabled, CancellationToken cancellationToken)
        {
			try
			{
                if (enabled)
                    await Mvx.Resolve<IPushService>().RegisterDevice(AppSettings.PushToken, cancellationToken);
                else
                    await Mvx.Resolve<IPushService>().RegisterDevice(string.Empty, cancellationToken);
			}
            catch (OperationCanceledException)
            {
                throw;
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
	}
}