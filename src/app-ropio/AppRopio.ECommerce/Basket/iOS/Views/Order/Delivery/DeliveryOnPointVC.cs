using AppRopio.Base.Core;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.Enums;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery.Cells;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery
{
    public partial class DeliveryOnPointVC : CommonViewController<IDeliveryOnPointVM>
    {
        private UIView _navBarBack;
        private UIBarButtonItem _closeButton;
        protected Models.DeliveryOnPointList DeliveryOnPointListTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.OnPoint.List; } }
        protected OrderViewType OrderViewType { get { return Mvx.Resolve<IBasketConfigService>().Config.OrderViewType; } }

        public DeliveryOnPointVC() : base("DeliveryOnPointVC", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "DeliveryPoint_Title");
            NavigationController.NavigationBarHidden = false;

            _navBarBack = new UIView(new CGRect(0, -64, DeviceInfo.ScreenWidth, 64))
            {
                BackgroundColor = Theme.ControlPalette.NavigationBar.BackgroundColor.ToUIColor()
            };

            if (ViewModel.VmNavigationType == NavigationType.PresentModal)
            {
                _closeButton = new UIBarButtonItem();
                NavigationItem.SetRightBarButtonItem(_closeButton, true);
                SetupCloseButton(_closeButton);
            }

            SetupSearchBar(_searchBar);
            SetupTabelView(_tableView);
            SetupNextButton(_nextButton);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<DeliveryOnPointVC, IDeliveryOnPointVM>();

            BindCloseButton(_closeButton, set);
            BindSearchBar(_searchBar, set);
            BindTableView(_tableView, set);
            BindNextButton(_nextButton, set);

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
            closeButton.SetupStyle(DeliveryOnPointListTheme.CloseButton);
        }

        protected virtual void SetupSearchBar(BindableSearchBar searchBar)
        {
            searchBar.SetupStyle(DeliveryOnPointListTheme.SearchBar);
        }

        protected virtual void SetupTabelView(UITableView tableView)
        {
            tableView.TableFooterView = new UIView();
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            tableView.SeparatorColor = (UIColor)Theme.ColorPalette.Separator;

            tableView.RowHeight = UITableView.AutomaticDimension;
            tableView.EstimatedRowHeight = DeliveryOnPointListTheme.Cell.Size.Height ?? 150;

            tableView.ContentInset = new UIEdgeInsets(0, 0, 76, 0);
        }

        protected virtual void SetupNextButton(UIButton nextButton)
        {
            if (OrderViewType == OrderViewType.Full)
                nextButton.SetTitle(LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "DeliveryPoint_Confirm"), UIControlState.Normal);
            nextButton.SetupStyle(DeliveryOnPointListTheme.NextButton);
        }

        #endregion

        #region BindingControls

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            return new MvxSimpleTableViewSource(tableView, DeliveryPointCell.Key, DeliveryPointCell.Key);
        }

        protected virtual void BindCloseButton(UIBarButtonItem closeButton, MvxFluentBindingDescriptionSet<DeliveryOnPointVC, IDeliveryOnPointVM> set)
        {
            set.Bind(closeButton).To(vm => vm.CloseCommand);
        }

        protected virtual void BindSearchBar(BindableSearchBar searchBar, MvxFluentBindingDescriptionSet<DeliveryOnPointVC, IDeliveryOnPointVM> set)
        {
            set.Bind(searchBar).To(vm => vm.SearchText);
            set.Bind(searchBar).For(sb => sb.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(searchBar).For(sb => sb.CancelCommand).To(vm => vm.CancelSearchCommand);
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<DeliveryOnPointVC, IDeliveryOnPointVM> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            set.Bind(dataSource).For(s => s.ItemsSource).To(vm => vm.DeliveryPointsItems);
            set.Bind(dataSource).For(s => s.SelectionChangedCommand).To(vm => vm.DeliveryPointChangedCommand);

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        protected virtual void BindNextButton(UIButton nextButton, MvxFluentBindingDescriptionSet<DeliveryOnPointVC, IDeliveryOnPointVM> set)
        {
            if (OrderViewType == OrderViewType.Partial)
                set.Bind(nextButton).For("Title").To(vm => vm.Amount).WithConversion(
                    "StringFormat",
                    new StringFormatParameter()
                    {
                        StringFormat = (arg) =>
                        {
                            return string.Format(LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "DeliveryPoint_OrderFor"), arg) + $" {AppSettings.SettingsCulture.NumberFormat.CurrencySymbol}";
                        }
                    }
                );
            
            set.Bind(nextButton).To(vm => vm.NextCommand);
            set.Bind(nextButton).For(s => s.Enabled).To(vm => vm.CanGoNext);
        }

        #endregion

        #region Public

        public override void ViewDidAppear(bool animated)
        {
            View.AddSubview(_navBarBack);
            _navBarBack.SendSubviewToBack(View);

            NavigationController.SetTranparentNavBar(false);

            base.ViewDidAppear(animated);
        }

        #endregion
    }
}

