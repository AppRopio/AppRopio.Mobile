using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.Settings.Core.ViewModels.Regions.Items;
using AppRopio.Base.Settings.iOS.Models;
using AppRopio.Base.Settings.iOS.Services;
using AppRopio.Models.Settings.Responses;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.Base.Settings.iOS.Views.Regions.Cell
{
    public partial class RegionCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("RegionCell");
        public static readonly UINib Nib;

		protected SettingsThemeConfig ThemeConfig { get { return Mvx.Resolve<ISettingsThemeConfigService>().ThemeConfig; } }

		static RegionCell()
        {
            Nib = UINib.FromName("RegionCell", NSBundle.MainBundle);
        }

		protected RegionCell(IntPtr handle) : base(handle)
        {
			this.DelayBind(() =>
			{
				InitializeControls();
				BindControls();
			});
		}

		#region IntializationControls

		protected virtual void InitializeControls()
		{
            SetupTitle(TitleLabel);
            SetupSelectionImage(SelectionImageView);

            this.SetupStyle(ThemeConfig.Regions.RegionCell);
		}

		protected virtual void SetupTitle(UILabel title)
		{
            title.SetupStyle(ThemeConfig.Regions.RegionCell.Title);
		}

		protected virtual void SetupSelectionImage(UIImageView selectionImageView)
		{
			selectionImageView.Image = ImageCache.GetImage("Images/Settings/Selection.png");
		}

		#endregion

		#region BindingControls

		protected virtual void BindControls()
		{
			var bindingSet = this.CreateBindingSet<RegionCell, IRegionItemVm>();

            BindTitle(TitleLabel, bindingSet);

            BindSelectionImage(SelectionImageView, bindingSet);

			bindingSet.Apply();
		}

		protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<RegionCell, IRegionItemVm> set)
		{
            set.Bind(title).To(vm => vm.Title);
		}

        protected virtual void BindSelectionImage(UIImageView selectionImageView, MvxFluentBindingDescriptionSet<RegionCell, IRegionItemVm> set)
		{
			set.Bind(selectionImageView).For("Visibility").To(vm => vm.Selected).WithConversion("Visibility");
		}

		#endregion
	}
}