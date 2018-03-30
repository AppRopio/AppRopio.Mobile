using System.Collections.Generic;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using MapKit;
using System.Linq;
using System;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery.Map
{
    public class PlaceAnnotationManager<T> : MvxAnnotationManager
        where T : IHasCoordinates
    {
        public HashSet<MKAnnotation> Annotations { get; } = new HashSet<MKAnnotation>();

        public Action OnAllDeselected;

        public T SelectedItem
        {
            get
            {
                var mapDelegate = MapView.Delegate as PlaceMapDelegate<T>;
                if (mapDelegate == null)
                    return default(T);

                return mapDelegate.SelectedItem;
            }
            set
            {
                var mapDelegate = MapView.Delegate as PlaceMapDelegate<T>;
                if (mapDelegate == null || Equals(mapDelegate.SelectedItem, value))
                    return;

                mapDelegate.SelectedItem = value;

                var annotation = Annotations.FirstOrDefault(x => (x as PlaceAnnotation<T>).Item.Equals(value));
                if (annotation != null)
                {
                    MapView.SelectAnnotation(annotation, true);
                }
            }
        }

        public PlaceAnnotationManager(MKMapView mapView)
            : base(mapView)
        {
            var gr = new UITapGestureRecognizer(OnMapTap);
            gr.Delegate = new MapGestureRecognizerDelegate();
            MapView.AddGestureRecognizer(gr);
        }

        protected override MKAnnotation CreateAnnotation(object item)
        {
            var annotation = new PlaceAnnotation<T>((T)item);
            Annotations.Add(annotation);
            return annotation;
        }

        private void OnMapTap()
        {
            if (OnAllDeselected != null)
            {
                var mapDelegate = MapView.Delegate as PlaceMapDelegate<T>;
                if (mapDelegate == null)
                    mapDelegate.SelectedItem = default(T);
                OnAllDeselected.Invoke();
            }
        }
    }

    public class MapGestureRecognizerDelegate : UIGestureRecognizerDelegate
    {
        public override bool ShouldReceiveTouch(UIGestureRecognizer recognizer, UITouch touch)
        {
            if (touch.View is MKAnnotationView)
                return false;
            
            return true;
        }
    }
}

