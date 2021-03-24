using System;
using AppRopio.Base.iOS;
using AppRopio.Base.Filters.Core.ViewModels.Sort.Items;
using AppRopio.Base.Filters.iOS.Models;
using AppRopio.Base.Filters.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Base.Filters.iOS.Views.Sort.Cells
{
    public partial class SortCell : MvxTableViewCell
    {
        protected FiltersThemeConfig ThemeConfig { get { return Mvx.Resolve<IFiltersThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("SortCell");
        public static readonly UINib Nib = UINib.FromName("SortCell", NSBundle.MainBundle);

        protected SortCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupName(_name);

            SetupSelectionImage(_selectionImageView);

            this.SetupStyle(ThemeConfig.SortTypes.SortCell);
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.SortTypes.SortCell.Title);
        }

        protected virtual void SetupSelectionImage(UIImageView selectionImageView)
        {
            selectionImageView.Image = ImageCache.GetImage("Images/Filters/Choice.png");
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<SortCell, ISortItemVM>();

            BindName(_name, set);

            BindSelectionImage(_selectionImageView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<SortCell, ISortItemVM> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindSelectionImage(UIImageView selectionImageView, MvxFluentBindingDescriptionSet<SortCell, ISortItemVM> set)
        {
            set.Bind(selectionImageView).For("Visibility").To(vm => vm.Selected).WithConversion("Visibility");
        }

        #endregion
    }
}
