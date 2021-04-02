using System;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Shops.Cells
{
    public partial class PDHShopIndicatorCell : MvxCollectionViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("PDHShopIndicatorCell");
        public static readonly UINib Nib;

        static PDHShopIndicatorCell()
        {
            Nib = UINib.FromName("PDHShopIndicatorCell", NSBundle.MainBundle);
        }

        protected PDHShopIndicatorCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }
        #region InitializationControls

        protected virtual void InitializeControls()
        {
            Layer.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.ShopsCompilation.Cell.Layer);
            
            SetupIndicator(_indicator);
            
            SetupName(_name);

            SetupAddress(_address);
        }

        protected virtual void SetupIndicator(UIView indicator)
        {
            indicator.ClipsToBounds = true;
            indicator.Layer.MasksToBounds = true;
            indicator.Layer.CornerRadius = 5;
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.ShopsCompilation.Cell.Name);
        }

        protected virtual void SetupAddress(UILabel address)
        {
            address.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.ShopsCompilation.Cell.Address);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDHShopIndicatorCell, IShopAvailabilityItemVM>();

            BindIndicator(_indicator, set);

            BindName(_name, set);

            BindAddress(_address, set);

            set.Apply();
        }

        protected virtual void BindIndicator(UIView indicator, MvxFluentBindingDescriptionSet<PDHShopIndicatorCell, IShopAvailabilityItemVM> set)
        {
            set.Bind(indicator)
               .For(v => v.BackgroundColor)
               .To(vm => vm.IsProductAvailable)
               .WithConversion(
                   "TrueFalse", 
                   new TrueFalseParameter { True = Theme.ColorPalette.Accent.ToUIColor(), False = Theme.ColorPalette.DisabledControl.ToUIColor() }
                  );
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDHShopIndicatorCell, IShopAvailabilityItemVM> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindAddress(UILabel address, MvxFluentBindingDescriptionSet<PDHShopIndicatorCell, IShopAvailabilityItemVM> set)
        {
            set.Bind(address).To(vm => vm.Address);
        }

        #endregion
    }
}
