using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Core.ViewModels.Search;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points.List
{
    public interface IPointsListViewModel : IPointsCollectionVM, IRefreshViewModel, ISearchViewModel
    {
        IMvxCommand MapCommand { get; }
    }
}
