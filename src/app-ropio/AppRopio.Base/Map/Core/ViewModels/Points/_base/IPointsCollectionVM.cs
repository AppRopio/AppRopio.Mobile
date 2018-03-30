using System;
using System.Windows.Input;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Map.Core.ViewModels.Points
{
    public interface IPointsCollectionVM : IBaseViewModel
    {
        MvxObservableCollection<IPointItemVM> Items { get; }

        IPointItemVM SelectedItem { get; }

        ICommand SelectionChangedCommand { get; }
    }
}
