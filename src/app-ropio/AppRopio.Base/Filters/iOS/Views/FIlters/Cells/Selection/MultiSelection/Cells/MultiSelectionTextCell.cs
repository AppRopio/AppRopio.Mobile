using System;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection.Items;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Filters.iOS.Views.FIlters.Cells.Selection.MultiSelection.Cells
{
    public partial class MultiSelectionTextCell : MvxCollectionViewCell
    {
        public const float HORIZONTAL_MARGINS = 30;
        public const float LABEL_HEIGHT = 20;

        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("MultiSelectionTextCell");
        public static readonly UINib Nib;

        static MultiSelectionTextCell()
        {
            Nib = UINib.FromName("MultiSelectionTextCell", NSBundle.MainBundle);
        }

        protected MultiSelectionTextCell(IntPtr handle)
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

            _crossImageView.SetupStyle(ThemeConfig.Filters.FiltersCell.MultiSelection.MultiSelectionCell.Image);
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.Filters.FiltersCell.MultiSelection.MultiSelectionCell.Value);
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
            var set = this.CreateBindingSet<MultiSelectionTextCell, MultiCollectionItemVM>();

            BindName(_name, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<MultiSelectionTextCell, MultiCollectionItemVM> set)
        {
            set.Bind(name).To(vm => vm.ValueName);
        }

        #endregion

        #endregion
    }
}
