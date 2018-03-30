using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Basket.iOS.Services;
using Foundation;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Cells
{
    public partial class SectionHeader : UITableViewHeaderFooterView
    {
        protected iOS.Models.Order ThemeConfig => Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order;
        
        public static readonly NSString Key = new NSString("SectionHeader");
        public static readonly UINib Nib = UINib.FromName("SectionHeader", NSBundle.MainBundle);

        public string Title 
        {
            get { return _titleLabel.Text; }
            set { _titleLabel.Text = value; }
        }

        protected SectionHeader(IntPtr handle) : base(handle)
        {
            
        }

        public override void AwakeFromNib()
        {
            InitializeControls();
        }

        #region InitializationControls

        protected virtual void InitializeControls() 
        {
            SetupSeparators(_bottomSeparatorView);
            SetupTitleLabel(_titleLabel);

            ContentView.BackgroundColor = ThemeConfig.HeaderLabel.Background?.ToUIColor() ?? UIColor.Clear;
        }

        protected virtual void SetupSeparators(UIView bottomSeparatorView)
        {
            bottomSeparatorView.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupTitleLabel(UILabel titleLabel)
        {
            titleLabel.SetupStyle(ThemeConfig.HeaderLabel);
        }

        #endregion
    }
}
