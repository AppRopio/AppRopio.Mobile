using System;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;

namespace AppRopio.Base.Map.Core.ViewModels.Points
{
    public interface IPointAdditionalInfoVM : IBaseViewModel
    {
        IPointItemVM Item { get; }

        ICommand CloseCommand { get; }
    }
}
