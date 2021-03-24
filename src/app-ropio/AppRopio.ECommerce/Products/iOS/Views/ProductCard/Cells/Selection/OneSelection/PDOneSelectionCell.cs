using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.OneSelection;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.OneSelection
{
    public partial class PDOneSelectionCell : MvxTableViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public const float ONE_SELECTION_HEIGHT = 52;
        public const float ONE_SELECTION_CONTENT_HEIGHT = 20;

        public static readonly NSString Key = new NSString("PDOneSelectionCell");
        public static readonly UINib Nib;

        static PDOneSelectionCell()
        {
            Nib = UINib.FromName("PDOneSelectionCell", NSBundle.MainBundle);
        }

        protected PDOneSelectionCell(IntPtr handle)
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

            SetupValueName(_valueName);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();

            _accessoryImageView.Image = ImageCache.GetImage("Images/Main/accessory_arrow.png");
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.OneSelection.Title ?? ThemeConfig.ProductDetails.DetailsCell.Title);
        }

        protected virtual void SetupValueName(UILabel valueName)
        {
            valueName.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.OneSelection.Value);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDOneSelectionCell, IOneSelectionPciVm>();

            BindName(_name, set);

            BindValueName(_valueName, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDOneSelectionCell, IOneSelectionPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindValueName(UILabel valueName, MvxFluentBindingDescriptionSet<PDOneSelectionCell, IOneSelectionPciVm> set)
        {
            set.Bind(valueName).To(vm => vm.ValueName);
            set.Bind(valueName).For("Visibility").To(vm => vm.ValueName).WithConversion("Visibility");
        }

        #endregion

        #endregion
    }
}
