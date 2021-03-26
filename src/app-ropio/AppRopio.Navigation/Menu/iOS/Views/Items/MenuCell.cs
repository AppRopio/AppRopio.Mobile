using System;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Navigation.Menu.Core.ViewModels.Items;
using AppRopio.Navigation.Menu.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS.ValueConverters;
using System.Globalization;
using FFImageLoading.Cross;

namespace AppRopio.Navigation.Menu.iOS.Views
{
    public partial class MenuCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("MenuCell");
        public static readonly UINib Nib = UINib.FromName("MenuCell", NSBundle.MainBundle);

        protected Models.MenuCell ThemeConfig { get; set; }

        protected MenuCell(IntPtr handle)
            : base(handle)
        {
            ThemeConfig = Mvx.IoCProvider.Resolve<IMenuThemeConfigService>().ThemeConfig.LeftViewController.MenuTable.MenuCell;

            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupIcon(_icon);
            SetupTitle(_title, ThemeConfig.Name);
            SetupBadge(_badge, _badgeView, ThemeConfig.Badge);

            this.SetupStyle(ThemeConfig);
        }

        protected virtual void SetupIcon(UIImageView icon)
        {

        }

        protected virtual void SetupTitle(UILabel title, Label model)
        {
            title.SetupStyle(model);
        }

        protected virtual void SetupBadge(UILabel badge, UIView badgeView, Label model)
        {
            badge.SetupStyle(model);
            badgeView.BackgroundColor = badge.BackgroundColor;
            badgeView.Layer.MasksToBounds = true;
            badgeView.Layer.CornerRadius = badgeView.Frame.Height / 2;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<MenuCell, MenuItemVM>();

            BindIcon(_icon, _stackViewLeftConstraint, set);
            BindTitle(_title, set);
            BindBadge(_badge, _badgeView, set);

            set.Apply();
        }

        protected virtual void BindIcon(UIImageView icon, NSLayoutConstraint stackViewLeftConstraint, MvxFluentBindingDescriptionSet<MenuCell, MenuItemVM> set)
        {
            if (icon is MvxCachedImageView imageView)
            {
                imageView.OnFinish += (sender, ev) =>
                {
                    icon.Image = (UIKit.UIImage)new ColorMaskValueConverter().Convert(icon, typeof(UIImageView), ThemeConfig.Name.TextColor.ToUIColor(), CultureInfo.CurrentUICulture);
                };
                set.Bind(imageView).For(i => i.ImagePath).To(vm => vm.Icon);
            }
            else
            {
                icon.Image = (UIKit.UIImage)new ColorMaskValueConverter().Convert(icon, typeof(UIImageView), ThemeConfig.Name.TextColor.ToUIColor(), CultureInfo.CurrentUICulture);
            }

            set.Bind(icon).For("Visibility").To(vm => vm.Icon).WithConversion("Visibility");
            set.Bind(stackViewLeftConstraint).For(c => c.Constant).To(vm => vm.HasIcon).WithConversion("TrueFalse", new TrueFalseParameter { True = (nfloat)0.0f, False = (nfloat)16.0f });
        }

        protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<MenuCell, MenuItemVM> set)
        {
            set.Bind(title).To(vm => vm.Title);
        }

        protected virtual void BindBadge(UILabel badge, UIView badgeView, MvxFluentBindingDescriptionSet<MenuCell, MenuItemVM> set)
        {
            set.Bind(badge).To(vm => vm.BadgeCount);
            set.Bind(badge).For("Visibility").To(vm => vm.BadgeEnabled).WithConversion("Visibility");
            set.Bind(badgeView).For("Visibility").To(vm => vm.BadgeEnabled).WithConversion("Visibility");
        }

        #endregion

        #region Public

        public void OverrideExistingTheme(Models.MenuCell themeModel)
        {
            ThemeConfig = themeModel;
            
            SetupTitle(_title, ThemeConfig.Name);
            SetupBadge(_badge, _badgeView, ThemeConfig.Badge);

            this.SetupStyle(ThemeConfig);
        }

        #endregion
    }
}
