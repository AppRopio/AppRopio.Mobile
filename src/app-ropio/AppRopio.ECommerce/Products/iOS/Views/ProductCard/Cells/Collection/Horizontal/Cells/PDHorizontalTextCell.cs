using System;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Cells
{
    public partial class PDHorizontalTextCell : MvxCollectionViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public const float HORIZONTAL_MARGINS = 16;

        public static readonly NSString Key = new NSString("PDHorizontalTextCell");
        public static readonly UINib Nib;

        static PDHorizontalTextCell()
        {
            Nib = UINib.FromName("PDHorizontalTextCell", NSBundle.MainBundle);
        }

        protected PDHorizontalTextCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>

            {
                InitializeControls();
                BindControls();
            });
        }

        #region Protected

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupColorView(_colorView);
            SetupValueName(_valueName);
        }

        protected virtual void SetupColorView(UIView colorView)
        {
            colorView.Layer.CornerRadius = 4;
        }

        protected virtual void SetupValueName(UILabel valueName)
        {
            valueName.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Collection.Value);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDHorizontalTextCell, CollectionItemVM>();

            BindColorView(_colorView, set);
            BindValueName(_valueName, set);

            set.Apply();
        }

        protected virtual void BindColorView(UIView colorView, MvxFluentBindingDescriptionSet<PDHorizontalTextCell, CollectionItemVM> set)
        {
            set
                .Bind(colorView)
                .For(v => v.BackgroundColor)
                .To(vm => vm.Selected)
                .WithConversion("TrueFalse", new TrueFalseParameter { True = Theme.ColorPalette.Accent.ToUIColor(), False = Theme.ColorPalette.DisabledControl.ToUIColor() });
        }

        protected virtual void BindValueName(UILabel valueName, MvxFluentBindingDescriptionSet<PDHorizontalTextCell, CollectionItemVM> set)
        {
            var valueConfig = ThemeConfig.ProductDetails.DetailsCell.Collection.Value;

			set.Bind(valueName).To(vm => vm.ValueName);
            set
                .Bind(valueName)
                .For(v => v.TextColor)
                .To(vm => vm.Selected)
                .WithConversion("TrueFalse", new TrueFalseParameter { True = valueConfig.HighlightedTextColor.ToUIColor(), False = valueConfig.TextColor.ToUIColor() });
        }

        #endregion

        #endregion
    }
}
