using System;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using AppRopio.ECommerce.Basket.iOS.Services;
using CoreGraphics;
using MapKit;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery.Map
{
    public class PlaceMapDelegate<T> : MKMapViewDelegate
        where T : IHasCoordinates
    {
        public Image PinImage { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.OnPoint.Map.PinImage; } }

        public T SelectedItem { get; set; }
        public Action<T> OnItemSelected;

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            var instAnnotation = annotation as PlaceAnnotation<T>;
            if (instAnnotation != null)
            {
                var pin = new MKAnnotationView(instAnnotation, "instAnnotation");

                pin.Image = UIImage.FromFile(PinImage.Path);

                if (pin.Image != null)
                    pin.CenterOffset = new CGPoint(0, -pin.Image.Size.Height / 2);

                pin.Draggable = false;
                pin.CanShowCallout = false;

                return pin;
            }

            return null;
        }

        public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView pin)
        {
            var instAnnotation = pin.Annotation as PlaceAnnotation<T>;
            if (instAnnotation != null)
            {
                if (PinImage.HighlightedPath != null)
                {
                    pin.Image = UIImage.FromFile(PinImage.HighlightedPath);
                    if (!Equals(SelectedItem, instAnnotation.Item))
                    {
                        SelectedItem = instAnnotation.Item;
                        OnItemSelected?.Invoke(instAnnotation.Item);
                    }
                }
            }
        }

        public override void DidDeselectAnnotationView(MKMapView mapView, MKAnnotationView pin)
        {
            var instAnnotation = pin.Annotation as PlaceAnnotation<T>;
            if (instAnnotation != null)
            {
                pin.Image = UIImage.FromFile(PinImage.Path);
            }
        }
    }
}

