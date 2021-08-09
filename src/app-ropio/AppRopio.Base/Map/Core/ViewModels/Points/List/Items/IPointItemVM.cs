using System.Windows.Input;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points.List.Items
{
    public interface IPointItemVM : IMvxViewModel, IHasCoordinates
    {
        string Id { get; }

        string Name { get; }

        string Address { get; }

        string WorkTime { get; }

        string Distance { get; }

        string Phone { get; }

        string AdditionalInfo { get; }

        bool IsSelected { get; set; }

        ICommand CallCommand { get; }

        ICommand AdditionalInfoCommand { get; }

        ICommand RouteCommand { get; }
    }
}
