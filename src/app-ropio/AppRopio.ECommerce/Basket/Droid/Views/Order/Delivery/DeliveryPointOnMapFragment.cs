using Android.Gms.Maps;
using Android.Graphics;
using Android.Util;
using Android.Views;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using Android.Gms.Maps.Model;
using AppRopio.ECommerce.Basket.Core;

namespace AppRopio.ECommerce.Basket.Droid.Views.Order.Delivery
{
    public class DeliveryPointOnMapFragment : CommonFragment<IDeliveryPointOnMapVM>, Android.Gms.Maps.IOnMapReadyCallback
    {
        private Android.Support.V7.Widget.Toolbar _toolbar;
        private View _statusBar;

        private MapView _mapView;

        public DeliveryPointOnMapFragment()
            : base(Resource.Layout.app_basket_delivery_point_on_map)
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "DeliveryPoint_OnMap");
        }

        private int GetStatusBarHeight()
        {
            int result = 0;
            int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
                result = Resources.GetDimensionPixelSize(resourceId);

            return result;
        }

        private void SetupStatusBar()
        {
            _statusBar = View.FindViewById<View>(Resource.Id.app_basket_delivery_on_point_status_bar_background);

            var statusBarColor = new TypedValue();
            Context.Theme.ResolveAttribute(Android.Resource.Attribute.StatusBarColor, statusBarColor, true);
            int color = statusBarColor.Data;

            if (_statusBar != null)
            {
                var layoutParameters = _statusBar.LayoutParameters;
                layoutParameters.Height = GetStatusBarHeight();
                _statusBar.LayoutParameters = layoutParameters;

                if (color != Color.Transparent.ToArgb())
                    _statusBar.Visibility = ViewStates.Gone;
            }
        }

        private void SetupBackground()
        {
            var appBackground = new TypedValue();
            Context.Theme.ResolveAttribute(Resource.Attribute.app_color_background, appBackground, true);
            var backgroundColor = new Color(appBackground.Data);

            View?.SetBackgroundColor(backgroundColor);
        }

        private void SetupMapView()
        {
            if (_mapView != null)
            {
                _mapView.GetMapAsync(this);
            }
        }

        protected override void SetToolbar()
        {
            _toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_basket_delivery_on_point_toolbar);

            _toolbar.Title = Title;

            _toolbar.SetNavigationIcon(Resource.Drawable.abc_ic_ab_back_material);
            _toolbar.NavigationClick += (sender, e) => ViewModel.CloseCommand.Execute(null);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _mapView = view.FindViewById<MapView>(Resource.Id.app_basket_delivery_point_on_map_mapView);
            _mapView.OnCreate(savedInstanceState);

            return view;
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SetupStatusBar();

            SetupBackground();

            SetupMapView();
        }

        public void OnMapReady(GoogleMap map)
        {
            var markerIconValue = new TypedValue();
            Context.Theme.ResolveAttribute(Resource.Attribute.app_basket_delivery_point_on_map_ic_pin, markerIconValue, false);

            var position = new LatLng(ViewModel.Item.Coordinates.Latitude, ViewModel.Item.Coordinates.Longitude);
            var marker = new MarkerOptions()
                        .SetPosition(position)
                        .SetIcon(BitmapDescriptorFactory.FromResource(markerIconValue.Data));
            
            map.AddMarker(marker);

            var center = CameraUpdateFactory.NewLatLngZoom(position, 15);
            map.AnimateCamera(center);
        }

        public override void OnResume()
        {
            base.OnResume();

            _mapView?.OnResume();
        }

        public override void OnPause()
        {
            _mapView?.OnPause();

            base.OnPause();
        }

        public override void OnDestroy()
        {
            try
            {
                _mapView?.OnDestroy();
            }
            catch
            { }

            base.OnDestroy();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();
            _mapView?.OnLowMemory();
        }

        public override void OnSaveInstanceState(Android.OS.Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            _mapView?.OnSaveInstanceState(outState);
        }
    }
}
