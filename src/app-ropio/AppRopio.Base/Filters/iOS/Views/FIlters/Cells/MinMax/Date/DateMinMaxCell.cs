using System;
using System.Globalization;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax.Date;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.MinMax.Date
{
    public partial class DateMinMaxCell : BaseMinMaxCell
    {
        protected UIDatePicker _fromDatePicker;
        protected UIDatePicker _toDatePicker;

        public static readonly NSString Key = new NSString("DateMinMaxCell");
        public static readonly UINib Nib;

        static DateMinMaxCell()
        {
            Nib = UINib.FromName("DateMinMaxCell", NSBundle.MainBundle);
        }

        protected DateMinMaxCell(IntPtr handle)
            : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        #region Protected

        protected override void KeyboardWillShowNotification(NSNotification notification)
        {
            var activeField = _fromField.IsFirstResponder ? _fromField : _toField;
            if (activeField != null)
            {
                var keyboard = activeField.WeakInputDelegate as UIView;
                if (keyboard != null)
                {
                    if (keyboard.Superview != null)
                        keyboard.Superview.AddSubview(_fromField.IsFirstResponder ? _hideFromButton : _hideToButton);
                }
            }
        }

        #region InitializationControls

        protected override void InitializeControls()
        {
            SetupName(_name);

            SetupFromLabel(_fromLabel);

            SetupFromField(_fromField);

            SetupToLabel(_toLabel);

            SetupToField(_toField);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.Filters.FiltersCell.Title);
        }

        protected virtual void SetupFromLabel(UILabel fromLabel)
        {
            fromLabel.SetupStyle(ThemeConfig.Filters.FiltersCell.MinMax.Label);
        }

        protected virtual void SetupFromField(UITextField fromField)
        {
            fromField.SetupStyle(ThemeConfig.Filters.FiltersCell.MinMax.TextField);

            fromField.ReturnKeyType = UIReturnKeyType.Done;
            fromField.KeyboardType = UIKeyboardType.NumberPad;
            fromField.Placeholder = AppSettings.SettingsCulture == new CultureInfo("ru-RU") ? "дд.мм.гггг" : $"MM{AppSettings.SettingsCulture.DateTimeFormat.DateSeparator}dd{AppSettings.SettingsCulture.DateTimeFormat.DateSeparator}yyyy";

            if (ThemeConfig.Filters.FiltersCell.MinMax.DateInputMode == Models.DateInputMode.Picker)
                SetupDatePickerFor(_fromDatePicker = new UIDatePicker(), fromField);
        }

        protected virtual void SetupToLabel(UILabel toLabel)
        {
            toLabel.SetupStyle(ThemeConfig.Filters.FiltersCell.MinMax.Label);
        }

        protected virtual void SetupToField(UITextField toField)
        {
            toField.SetupStyle(ThemeConfig.Filters.FiltersCell.MinMax.TextField);

            toField.ReturnKeyType = UIReturnKeyType.Done;
            toField.KeyboardType = UIKeyboardType.NumberPad;
            toField.Placeholder = AppSettings.SettingsCulture == new CultureInfo("ru-RU") ? "дд.мм.гггг" : $"MM{AppSettings.SettingsCulture.DateTimeFormat.DateSeparator}dd{AppSettings.SettingsCulture.DateTimeFormat.DateSeparator}yyyy";

            if (ThemeConfig.Filters.FiltersCell.MinMax.DateInputMode == Models.DateInputMode.Picker)
                SetupDatePickerFor(_toDatePicker = new UIDatePicker(), toField);
        }

        protected virtual void SetupDatePickerFor(UIDatePicker datePicker, UITextField textField)
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
                    var date = picker.Date.ToDateTimeUTC();
                    textField.Text = string.Format(StringExtentionsMethods.StringDate(date), date);
                    textField.SendActionForControlEvents(UIControlEvent.ValueChanged);
                    textField.SendActionForControlEvents(UIControlEvent.EditingChanged);
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
                        tune.SetupStyle(ThemeConfig.Filters.FiltersCell.MinMax.DoneButton);
                        tune.SetTitle(Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString("Base", "Done"), UIControlState.Normal);
                        tune.TouchUpInside += (sender, e) => textField.EndEditing(true);
                    }),
                    new UIView().WithFrame(0, 0, DeviceInfo.ScreenWidth, 1).WithBackground(Theme.ColorPalette.Separator.ToUIColor()),
                    new UIView().WithFrame(0, 43, DeviceInfo.ScreenWidth, 1).WithBackground(Theme.ColorPalette.Separator.ToUIColor())
                );

            textField.InputView = datePicker;
            textField.InputAccessoryView = accessoryView;
        }

        #endregion

        #region BindingControls

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<DateMinMaxCell, IDateMinMaxFiVm>();

            BindName(_name, set);

            BindFromField(_fromField, set);
            BindHideFromBtn(_hideFromButton);

            BindToField(_toField, set);
            BindHideToBtn(_hideToButton);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<DateMinMaxCell, IDateMinMaxFiVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindFromField(UITextField fromField, MvxFluentBindingDescriptionSet<DateMinMaxCell, IDateMinMaxFiVm> set)
        {
            if (ThemeConfig.Filters.FiltersCell.MinMax.DateInputMode == Models.DateInputMode.Manual)
                set.Bind(fromField).For("FiltersDateBinding").To(vm => vm.Min);
            else
                set.Bind(fromField).To(vm => vm.Min).WithConversion("StringFormat", new StringFormatParameter { StringFormat = StringExtentionsMethods.StringDate });

            fromField.ShouldEndEditing = (textField) =>
            {
                (DataContext as IDateMinMaxFiVm).MinValueChangedCommand.Execute(null);

                return true;
            };

            if (ThemeConfig.Filters.FiltersCell.MinMax.DateInputMode == Models.DateInputMode.Picker)
            {
                var model = (DataContext as IDateMinMaxFiVm);
                BindDatePicker(_fromDatePicker, model.AbsoluteMin, model.AbsoluteMax, model.Min);
            }
        }

        protected virtual void BindHideFromBtn(UIButton hideFromButton)
        {
            hideFromButton.TouchUpInside += (sender, e) =>
            {
                this.EndEditing(true);
                (DataContext as IDateMinMaxFiVm).MinValueChangedCommand.Execute(null);
            };
        }

        protected virtual void BindToField(UITextField toField, MvxFluentBindingDescriptionSet<DateMinMaxCell, IDateMinMaxFiVm> set)
        {
            if (ThemeConfig.Filters.FiltersCell.MinMax.DateInputMode == Models.DateInputMode.Manual)
                set.Bind(toField).For("FiltersDateBinding").To(vm => vm.Max);
            else
                set.Bind(toField).To(vm => vm.Max).WithConversion("StringFormat", new StringFormatParameter { StringFormat = StringExtentionsMethods.StringDate });

            toField.ShouldEndEditing = (textField) =>
            {
                (DataContext as IDateMinMaxFiVm).MaxValueChangedCommand.Execute(null);
                return true;
            };

            if (ThemeConfig.Filters.FiltersCell.MinMax.DateInputMode == Models.DateInputMode.Picker)
            {
                var model = (DataContext as IDateMinMaxFiVm);
                BindDatePicker(_toDatePicker, model.AbsoluteMin, model.AbsoluteMax, model.Max);
            }
        }

        protected virtual void BindHideToBtn(UIButton hideToButton)
        {
            hideToButton.TouchUpInside += (sender, e) =>
            {
                this.EndEditing(true);
                (DataContext as IDateMinMaxFiVm).MaxValueChangedCommand.Execute(null);
            };
        }

        protected virtual void BindDatePicker(UIDatePicker datePicker, DateTime minDate, DateTime maxDate, DateTime date)
        {
            datePicker.MinimumDate = minDate.ToNSDateUTC();
            datePicker.MaximumDate = maxDate.ToNSDateUTC();
            datePicker.Date = date.ToNSDateUTC();
        }

        #endregion

        #endregion
    }
}
