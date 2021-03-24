using System;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery
{
    public partial class DeliveryPointAdditionalInfoVC : CommonViewController<IDeliveryPointAdditionalInfoVM>
    {
        private UIBarButtonItem _closeButton;

        protected Models.DeliveryOnPointMap MapTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.OnPoint.Map; } }
        protected Models.DeliveryPointInfo InfoTheme { get { return MapTheme.Info ?? Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.OnPoint.BaseDeliveryPointInfo; } }

        public DeliveryPointAdditionalInfoVC() : base("DeliveryPointAdditionalInfoVC", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "DeliveryPoint_Details");
            NavigationController.NavigationBarHidden = false;

            if (ViewModel.VmNavigationType == NavigationType.PresentModal)
            {
                _closeButton = new UIBarButtonItem();
                NavigationItem.SetRightBarButtonItem(_closeButton, true);
                SetupCloseButton(_closeButton);
            }

            SetupTitleLabel(_titleLabel);
            SetupAddressLabel(_addressLabel);

            SetupDistanceImageView(_distanceImageView);
            SetupDistanceLabel(_distanceLabel);

            SetupWorkTimeLabel(_workTimeLabel);

            SetupInfoLabel(_infoLabel);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<DeliveryPointAdditionalInfoVC, IDeliveryPointAdditionalInfoVM>();

            BindCloseButton(_closeButton, set);

            BindTitleLabel(_titleLabel, set);
            BindAddressLabel(_addressLabel, set);

            BindDistanceView(_distanceView, set);
            BindDistanceLabel(_distanceLabel, set);

            BindWorkTimeLabel(_workTimeLabel, set);

            BindInfoLabel(_infoLabel, set);

            set.Apply();
        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();
        }

        #endregion

        #region InitializationControls

        /// <summary>
        /// Используется (и отображается) только в модальном варианте экрана
        /// </summary>
        protected virtual void SetupCloseButton(UIBarButtonItem closeButton)
        {
            closeButton.SetupStyle(MapTheme.CloseButton);
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

        protected virtual void SetupWorkTimeLabel(UILabel workTimeLabel)
        {
            workTimeLabel.SetupStyle(InfoTheme.WorkTimeLabel);
        }

        protected virtual void SetupInfoLabel(UILabel infoLabel)
        {
            infoLabel.SetupStyle(InfoTheme.InfoLabel);
        }

        #endregion

        #region BindingControls

        protected virtual void BindCloseButton(UIBarButtonItem closeButton, MvxFluentBindingDescriptionSet<DeliveryPointAdditionalInfoVC, IDeliveryPointAdditionalInfoVM> set)
        {
            set.Bind(closeButton).To(vm => vm.CloseCommand);
        }

        protected virtual void BindTitleLabel(UILabel titleLabel, MvxFluentBindingDescriptionSet<DeliveryPointAdditionalInfoVC, IDeliveryPointAdditionalInfoVM> set)
        {
            set.Bind(titleLabel).To(vm => vm.Item.Name);
            set.Bind(titleLabel).For("AnimatedVisibility").To(vm => vm.Item.Name).WithConversion("Visibility");
        }

        protected virtual void BindAddressLabel(UILabel addressLabel, MvxFluentBindingDescriptionSet<DeliveryPointAdditionalInfoVC, IDeliveryPointAdditionalInfoVM> set)
        {
            set.Bind(addressLabel).To(vm => vm.Item.Address);
            set.Bind(addressLabel).For("AnimatedVisibility").To(vm => vm.Item.Address).WithConversion("Visibility");
        }

        protected virtual void BindDistanceView(UIView distanceView, MvxFluentBindingDescriptionSet<DeliveryPointAdditionalInfoVC, IDeliveryPointAdditionalInfoVM> set)
        {
            set.Bind(distanceView).For("AnimatedVisibility").To(vm => vm.Item.Distance).WithConversion("Visibility");
        }

        protected virtual void BindDistanceLabel(UILabel distanceLabel, MvxFluentBindingDescriptionSet<DeliveryPointAdditionalInfoVC, IDeliveryPointAdditionalInfoVM> set)
        {
            set.Bind(distanceLabel).To(vm => vm.Item.Distance);
        }

        protected virtual void BindWorkTimeLabel(UILabel workTimeLabel, MvxFluentBindingDescriptionSet<DeliveryPointAdditionalInfoVC, IDeliveryPointAdditionalInfoVM> set)
        {
            set.Bind(workTimeLabel).To(vm => vm.Item.WorkTime);
            set.Bind(workTimeLabel).For("AnimatedVisibility").To(vm => vm.Item.WorkTime).WithConversion("Visibility");
        }

        protected virtual void BindInfoLabel(UILabel infoLabel, MvxFluentBindingDescriptionSet<DeliveryPointAdditionalInfoVC, IDeliveryPointAdditionalInfoVM> set)
        {
            set.Bind(infoLabel).To(vm => vm.Item.AdditionalInfo);
            set.Bind(infoLabel).For("AnimatedVisibility").To(vm => vm.Item.AdditionalInfo).WithConversion("Visibility");
        }

        #endregion
    }
}

