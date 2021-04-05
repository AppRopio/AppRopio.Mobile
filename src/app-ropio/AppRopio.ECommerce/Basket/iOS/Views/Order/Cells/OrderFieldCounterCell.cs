using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Basket.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.Base.Core.Converters;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
    public partial class OrderFieldCounterCell : OrderFieldBaseCell
    {
        public static readonly NSString Key = new NSString("OrderFieldCounterCell");
        public static readonly UINib Nib;

        public Action<bool> OnExpanded { get; set; }

        private bool _expanded;
        public bool Expanded
        {
            get => _expanded;
            set
            {
                if (value != _expanded)
                {
                    _expanded = value;
                    OnExpanded?.Invoke(value);
                }
            }
        }

        static OrderFieldCounterCell()
        {
            Nib = UINib.FromName("OrderFieldCounterCell", NSBundle.MainBundle);
        }

        protected OrderFieldCounterCell(IntPtr handle) 
            : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        #region Private

        private class PickerViewModel : MvxPickerViewModel
        {
            protected Models.OrderFieldCell CellTheme { get { return Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.BaseOrderFieldCell; } }

            public PickerViewModel(UIPickerView pickerView)
                : base(pickerView)
            {

            }

            public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
            {
                var item = ItemsSource.ElementAt((int)row) as string;
                var label = new AppRopio.Base.iOS.Controls.ARLabel()
                    .WithFrame(0, 0, pickerView.Bounds.Width, GetRowHeight(pickerView, component))
                    .WithTune(tune =>
                    {
                        tune.Text = item;
                        tune.TextAlignment = UITextAlignment.Center;
                    });

                label.SetupStyle(CellTheme.CounterPickerCell.Value);

                return label;
            }

            public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
            {
                return (nfloat)CellTheme.CounterPickerCell.Size.Height;
            }
        }

        #endregion

        #region Protected

        #region IntializationControls

        protected override void InitializeControls()
        {
            this.ClipsToBounds = true;
            this.Layer.MasksToBounds = true;

            _topViewHeightConstraint.Constant = (nfloat)CellTheme.Size.Height;

            SetupName(_name);

            SetupValueName(_valueName);

            SetupValuePicker(_valuePicker);

            this.SetupStyle(CellTheme);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(AppRopio.Base.iOS.Controls.ARLabel name)
        {
            name.SetupStyle(CellTheme.CounterTitle);
        }

        protected virtual void SetupValueName(UILabel valueName)
        {
            valueName.SetupStyle(CellTheme.CounterValue);
        }

        protected virtual void SetupValuePicker(UIPickerView valuePicker)
        {
            valuePicker.BackgroundColor = UIColor.Clear;
            valuePicker.ShowSelectionIndicator = true;
        }

        #endregion

        #region BindingControls

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<OrderFieldCounterCell, IOrderFieldItemVM>();

            BindName(_name, set);

            BindValueName(_valueName, set);

            BindValuePicker(_valuePicker, set);

            set.Bind().For(c => c.Expanded).To(vm => vm.Expanded);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<OrderFieldCounterCell, IOrderFieldItemVM> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindValueName(UILabel valueName, MvxFluentBindingDescriptionSet<OrderFieldCounterCell, IOrderFieldItemVM> set)
        {
            set.Bind(valueName).To(vm => vm.Value);
            set
                .Bind(valueName)
                .For(v => v.TextColor)
                .To(vm => vm.Expanded)
                .WithConversion(
                    "TrueFalse",
                    new TrueFalseParameter
                    {
                        True = CellTheme.CounterValue.HighlightedTextColor.ToUIColor(),
                        False = CellTheme.CounterValue.TextColor.ToUIColor()
                    }
                );
        }

        protected virtual void BindValuePicker(UIPickerView valuePicker, MvxFluentBindingDescriptionSet<OrderFieldCounterCell, IOrderFieldItemVM> set)
        {
            var pickerModel = new PickerViewModel(valuePicker);

            set.Bind(pickerModel).For(m => m.ItemsSource).To(vm => vm.Values);
            set.Bind(pickerModel).For(m => m.SelectedItem).To(vm => vm.Value);
            set.Bind(_valuePicker).For("Visibility").To(vm => vm.Expanded).WithConversion("Visibility");

            valuePicker.Model = pickerModel;
        }

        #endregion

        #endregion
    }
}
