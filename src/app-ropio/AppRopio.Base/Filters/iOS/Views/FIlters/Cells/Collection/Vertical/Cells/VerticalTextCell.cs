using System;

using Foundation;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;
using MvvmCross.Binding.BindingContext;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.Filters.iOS.Models;
using MvvmCross;
using AppRopio.Base.Filters.iOS.Services;
using AppRopio.Base.Core.Converters;

namespace AppRopio.Base.Filters.iOS.Views.Filters.Cells.Collection.Vertical.Cells
{
    public partial class VerticalTextCell : MvxCollectionViewCell
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("VerticalTextCell");
        public static readonly UINib Nib;

        public override bool Selected
        {
            get;
            set;
        }

        static VerticalTextCell()
        {
            Nib = UINib.FromName("VerticalTextCell", NSBundle.MainBundle);
        }

        protected VerticalTextCell(IntPtr handle)
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
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.Filters.FiltersCell.Collection.Value);
        }

        protected virtual void SetupSelectedView(UIView selectedView)
        {
            selectedView.Layer.CornerRadius = 4;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<VerticalTextCell, CollectionItemVM>();

            BindName(_name, set);
            BindSelectedView(_selectedView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel valueName, MvxFluentBindingDescriptionSet<VerticalTextCell, CollectionItemVM> set)
        {
            set.Bind(valueName).To(vm => vm.ValueName);
			set
				.Bind(valueName)
				.For(v => v.Highlighted)
				.To(vm => vm.Selected);
        }

        protected virtual void BindSelectedView(UIView selectedView, MvxFluentBindingDescriptionSet<VerticalTextCell, CollectionItemVM> set)
        {
            set.Bind(selectedView).For(v => v.BackgroundColor).To(vm => vm.Selected).WithConversion("TrueFalse", new TrueFalseParameter { True = Theme.ColorPalette.HighlightedControl.ToUIColor(), False = Theme.ColorPalette.DisabledControl.ToUIColor() });
        }

        #endregion

        #endregion
    }
}
