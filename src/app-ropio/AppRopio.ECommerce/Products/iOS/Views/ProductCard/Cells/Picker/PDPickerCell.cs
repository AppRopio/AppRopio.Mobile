using System;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Picker.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Picker
{
    public partial class PDPickerCell : MvxTableViewCell
    {
        private UIPickerView _valuePicker;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public const float PICKER_TITLE_HEIGHT = 52;
        public const float PICKER_CONTENT_HEIGHT = 220;

        public static readonly NSString Key = new NSString("PDPickerCell");
        public static readonly UINib Nib;

        static PDPickerCell()
        {
            Nib = UINib.FromName("PDPickerCell", NSBundle.MainBundle);
        }

        protected PDPickerCell(IntPtr handle)
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
            protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

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

                label.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Picker.PickerCell.Value);

                return label;
            }

            public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
            {
                return (nfloat)ThemeConfig.ProductDetails.DetailsCell.Picker.PickerCell.Size.Height;
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

        protected virtual void SetupName(AppRopio.Base.iOS.Controls.ARLabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Picker.Title ?? ThemeConfig.ProductDetails.DetailsCell.Title);
        }

        protected virtual void SetupValueName(UILabel valueName)
        {
            valueName.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Picker.Value);
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
            var set = this.CreateBindingSet<PDPickerCell, IPickerPciVm>();

            BindName(_name, set);

            BindValueName(_valueName, set);

            BindValuePicker(_valuePicker, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDPickerCell, IPickerPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindValueName(UILabel valueName, MvxFluentBindingDescriptionSet<PDPickerCell, IPickerPciVm> set)
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
                        True = ThemeConfig.ProductDetails.DetailsCell.Picker.Value.HighlightedTextColor.ToUIColor(),
                        False = ThemeConfig.ProductDetails.DetailsCell.Picker.Value.TextColor.ToUIColor()
                    }
                );
        }

        protected virtual void BindValuePicker(UIPickerView valuePicker, MvxFluentBindingDescriptionSet<PDPickerCell, IPickerPciVm> set)
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
