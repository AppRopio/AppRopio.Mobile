using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using MapKit;
using MvvmCross;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Extensions;
using MvvmCross.Logging;
using MvvmCross.WeakSubscription;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery.Map
{
	public abstract class MvxAnnotationManager
    {
        private IEnumerable _itemsSource;
        private IDisposable _subscription;
        public Dictionary<object, MKAnnotation> _annotations = new Dictionary<object, MKAnnotation>();

        public MKMapView MapView { get; private set; }

        protected MvxAnnotationManager(MKMapView mapView)
        {
            MapView = mapView;
        }

        [MvxSetToNullAfterBinding]
        public virtual IEnumerable ItemsSource
        {
            get { return _itemsSource; }
            set { SetItemsSource(value); }
        }

        public virtual object Item
        {
            get { return _itemsSource?.ElementAt(0); }
            set { SetItemsSource(new List<object>() { value }); }
        }

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
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn("Binding to IEnumerable rather than IList - this can be inefficient, especially for large lists");

            ReloadAllAnnotations();

            var newObservable = _itemsSource as INotifyCollectionChanged;
            if (newObservable != null)
            {
                _subscription = newObservable.WeakSubscribe(OnItemsSourceCollectionChanged);
            }
        }

        protected virtual void OnItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
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

        protected virtual void ReloadAllAnnotations()
        {
            MapView.RemoveAnnotations(_annotations.Values.ToArray());
            _annotations.Clear();

            if (_itemsSource == null)
                return;

            AddAnnotations(_itemsSource);
        }

        protected abstract MKAnnotation CreateAnnotation(object item);

        protected virtual void RemoveAnnotations(IEnumerable oldItems)
        {
            foreach (var item in oldItems)
            {
                RemoveAnnotationFor(item);
            }
        }

        protected virtual void RemoveAnnotationFor(object item)
        {
            var annotation = _annotations[item];
            MapView.RemoveAnnotation(annotation);
            _annotations.Remove(item);
        }

        protected virtual void AddAnnotations(IEnumerable newItems)
        {
            foreach (object item in newItems)
            {
                AddAnnotationFor(item);
            }
        }

        protected virtual void AddAnnotationFor(object item)
        {
            var annotation = CreateAnnotation(item);
            _annotations[item] = annotation;
            MapView.AddAnnotation(annotation);
        }
    }
}
