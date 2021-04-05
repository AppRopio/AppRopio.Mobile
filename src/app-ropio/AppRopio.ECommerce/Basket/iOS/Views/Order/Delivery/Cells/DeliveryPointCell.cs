using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery.Items;
using AppRopio.ECommerce.Basket.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery.Cells
{
    public partial class DeliveryPointCell : MvxTableViewCell
    {
        protected Models.DeliveryPointCell CellTheme { get { return Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.OnPoint.List.Cell; } }
        protected Models.DeliveryPointInfo InfoTheme { get { return CellTheme.Info ?? Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.OnPoint.BaseDeliveryPointInfo; } }

        public static readonly NSString Key = new NSString("DeliveryPointCell");
        public static readonly UINib Nib = UINib.FromName("DeliveryPointCell", NSBundle.MainBundle);

        protected DeliveryPointCell(IntPtr handle) : base(handle)
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
            SetupTitleLabel(_titleLabel);
            SetupAddressLabel(_addressLabel);

            SetupDistanceImageView(_distanceImageView);
            SetupDistanceLabel(_distanceLabel);

            SetupCheckImageView(_checkImageView);

            SetupCallButton(_callButton);
            SetupInfoButton(_infoButton);
            SetupMapButton(_mapButton);
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

        protected virtual void SetupMapButton(UIButton mapButton)
        {
            mapButton.SetupStyle(InfoTheme.MapButton);
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
            var set = this.CreateBindingSet<DeliveryPointCell, IDeliveryPointItemVM>();

            BindTitleLabel(_titleLabel, set);
            BindAddressLabel(_addressLabel, set);

            BindDistanceView(_distanceView, set);
            BindDistanceLabel(_distanceLabel, set);

            BindCheckImageView(_checkImageView, set);

            BindCallButton(_callButton, set);
            BindInfoButton(_infoButton, set);
            BindMapButton(_mapButton, set);
            BindRouteButton(_routeButton, set);

            set.Apply();
        }

        protected virtual void BindTitleLabel(UILabel titleLabel, MvxFluentBindingDescriptionSet<DeliveryPointCell, IDeliveryPointItemVM> set)
        {
            set.Bind(titleLabel).To(vm => vm.Name);
            set.Bind(titleLabel).For("AnimatedVisibility").To(vm => vm.Name).WithConversion("Visibility");
        }

        protected virtual void BindAddressLabel(UILabel addressLabel, MvxFluentBindingDescriptionSet<DeliveryPointCell, IDeliveryPointItemVM> set)
        {
            set.Bind(addressLabel).To(vm => vm.Address);
            set.Bind(addressLabel).For("AnimatedVisibility").To(vm => vm.Address).WithConversion("Visibility");
        }

        protected virtual void BindDistanceView(UIView distanceView, MvxFluentBindingDescriptionSet<DeliveryPointCell, IDeliveryPointItemVM> set)
        {
            set.Bind(distanceView).For("AnimatedVisibility").To(vm => vm.Distance).WithConversion("Visibility");
        }

        protected virtual void BindDistanceLabel(UILabel distanceLabel, MvxFluentBindingDescriptionSet<DeliveryPointCell, IDeliveryPointItemVM> set)
        {
            set.Bind(distanceLabel).To(vm => vm.Distance);
        }

        protected virtual void BindCheckImageView(UIImageView checkImageView, MvxFluentBindingDescriptionSet<DeliveryPointCell, IDeliveryPointItemVM> set)
        {
            set.Bind(checkImageView).For("AnimatedVisibility").To(vm => vm.IsSelected).WithConversion("Visibility");
        }

        protected virtual void BindCallButton(UIButton callButton, MvxFluentBindingDescriptionSet<DeliveryPointCell, IDeliveryPointItemVM> set)
        {
            set.Bind(callButton).To(vm => vm.CallCommand);
            set.Bind(callButton).For("AnimatedVisibility").To(vm => vm.Phone).WithConversion("Visibility");
        }

        protected virtual void BindInfoButton(UIButton infoButton, MvxFluentBindingDescriptionSet<DeliveryPointCell, IDeliveryPointItemVM> set)
        {
            set.Bind(infoButton).To(vm => vm.AdditionalInfoCommand);
            set.Bind(infoButton).For("AnimatedVisibility").To(vm => vm.AdditionalInfo).WithConversion("Visibility");
        }

        protected virtual void BindMapButton(UIButton mapButton, MvxFluentBindingDescriptionSet<DeliveryPointCell, IDeliveryPointItemVM> set)
        {
            set.Bind(mapButton).To(vm => vm.MapCommand);
            set.Bind(mapButton).For("AnimatedVisibility").To(vm => vm.Coordinates).WithConversion("Visibility");
        }

        protected virtual void BindRouteButton(UIButton routeButton, MvxFluentBindingDescriptionSet<DeliveryPointCell, IDeliveryPointItemVM> set)
        {
            set.Bind(routeButton).To(vm => vm.RouteCommand);
            set.Bind(routeButton).For("AnimatedVisibility").To(vm => vm.Coordinates).WithConversion("Visibility");
        }

        #endregion
    }
}
