using System;
using Android.Gms.Maps.Model;

namespace AppRopio.Base.Droid.Map
{
    public class MapMarkerWrapper
    {
        public object Item { get; private set; }
        public Marker Marker { get; private set; }

        public LatLng Position
        {
            get { return Marker.Position; }
            set { Marker.Position = value; }
        }

        public MapMarkerWrapper(object item, Marker marker)
        {
            Item = item;
            Marker = marker;
        }
    }
}
