using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items;
using AppRopio.ECommerce.Basket.iOS.Services;
using FFImageLoading.Cross;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace AppRopio.ECommerce.Basket.iOS.Views.Basket.Cells
{
	public partial class BasketCell : MvxTableViewCell
    {
        protected Models.BasketCell CellTheme { get { return Mvx.IoCProvider.Resolve<IBasketThemeConfigService>().ThemeConfig.Basket.Cell; } }
        
        public static readonly NSString Key = new NSString("BasketCell");
        public static readonly UINib Nib = UINib.FromName("BasketCell", NSBundle.MainBundle);

        protected BasketCell(IntPtr handle)
            : base(handle)
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
            SetupImageView(_imageView);
            SetupTitleLabel(_titleLabel);
            SetupPriceLabel(_priceLabel);
            SetupQuantityLabel(_quantityLabel);
            SetupQuantityActivityIndicator(_quantityActivityIndicator);
            SetupIncButton(_incButton);
            SetupDecButton(_decButton);

            this.SetupStyle(CellTheme);
        }

        protected virtual void SetupImageView(UIImageView imageView)
        {
            
        }

        protected virtual void SetupTitleLabel(UILabel titleLabel)
        {
            titleLabel.SetupStyle(CellTheme.NameLabel);
        }

        protected virtual void SetupPriceLabel(UILabel priceLabel)
        {
            priceLabel.SetupStyle(CellTheme.PriceLabel);
        }

        protected virtual void SetupQuantityLabel(UILabel quantityLabel)
        {
            quantityLabel.SetupStyle(CellTheme.QuantityLabel);
        }

        protected virtual void SetupQuantityActivityIndicator(UIActivityIndicatorView quantityActivityIndicator)
        {
            quantityActivityIndicator.Color = (UIColor)Theme.ColorPalette.Accent;
        }

        protected virtual void SetupIncButton(UIButton incButton)
        {
            incButton.SetupStyle(CellTheme.IncButton);
        }

        protected virtual void SetupDecButton(UIButton decButton)
        {
            decButton.SetupStyle(CellTheme.DecButton);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<BasketCell, IBasketItemVM>();

            BindImageView(_imageView, set);
            BindTitleLabel(_titleLabel, set);
            BindPriceLabel(_priceLabel, set);
            BindQuantityLabel(_quantityLabel, set);
            BindQuantityActivityIndicator(_quantityActivityIndicator, set);
            BindIncButton(_incButton, set);
            BindDecButton(_decButton, set);

            set.Apply();
        }

        protected virtual void BindImageView(UIImageView image, MvxFluentBindingDescriptionSet<BasketCell, IBasketItemVM> set)
        {
            if (image is MvxCachedImageView imageView)
            {
                set.Bind(imageView).For(i => i.ImagePath).To(vm => vm.ImageUrl);
            }
        }

        protected virtual void BindTitleLabel(UILabel titleLabel, MvxFluentBindingDescriptionSet<BasketCell, IBasketItemVM> set)
        {
            set.Bind(titleLabel).To(vm => vm.Name);
        }

        protected virtual void BindPriceLabel(UILabel priceLabel, MvxFluentBindingDescriptionSet<BasketCell, IBasketItemVM> set)
        {
            set.Bind(priceLabel).To(vm => vm.Price).WithConversion("PriceFormat");
        }

        protected virtual void BindQuantityLabel(UILabel quantityLabel, MvxFluentBindingDescriptionSet<BasketCell, IBasketItemVM> set)
        {
            set.Bind(quantityLabel).To(vm => vm.QuantityString);
            set.Bind(quantityLabel).For("Visibility").To(vm => vm.QuantityLoading).WithConversion("InvertedVisibility");
        }

        protected virtual void BindQuantityActivityIndicator(UIActivityIndicatorView quantityActivityIndicator, MvxFluentBindingDescriptionSet<BasketCell, IBasketItemVM> set)
        {
            set.Bind(quantityActivityIndicator).For("Visibility").To(vm => vm.QuantityLoading).WithConversion("Visibility");
        }

        protected virtual void BindIncButton(UIButton incButton, MvxFluentBindingDescriptionSet<BasketCell, IBasketItemVM> set)
        {
            set.Bind(incButton).To(vm => vm.IncCommand);
        }

        protected virtual void BindDecButton(UIButton decButton, MvxFluentBindingDescriptionSet<BasketCell, IBasketItemVM> set)
        {
            set.Bind(decButton).To(vm => vm.DecCommand);
        }

        #endregion
    }
}
