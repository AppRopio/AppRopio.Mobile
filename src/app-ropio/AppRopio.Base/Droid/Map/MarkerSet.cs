using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using MvvmCross.Exceptions;
using MvvmCross.WeakSubscription;

namespace AppRopio.Base.Droid.Map
{
    public class MarkerSet
    {
        private IDisposable _token;

        protected readonly GoogleMap _map;

        private IEnumerable _itemsSource;
        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set
            {
                if (_itemsSource == value)
                    return;

                if (_token != null)
                {
                    _token.Dispose();
                    _token = null;
                }

                _itemsSource = value;

                var notify = _itemsSource as INotifyCollectionChanged;
                if (notify != null)
                    _token = notify.WeakSubscribe(HandleChangedMessage);

                ReloadAll();
            }
        }

        public List<MapMarkerWrapper> MarkerWrappers { get; private set; }

        public Func<object, LatLng> ItemPosition { get; set; }

        public Func<object, BitmapDescriptor> ItemIcon { get; set; }

        public Action OnMapReloaded { get; set; }

        public MarkerSet(GoogleMap map)
        {
            _map = map;
            MarkerWrappers = new List<MapMarkerWrapper>();
        }

        private void AddAll()
        {
            if (_itemsSource == null)
                return;

            foreach (var item in _itemsSource)
                AddMarker(item);
        }

        private void RemoveAll()
        {
            foreach (var item in MarkerWrappers)
                RemoveMarker(item);
            MarkerWrappers.Clear();
        }

        private void HandleChangedMessage(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                        AddMarker(item);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                        RemoveMarker(item);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    // ignore this - not used
                    throw new MvxException("Markers should not be moved");
                case NotifyCollectionChangedAction.Move:
                    // ignore this - not used
                    throw new MvxException("Markers should not be moved");
                case NotifyCollectionChangedAction.Reset:
                    ReloadAll();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected void AddMarker(object item)
        {
            var markerOptions = GetMarkerOptions(item);

            var marker = _map.AddMarker(markerOptions);

            MarkerWrappers.Add(new MapMarkerWrapper(item, marker));
        }

        protected virtual MarkerOptions GetMarkerOptions(object item)
        {
            return new MarkerOptions()
                        .SetPosition(ItemPosition(item))
                        .SetIcon(ItemIcon(item));
        }

        protected void RemoveMarker(object item)
        {
            if (MarkerWrappers != null && MarkerWrappers.Any())
            {
                var marker = MarkerWrappers.FirstOrDefault(x => x.Item.Equals(item))?.Marker;

                marker?.Remove();
            }
        }

        protected void RemoveMarker(MapMarkerWrapper item)
        {
            item?.Marker?.Remove();
        }

        protected virtual void ReloadAll()
        {
            RemoveAll();
            AddAll();

            OnMapReloaded?.Invoke();
        }

        public object GetRawItem(Marker marker)
        {
            object item = null;

            if (MarkerWrappers != null && MarkerWrappers.Any())
            {
                item = MarkerWrappers.FirstOrDefault(x => x.Marker.Id == marker.Id)?.Item;
            }

            return item;
        }
    }
}
