using System;
using System.Threading.Tasks;
using AppRopio.Base.Settings.Core.ViewModels.Regions.Items;
using AppRopio.Models.Settings.Responses;
using MvvmCross.Core.ViewModels;
using System.Threading;

namespace AppRopio.Base.Settings.Core.ViewModels.Services
{
    public interface ISettingsVmService
    {
        Task<MvxObservableCollection<IRegionGroupItemVm>> LoadRegions();

        Task<IRegionItemVm> LoadRegion(string id);

        Task<MvxObservableCollection<IRegionGroupItemVm>> SearchRegions(string query);

        void ChangeSelectedRegion(IRegionItemVm regionItem);

        Task ChangeNotifications(bool enabled, CancellationToken cancellationToken);
    }
}