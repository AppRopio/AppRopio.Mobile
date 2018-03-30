using System;

using Foundation;
using MvvmCross.Binding.iOS.Views;
using UIKit;
using MvvmCross.Binding.BindingContext;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.OneSelection;
using AppRopio.Base.Filters.iOS.Models;
using MvvmCross.Platform;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Selection.OneSelection
{
    public partial class OneSelectionCell : MvxTableViewCell
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public const float ONE_SELECTION_HEIGHT = 52;
        public const float ONE_SELECTION_CONTENT_HEIGHT = 20;

        public static readonly NSString Key = new NSString("OneSelectionCell");
        public static readonly UINib Nib;

        static OneSelectionCell()
        {
            Nib = UINib.FromName("OneSelectionCell", NSBundle.MainBundle);
        }

        protected OneSelectionCell(IntPtr handle)
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
            name.SetupStyle(ThemeConfig.Filters.FiltersCell.Title);
        }

        protected virtual void SetupValueName(UILabel valueName)
        {
            valueName.SetupStyle(ThemeConfig.Filters.FiltersCell.OneSelection.Value);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<OneSelectionCell, IOneSelectionFiVm>();

            BindName(_name, set);

            BindValueName(_valueName, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<OneSelectionCell, IOneSelectionFiVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindValueName(UILabel valueName, MvxFluentBindingDescriptionSet<OneSelectionCell, IOneSelectionFiVm> set)
        {
            set.Bind(valueName).To(vm => vm.ValueName);
            set.Bind(valueName).For("Visibility").To(vm => vm.ValueName).WithConversion("Visibility");
        }

        #endregion

        #endregion
    }
}
