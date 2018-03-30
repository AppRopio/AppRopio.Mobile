using AppRopio.Models.Base.Responses;

namespace AppRopio.Base.Map.Core.ViewModels.Points.List.Items
{
    public interface IHasCoordinates
    {
        Coordinates Coordinates { get; }
    }
}
