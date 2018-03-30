using System;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using CoreLocation;
using MapKit;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;

namespace AppRopio.Base.Map.iOS.Controls.Map.Native
{
    public class Annotation<T> : MKAnnotation
        where T : class, IHasCoordinates
    {
        private CLLocationCoordinate2D _coordinate;
        public T Item { get; private set; }

        public Annotation(T item)
        {
            Item = item;
            if (item?.Coordinates == null)
                return;

            try
            {
                _coordinate = new CLLocationCoordinate2D(item.Coordinates.Latitude, item.Coordinates.Longitude);
            }
            catch (Exception ex)
            {
                MvxBindingTrace.Trace(MvxTraceLevel.Error, ex.ToString());
            }
        }

        public override CLLocationCoordinate2D Coordinate
        {
            get { return _coordinate; }
        }
    }
}
