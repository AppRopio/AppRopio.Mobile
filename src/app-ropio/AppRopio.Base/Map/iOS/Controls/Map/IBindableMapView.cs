using System.Collections.Generic;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using System.Windows.Input;
using System.Collections.ObjectModel;
using MvvmCross.Views;

namespace AppRopio.Base.Map.iOS.Controls.Map
{
    public interface IBindableMapView : IBindableMapView<IPointItemVM>
    {
        
    }

    public interface IBindableMapView<T>
        where T: class, IHasCoordinates
    {
        ObservableCollection<T> Items { get; set; }

        T SelectedItem { get; }

        ICommand SelectionChangedCommand { get; set; }

        void ZoomToAll();

        void ZoomToSelected();
    }
}
