using MapKit;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using System;
using System.Windows.Input;
using Foundation;
using System.ComponentModel;
using CoreGraphics;
using System.Collections.ObjectModel;
using CoreLocation;
using System.Linq;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;

namespace AppRopio.Base.Map.iOS.Controls.Map.Native
{
    [Register("BindableMKMapView"), DesignTimeVisible(true)]
    public class BindableMKMapView : BindableMKMapView<IPointItemVM>, IBindableMapView
    {
        public BindableMKMapView()
        {
        }

        public BindableMKMapView(CGRect frame)
            : base(frame)
        {
        }

        public BindableMKMapView(IntPtr handle)
            : base(handle)
        {
        }
    }

    public class BindableMKMapView<T> : MKMapView, IBindableMapView<T>
        where T: class, IHasCoordinates
    {
        private const double MAP_PADDING = 1.5;
        private const double MIN_VISIBLE_LATITUDE = 0.01;

        public AnnotationManager<T> AnnotationManager { get; private set; }

        private ObservableCollection<T> _items;
        public ObservableCollection<T> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;

                if (AnnotationManager != null)
                    AnnotationManager.ItemsSource = value;
                
                ZoomToAll();
            }
        }

        private T _selectedItem;
        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;

                ZoomToSelected();
            }
        }

        public ICommand SelectionChangedCommand { get; set; }

        #region Constructor

        public BindableMKMapView()
        {
            Initialize();
        }

        public BindableMKMapView(CGRect frame)
            : base(frame)
        {
            Initialize();
        }

        public BindableMKMapView(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        #endregion

        #region Private

        #endregion

        #region Protected

        protected virtual void Initialize()
        {
            Delegate = new MapDelegate<T>()
            {
                OnItemSelected = OnItemSelected
            };
            AnnotationManager = new AnnotationManager<T>(this)
            {
                OnAllDeselected = () => OnItemSelected(default(T))
            };
        }

        protected void OnItemSelected(T item)
        {
            SelectedItem = item;

            if (SelectionChangedCommand != null && SelectionChangedCommand.CanExecute(item))
                SelectionChangedCommand.Execute(item);
        }

        #endregion

        #region Public

        public void ZoomToAll()
        {
            if (Items == null || Items.Count < 1 || Items.All(x => x.Coordinates == null))
                return;

            try
            {
                double minLatitude = Items.Min(x => x.Coordinates?.Latitude ?? double.MaxValue);
                double maxLatitude = Items.Max(x => x.Coordinates?.Latitude ?? double.MinValue);
                double minLongitude = Items.Min(x => x.Coordinates?.Longitude ?? double.MaxValue);
                double maxLongitude = Items.Max(x => x.Coordinates?.Longitude ?? double.MinValue);

                MKCoordinateRegion region;
                region.Center.Latitude = (minLatitude + maxLatitude) / 2;
                region.Center.Longitude = (minLongitude + maxLongitude) / 2;

                double latitudeDelta = (maxLatitude - minLatitude) * MAP_PADDING;
                region.Span.LatitudeDelta = (latitudeDelta < MIN_VISIBLE_LATITUDE) ? MIN_VISIBLE_LATITUDE : latitudeDelta;
                region.Span.LongitudeDelta = (maxLongitude - minLongitude) * MAP_PADDING;

                SetRegion(RegionThatFits(region), true);
            }
            catch (Exception ex)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, ex.ToString());
            }
        }

        public void ZoomToSelected()
        {
            var point = SelectedItem?.Coordinates;
            if (point == null)
                return;

            MKCoordinateRegion region;
            region.Center = new CLLocationCoordinate2D(point.Latitude - 0.001, point.Longitude);
            region.Span.LatitudeDelta = MIN_VISIBLE_LATITUDE;
            region.Span.LongitudeDelta = MIN_VISIBLE_LATITUDE;
            SetRegion(RegionThatFits(region), true);
        }

        #endregion
    }
}
