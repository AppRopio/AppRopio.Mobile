using System;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.MinMax.Number;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MinMax.Number
{
    public partial class PDNumberMinMaxCell : BaseMinMaxCell
    {
        public static readonly NSString Key = new NSString("PDNumberMinMaxCell");
        public static readonly UINib Nib;

        static PDNumberMinMaxCell()
        {
            Nib = UINib.FromName("PDNumberMinMaxCell", NSBundle.MainBundle);
        }

        protected PDNumberMinMaxCell(IntPtr handle)
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
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MinMax.Title ?? ThemeConfig.ProductDetails.DetailsCell.Title);
        }

        protected virtual void SetupFromLabel(UILabel fromLabel)
        {
            fromLabel.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MinMax.Label);
            fromLabel.Text = Mvx.Resolve<ILocalizationService>().GetLocalizableString(ProductsConstants.RESX_NAME, "ProductCard_From");
        }

        protected virtual void SetupFromField(UITextField fromField)
        {
            fromField.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MinMax.TextField);
            fromField.ReturnKeyType = UIReturnKeyType.Done;
            fromField.KeyboardType = UIKeyboardType.NumberPad;
        }

        protected virtual void SetupToLabel(UILabel toLabel)
        {
            toLabel.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MinMax.Label);
            toLabel.Text = Mvx.Resolve<ILocalizationService>().GetLocalizableString(ProductsConstants.RESX_NAME, "ProductCard_To");
        }

        protected virtual void SetupToField(UITextField toField)
        {
            toField.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MinMax.TextField);
            toField.ReturnKeyType = UIReturnKeyType.Done;
            toField.KeyboardType = UIKeyboardType.NumberPad;
        }

        #endregion

        #region BindingControls

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<PDNumberMinMaxCell, INumberMinMaxPciVm>();

            BindName(_name, set);

            BindFromField(_fromField, set);
            BindHideFromBtn(_hideFromButton);

            BindToField(_toField, set);
            BindHideToBtn(_hideToButton);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDNumberMinMaxCell, INumberMinMaxPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindFromField(UITextField fromField, MvxFluentBindingDescriptionSet<PDNumberMinMaxCell, INumberMinMaxPciVm> set)
        {
            set.Bind(fromField).To(vm => vm.Min).WithConversion("StringFormat", new StringFormatParameter { StringFormat = StringExtentionsMethods.StringPrice });

            fromField.ShouldEndEditing = (textField) =>
            {
                (DataContext as INumberMinMaxPciVm).MinValueChangedCommand.Execute(null);
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

        protected virtual void BindToField(UITextField toField, MvxFluentBindingDescriptionSet<PDNumberMinMaxCell, INumberMinMaxPciVm> set)
        {
            set.Bind(toField).To(vm => vm.Max).WithConversion("StringFormat", new StringFormatParameter { StringFormat = StringExtentionsMethods.StringPrice });

            toField.ShouldEndEditing = (textField) =>
            {
                (DataContext as INumberMinMaxPciVm).MaxValueChangedCommand.Execute(null);
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
