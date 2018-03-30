using System.Threading.Tasks;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points.Services
{
    public interface IPointsVmService
    {
        Task<MvxObservableCollection<IPointItemVM>> LoadPoints(string searchText, int offset = 0, int count = 10);
    }
}
