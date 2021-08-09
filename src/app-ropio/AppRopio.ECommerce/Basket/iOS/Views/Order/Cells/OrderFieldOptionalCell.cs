using System;

using Foundation;
using UIKit;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using AppRopio.ECommerce.Basket.iOS.Services;
using MvvmCross.Binding.BindingContext;
using AppRopio.Models.Basket.Responses.Enums;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using System.Linq;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
    public partial class OrderFieldOptionalCell : OrderFieldBaseCell
    {
        public static readonly NSString Key = new NSString("OrderFieldOptionalCell");
        public static readonly UINib Nib = UINib.FromName("OrderFieldOptionalCell", NSBundle.MainBundle);

        public Action<bool> OnSwitchChanged { get; set; }

        protected OrderFieldOptionalCell(IntPtr handle) : base(handle)
        {
        }

        #region InitializationControls

        protected override void InitializeControls() 
        {
            SetupTitleLabel(_titleLabel);
            SetupTitleLayout(_titleLayout);

            SetupSwitch(_switch);

            SetupTextView(_textView);
            SetupTextViewLayout(_textViewLayout);

            SetupSeparators(_middleSeparatorView, _bottomSeparatorView);

            this.SetupStyle(CellTheme);
        }

        protected virtual void SetupTitleLabel(UILabel titleLabel)
        {
            titleLabel.SetupStyle(CellTheme.OptionalTitleLabel);
        }

        protected virtual void SetupTitleLayout(UIView titleLayout)
        {
            if (CellTheme.Size?.Height != null)
            {
                var heightConst = titleLayout.Constraints.FirstOrDefault(x => x.FirstAttribute == NSLayoutAttribute.Height);
                if (heightConst != null)
                {
                    heightConst.Constant = (nfloat)CellTheme.Size.Height;
                }
                else
                {
                    titleLayout.HeightAnchor.ConstraintEqualTo((nfloat)CellTheme.Size.Height).Active = true;
                }
            }
        }

        protected virtual void SetupSwitch(UISwitch @switch)
        {
            @switch.SetupStyle(CellTheme.OptionalSwitch);

            @switch.ValueChanged += SwitchChanged;
        }

        protected virtual void SetupTextView(UITextView textView)
        {
            textView.SetupStyle(CellTheme.OptionalTextView);

            if (FieldInputAccessoryView != null)
                textView.InputAccessoryView = FieldInputAccessoryView;

            var type = ((IOrderFieldItemVM)DataContext).Type;
            switch (type)
            {
                case OrderFieldType.Number:
                case OrderFieldType.Phone:
                    textView.KeyboardType = UIKeyboardType.NumberPad;
                    break;
                case OrderFieldType.Email:
                    textView.KeyboardType = UIKeyboardType.EmailAddress;
                    break;
                default:
                    textView.KeyboardType = UIKeyboardType.Default;
                    textView.AutocorrectionType = UITextAutocorrectionType.Yes;
                    textView.AutocapitalizationType = UITextAutocapitalizationType.Sentences;
                    break;
            }
        }

        protected virtual void SetupTextViewLayout(UIView textViewLayout)
        {
            if (CellTheme.OptionalTextViewSize?.Height != null)
            {
                var heightConst = textViewLayout.Constraints.FirstOrDefault(x => x.FirstAttribute == NSLayoutAttribute.Height);
                if (heightConst != null)
                {
                    heightConst.Constant = (nfloat)CellTheme.OptionalTextViewSize.Height;
                }
                else
                {
                    textViewLayout.HeightAnchor.ConstraintEqualTo((nfloat)CellTheme.OptionalTextViewSize.Height).Active = true;
                }
            }
        }

        protected virtual void SetupSeparators(UIView middleSeparator, UIView bottomSeparator)
        {
            middleSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
            bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        #endregion

        #region BindingControls

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<OrderFieldOptionalCell, IOrderFieldItemVM>();

            BindTitleLabel(_titleLabel, set);
            BindTextView(_textView, set);

            set.Apply();
        }

        protected virtual void BindTitleLabel(UILabel titleLabel, MvxFluentBindingDescriptionSet<OrderFieldOptionalCell, IOrderFieldItemVM> set)
        {
            set.Bind(titleLabel).To(vm => vm.Name);
        }

        protected virtual void BindTextView(UITextView textView, MvxFluentBindingDescriptionSet<OrderFieldOptionalCell, IOrderFieldItemVM> set)
        {
            //var type = ((IOrderFieldItemVM)DataContext).Type;
            //if (type == OrderFieldType.Phone)
            //    set.Bind(textView).For("PhoneBinding").To(vm => vm.Value);
            //else
                set.Bind(textView).To(vm => vm.Value);
        }

        #endregion

        #region Private

        void SwitchChanged(object sender, EventArgs e)
        {
            _textViewLayout.Hidden = !_switch.On;

            if (OnSwitchChanged != null)
                OnSwitchChanged.Invoke(_switch.On);

            if (_textView.IsFirstResponder)
                _textView.EndEditing(true);
            else if (!_textViewLayout.Hidden)
                _textView.BecomeFirstResponder();
        }

        #endregion

        #region Protected

        protected override void Dispose(bool disposing)
        {
            _switch.ValueChanged -= SwitchChanged;

            base.Dispose(disposing);
        }

        #endregion
    }
}
