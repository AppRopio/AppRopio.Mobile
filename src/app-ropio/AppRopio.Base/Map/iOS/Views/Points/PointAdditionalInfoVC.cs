using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Views;
using AppRopio.Base.Map.Core.ViewModels.Points;
using AppRopio.Base.Map.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross;
using UIKit;
using AppRopio.Base.Map.Core;

namespace AppRopio.Base.Map.iOS.Views.Points
{
    public partial class PointAdditionalInfoVC : CommonViewController<IPointAdditionalInfoVM>
    {
        private UIBarButtonItem _closeButton;

        protected Models.AdditionalInfo AdditionalInfoTheme { get { return Mvx.Resolve<IMapThemeConfigService>().ThemeConfig.Points.AdditionalInfo; } }
        protected Models.PointInfo InfoTheme { get { return AdditionalInfoTheme.Info ?? Mvx.Resolve<IMapThemeConfigService>().ThemeConfig.Points.BasePointInfo; } }

        public PointAdditionalInfoVC() 
            : base("PointAdditionalInfoVC", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(MapConstants.RESX_NAME, "Details_Title");

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
            var set = this.CreateBindingSet<PointAdditionalInfoVC, IPointAdditionalInfoVM>();

            BindCloseButton(_closeButton, set);

            BindTitleLabel(_titleLabel, set);
            BindAddressLabel(_addressLabel, set);

            BindDistanceView(_distanceView, set);
            BindDistanceLabel(_distanceLabel, set);

            BindWorkTimeLabel(_workTimeLabel, set);

            BindInfoLabel(_infoLabel, set);

            set.Apply();
        }

        #endregion


        #region InitializationControls

        /// <summary>
        /// Используется (и отображается) только в модальном варианте экрана
        /// </summary>
        protected virtual void SetupCloseButton(UIBarButtonItem closeButton)
        {
            closeButton.SetupStyle(AdditionalInfoTheme.CloseButton);
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

        protected virtual void BindCloseButton(UIBarButtonItem closeButton, MvxFluentBindingDescriptionSet<PointAdditionalInfoVC, IPointAdditionalInfoVM> set)
        {
            set.Bind(closeButton).To(vm => vm.CloseCommand);
        }

        protected virtual void BindTitleLabel(UILabel titleLabel, MvxFluentBindingDescriptionSet<PointAdditionalInfoVC, IPointAdditionalInfoVM> set)
        {
            set.Bind(titleLabel).To(vm => vm.Item.Name);
            set.Bind(titleLabel).For("Visibility").To(vm => vm.Item.Name).WithConversion("Visibility");
        }

        protected virtual void BindAddressLabel(UILabel addressLabel, MvxFluentBindingDescriptionSet<PointAdditionalInfoVC, IPointAdditionalInfoVM> set)
        {
            set.Bind(addressLabel).To(vm => vm.Item.Address);
            set.Bind(addressLabel).For("Visibility").To(vm => vm.Item.Address).WithConversion("Visibility");
        }

        protected virtual void BindDistanceView(UIView distanceView, MvxFluentBindingDescriptionSet<PointAdditionalInfoVC, IPointAdditionalInfoVM> set)
        {
            set.Bind(distanceView).For("Visibility").To(vm => vm.Item.Distance).WithConversion("Visibility");
        }

        protected virtual void BindDistanceLabel(UILabel distanceLabel, MvxFluentBindingDescriptionSet<PointAdditionalInfoVC, IPointAdditionalInfoVM> set)
        {
            set.Bind(distanceLabel).To(vm => vm.Item.Distance);
        }

        protected virtual void BindWorkTimeLabel(UILabel workTimeLabel, MvxFluentBindingDescriptionSet<PointAdditionalInfoVC, IPointAdditionalInfoVM> set)
        {
            set.Bind(workTimeLabel).To(vm => vm.Item.WorkTime);
            set.Bind(workTimeLabel).For("Visibility").To(vm => vm.Item.WorkTime).WithConversion("Visibility");
        }

        protected virtual void BindInfoLabel(UILabel infoLabel, MvxFluentBindingDescriptionSet<PointAdditionalInfoVC, IPointAdditionalInfoVM> set)
        {
            set.Bind(infoLabel).To(vm => vm.Item.AdditionalInfo);
            set.Bind(infoLabel).For("Visibility").To(vm => vm.Item.AdditionalInfo).WithConversion("Visibility");
        }

        #endregion
    }
}

