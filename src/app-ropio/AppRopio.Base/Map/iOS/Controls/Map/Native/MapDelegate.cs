using System;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using AppRopio.Base.Map.iOS.Services;
using CoreGraphics;
using MapKit;
using MvvmCross.Platform;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace AppRopio.Base.Map.iOS.Controls.Map.Native
{
    public class MapDelegate<T> : MKMapViewDelegate
        where T : class, IHasCoordinates
    {
        private Dictionary<IMKAnnotation, MKAnnotationView> _annotationViews = new Dictionary<IMKAnnotation, MKAnnotationView>();
        private Image PinImage { get { return Mvx.Resolve<IMapThemeConfigService>().ThemeConfig.Points.Map.PinImage; } }

        private T _selectedItem;
        public T SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                if (_selectedItem == default(T))
                {
                    _annotationViews?.Values.ForEach(x => 
                    {
                        x.Image = UIImage.FromFile(PinImage.HighlightedPath);
                    });
                }
            }
        }
        public Action<T> OnItemSelected;

        public override MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            var instAnnotation = annotation as Annotation<T>;
            if (instAnnotation != null)
            {
                var pin = new MKAnnotationView(instAnnotation, "instAnnotation");

                pin.Image = UIImage.FromFile((SelectedItem == default(T)) ? PinImage.HighlightedPath : PinImage.Path);

                if (pin.Image != null)
                    pin.CenterOffset = new CGPoint(0, -pin.Image.Size.Height / 2);

                pin.Draggable = false;
                pin.CanShowCallout = false;

                if (!_annotationViews.Keys.Contains(annotation))
                    _annotationViews.Add(annotation, pin);
                else
                    _annotationViews[annotation] = pin;

                return pin;
            }

            return null;
        }

        public override void DidSelectAnnotationView(MKMapView mapView, MKAnnotationView pin)
        {
            //малость хитрый дизайн с пинами =/
            _annotationViews?.Values.ForEach(x => 
            {
                x.Image = UIImage.FromFile(PinImage.Path);
            });

            var instAnnotation = pin.Annotation as Annotation<T>;
            if (instAnnotation != null)
            {
                pin.Image = UIImage.FromFile(PinImage.HighlightedPath);

                if (!Equals(SelectedItem, instAnnotation.Item))
                {
                    SelectedItem = instAnnotation.Item;
                    OnItemSelected?.Invoke(instAnnotation.Item);
                }
            }
        }

        public override void DidDeselectAnnotationView(MKMapView mapView, MKAnnotationView view)
        {
            //ничего не делаем,
            //выделение убирается при нажатии на карту
        }
    }
}
