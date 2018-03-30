using System;
using System.Linq.Expressions;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Util;
using Android.Views;
using AppRopio.Base.Droid.Map;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using AppRopio.Base.Map.Core.ViewModels.Points.Map;
using MvvmCross.Binding.BindingContext;

namespace AppRopio.Base.Map.Droid.Views.Points.Map
{
    public class PointsMapFragment : CommonMapFragment<IPointsMapViewModel>
    {
        protected MapView _mapView;
        protected override MapView MapView => _mapView;

        public PointsMapFragment()
            : base (Resource.Layout.app_map_points)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _mapView = view.FindViewById<MapView>(Resource.Id.app_map_points_mapView);

            return view;
        }

        protected override void CreateBinding()
        {
            base.CreateBinding();

            var set = this.CreateBindingSet<PointsMapFragment, IPointsMapViewModel>();

            set.Bind().For("MarkerClick").To(vm => vm.SelectionChangedCommand);

            set.Apply();
        }

        protected override Expression<Func<IPointsMapViewModel, object>> GetMapItemsSourcePropertyName()
        {
            return (vm) => vm.Items;
        }

        protected override LatLng GetItemPosition(object item)
        {
            var itemWithCoordinates = (item as IHasCoordinates);
            return new LatLng(itemWithCoordinates.Coordinates.Latitude, itemWithCoordinates.Coordinates.Longitude);
        }

        protected override BitmapDescriptor GetItemIcon(object item)
        {
            var selectedMarkerIconValue = new TypedValue();
            Context.Theme.ResolveAttribute(Resource.Attribute.app_map_points_ic_pin, selectedMarkerIconValue, false);

            return BitmapDescriptorFactory.FromResource(selectedMarkerIconValue.Data);
        }

        public override bool OnMarkerClick(Marker marker)
        {
            var result = base.OnMarkerClick(marker);

            var markerDeselectedIconValue = new TypedValue();
            Context.Theme.ResolveAttribute(Resource.Attribute.app_map_points_ic_pin_deselected, markerDeselectedIconValue, false);

            MarkerSet.MarkerWrappers?.ForEach(x =>
            {
                x.Marker.SetIcon(BitmapDescriptorFactory.FromResource(markerDeselectedIconValue.Data));
            });

            marker.SetIcon(GetItemIcon(null));

            var center = CameraUpdateFactory.NewLatLng(marker.Position);
            Map.AnimateCamera(center);

            return result;
        }

        public override void OnMapClick(LatLng point)
        {
            base.OnMapClick(point);

            MarkerSet.MarkerWrappers?.ForEach(x =>
            {
                x.Marker.SetIcon(GetItemIcon(null));
            });
        }
    }
}
