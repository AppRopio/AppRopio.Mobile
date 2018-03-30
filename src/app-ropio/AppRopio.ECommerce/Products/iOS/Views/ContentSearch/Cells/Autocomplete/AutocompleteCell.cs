using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch.Items.Autocomplete;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.ECommerce.Products.iOS.Views.ContentSearch.Cells.Autocomplete
{
    public partial class AutocompleteCell : MvxCollectionViewCell
    {
        public const float HORIZONTAL_MARGINS = 30;
        public const float CONTENT_HEIGHT = 30;

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("AutocompleteCell");
        public static readonly UINib Nib;

        static AutocompleteCell()
        {
            Nib = UINib.FromName("AutocompleteCell", NSBundle.MainBundle);
        }

        protected AutocompleteCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>

            {
                InitializeControls();
                BindControls();
            });
        }

        #region Protected

        #region InitializaitonControls

        protected virtual void InitializeControls()
        {
            SetupTitle(_title);
            SetupBackgroundView(_backgroundView);
        }

        protected virtual void SetupTitle(UILabel title)
        {
            title.SetupStyle(ThemeConfig.ContentSearch.AutocompeleteCell.Title);
        }

        protected virtual void SetupBackgroundView(UIView backgroundView)
        {
            //backgroundView.BackgroundColor = Theme.ColorPalette.DisabledControl.ToUIColor();
            //backgroundView.Layer.CornerRadius = 15;
            backgroundView.SetupStyle(ThemeConfig.ContentSearch.AutocompeleteCell);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<AutocompleteCell, IAutocompleteItemVM>();

            BindTitle(_title, set);

            set.Apply();
        }

        protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<AutocompleteCell, IAutocompleteItemVM> set)
        {
            set.Bind(title).To(vm => vm.AutocompleteText);
        }

        #endregion

        #endregion
    }
}
