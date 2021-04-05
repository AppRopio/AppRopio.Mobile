using System;
using System.Collections.ObjectModel;
using System.Linq;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;
using AppRopio.ECommerce.Basket.iOS.Models;
using CoreGraphics;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Controls {
	public class DeliveryTimePickerViewModel : MvxPickerViewModel
    {
        private UIPickerView _pickerView;

        protected Collection<IDeliveryDayItemVM> DeliveryDays => ItemsSource as Collection<IDeliveryDayItemVM>;

        protected IDeliveryDayItemVM SelectedDay { get; private set; }
        public IDeliveryTimeItemVM SelectedTime { get; private set; }

        public override System.Collections.IEnumerable ItemsSource
        {
            get { return base.ItemsSource; }
            set
            {
                var deliveryDays = value as Collection<IDeliveryDayItemVM>;

                SelectedDay = deliveryDays?.FirstOrDefault();
                SelectedTime = SelectedDay?.Times?.FirstOrDefault();
                SelectedItem = SelectedTime;

                base.ItemsSource = value;
            }
        }

        public DelivetyTimeCell ThemeConfig { get; }

        public DeliveryTimePickerViewModel(UIPickerView pickerView, DelivetyTimeCell themeConfig)
            : base(pickerView)
        {
            ThemeConfig = themeConfig;
            _pickerView = pickerView;
        }

        protected override void CollectionChangedOnCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.CollectionChangedOnCollectionChanged(sender, e);

            SelectedDay = DeliveryDays?.FirstOrDefault();
            SelectedTime = SelectedDay?.Times?.FirstOrDefault();
            SelectedItem = SelectedTime;
        }

        protected override void Reload()
        {
            _pickerView.ReloadAllComponents();
            _pickerView.Select(0, 0, false);
            _pickerView.Select(0, 1, false);
        }

        public override nint GetComponentCount(UIPickerView picker)
        {
            return 2;
        }

        public override nint GetRowsInComponent(UIPickerView picker, nint component)
        {
            if (component == 0)
                return DeliveryDays?.Count ?? 0;
            else
                return SelectedDay?.Times?.Count ?? 0;
        }

        public override void Selected(UIPickerView picker, nint row, nint component)
        {
            if (DeliveryDays.IsNullOrEmpty() || (int)row >= DeliveryDays.Count)
                return;
            
            if (component == 0)
            {
                SelectedDay = DeliveryDays[(int)row];

                picker.ReloadComponent(1);

                var sameValueRow = SelectedDay?.Times?.FindIndex(x => x.Name.Equals(SelectedTime?.Name)) ?? -1;
                var newTimeRow = (sameValueRow > 0 ? sameValueRow : 0);

                picker.Select(newTimeRow, 1, true);
                SelectedTime = SelectedDay.Times[newTimeRow];
            }
            else
            {
                SelectedTime = SelectedDay.Times[(int)row];
            }

            SelectedItem = SelectedTime;
            if (SelectedChangedCommand != null && SelectedChangedCommand.CanExecute(SelectedItem))
            {
                SelectedChangedCommand.Execute(SelectedItem);
            }
        }

        public override string GetTitle(UIPickerView picker, nint row, nint component)
        {
            if (component == 0)
                return DeliveryDays[(int)row].Name;
            else
                return SelectedDay.Times[(int)row].Name;
        }

        public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
        {
            var label = new AppRopio.Base.iOS.Controls.ARLabel(new CGRect(0, 0, pickerView.Bounds.Width / 2, 32))
            {
                Text = GetTitle(pickerView, row, component),
                Lines = 0
            };
            label.SetupStyle(ThemeConfig.Title);
            label.SizeToFit();

            return label;
        }

        public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
        {
            return ThemeConfig.Size?.Height ?? 44;
        }
    }
}
