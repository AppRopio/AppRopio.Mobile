using System;
using AppRopio.Base.iOS.Views;
using UIKit;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order;
using MvvmCross.Platform;
using AppRopio.ECommerce.Basket.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Cells;
using AppRopio.Models.Basket.Responses.Enums;
using AppRopio.Base.iOS.Controls;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS.Helpers;
using System.Linq;
using AppRopio.ECommerce.Basket.Core;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order
{
    public partial class OrderFieldAutocompleteViewController : CommonViewController<IOrderFieldAutocompleteVM>
    {
        protected Models.OrderFieldCell FieldTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.BaseOrderFieldCell; } }
        protected Models.AutocompleteCell CellTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.AutocompleteCell; } }

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

        public OrderFieldAutocompleteViewController() : base("OrderFieldAutocompleteViewController", null)
        {
            _phoneFormatter = new PhoneFormatter { FireValueChanged = text => (DataContext as IOrderFieldAutocompleteVM).OrderFieldItem.Value = text};
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "DeliveryAddress_Title");

            RegisterKeyboardActions = true;
            NavigationController.NavigationBarHidden = false;
            //ModalPresentationStyle = UIModalPresentationStyle.Custom;

            SetupAutocompleteTableView(_autocompleteTableView);
            SetupTextField(_textField);
            SetupIconImageView(_iconImageView);
            SetupActionButton(_actionButton);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<OrderFieldAutocompleteViewController, IOrderFieldAutocompleteVM>();

            BindAutocompleteTableView(_autocompleteTableView, set);
            BindTextField(_textField);
            BindActionButton(_actionButton);

            set.Apply();
        }

        #endregion

        #region InitializationControls

        protected virtual void SetupAutocompleteTableView(UITableView tableView)
        {
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            tableView.RowHeight = CellTheme.Size.Height ?? 44;
            tableView.SeparatorColor = (UIColor)Theme.ColorPalette.Separator;
        }

        protected virtual void SetupTextField(UITextField textField)
        {
            textField.SetupStyle(FieldTheme.Field);

            textField.ReturnKeyType = UIReturnKeyType.Done;
            textField.ShouldReturn = (sender) => 
            {
                ViewModel?.ApplyCommand?.Execute();
                sender.EndEditing(true);
                return true;
            };

            //if (FieldInputAccessoryView != null)
                //textField.InputAccessoryView = FieldInputAccessoryView;

            textField.AutocorrectionType = UITextAutocorrectionType.No;
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
            if (!FieldTheme.TypeImages.IsNullOrEmpty() && FieldTheme.TypeImages.Keys.Contains(typeString.ToLower()))
            {
                iconImageView.Hidden = false;
                iconImageView.SetupStyle(FieldTheme.TypeImages[typeString.ToLower()]);
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
            if (!FieldTheme.TypeActionButtons.IsNullOrEmpty() && FieldTheme.TypeActionButtons.Keys.Contains(typeString.ToLower()))
            {
                actionButton.Hidden = false;
                actionButton.SetupStyle(FieldTheme.TypeActionButtons[typeString.ToLower()]);
            }
            else
            {
                actionButton.Hidden = true;
            }
        }

        #endregion

        #region BindingControls

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            var source = new MvxSimpleTableViewSource(tableView, OrderFieldAutocompleteCell.Key, OrderFieldAutocompleteCell.Key);

            return source;
        }

        protected virtual void BindAutocompleteTableView(UITableView tableView, MvxFluentBindingDescriptionSet<OrderFieldAutocompleteViewController, IOrderFieldAutocompleteVM> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            set.Bind(dataSource).For(s => s.ItemsSource).To(vm => vm.Items);
            set.Bind(dataSource).For(s => s.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        protected virtual void BindTextField(ARTextField textField)
        {
            this.CreateBinding(textField).For(s => s.Placeholder).To<IOrderFieldAutocompleteVM>(vm => vm.OrderFieldItem.Name).Apply();
            this.CreateBinding(textField).For(s => s.Enabled).To<IOrderFieldAutocompleteVM>(vm => vm.OrderFieldItem.Editable).Apply();
            this.CreateBinding(textField).For(s => s.Error).To<IOrderFieldAutocompleteVM>(vm => vm.OrderFieldItem.IsValid).WithConversion("TrueFalse", new TrueFalseParameter { True = false, False = true }).Apply();

            if (Type == OrderFieldType.Phone)
            {
                textField.ShouldChangeCharacters = _phoneFormatter.ShouldChangePhoneNumber;
            }
            else
            {
                textField.ShouldChangeCharacters = (tF, range, replacementString) => true;
            }

            this.CreateBinding(textField).To<IOrderFieldAutocompleteVM>(vm => vm.OrderFieldItem.Value).Apply();
            this.CreateBinding(textField).For("EditingChanged").To<IOrderFieldAutocompleteVM>(vm => vm.ValueChangedCommand).Apply();
        }

        protected virtual void BindActionButton(UIButton actionButton)
        {
            this.CreateBinding(actionButton).To<IOrderFieldAutocompleteVM>(vm => vm.OrderFieldItem.ActionCommand).Apply();
        }

        protected override void KeyboardWillShowNotification(Foundation.NSNotification notification)
        {
            base.KeyboardWillShowNotification(notification);

            var insetCorrection = 144;
            _autocompleteTableView.ContentInset = new UIEdgeInsets(0, 0, _autocompleteTableView.ContentInset.Bottom + insetCorrection, 0);
            _autocompleteTableView.ScrollIndicatorInsets = new UIEdgeInsets(0, 0, _autocompleteTableView.ScrollIndicatorInsets.Bottom + insetCorrection, 0);
        }

        #endregion

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            _textField.BecomeFirstResponder();
            _autocompleteTableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.None;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            _textField.ResignFirstResponder();
        }
    }
}

