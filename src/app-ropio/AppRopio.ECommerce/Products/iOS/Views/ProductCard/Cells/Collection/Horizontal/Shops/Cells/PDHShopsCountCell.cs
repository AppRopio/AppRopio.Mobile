using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Shops.Cells
{
    public partial class PDHShopsCountCell : MvxCollectionViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("PDHShopsCountCell");
        public static readonly UINib Nib;

        static PDHShopsCountCell()
        {
            Nib = UINib.FromName("PDHShopsCountCell", NSBundle.MainBundle);
        }

        protected PDHShopsCountCell(IntPtr handle) : base(handle)
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

            SetupName(_name);

            SetupAddress(_address);

            SetupCount(_count);
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.ShopsCompilation.Cell.Name);
        }

        protected virtual void SetupAddress(UILabel address)
        {
            address.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.ShopsCompilation.Cell.Address);
        }

        protected virtual void SetupCount(UILabel count)
        {
            count.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.ShopsCompilation.Cell.Count);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDHShopsCountCell, IShopAvailabilityItemVM>();

            BindName(_name, set);

            BindAddress(_address, set);

            BindCount(_count, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDHShopsCountCell, IShopAvailabilityItemVM> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindAddress(UILabel address, MvxFluentBindingDescriptionSet<PDHShopsCountCell, IShopAvailabilityItemVM> set)
        {
            set.Bind(address).To(vm => vm.Address);
        }

        private void BindCount(UILabel count, MvxFluentBindingDescriptionSet<PDHShopsCountCell, IShopAvailabilityItemVM> set)
        {
            set.Bind(count).To(vm => vm.Count);
        }

        #endregion
    }
}
