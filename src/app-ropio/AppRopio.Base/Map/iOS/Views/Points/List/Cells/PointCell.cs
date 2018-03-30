using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.Map.Core.ViewModels.Points.List.Items;
using AppRopio.Base.Map.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.Base.Map.iOS.Views.Points.List.Cells
{
    public partial class PointCell : MvxTableViewCell
    {
        protected Models.PointCell CellTheme { get { return Mvx.Resolve<IMapThemeConfigService>().ThemeConfig.Points.List.Cell; } }
        protected Models.PointInfo InfoTheme { get { return CellTheme.Info ?? Mvx.Resolve<IMapThemeConfigService>().ThemeConfig.Points.BasePointInfo; } }

        public static readonly NSString Key = new NSString("PointCell");
        public static readonly UINib Nib = UINib.FromName("PointCell", NSBundle.MainBundle);

        protected PointCell(IntPtr handle) : base(handle)
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
            SelectedBackgroundView = new UIView(Bounds)
                .WithBackground(Theme.ColorPalette.Accent.ToUIColor().ColorWithAlpha(0.2f));
            
            SetupTitleLabel(_titleLabel);
            SetupAddressLabel(_addressLabel);

            SetupDistanceImageView(_distanceImageView);
            SetupDistanceLabel(_distanceLabel);

            //SetupCheckImageView(_checkImageView); //TODO подумать о вынесении в настройки

            SetupCallButton(_callButton);
            SetupInfoButton(_infoButton);
            SetupRouteButton(_routeButton);

            this.SetupStyle(CellTheme);
        }

        protected virtual void SetupTitleLabel(UILabel titleLabel)
        {
            titleLabel.SetupStyle(InfoTheme.TitleLabel);
        }

        protected virtual void SetupAddressLabel(UILabel addressLabel)
        {
            addressLabel.SetupStyle(InfoTheme.AddressLabel);
        }

        protected virtual void SetupDistanceImageView(UIImageView distanceImageView)
        {
            distanceImageView.SetupStyle(InfoTheme.DistanceImage);
        }

        protected virtual void SetupDistanceLabel(UILabel distanceLabel)
        {
            distanceLabel.SetupStyle(InfoTheme.DistanceLabel);
        }

        protected virtual void SetupCheckImageView(UIImageView checkImageView)
        {
            checkImageView.SetupStyle(CellTheme.CheckImage);
        }

        protected virtual void SetupCallButton(UIButton callButton)
        {
            callButton.SetupStyle(InfoTheme.CallButton);
        }

        protected virtual void SetupInfoButton(UIButton infoButton)
        {
            infoButton.SetupStyle(InfoTheme.InfoButton);
        }

        protected virtual void SetupRouteButton(UIButton routeButton)
        {
            routeButton.SetupStyle(InfoTheme.RouteButton);
            routeButton.Hidden = true;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PointCell, IPointItemVM>();

            BindTitleLabel(_titleLabel, set);
            BindAddressLabel(_addressLabel, set);

            BindDistanceView(_distanceView, set);
            BindDistanceLabel(_distanceLabel, set);

            //BindCheckImageView(_checkImageView, set); //TODO подумать о вынесении в настройки

            BindCallButton(_callButton, set);
            BindInfoButton(_infoButton, set);
            BindRouteButton(_routeButton, set);

            set.Apply();
        }

        protected virtual void BindTitleLabel(UILabel titleLabel, MvxFluentBindingDescriptionSet<PointCell, IPointItemVM> set)
        {
            set.Bind(titleLabel).To(vm => vm.Name);
            set.Bind(titleLabel).For("Visibility").To(vm => vm.Name).WithConversion("Visibility");
        }

        protected virtual void BindAddressLabel(UILabel addressLabel, MvxFluentBindingDescriptionSet<PointCell, IPointItemVM> set)
        {
            set.Bind(addressLabel).To(vm => vm.Address);
            set.Bind(addressLabel).For("Visibility").To(vm => vm.Address).WithConversion("Visibility");
        }

        protected virtual void BindDistanceView(UIView distanceView, MvxFluentBindingDescriptionSet<PointCell, IPointItemVM> set)
        {
            set.Bind(distanceView).For("Visibility").To(vm => vm.Distance).WithConversion("Visibility");
        }

        protected virtual void BindDistanceLabel(UILabel distanceLabel, MvxFluentBindingDescriptionSet<PointCell, IPointItemVM> set)
        {
            set.Bind(distanceLabel).To(vm => vm.Distance);
        }

        protected virtual void BindCheckImageView(UIImageView checkImageView, MvxFluentBindingDescriptionSet<PointCell, IPointItemVM> set)
        {
            set.Bind(checkImageView).For("Visibility").To(vm => vm.IsSelected).WithConversion("Visibility");
        }

        protected virtual void BindCallButton(UIButton callButton, MvxFluentBindingDescriptionSet<PointCell, IPointItemVM> set)
        {
            set.Bind(callButton).To(vm => vm.CallCommand);
            set.Bind(callButton).For("Visibility").To(vm => vm.Phone).WithConversion("Visibility");
        }

        protected virtual void BindInfoButton(UIButton infoButton, MvxFluentBindingDescriptionSet<PointCell, IPointItemVM> set)
        {
            set.Bind(infoButton).To(vm => vm.AdditionalInfoCommand);
            set.Bind(infoButton).For("Visibility").To(vm => vm.AdditionalInfo).WithConversion("Visibility");
        }

        protected virtual void BindRouteButton(UIButton routeButton, MvxFluentBindingDescriptionSet<PointCell, IPointItemVM> set)
        {
            set.Bind(routeButton).To(vm => vm.RouteCommand);
            set.Bind(routeButton).For("Visibility").To(vm => vm.Coordinates).WithConversion("Visibility");
        }

        #endregion
    }
}
