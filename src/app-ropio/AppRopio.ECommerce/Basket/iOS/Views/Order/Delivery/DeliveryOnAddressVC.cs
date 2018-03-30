
using System;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.Basket.Core.Enums;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.iOS.Services;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Cells;
using AppRopio.ECommerce.Basket.iOS.Views.Order.Partial;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Order.Delivery
{
    public partial class DeliveryOnAddressVC : CommonViewController<IDeliveryOnAddressVM>
    {
        private UIBarButtonItem _closeButton;
        private UIButton _accessoryNextButton;

        protected Models.DeliveryOnAddress DeliveryOnAddressTheme { get { return Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.DeliveryInfo.OnAddress; } }
        protected Models.OrderFieldCell CellTheme { get { return DeliveryOnAddressTheme.Cell ?? Mvx.Resolve<IBasketThemeConfigService>().ThemeConfig.Order.BaseOrderFieldCell; } }
        protected OrderViewType OrderViewType { get { return Mvx.Resolve<IBasketConfigService>().Config.OrderViewType; } }

        public DeliveryOnAddressVC() : base("DeliveryOnAddressVC", null)
        {
        }

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            Title = "Адрес";
            RegisterKeyboardActions = true;
            NavigationController.NavigationBarHidden = false;

            if (ViewModel.VmNavigationType == NavigationType.PresentModal)
            {
                _closeButton = new UIBarButtonItem();
                NavigationItem.SetRightBarButtonItem(_closeButton, true);
                SetupCloseButton(_closeButton);
            }

            SetupTabelView(_tableView);
            SetupDeliveryPriceTitle(_deliveryPriceTitle);
            SetupDeliveryPriceLabel(_deliveryPriceLabel);
            SetupNextButton(_nextButton, _accessoryNextButton ?? (_accessoryNextButton = new UIButton()));
            SetupBottomView(_bottomView);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<DeliveryOnAddressVC, IDeliveryOnAddressVM>();

            BindCloseButton(_closeButton, set);
            BindTableView(_tableView, set);
            BindDeliveryPriceLabel(_deliveryPriceLabel, set);
            BindNextButton(_nextButton, _accessoryNextButton, set);

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
            closeButton.SetupStyle(DeliveryOnAddressTheme.CloseButton);
        }

        protected virtual void SetupBottomView(UIView bottomView)
        {
            bottomView.SetupStyle(DeliveryOnAddressTheme.BottomView);
        }

        protected virtual void SetupTabelView(UITableView tableView)
        {
            tableView.TableHeaderView = new UIView();
            tableView.TableFooterView = new UIView();
            tableView.KeyboardDismissMode = UIScrollViewKeyboardDismissMode.Interactive;

            tableView.RowHeight = CellTheme.Size.Height  ?? 50;
            tableView.SeparatorColor = (UIColor)Theme.ColorPalette.Separator;
        }

        protected virtual void SetupDeliveryPriceTitle(UILabel deliveryPriceTitle)
        {
            deliveryPriceTitle.Text = "ДОСТАВКА";
            deliveryPriceTitle.SetupStyle(DeliveryOnAddressTheme.DeliveryPriceLabel);
        }

        protected virtual void SetupDeliveryPriceLabel(UILabel deliveryPriceLabel)
        {
            deliveryPriceLabel.SetupStyle(DeliveryOnAddressTheme.DeliveryPriceLabel);
        }

        protected virtual void SetupNextButton(UIButton nextButton, UIButton accessoryNextButton)
        {
            if (OrderViewType == OrderViewType.Full)
            {
                nextButton.WithTitleForAllStates("ПОДТВЕРДИТЬ");
                accessoryNextButton.WithTitleForAllStates("ПОДТВЕРДИТЬ");
            }

            nextButton.SetupStyle(DeliveryOnAddressTheme.NextButton);

            accessoryNextButton.ChangeFrame(w: DeviceInfo.ScreenWidth, h: 44);
            accessoryNextButton.SetupStyle(DeliveryOnAddressTheme.AccessoryNextButton);
        }

        #endregion

        #region BindingControls

        protected virtual MvxTableViewSource SetupTableViewDataSource(UITableView tableView)
        {
            var source = new OrderFieldsTableSource(tableView);

            source.FieldInputAccessoryView = _accessoryNextButton;

            return source;
        }

        protected virtual void BindCloseButton(UIBarButtonItem closeButton, MvxFluentBindingDescriptionSet<DeliveryOnAddressVC, IDeliveryOnAddressVM> set)
        {
            set.Bind(closeButton).To(vm => vm.CloseCommand);
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<DeliveryOnAddressVC, IDeliveryOnAddressVM> set)
        {
            var dataSource = SetupTableViewDataSource(tableView);

            set.Bind(dataSource).For(s => s.ItemsSource).To(vm => vm.AddressFieldsItems);

            tableView.Source = dataSource;
            tableView.ReloadData();
        }

        protected virtual void BindDeliveryPriceLabel(UILabel deliveryPriceLabel, MvxFluentBindingDescriptionSet<DeliveryOnAddressVC, IDeliveryOnAddressVM> set)
        {
            set.Bind(deliveryPriceLabel).To(vm => vm.DeliveryPrice).WithConversion("StringFormat", "{0:# ### ##0.## \u20BD;;бесплатно}");
        }

        protected virtual void BindNextButton(UIButton nextButton, UIButton accessoryNextButton, MvxFluentBindingDescriptionSet<DeliveryOnAddressVC, IDeliveryOnAddressVM> set)
        {
            if (OrderViewType == OrderViewType.Partial)
            {
                set.Bind(nextButton).For("Title").To(vm => vm.Amount).WithConversion("StringFormat", "Заказать{0: за # ### ##0.## \u20BD;;}");
                set.Bind(accessoryNextButton).For("Title").To(vm => vm.Amount).WithConversion("StringFormat", "Заказать{0: за # ### ##0.## \u20BD;;}");
            }

            set.Bind(nextButton).To(vm => vm.NextCommand);
            set.Bind(nextButton).For(s => s.Enabled).To(vm => vm.CanGoNext);

            set.Bind(accessoryNextButton).To(vm => vm.NextCommand);
            set.Bind(accessoryNextButton).For(s => s.Enabled).To(vm => vm.CanGoNext);
        }

        #endregion
    }
}

