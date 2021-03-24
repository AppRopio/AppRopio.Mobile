using System;
using AppRopio.Base.iOS.Views;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.iOS.Converters;
using AppRopio.ECommerce.HistoryOrders.iOS.Models;
using AppRopio.ECommerce.HistoryOrders.iOS.Services;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.iOS;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.ECommerce.HistoryOrders.Core;
using AppRopio.Base.iOS.UIExtentions;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders
{
    public partial class HistoryOrderDetailsViewController : CommonViewController<IHistoryOrderDetailsViewModel>
    {
        public float OrderStatusTableHeight
        {
            get { return (float)OrderStatusHeight.Constant; }
            set 
            {
                OrderStatusHeight.Constant = value;
                OrderStatusTableView.SetNeedsUpdateConstraints();
            }
        }

		protected HistoryOrdersThemeConfig ThemeConfig { get { return Mvx.Resolve<IHistoryOrdersThemeConfigService>().ThemeConfig; } }

        public HistoryOrderDetailsViewController() 
            : base("HistoryOrderDetailsViewController", null)
        {
        }

		#region CommonViewController implementation

		protected override void InitializeControls()
		{
            Title = LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderDetails_Title");

            SetupItemsLabel(ItemsLabel);

            SetupInfoLabel(DeliveryNameLabel);
            SetupInfoLabel(DeliveryPriceLabel);

            SetupDeliveryPointLabel(DeliveryPointNameLabel);
            PaymentHintLabel.Text = LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderDetails_Delivery");
            SetupDeliveryAddressLabel(DeliveryPointAddressLabel);

            SetupInfoLabel(PaymentHintLabel);
            PaymentHintLabel.Text = LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderDetails_PaymentType");
            SetupInfoLabel(PaymentNameLabel);

            SetupInfoLabel(AmountHintLabel);
            AmountHintLabel.Text = LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderDetails_Amount");
            SetupInfoLabel(AmountLabel);

            SetupOrderStatusTableView(OrderStatusTableView);
            SetupRepeatButton(RepeatButton);
		}

        protected virtual void SetupDeliveryPointLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.DeliveryPointLabel);
        }

        protected virtual void SetupDeliveryAddressLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.DeliveryAddressLabel);
        }

        protected virtual void SetupItemsLabel(UILabel itemsLabel)
        {
            itemsLabel.SetupStyle(ThemeConfig.ItemsLabel);
        }

        protected virtual void SetupInfoLabel(UILabel label)
        {
            label.SetupStyle(ThemeConfig.InfoLabel);
        }

        protected virtual void SetupRepeatButton(UIButton repeatButton)
        {
            repeatButton.SetupStyle(ThemeConfig.RepeatButton);
            repeatButton.WithTitleForAllStates(LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderDetails_RepeatOrder"));
        }

        protected virtual void SetupOrderStatusTableView(UITableView tableView)
		{
            tableView.RegisterNibForCellReuse(HistoryOrderStatusCell.Nib, HistoryOrderStatusCell.Key);

            tableView.RowHeight = (nfloat)ThemeConfig.HistoryOrderStatusCell.Size.Height;
			tableView.TableFooterView = new UIView();
		}

		protected override void BindControls()
		{
			var bindingSet = this.CreateBindingSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel>();

            BindTitle(bindingSet);
            BindContent(ContentView, bindingSet);

            BindItems(ItemsView, bindingSet);
            BindItemsCount(ItemsLabel, bindingSet);
            BindOrderStatus(bindingSet);
            BindDeliveryPointName(DeliveryPointNameLabel, bindingSet);
            BindDeliveryPointAddress(DeliveryPointAddressLabel, bindingSet);
            BindDeliveryName(DeliveryNameLabel, bindingSet);
            BindDeliveryPrice(DeliveryPriceLabel, bindingSet);
            BindPaymentName(PaymentNameLabel, bindingSet);
            BindAmount(AmountLabel, bindingSet);
            BindRepeatButton(RepeatButton, bindingSet);

			bindingSet.Apply();
		}

        protected virtual void BindTitle(MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(this).To(vm => vm.OrderNumber).For(v => v.Title).WithConversion("StringFormat", $"{LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderDetails_Title")} {{0}}");
        }

		protected virtual void BindContent(UIView content, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(content).To(vm => vm.Loading).For(v => v.Hidden);
		}

		protected virtual void BindItems(UIView items, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
			set.Bind(items).To(vm => vm.GoToItemsCommand).For("Tap");
		}

		protected virtual void BindItemsCount(UILabel items, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(items).To(vm => vm.ItemsCount).WithConversion("StringFormat", $"{LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderDetails_OrderList")} ({{0}})");
            set.Bind(items).To(vm => vm.GoToItemsCommand).For("Tap");
		}

        protected virtual void BindOrderStatus(MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            var dataSource = new MvxStandardTableViewSource(OrderStatusTableView, HistoryOrderStatusCell.Key);

            OrderStatusTableView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.OrderStatus);

			OrderStatusTableView.ReloadData();

            set.Bind(this).To(vm => vm.OrderStatus.Count).For(v => v.OrderStatusTableHeight)
               .WithConversion(new MultiplyValueConverter(), OrderStatusTableView.RowHeight);
		}

		protected virtual void BindDeliveryPointName(UILabel deliveryPointName, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(deliveryPointName).To(vm => vm.DeliveryPointName);
		}

		protected virtual void BindDeliveryPointAddress(UILabel deliveryPointAddress, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(deliveryPointAddress).To(vm => vm.DeliveryPointAddress);
		}

		protected virtual void BindDeliveryName(UILabel deliveryName, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(deliveryName).To(vm => vm.DeliveryName);
		}

		protected virtual void BindDeliveryPrice(UILabel deliveryPrice, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(deliveryPrice).To(vm => vm.DeliveryPrice).WithConversion("PriceFormat");
		}

		protected virtual void BindPaymentName(UILabel paymentName, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(paymentName).To(vm => vm.PaymentName);
		}

		protected virtual void BindAmount(UILabel amount, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(amount).To(vm => vm.TotalPrice).WithConversion("PriceFormat");
		}

		protected virtual void BindRepeatButton(UIButton repeatButton, MvxFluentBindingDescriptionSet<HistoryOrderDetailsViewController, IHistoryOrderDetailsViewModel> set)
		{
            set.Bind(repeatButton).To(vm => vm.RepeatOrderCommand);
		}

		#endregion
	}
}

