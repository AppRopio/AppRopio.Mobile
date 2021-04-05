using System;
using System.Linq;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.Models.Basket.Responses.Enums;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using ObjCRuntime;
using UIKit;
using AppRopio.Base.iOS.Controls;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS.Helpers;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
    public partial class OrderFieldCell : OrderFieldBaseCell
    {
        public static readonly NSString Key = new NSString("OrderFieldCell");
        public static readonly UINib Nib = UINib.FromName("OrderFieldCell", NSBundle.MainBundle);

        private PhoneFormatter _phoneFormatter;

        private OrderFieldType _type;
        public virtual OrderFieldType Type
        {
            get => _type;
            set
            {
                _type = value;

                SetupTextFieldByType(_textField, value);
                SetupIconImageViewByType(_iconImageView, value);
                SetupActionButtonByType(_actionButton, value);

                this.ClearBindings(_textField);

                BindTextField(_textField);
            }
        }

        protected OrderFieldCell(IntPtr handle) 
            : base(handle)
        {
            _phoneFormatter = new PhoneFormatter { FireValueChanged = text => (DataContext as IOrderFieldItemVM).Value = text};
        }

        #region InitializationControls

        protected override void InitializeControls() 
        {
            SetupTextField(_textField);
            SetupIconImageView(_iconImageView);
            SetupActionButton(_actionButton);

            this.SetupStyle(CellTheme);
        }

        protected virtual void SetupTextField(UITextField textField)
        {
            textField.SetupStyle(CellTheme.Field);

            textField.ReturnKeyType = UIReturnKeyType.Done;
            textField.ShouldReturn = (sender) => 
            {
                sender.EndEditing(true);
                return true;
            };

            if (FieldInputAccessoryView != null)
                textField.InputAccessoryView = FieldInputAccessoryView;
        }

        protected virtual void SetupTextFieldByType(UITextField textField, OrderFieldType type)
        {
            switch (type)
            {
                case OrderFieldType.Number:
                case OrderFieldType.Phone:
                    textField.KeyboardType = UIKeyboardType.NumberPad;
                    break;
                case OrderFieldType.Email:
                    textField.KeyboardType = UIKeyboardType.EmailAddress;
                    textField.AutocapitalizationType = UITextAutocapitalizationType.None;
                    textField.AutocorrectionType = UITextAutocorrectionType.No;
                    break;
                default:
                    textField.KeyboardType = UIKeyboardType.Default;
                    textField.AutocapitalizationType = UITextAutocapitalizationType.Words;
                    textField.AutocorrectionType = UITextAutocorrectionType.Default;
                    break;
            }
        }

        protected virtual void SetupIconImageView(UIImageView iconImageView)
        {
            
        }

        protected virtual void SetupIconImageViewByType(UIImageView iconImageView, OrderFieldType type)
        {
            var typeString = type.ToString();
            if (!CellTheme.TypeImages.IsNullOrEmpty() && CellTheme.TypeImages.Keys.Contains(typeString.ToLower()))
            {
                iconImageView.Hidden = false;
                iconImageView.SetupStyle(CellTheme.TypeImages[typeString.ToLower()]);
            }
            else
            {
                iconImageView.Hidden = true;
            }
        }

        protected virtual void SetupActionButton(UIButton actionButton)
        {
            
        }

        protected virtual void SetupActionButtonByType(UIButton actionButton, OrderFieldType type)
        {
            var typeString = type.ToString();
            if (!CellTheme.TypeActionButtons.IsNullOrEmpty() && CellTheme.TypeActionButtons.Keys.Contains(typeString.ToLower()))
            {
                actionButton.Hidden = false;
                actionButton.SetupStyle(CellTheme.TypeActionButtons[typeString.ToLower()]);
            }
            else
            {
                actionButton.Hidden = true;
            }
        }

        #endregion

        #region BindingControls

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<OrderFieldCell, IOrderFieldItemVM>();

            BindTextField(_textField);
            BindActionButton(_actionButton);

            set.Bind().For(c => c.Type).To(vm => vm.Type);

            set.Apply();
        }

        protected virtual void BindTextField(ARTextField textField)
        {
            this.CreateBinding(textField).For(s => s.Placeholder).To<IOrderFieldItemVM>(vm => vm.Name).Apply();
            this.CreateBinding(textField).For(s => s.Enabled).To<IOrderFieldItemVM>(vm => vm.Editable).Apply();
            this.CreateBinding(textField).For(s => s.Error).To<IOrderFieldItemVM>(vm => vm.IsValid).WithConversion("TrueFalse", new TrueFalseParameter { True = false, False = true }).Apply();

            if (Type == OrderFieldType.Phone)
            {
                textField.ShouldChangeCharacters = _phoneFormatter.ShouldChangePhoneNumber;
            }
            else
            {
                textField.ShouldChangeCharacters = (tF, range, replacementString) => true;
            }

            this.CreateBinding(textField).To<IOrderFieldItemVM>(vm => vm.Value).Apply();
            this.CreateBinding(textField).For("EditingChanged").To<IOrderFieldItemVM>(vm => vm.AutocompleteCommand).Apply();
        }

        protected virtual void BindActionButton(UIButton actionButton)
        {
            this.CreateBinding(actionButton).To<IOrderFieldItemVM>(vm => vm.ActionCommand).Apply();
        }

        #endregion
    }
}
