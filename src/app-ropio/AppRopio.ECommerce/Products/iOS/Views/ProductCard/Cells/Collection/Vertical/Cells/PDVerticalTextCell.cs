using System;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Vertical.Cells
{
    public partial class PDVerticalTextCell : MvxCollectionViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("PDVerticalTextCell");
        public static readonly UINib Nib;

        public override bool Selected
        {
            get;
            set;
        }

        static PDVerticalTextCell()
        {
            Nib = UINib.FromName("PDVerticalTextCell", NSBundle.MainBundle);
        }

        protected PDVerticalTextCell(IntPtr handle)
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
            SetupName(_name);
            SetupSelectedView(_selectedView);
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Collection.Value);
        }

        protected virtual void SetupSelectedView(UIView selectedView)
        {
            selectedView.Layer.CornerRadius = 4;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDVerticalTextCell, CollectionItemVM>();

            BindName(_name, set);
            BindSelectedView(_selectedView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel valueName, MvxFluentBindingDescriptionSet<PDVerticalTextCell, CollectionItemVM> set)
        {
            var valueConfig = ThemeConfig.ProductDetails.DetailsCell.Collection.Value;

			set.Bind(valueName).To(vm => vm.ValueName);
			set
				.Bind(valueName)
				.For(v => v.TextColor)
				.To(vm => vm.Selected)
                .WithConversion("TrueFalse", new TrueFalseParameter { True = valueConfig.HighlightedTextColor.ToUIColor(), False = valueConfig.TextColor.ToUIColor() });
        }

        protected virtual void BindSelectedView(UIView selectedView, MvxFluentBindingDescriptionSet<PDVerticalTextCell, CollectionItemVM> set)
        {
            set.Bind(selectedView).For(v => v.BackgroundColor).To(vm => vm.Selected).WithConversion("TrueFalse", new TrueFalseParameter { True = Theme.ColorPalette.Accent.ToUIColor(), False = Theme.ColorPalette.DisabledControl.ToUIColor() });
        }

        #endregion

        #endregion
    }
}
