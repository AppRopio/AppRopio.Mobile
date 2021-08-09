using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.MultiSelection.Cells
{
    public partial class PDMultiSelectionTextCell : MvxCollectionViewCell
    {
        public const float HORIZONTAL_MARGINS = 30;
        public const float LABEL_HEIGHT = 20;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("PDMultiSelectionTextCell");
        public static readonly UINib Nib;

        static PDMultiSelectionTextCell()
        {
            Nib = UINib.FromName("PDMultiSelectionTextCell", NSBundle.MainBundle);
        }

        protected PDMultiSelectionTextCell(IntPtr handle)
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

            _crossImageView.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MultiSelection.MultiSelectionCell.Image);
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MultiSelection.MultiSelectionCell.Value);
            name.Alpha = 0.5f;
        }

        protected virtual void SetupSelectedView(UIView selectedView)
        {
            selectedView.Layer.CornerRadius = 4;
            selectedView.BackgroundColor = Theme.ColorPalette.DisabledControl.ToUIColor();
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDMultiSelectionTextCell, MultiCollectionItemVM>();

            BindName(_name, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDMultiSelectionTextCell, MultiCollectionItemVM> set)
        {
            set.Bind(name).To(vm => vm.ValueName);
        }

        #endregion

        #endregion
    }
}
