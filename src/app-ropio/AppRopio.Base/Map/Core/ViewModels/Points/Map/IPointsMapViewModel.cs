using AppRopio.Base.Map.Core.ViewModels.Points.Paged;
namespace AppRopio.Base.Map.Core.ViewModels.Points.Map
{
    public interface IPointsMapViewModel : IPointsCollectionVM
    {
        bool IsShowInfoBlock { get; }
    }
}
