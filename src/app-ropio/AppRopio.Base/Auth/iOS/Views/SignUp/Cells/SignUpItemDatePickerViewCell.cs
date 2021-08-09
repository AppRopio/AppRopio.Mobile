using System;
using AppRopio.Base.Auth.Core.ViewModels.SignUp.Items;
using AppRopio.Base.Auth.iOS.Models;
using AppRopio.Base.Auth.iOS.Services;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Controls;
using AppRopio.Base.iOS.UIExtentions;
using CoreGraphics;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Extensions;
using MvvmCross.Platforms.Ios;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace AppRopio.Base.Auth.iOS.Views.SignUp.Cells
{
    public partial class SignUpItemDatePickerViewCell : MvxTableViewCell
    {
        protected AuthThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IAuthThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("SignUpItemDatePickerViewCell");
        public static readonly UINib Nib = UINib.FromName("SignUpItemDatePickerViewCell", NSBundle.MainBundle);

        protected UIDatePicker _pickerView;

        public ARTextField TextField
        {
            get
            {
                return _textField;
            }
            set
            {
                _textField = value;
            }
        }

        protected SignUpItemDatePickerViewCell(IntPtr handle) : base(handle)
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
            protected AuthThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IAuthThemeConfigService>().ThemeConfig; } }

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

                label.SetupStyle(ThemeConfig.PickerCell.Value);

                return label;
            }

            public override nfloat GetRowHeight(UIPickerView pickerView, nint component)
            {
                return (nfloat)ThemeConfig.PickerCell.Size.Height;
            }
        }

        private void SetDate(NSDate nsDate)
        {
            var date = nsDate.ToDateTimeUtc();
            var model = (DataContext as SignUpItemDatePickerViewModel);
            model.Value = string.Format(StringExtentionsMethods.StringDate(date), date);
            model.SelectedDate = new DateTime(date.Year, date.Month, date.Day);
        }


        #endregion

        #region Protected

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupTextField(_textField);
            SetupDatePicker(_pickerView = new UIDatePicker(), _textField);
        }

        protected virtual void SetupTextField(UITextField textField)
        {
            textField.SetupStyle(ThemeConfig.TextField);
        }

        protected virtual void SetupDatePicker(UIDatePicker datePicker, UITextField textField)
        {
            datePicker.Frame = new CGRect(0, 0, DeviceInfo.ScreenWidth, 220);
            datePicker.BackgroundColor = Theme.ColorPalette.Background.ToUIColor();
            datePicker.Mode = UIDatePickerMode.Date;
            datePicker.TimeZone = NSTimeZone.FromName(@"UTC");
            datePicker.Calendar = NSCalendar.CurrentCalendar;
            datePicker.ValueChanged += (sender, e) =>
            {
                var picker = sender as UIDatePicker;
                if (picker != null)
                {
                    SetDate(picker.Date);
                }
            };

            var accessoryView = new UIView()
                .WithFrame(0, 0, DeviceInfo.ScreenWidth, 44)
                .WithBackground(Theme.ColorPalette.Background.ToUIColor())
                .WithSubviews(
                    new UIButton(UIButtonType.Custom)
                    .WithFrame(DeviceInfo.ScreenWidth - 90, 0, 90, 44)
                    .WithTune(tune =>
                    {
                        tune.SetupStyle(ThemeConfig.TextButton);
                        tune.SetTitle(Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString("Base", "Done"), UIControlState.Normal);
                        tune.TouchUpInside += (sender, e) => textField.EndEditing(true);
                    }),
                    new UIView().WithFrame(0, 0, DeviceInfo.ScreenWidth, 1).WithBackground(Theme.ColorPalette.Separator.ToUIColor()),
                    new UIView().WithFrame(0, 43, DeviceInfo.ScreenWidth, 1).WithBackground(Theme.ColorPalette.Separator.ToUIColor())
                );

            textField.InputView = datePicker;
            textField.InputAccessoryView = accessoryView;
            textField.ShouldBeginEditing += (tF) =>
            {
                SetDate(datePicker.Date);
                return true;
            };

            textField.EditingChanged += (sender, e) =>
            {
                textField.Text = (DataContext as SignUpItemDatePickerViewModel)?.Value ?? string.Empty;
            };
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<SignUpItemDatePickerViewCell, SignUpItemDatePickerViewModel>();

            BindTextField(_textField, set);

            var model = (DataContext as SignUpItemDatePickerViewModel);

            BindDatePicker(_pickerView, model?.MinDate, model?.MaxDate);

            set.Apply();
        }

        protected virtual void BindDatePicker(UIDatePicker datePicker, DateTime? minDate, DateTime? maxDate)
        {
            if (minDate != null)
                datePicker.MinimumDate = minDate.Value.ToNSDateUTC();
            if (maxDate != null)
            {
                datePicker.SetDate((maxDate.Value.AddDays(-1).ToNSDateUTC()), false);
                datePicker.MaximumDate = maxDate.Value.ToNSDateUTC();
            }
        }

        protected virtual void BindTextField(ARTextField textField, MvxFluentBindingDescriptionSet<SignUpItemDatePickerViewCell, SignUpItemDatePickerViewModel> set)
        {
            set.Bind(textField).To(vm => vm.Value).OneWay();
            set.Bind(textField)
               .For(p => p.Error)
               .To(vm => vm.Invalid);
        }

        #endregion

        #endregion

    }
}
