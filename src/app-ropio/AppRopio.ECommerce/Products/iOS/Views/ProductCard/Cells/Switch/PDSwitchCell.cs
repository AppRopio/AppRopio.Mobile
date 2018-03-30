using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Switch;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Switch
{
    public partial class PDSwitchCell : MvxTableViewCell
    {
        public const float SWITCH_HEIGHT = 52;

        protected ProductsThemeConfig ThemeConfig => Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig;

        public static readonly NSString Key = new NSString("PDSwitchCell");
        public static readonly UINib Nib;

        static PDSwitchCell()
        {
            Nib = UINib.FromName("PDSwitchCell", NSBundle.MainBundle);
        }

        protected PDSwitchCell(IntPtr handle) : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region Protected

        #region IntializationControls

        protected virtual void InitializeControls()
        {
            SetupName(_name);
            SetupSwitch(_switch);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Switch.Title ?? ThemeConfig.ProductDetails.DetailsCell.Title);
        }

        protected virtual void SetupSwitch(UISwitch uiSwitch)
        {
            uiSwitch.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Switch.Switch);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDSwitchCell, ISwitchPciVm>();

            BindName(_name, set);
            BindSwitch(_switch, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDSwitchCell, ISwitchPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindSwitch(UISwitch uiSwitch, MvxFluentBindingDescriptionSet<PDSwitchCell, ISwitchPciVm> set)
        {
            set.Bind(uiSwitch).To(vm => vm.Enabled);
        }

        #endregion

        #endregion
    }
}
