using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using CoreLocation;
using MapKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery.Map
{
    public class PlaceAnnotation<T> : MKAnnotation
        where T : IHasCoordinates
    {
        private CLLocationCoordinate2D _coordinate;
        public T Item { get; private set; }

        public PlaceAnnotation(T item)
        {
            Item = item;
            if (item?.Coordinates == null)
                return;

            try
            {
                _coordinate = new CLLocationCoordinate2D(item.Coordinates.Latitude, item.Coordinates.Longitude);
            }
            catch
            {
                
            }
        }

        public override CLLocationCoordinate2D Coordinate
        {
            get
            {
                return _coordinate;
            }
        }
    }
}

