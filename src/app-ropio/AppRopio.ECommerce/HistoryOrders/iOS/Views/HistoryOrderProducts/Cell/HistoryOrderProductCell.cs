using System;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrderProducts.Items;
using AppRopio.ECommerce.HistoryOrders.iOS.Models;
using AppRopio.ECommerce.HistoryOrders.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.ECommerce.HistoryOrders.Core;

namespace AppRopio.ECommerce.HistoryOrders.iOS.Views.HistoryOrders
{
    public partial class HistoryOrderProductCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("HistoryOrderProductCell");
        public static readonly UINib Nib;

        protected MvxImageViewLoader _imageLoader;

		protected HistoryOrdersThemeConfig ThemeConfig { get { return Mvx.Resolve<IHistoryOrdersThemeConfigService>().ThemeConfig; } }

		static HistoryOrderProductCell()
        {
            Nib = UINib.FromName("HistoryOrderProductCell", NSBundle.MainBundle);
        }

		protected HistoryOrderProductCell(IntPtr handle) : base(handle)
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
            SetupImage(ItemImageView);
            SetupTitle(TitleLabel);
            SetupAmount(AmountLabel);
            SetupPrice(TotalPriceLabel);
            SetupBadge(BadgeLabel);

            this.SetupStyle(ThemeConfig.HistoryOrderItemCell);
		}

        protected virtual void SetupImage(UIImageView itemImageView)
        {
            itemImageView.SetupStyle(ThemeConfig.HistoryOrderItemCell.Image);
        }

        protected virtual void SetupTitle(UILabel title)
		{
            title.SetupStyle(ThemeConfig.HistoryOrderItemCell.Title);
		}

		protected virtual void SetupAmount(UILabel amount)
		{
            amount.SetupStyle(ThemeConfig.HistoryOrderItemCell.Amount);
		}

		protected virtual void SetupPrice(UILabel price)
		{
            price.SetupStyle(ThemeConfig.HistoryOrderItemCell.TotalPrice);
		}

		protected virtual void SetupBadge(UILabel badge)
		{
            badge.SetupStyle(ThemeConfig.HistoryOrderItemCell.Badge);
		}

		#endregion

		#region BindingControls

		protected virtual void BindControls()
		{
			var bindingSet = this.CreateBindingSet<HistoryOrderProductCell, IHistoryOrderProductItemVM>();

            BindTitle(TitleLabel, bindingSet);
            BindAmount(AmountLabel, bindingSet);
            BindPrice(TotalPriceLabel, bindingSet);
            BindImage(ItemImageView, bindingSet);
            BindBadge(BadgeLabel, bindingSet);
            BindIsAvailable(bindingSet);

			bindingSet.Apply();
		}

        protected virtual void BindTitle(UILabel title, MvxFluentBindingDescriptionSet<HistoryOrderProductCell, IHistoryOrderProductItemVM> set)
		{
            set.Bind(title).To(vm => vm.Title);
		}

		protected virtual void BindAmount(UILabel amount, MvxFluentBindingDescriptionSet<HistoryOrderProductCell, IHistoryOrderProductItemVM> set)
		{
            set.Bind(amount).To(vm => vm.Amount).WithConversion("StringFormat", $"{{0}} {Mvx.Resolve<ILocalizationService>().GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderProducts_Pieces")}");
		}

		protected virtual void BindPrice(UILabel price, MvxFluentBindingDescriptionSet<HistoryOrderProductCell, IHistoryOrderProductItemVM> set)
		{
            set.Bind(price).To(vm => vm.TotalPrice).WithConversion("PriceFormat");
		}

		protected virtual void BindImage(UIImageView image, MvxFluentBindingDescriptionSet<HistoryOrderProductCell, IHistoryOrderProductItemVM> set)
		{
			_imageLoader = new MvxImageViewLoader(() => image)
            {
                DefaultImagePath = $"res:{ThemeConfig.HistoryOrderItemCell.Image.Path}",
                ErrorImagePath = $"res:{ThemeConfig.HistoryOrderItemCell.Image.Path}"
            };
            set.Bind(_imageLoader).To(vm => vm.ImageUrl);
		}

		protected virtual void BindIsAvailable(MvxFluentBindingDescriptionSet<HistoryOrderProductCell, IHistoryOrderProductItemVM> set)
		{
            var imageOpacity = new TrueFalseParameter() { True = 1f, False = 0.5f };
            var textColor = new TrueFalseParameter()
            {
                True = ThemeConfig.HistoryOrderItemCell.Title.TextColor.ToUIColor(),
                False = Theme.ColorPalette.TextGray.ToUIColor()
            };

            set.Bind(OverlayView).To(vm => vm.IsAvailable).For(v => v.Hidden);
            set.Bind(TotalPriceLabel).To(vm => vm.IsAvailable).For("Visibility").WithConversion("Visibility");
            set.Bind(ItemImageView).To(vm => vm.IsAvailable).For(v => v.Alpha)
               .WithConversion("TrueFalse", imageOpacity);
            set.Bind(TitleLabel).To(vm => vm.IsAvailable).For(v => v.TextColor)
               .WithConversion("TrueFalse", textColor);
            set.Bind(AmountLabel).To(vm => vm.IsAvailable).For(v => v.TextColor)
               .WithConversion("TrueFalse", textColor);
		}

		protected virtual void BindBadge(UILabel badge, MvxFluentBindingDescriptionSet<HistoryOrderProductCell, IHistoryOrderProductItemVM> set)
		{
            set.Bind(badge).To(vm => vm.Badge);
		}


		#endregion
	}
}
