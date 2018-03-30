using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.Base.Settings.Core.ViewModels.Regions.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Settings.Core.ViewModels.Regions
{
    public interface IRegionsViewModel : ISearchViewModel
    {
        IMvxCommand SelectionChangedCommand { get; }

        IRegionItemVm SelectedRegion { get; }

        MvxObservableCollection<IRegionGroupItemVm> Regions { get; }
    }
}