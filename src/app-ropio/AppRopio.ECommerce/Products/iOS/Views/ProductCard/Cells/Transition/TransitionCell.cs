using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Transition;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Transition
{
    public partial class TransitionCell : MvxTableViewCell
    {
        public const float TRANSITION_HEIGHT = 52;

        protected ProductsThemeConfig ThemeConfig => Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig;

        public static readonly NSString Key = new NSString("TransitionCell");
        public static readonly UINib Nib;

        static TransitionCell()
        {
            Nib = UINib.FromName("TransitionCell", NSBundle.MainBundle);
        }

        protected TransitionCell(IntPtr handle)
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
            SetupValue(_value);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();

            _accessoryImageView.Image = ImageCache.GetImage("Images/Main/accessory_arrow.png");
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Transition.Title ?? ThemeConfig.ProductDetails.DetailsCell.Title);
        }

        protected virtual void SetupValue(UILabel value)
        {
            value.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Transition.Value);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<TransitionCell, ITransitionPciVm>();

            BindName(_name, set);
            BindValue(_value, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<TransitionCell, ITransitionPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindValue(UILabel label, MvxFluentBindingDescriptionSet<TransitionCell, ITransitionPciVm> set)
        {
            set.Bind(label).To(vm => vm.Value);
        }

        #endregion

        #endregion
    }
}
