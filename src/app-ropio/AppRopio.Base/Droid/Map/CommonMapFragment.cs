using System;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Util;
using Android.Views;
using AppRopio.Base.Droid.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Core;
using static Android.Gms.Maps.GoogleMap;

namespace AppRopio.Base.Droid.Map
{
    public abstract class CommonMapFragment<TViewModel> : CommonFragment<TViewModel>, IOnMapReadyCallback, IOnMapLoadedCallback, IOnMarkerClickListener, IOnMapClickListener
        where TViewModel : class, IMvxViewModel
    {
        protected MarkerSet MarkerSet { get; private set; }

        public ICommand MarkerClick { get; set; }

        protected CommonMapFragment()
        {
        }

        protected CommonMapFragment(int layoutId) 
            : base(layoutId)
        {
        }

        protected CommonMapFragment(int layoutId, string title) 
            : base(layoutId, title)
        {
        }

        protected GoogleMap Map { get; private set; }

        protected abstract MapView MapView { get; }

        public event EventHandler OnMapCreated;

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            MapView.OnCreate(savedInstanceState);
            MapView.GetMapAsync(this);
        }

        protected virtual void CreateBinding()
        {
            var set = this.CreateBindingSet<CommonMapFragment<TViewModel>, TViewModel>();

            set.Bind(MarkerSet).For(ms => ms.ItemsSource).To(GetMapItemsSourcePropertyName());

            set.Apply();
        }

        protected abstract Expression<Func<TViewModel, object>> GetMapItemsSourcePropertyName();

        protected virtual MarkerSet CreateMarkerSet(GoogleMap googleMap)
        {
            return new MarkerSet(googleMap)
            {
                ItemIcon = GetItemIcon,
                ItemPosition = GetItemPosition,
                OnMapReloaded = OnMapReloaded
            };
        }

        protected virtual void OnMapReloaded()
        {
            ZoomToAll();
        }

        protected void ZoomToAll()
        {
            if (MarkerSet?.MarkerWrappers?.Any() ?? false)
            {
                var boundsBuilder = new LatLngBounds.Builder();
                foreach (var markerWrapper in MarkerSet.MarkerWrappers)
                {
                    boundsBuilder.Include(markerWrapper.Position);
                }

                var bounds = CameraUpdateFactory.NewLatLngBounds(boundsBuilder.Build(), (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 70, Context.Resources.DisplayMetrics));
                Map.AnimateCamera(bounds);
            }
        }

        protected abstract LatLng GetItemPosition(object item);

        protected abstract BitmapDescriptor GetItemIcon(object item);

        #region Listeners

        public void OnMapReady(GoogleMap googleMap)
        {
            Map = googleMap;

            MarkerSet = CreateMarkerSet(googleMap);

            Map.SetOnMapLoadedCallback(this);
            Map.SetOnMarkerClickListener(this);
            Map.SetOnMapClickListener(this);
        }

        public void OnMapLoaded()
        {
            Map.SetOnMapLoadedCallback(null);
            CreateBinding();
            OnMapCreated.Raise(this);
        }

        public virtual bool OnMarkerClick(Marker marker)
        {
            try
            {
                MarkerClick?.Execute(MarkerSet.GetRawItem(marker));
                return true;
            }
            catch
            {

            }

            return false;
        }

        public virtual void OnMapClick(LatLng point)
        {
            try
            {
                MarkerClick?.Execute(null);
            }
            catch { }
        }

        #endregion
    
        #region Public

        public override void OnResume()
        {
            base.OnResume();

            try
            {
                MapView?.OnResume();
            }
            catch { }
        }

        public override void OnPause()
        {
            try
            {
                MapView?.OnPause();
            }
            catch { }

            base.OnPause();
        }

        public override void OnDestroy()
        {
            try
            {
                MapView?.OnDestroy();
            }
            catch { }

            base.OnDestroy();
        }

        public override void OnLowMemory()
        {
            base.OnLowMemory();

            try
            {
                MapView?.OnLowMemory();
            }
            catch { }
        }

        public override void OnSaveInstanceState(Android.OS.Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            try
            {
                MapView?.OnSaveInstanceState(outState);
            }
            catch { }
        }

        #endregion
    }
}
