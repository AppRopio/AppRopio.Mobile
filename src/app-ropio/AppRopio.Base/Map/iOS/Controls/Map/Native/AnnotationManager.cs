using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MapKit;
using MvvmCross.Binding;
using MvvmCross.Binding.Attributes;
using MvvmCross.Platform.Platform;
using MvvmCross.WeakSubscription;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using UIKit;

namespace AppRopio.Base.Map.iOS.Controls.Map.Native
{
    public class AnnotationManager<T>
        where T: class, IHasCoordinates
    {
        private IEnumerable _itemsSource;
        private IDisposable _subscription;
        private Dictionary<object, MKAnnotation> _annotations = new Dictionary<object, MKAnnotation>();

        public MKMapView MapView { get; private set; }
        public Action OnAllDeselected;

        [MvxSetToNullAfterBinding]
        public IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { SetItemsSource(value); }
        }

        #region Constructor

        public AnnotationManager(MKMapView mapView)
        {
            MapView = mapView;

            var gr = new UITapGestureRecognizer(OnMapTap);
            gr.Delegate = new MapGestureRecognizerDelegate();
            MapView.AddGestureRecognizer(gr);
        }

        #endregion

        #region Private

        private void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddAnnotations(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveAnnotations(e.OldItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveAnnotations(e.OldItems);
                    AddAnnotations(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    // not interested in this
                    break;
                case NotifyCollectionChangedAction.Reset:
                    ReloadAllAnnotations();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ReloadAllAnnotations()
        {
            MapView.RemoveAnnotations(_annotations.Values.ToArray());
            _annotations.Clear();

            if (_itemsSource == null)
                return;

            AddAnnotations(_itemsSource);
        }

        private void RemoveAnnotations(IEnumerable oldItems)
        {
            foreach (var item in oldItems)
            {
                RemoveAnnotationFor(item);
            }
        }

        private void RemoveAnnotationFor(object item)
        {
            var annotation = _annotations[item];
            MapView.RemoveAnnotation(annotation);
            _annotations.Remove(item);
        }

        private void AddAnnotations(IEnumerable newItems)
        {
            foreach (object item in newItems)
            {
                AddAnnotationFor(item);
            }
        }

        private void AddAnnotationFor(object item)
        {
            var annotation = CreateAnnotation(item);
            _annotations[item] = annotation;
            MapView.AddAnnotation(annotation);
        }

        private void OnMapTap()
        {
            if (OnAllDeselected != null)
            {
                var mapDelegate = MapView.Delegate as MapDelegate<T>;
                if (mapDelegate != null)
                    mapDelegate.SelectedItem = default(T);
                OnAllDeselected.Invoke();
            }
        }

        #endregion

        #region Protected

        protected virtual void SetItemsSource(IEnumerable value)
        {
            if (_itemsSource == value)
                return;

            if (_subscription != null)
            {
                _subscription.Dispose();
                _subscription = null;
            }
            _itemsSource = value;
            if (_itemsSource != null && !(_itemsSource is IList))
                MvxBindingTrace.Trace(MvxTraceLevel.Warning, "Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");

            ReloadAllAnnotations();

            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
            {
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);
            }
        }

        protected virtual MKAnnotation CreateAnnotation(T item)
        {
            return new Annotation<T>(item);
        }

        protected virtual MKAnnotation CreateAnnotation(object item)
        {
            return CreateAnnotation(item as T);
        }

        #endregion
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
