using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;
using AppRopio.Base.Core.ViewModels.Selection.Items;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Selection.Cells
{
    public partial class PDSelectionCell : MvxTableViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("SelectionCell");
        public static readonly UINib Nib;

        static PDSelectionCell()
        {
            Nib = UINib.FromName("SelectionCell", NSBundle.MainBundle);
        }

        protected PDSelectionCell(IntPtr handle)
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
            SetupValueName(_valueName);

            SetupSelectionImage(_selectionImageView);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupValueName(UILabel valueName)
        {
            valueName.SetupStyle(ThemeConfig.ProductDetails.Selection.SelectionCell.Value);
        }

        protected virtual void SetupSelectionImage(UIImageView selectionImageView)
        {
            selectionImageView.Image = ImageCache.GetImage("Images/Filters/Choice.png");
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDSelectionCell, ISelectionItemVM>();

            BindValueName(_valueName, set);

            BindSelectionImage(_selectionImageView, set);

            set.Apply();
        }

        protected virtual void BindValueName(UILabel valueName, MvxFluentBindingDescriptionSet<PDSelectionCell, ISelectionItemVM> set)
        {
            set.Bind(valueName).To(vm => vm.ValueName);
            set.Bind(valueName).For(v => v.Highlighted).To(vm => vm.Selected);
        }

        protected virtual void BindSelectionImage(UIImageView selectionImageView, MvxFluentBindingDescriptionSet<PDSelectionCell, ISelectionItemVM> set)
        {
            set.Bind(selectionImageView).For("Visibility").To(vm => vm.Selected).WithConversion("Visibility");
        }

        #endregion

        #endregion
    }
}
