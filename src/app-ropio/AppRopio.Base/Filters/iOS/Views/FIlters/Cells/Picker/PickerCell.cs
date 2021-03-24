using System;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Picker;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using MvvmCross.Binding.Extensions;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Picker.Items;
using AppRopio.Base.Core.Converters;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Picker
{
    public partial class PickerCell : MvxTableViewCell
    {
        private UIPickerView _valuePicker;

        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public const float PICKER_TITLE_HEIGHT = 52;
        public const float PICKER_CONTENT_HEIGHT = 220;

        public static readonly NSString Key = new NSString("PickerCell");
        public static readonly UINib Nib;

        static PickerCell()
        {
            Nib = UINib.FromName("PickerCell", NSBundle.MainBundle);
        }

        protected PickerCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region Private

        private class PickerViewModel : MvxPickerViewModel
        {
            protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

            public PickerViewModel(UIPickerView pickerView)
                : base(pickerView)
            {

            }

            public override UIView GetView(UIPickerView pickerView, nint row, nint component, UIView view)
            {
                var item = ItemsSource.ElementAt((int)row) as PickerCollectionItemVM;
                var label = new AppRopio.Base.iOS.Controls.ARLabel()
                    .WithFrame(0, 0, pickerView.Bounds.Width, GetRowHeight(pickerView, component))
                    .WithTune(tune =>
                    {
                        tune.Text = item.ToString();
                        tune.TextAlignment = UITextAlignment.Center;
                    });

                label.SetupStyle(ThemeConfig.Filters.FiltersCell.Picker.PickerCell.Value);

                return label;
            }

            public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
            {
                return (nfloat)ThemeConfig.Filters.FiltersCell.Picker.PickerCell.Size.Height;
            }
        }

        #endregion

        #region Protected

        #region IntializationControls

        protected virtual void InitializeControls()
        {
            this.ClipsToBounds = true;
            this.Layer.MasksToBounds = true;

            SetupName(_name);

            SetupValueName(_valueName);

            SetupValuePicker(_valuePicker = new UIPickerView());

            AddSubview(_valuePicker);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.Filters.FiltersCell.Title);
        }

        protected virtual void SetupValueName(UILabel valueName)
        {
            valueName.SetupStyle(ThemeConfig.Filters.FiltersCell.Picker.Value);
        }

        protected virtual void SetupValuePicker(UIPickerView valuePicker)
        {
            valuePicker.Frame = new CoreGraphics.CGRect(0, this.Bounds.Height, DeviceInfo.ScreenWidth, PICKER_CONTENT_HEIGHT);
            valuePicker.BackgroundColor = UIColor.Clear;
            valuePicker.ShowSelectionIndicator = true;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PickerCell, IPickerFiVm>();

            BindName(_name, set);

            BindValueName(_valueName, set);

            BindValuePicker(_valuePicker, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PickerCell, IPickerFiVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindValueName(UILabel valueName, MvxFluentBindingDescriptionSet<PickerCell, IPickerFiVm> set)
        {
            set.Bind(valueName).To(vm => vm.ValueName);
            set
                .Bind(valueName)
                .For(v => v.TextColor)
                .To(vm => vm.Selected)
                .WithConversion(
                    "TrueFalse", 
                    new TrueFalseParameter
                    {
                        True = ThemeConfig.Filters.FiltersCell.Picker.Value.HighlightedTextColor.ToUIColor(),
                        False = ThemeConfig.Filters.FiltersCell.Picker.Value.TextColor.ToUIColor()
                    }
                );
        }

        protected virtual void BindValuePicker(UIPickerView valuePicker, MvxFluentBindingDescriptionSet<PickerCell, IPickerFiVm> set)
        {
            var pickerModel = new PickerViewModel(valuePicker);

            set.Bind(pickerModel).For(m => m.ItemsSource).To(vm => vm.Items);
            set.Bind(pickerModel).For(m => m.SelectedItem).To(vm => vm.SelectedItem);
            set.Bind(pickerModel).For(m => m.SelectedChangedCommand).To(vm => vm.SelectionChangedCommand);

            valuePicker.Model = pickerModel;
        }

        #endregion

        #endregion

    }
}
