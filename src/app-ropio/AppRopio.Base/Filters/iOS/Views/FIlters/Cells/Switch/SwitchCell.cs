using System;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Switch;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.iOS;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Base.Filters.iOS.Views.FIlters.Cells.Switch
{
    public partial class SwitchCell : MvxTableViewCell
    {
        public const float SWITCH_HEIGHT = 52;

        protected FiltersThemeConfig ThemeConfig
        {
            get
            {
                return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig;
            }
        }

        public static readonly NSString Key = new NSString("SwitchCell");
        public static readonly UINib Nib;

        static SwitchCell()
        {
            Nib = UINib.FromName("SwitchCell", NSBundle.MainBundle);
        }

        protected SwitchCell(IntPtr handle) : base(handle)
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
            SetupSwitch(_switch);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.Filters.FiltersCell.Title);
        }

        protected virtual void SetupSwitch(UISwitch uiSwitch)
        {
            uiSwitch.SetupStyle(ThemeConfig.Filters.FiltersCell.Switch.Switch);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<SwitchCell, ISwitchFiVm>();

            BindName(_name, set);
            BindSwitch(_switch, set);

            set.Apply();
        }

        #endregion

        #endregion

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<SwitchCell, ISwitchFiVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindSwitch(UISwitch uiSwitch, MvxFluentBindingDescriptionSet<SwitchCell, ISwitchFiVm> set)
        {
            set.Bind(uiSwitch).To(vm => vm.Enabled);
        }
    }
}
