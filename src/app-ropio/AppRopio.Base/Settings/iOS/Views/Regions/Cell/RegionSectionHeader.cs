using System;
using AppRopio.Base.iOS;
using AppRopio.Base.Settings.iOS.Models;
using AppRopio.Base.Settings.iOS.Services;
using Foundation;
using MvvmCross.Platform;
using UIKit;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.Base.Settings.iOS.Views.Regions.Cell
{
    public partial class RegionSectionHeader : UITableViewHeaderFooterView
    {
        public static readonly NSString Key = new NSString("RegionSectionHeader");
        public static readonly UINib Nib;

		protected SettingsThemeConfig ThemeConfig { get { return Mvx.Resolve<ISettingsThemeConfigService>().ThemeConfig; } }

		static RegionSectionHeader()
        {
            Nib = UINib.FromName("RegionSectionHeader", NSBundle.MainBundle);
        }

        protected RegionSectionHeader(IntPtr handle) 
            : base(handle)
        {
            
        }

        public override void SetNeedsDisplay()
        {
            base.SetNeedsDisplay();

            InitializeControls();
        }
		
		protected virtual void InitializeControls()
		{
            SetupTitle(TextLabel);
		}

		protected virtual void SetupTitle(UILabel title)
		{
            title.SetupStyle(ThemeConfig.Regions.RegionHeaderCell.Title);
		}
    }
}
