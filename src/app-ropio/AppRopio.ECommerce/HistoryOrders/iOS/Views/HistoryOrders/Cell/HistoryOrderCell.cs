using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.iOS.Converters;
using AppRopio.ECommerce.HistoryOrders.iOS.Models;
using AppRopio.ECommerce.HistoryOrders.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders
{
    public partial class HistoryOrderCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("HistoryOrderCell");
        public static readonly UINib Nib;

        protected MvxImageViewLoader _imageLoader;

		protected HistoryOrdersThemeConfig ThemeConfig { get { return Mvx.Resolve<IHistoryOrdersThemeConfigService>().ThemeConfig; } }

		static HistoryOrderCell()
        {
            Nib = UINib.FromName("HistoryOrderCell", NSBundle.MainBundle);
        }

        protected HistoryOrderCell(IntPtr handle) : base(handle)
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
            SetupOrderNumber(NumberLabel);
            SetupOrderStatus(OrderStatusLabel);
            SetupPrice(PriceLabel);
            SetupItemsCount(ItemsLabel);
            SetupImage(OrderImageView);
            SetupPaymentStatus(PaymentStatusLabel);

            this.SetupStyle(ThemeConfig.HistoryOrderCell);
		}

        protected virtual void SetupOrderNumber(UILabel number)
        {
            number.SetupStyle(ThemeConfig.HistoryOrderCell.OrderNumber);
        }

        protected virtual void SetupOrderStatus(UILabel status)
		{
            status.SetupStyle(ThemeConfig.HistoryOrderCell.OrderStatus);
		}

        protected virtual void SetupPrice(UILabel price)
		{
            price.SetupStyle(ThemeConfig.HistoryOrderCell.Price);
		}

		protected virtual void SetupItemsCount(UILabel itemsCount)
		{
            itemsCount.SetupStyle(ThemeConfig.HistoryOrderCell.ItemsCount);
        }

        protected virtual void SetupImage(UIImageView image)
		{
            image.SetupStyle(ThemeConfig.HistoryOrderCell.Image);
		}

        protected virtual void SetupPaymentStatus(UILabel paymentStatus)
		{
            paymentStatus.SetupStyle(ThemeConfig.HistoryOrderCell.PaymentStatus);
		}

		#endregion

		#region BindingControls

		protected virtual void BindControls()
		{
            var bindingSet = this.CreateBindingSet<HistoryOrderCell, IHistoryOrderItemVM>();

            BindNumber(NumberLabel, bindingSet);
            BindOrderStatus(OrderStatusLabel, bindingSet);
            BindPrice(PriceLabel, bindingSet);
            BindItems(ItemsView, bindingSet);
            BindItemsCount(ItemsLabel, bindingSet);
            BindOrderImage(OrderImageView, bindingSet);
            BindPaymentStatus(PaymentStatusLabel, bindingSet);
            BindPaymentStatusColor(PaymentStatusView, bindingSet);

			bindingSet.Apply();
		}

        protected virtual void BindNumber(UILabel number, MvxFluentBindingDescriptionSet<HistoryOrderCell, IHistoryOrderItemVM> set)
		{
            set.Bind(number).To(vm => vm.OrderNumber);
		}

		protected virtual void BindOrderStatus(UILabel orderStatus, MvxFluentBindingDescriptionSet<HistoryOrderCell, IHistoryOrderItemVM> set)
		{
            set.Bind(orderStatus).To(vm => vm.OrderStatus);
		}

		protected virtual void BindPrice(UILabel price, MvxFluentBindingDescriptionSet<HistoryOrderCell, IHistoryOrderItemVM> set)
		{
            set.Bind(price).To(vm => vm.TotalPrice).WithConversion("PriceFormat");
		}

        protected virtual void BindItems(UIView items, MvxFluentBindingDescriptionSet<HistoryOrderCell, IHistoryOrderItemVM> set)
		{
            set.Bind(items).To(vm => vm.GoToItemsCommand).For("Tap");
		}

		protected virtual void BindItemsCount(UILabel itemsCount, MvxFluentBindingDescriptionSet<HistoryOrderCell, IHistoryOrderItemVM> set)
		{
            set.Bind(itemsCount).To(vm => vm.ItemsCount).WithConversion("StringFormat", "Состав заказа ({0})");
		}

        protected virtual void BindOrderImage(UIImageView image, MvxFluentBindingDescriptionSet<HistoryOrderCell, IHistoryOrderItemVM> set)
		{
            _imageLoader = new MvxImageViewLoader(() => image)
            {
                DefaultImagePath = $"res:{ThemeConfig.HistoryOrderCell.Image.Path}",
                ErrorImagePath = $"res:{ThemeConfig.HistoryOrderCell.Image.Path}"
            };
            set.Bind(_imageLoader).To(vm => vm.ImageUrl);
		}

		protected virtual void BindPaymentStatus(UILabel paymentStatus, MvxFluentBindingDescriptionSet<HistoryOrderCell, IHistoryOrderItemVM> set)
		{
            set.Bind(paymentStatus).To(vm => vm.PaymentStatus).WithConversion("PaymentStatusToString");
		}

        protected virtual void BindPaymentStatusColor(UIView paymentStatusView, MvxFluentBindingDescriptionSet<HistoryOrderCell, IHistoryOrderItemVM> set)
		{
            var converter = new PaymentStatusToColorConverter()
            {
                PaidColor = "FFD535".ToUIColor(),
                NotPaidColor = "F3F4F9".ToUIColor()
            };

            set.Bind(paymentStatusView).To(vm => vm.PaymentStatus).For(v => v.BackgroundColor).WithConversion(converter);
		}

		#endregion
	}
}