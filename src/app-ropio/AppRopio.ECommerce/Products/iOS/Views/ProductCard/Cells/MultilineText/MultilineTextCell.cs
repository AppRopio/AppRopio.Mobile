using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Multiline;
using CoreGraphics;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.MultilineText
{
    public partial class MultilineTextCell : MvxTableViewCell
    {
        public const float MULTILINE_TEXT_HEIGHT = 52;
        public const float DEFAULT_OFFSET = 16;

        protected ProductsThemeConfig ThemeConfig
        {
            get
            {
                return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig;
            }
        }

        public static readonly NSString Key = new NSString("MultilineTextCell");
        public static readonly UINib Nib;

        static MultilineTextCell()
        {
            Nib = UINib.FromName("MultilineTextCell", NSBundle.MainBundle);
        }

        protected MultilineTextCell(IntPtr handle) 
            : base(handle)
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

            SetupText(_value);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MultiLineText.Title ?? ThemeConfig.ProductDetails.DetailsCell.Title);
        }

        protected virtual void SetupText(UILabel text)
        {
            text.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MultiLineText.Value);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<MultilineTextCell, IMultilinePciVm>();

            BindName(_name, set);

            BindText(_value, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<MultilineTextCell, IMultilinePciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindText(UILabel text, MvxFluentBindingDescriptionSet<MultilineTextCell, IMultilinePciVm> set)
        {
            set.Bind(text).To(vm => vm.Text);
        }

        #endregion

        #endregion

        internal static float GetHeightForContent(string text)
        {
            var label = new AppRopio.Base.iOS.Controls.ARLabel { Frame = new CGRect(16, 52, DeviceInfo.ScreenWidth - 32, 0), Text = text, Lines = 0 };

            label.SetupStyle(Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig.ProductDetails.DetailsCell.MultiLineText.Value);

            label.SizeToFit();

            return (float)(MULTILINE_TEXT_HEIGHT + label.Frame.Height + DEFAULT_OFFSET * 2);
        }
    }
}
