using System;
using AppRopio.Base.Core.ViewModels.Selection.Items;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Selection.Cells
{
    public partial class SelectionCell : MvxTableViewCell
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("SelectionCell");
        public static readonly UINib Nib;

        static SelectionCell()
        {
            Nib = UINib.FromName("SelectionCell", NSBundle.MainBundle);
        }

        protected SelectionCell(IntPtr handle)
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
            valueName.SetupStyle(ThemeConfig.Filters.Selection.SelectionCell.Value);
        }

        protected virtual void SetupSelectionImage(UIImageView selectionImageView)
        {
            selectionImageView.Image = ImageCache.GetImage("Images/Filters/Choice.png");
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<SelectionCell, ISelectionItemVM>();

            BindValueName(_valueName, set);

            BindSelectionImage(_selectionImageView, set);

            set.Apply();
        }

        protected virtual void BindValueName(UILabel valueName, MvxFluentBindingDescriptionSet<SelectionCell, ISelectionItemVM> set)
        {
            set.Bind(valueName).To(vm => vm.ValueName);
            set.Bind(valueName).For(v => v.Highlighted).To(vm => vm.Selected);
        }

        protected virtual void BindSelectionImage(UIImageView selectionImageView, MvxFluentBindingDescriptionSet<SelectionCell, ISelectionItemVM> set)
        {
            set.Bind(selectionImageView).For("Visibility").To(vm => vm.Selected).WithConversion("Visibility");
        }

        #endregion

        #endregion
    }
}
