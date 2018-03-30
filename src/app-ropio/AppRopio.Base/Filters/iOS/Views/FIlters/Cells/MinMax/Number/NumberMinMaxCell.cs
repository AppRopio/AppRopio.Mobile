using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.ValueConverters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax.Number;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;
using AppRopio.Base.Core.Converters;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.MinMax.Number
{
    public partial class NumberMinMaxCell : BaseMinMaxCell
    {
        public static readonly NSString Key = new NSString("NumberMinMaxCell");
        public static readonly UINib Nib;

        static NumberMinMaxCell()
        {
            Nib = UINib.FromName("NumberMinMaxCell", NSBundle.MainBundle);
        }

        protected NumberMinMaxCell(IntPtr handle)
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
        }

        #endregion

        #region BindingControls

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<NumberMinMaxCell, INumberMinMaxFiVm>();

            BindName(_name, set);

            BindFromField(_fromField, set);
            BindHideFromBtn(_hideFromButton);

            BindToField(_toField, set);
            BindHideToBtn(_hideToButton);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<NumberMinMaxCell, INumberMinMaxFiVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindFromField(UITextField fromField, MvxFluentBindingDescriptionSet<NumberMinMaxCell, INumberMinMaxFiVm> set)
        {
            set.Bind(fromField).To(vm => vm.Min).WithConversion("StringFormat", new StringFormatParameter { StringFormat = StringExtentionsMethods.StringPrice });

            fromField.ShouldEndEditing = (textField) =>
            {
                (DataContext as INumberMinMaxFiVm).MinValueChangedCommand.Execute(null);
                return true;
            };
        }

        protected virtual void BindHideFromBtn(UIButton hideFromButton)
        {
            hideFromButton.TouchUpInside += (sender, e) =>
            {
                this.EndEditing(true);
            };
        }

        protected virtual void BindToField(UITextField toField, MvxFluentBindingDescriptionSet<NumberMinMaxCell, INumberMinMaxFiVm> set)
        {
            set.Bind(toField).To(vm => vm.Max).WithConversion("StringFormat", new StringFormatParameter { StringFormat = StringExtentionsMethods.StringPrice });

            toField.ShouldEndEditing = (textField) =>
            {
                (DataContext as INumberMinMaxFiVm).MaxValueChangedCommand.Execute(null);
                return true;
            };
        }

        protected virtual void BindHideToBtn(UIButton hideToButton)
        {
            hideToButton.TouchUpInside += (sender, e) =>
            {
                this.EndEditing(true);
            };
        }

        #endregion

        #endregion
    }
}
