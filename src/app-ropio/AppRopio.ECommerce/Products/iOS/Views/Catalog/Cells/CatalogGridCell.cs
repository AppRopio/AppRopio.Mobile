using System;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.Combiners;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using FFImageLoading.Cross;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells
{
    public partial class CatalogGridCell : MvxCollectionViewCell
    {
        protected virtual ProductsConfig Config { get { return Mvx.IoCProvider.Resolve<IProductConfigService>().Config; } }

        protected virtual ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        protected ILocalizationService LocalizationService => Mvx.IoCProvider.Resolve<ILocalizationService>();

        public static NSString Key = new NSString("CatalogGridCell");
        public static UINib Nib = UINib.FromName("CatalogGridCell", NSBundle.MainBundle);

        protected virtual UIImageView Image => _image;
        protected virtual AppRopio.Base.iOS.Controls.ARLabel Name => _name;
        protected virtual AppRopio.Base.iOS.Controls.ARLabel Price => _price;
        protected virtual AppRopio.Base.iOS.Controls.ARLabel MaxPrice => _maxPrice;
        protected virtual AppRopio.Base.iOS.Controls.ARLabel OldPrice => _oldPrice;
        protected virtual UICollectionView Badges => _badges;
        protected virtual NSLayoutConstraint BadgesWidthConstraint => _badgesWidthContraint;
        protected virtual UIButton MarkButton => _markButton;
        protected virtual UIButton ActionButton => _actionButton;
        protected virtual NSLayoutConstraint ActionButtonHeightConstraint => _actionButtonHeightConstraint;
        protected virtual NSLayoutConstraint ActionButtonBottomMarginConstraint => _actionButtonBottomMarginConstraint;

        protected CatalogGridCell(IntPtr handle)
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
            var cell = ThemeConfig.Products.ProductCell;

            SetupImage(Image, cell.Image);

            SetupName(Name, cell.Title);

            SetupPrice(Price, cell.Price);
            SetupMaxPrice(MaxPrice, cell.Price);

            SetupOldPrice(OldPrice, cell.OldPrice);

            SetupBadgesCollection(Badges);

            SetupMarkButton(MarkButton, cell.MarkButton);

            SetupActionButton(ActionButton, cell.ActionButton);

            this.SetupStyle(cell);
        }

        protected virtual void SetupImage(UIImageView image, Image imageModel)
        {
            image?.SetupStyle(imageModel);
        }

        protected virtual void SetupName(UILabel name, Label nameLabel)
        {
            name?.SetupStyle(nameLabel);
        }

        protected virtual void SetupPrice(UILabel price, Label priceLabel)
        {
            price?.SetupStyle(priceLabel);
        }

        protected virtual void SetupMaxPrice(UILabel price, Label priceLabel)
        {
            price?.SetupStyle(priceLabel);
        }

        protected virtual void SetupOldPrice(UILabel oldPrice, Label oldPriceLabel)
        {
            oldPrice?.SetupStyle(oldPriceLabel);
        }

        protected virtual void SetupBadgesCollection(UICollectionView badges)
        {
            if (badges == null)
                return;
            
            (badges.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize = new CoreGraphics.CGSize(BadgeCell.WIDTH, BadgeCell.HEIGHT);

            if (BadgesWidthConstraint != null)
                BadgesWidthConstraint.Constant = (BadgeCell.WIDTH * 2f).If_iPhone6(BadgeCell.WIDTH * 3f).If_iPhone6Plus(BadgeCell.WIDTH * 3f);

            badges.RegisterNibForCell(BadgeCell.Nib, BadgeCell.Key);
        }

        protected virtual void SetupMarkButton(UIButton markButton, Button button)
        {
            markButton?.SetupStyle(button);
        }

        protected void SetupActionButton(UIButton actionButton, Button button)
        {
            actionButton?.SetupStyle(button);

            if (ActionButtonHeightConstraint != null && ActionButtonBottomMarginConstraint != null)
                ActionButtonHeightConstraint.Constant = ThemeConfig.Products.ProductCell.ActionButtonHeight - ActionButtonBottomMarginConstraint.Constant;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<CatalogGridCell, ICatalogItemVM>();

            BindImage(Image, set);
            BindName(Name, set);
            BindPrice(Price, set);
            BindMaxPrice(MaxPrice, set);
            BindOldPrice(OldPrice, set);
            BindBagesCollection(Badges, set);
            BindMarkButton(MarkButton, set);
            BindActionButton(ActionButton, set);

            set.Apply();
        }

        protected virtual void BindImage(UIImageView image, MvxFluentBindingDescriptionSet<CatalogGridCell, ICatalogItemVM> set)
        {
            if (image == null)
                return;

            if (image is MvxCachedImageView imageView)
            {
                imageView.LoadingPlaceholderImagePath = $"res:{ThemeConfig.Products.ProductCell.Image.Path}";
                imageView.ErrorPlaceholderImagePath = $"res:{ThemeConfig.Products.ProductCell.Image.Path}";

                set.Bind(imageView).For(i => i.ImagePath).To(vm => vm.ImageUrl);
            }
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<CatalogGridCell, ICatalogItemVM> set)
        {
            if (name == null) 
                return;

            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindPrice(UILabel price, MvxFluentBindingDescriptionSet<CatalogGridCell, ICatalogItemVM> set)
        {
            if (price == null)
                return;

            set.Bind(price).For(v => v.Text).To(vm => vm.Price);

            if (Config.PriceType != PriceType.FromTo) {
                set.Bind(price)
                    .For("Visibility")
                    .ByCombining(new PriceVisibilityCommonValueCombiner(), new[] { "Price", "MaxPrice" })
                    .WithConversion("VisibilityHidden");
            }
        }

        protected virtual void BindMaxPrice(UILabel maxPrice, MvxFluentBindingDescriptionSet<CatalogGridCell, ICatalogItemVM> set)
        {
            if (maxPrice == null)
                return;

            if (!(Config.PriceType == PriceType.To || Config.PriceType == PriceType.FromTo)) {
                maxPrice.Hidden = true;
                return;
            }

            set.Bind(maxPrice).For(v => v.Text).To(vm => vm.MaxPrice);

            set.Bind(maxPrice).For("Visibility").To(vm => vm.MaxPrice).WithConversion("Visibility");
        }

        protected virtual void BindOldPrice(UILabel oldPrice, MvxFluentBindingDescriptionSet<CatalogGridCell, ICatalogItemVM> set)
        {
            if (oldPrice == null)
                return;

            set.Bind(oldPrice).For(v => v.Text).To(vm => vm.OldPrice);

            set.Bind(oldPrice)
                .For("Visibility")
                .ByCombining(new PriceVisibilityCommonValueCombiner(), new[] { "OldPrice", "MaxPrice" })
                .WithConversion("VisibilityHidden");
        }

        protected virtual void BindBagesCollection(UICollectionView badges, MvxFluentBindingDescriptionSet<CatalogGridCell, ICatalogItemVM> set)
        {
            if (badges == null)
                return;
            
            var dataSource = SetupBadgesViewSource(badges);
            badges.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Badges);

            badges.ReloadData();
        }

        protected virtual MvxCollectionViewSource SetupBadgesViewSource(UICollectionView badges)
        {
            return new MvxCollectionViewSource(badges, BadgeCell.Key);
        }

        protected virtual void BindMarkButton(UIButton markButton, MvxFluentBindingDescriptionSet<CatalogGridCell, ICatalogItemVM> set)
        {
            if (markButton == null)
                return;
            
            set.Bind(markButton).To(vm => vm.MarkCommand);
            set.Bind(markButton).For(v => v.Selected).To(vm => vm.Marked);
            set.Bind(markButton).For("Visibility").To(vm => vm.MarkEnabled).WithConversion("Visibility");
        }

        protected virtual void BindActionButton(UIButton actionButton, MvxFluentBindingDescriptionSet<CatalogGridCell, ICatalogItemVM> set)
        {
            if (actionButton == null)
                return;
            
            set.Bind(actionButton).To(vm => vm.ActionCommand);
            set.Bind(actionButton).For("Title").To(vm => vm.ActionText);
            set.Bind(actionButton).For("Visibility").To(vm => vm.HasAction).WithConversion("Visibility");
        }

        #endregion
    }
}
