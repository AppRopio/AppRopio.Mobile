using System;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Horizontal.Cells
{
    public partial class HorizontalTextCell : MvxCollectionViewCell
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public const float HORIZONTAL_MARGINS = 16;

        public static readonly NSString Key = new NSString("HorizontalTextCell");
        public static readonly UINib Nib;

        static HorizontalTextCell()
        {
            Nib = UINib.FromName("HorizontalTextCell", NSBundle.MainBundle);
        }

        protected HorizontalTextCell(IntPtr handle)
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
            SetupColorView(_colorView);
            SetupValueName(_valueName);
        }

        protected virtual void SetupColorView(UIView colorView)
        {
            colorView.Layer.CornerRadius = 4;
        }

        protected virtual void SetupValueName(UILabel valueName)
        {
            valueName.SetupStyle(ThemeConfig.Filters.FiltersCell.Collection.Value);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<HorizontalTextCell, CollectionItemVM>();

            BindColorView(_colorView, set);
            BindValueName(_valueName, set);

            set.Apply();
        }

        protected virtual void BindColorView(UIView colorView, MvxFluentBindingDescriptionSet<HorizontalTextCell, CollectionItemVM> set)
        {
            set
                .Bind(colorView)
                .For(v => v.BackgroundColor)
                .To(vm => vm.Selected)
                .WithConversion("TrueFalse", new TrueFalseParameter { True = Theme.ColorPalette.HighlightedControl.ToUIColor(), False = Theme.ColorPalette.DisabledControl.ToUIColor() });
        }

        protected virtual void BindValueName(UILabel valueName, MvxFluentBindingDescriptionSet<HorizontalTextCell, CollectionItemVM> set)
        {
            set.Bind(valueName).To(vm => vm.ValueName);
            set
                .Bind(valueName)
                .For(v => v.Highlighted)
                .To(vm => vm.Selected);
        }

        #endregion

        #endregion
    }
}
