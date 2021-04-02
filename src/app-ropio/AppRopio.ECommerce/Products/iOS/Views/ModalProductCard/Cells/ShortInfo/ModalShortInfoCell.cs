using System;
using AppRopio.Base.Core.Combiners;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.ShortInfo;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ModalProductCard.Cells.ShortInfo
{
    public partial class ModalShortInfoCell : MvxTableViewCell
    {
        protected virtual ProductsConfig Config { get { return Mvx.IoCProvider.Resolve<IProductConfigService>().Config; } }

        protected ProductsThemeConfig ThemeConfig => Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig;

        public const float HEIGHT = 160;
        public const float BADGES_HEIGHT = 31;

        public static readonly UINib Nib;

        static ModalShortInfoCell()
        {
            Nib = UINib.FromName("ModalShortInfoCell", NSBundle.MainBundle);
        }

        protected ModalShortInfoCell(IntPtr handle)
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
            SetupName(_name);

            SetupPrice(_price);

            SetupOldPrice(_oldPrice);

            SetupExtraPrice(_extraPrice);

            SetupBadgesCollection(_badges);

            SetupShareButton(_shareButton);

            SetupMarkButton(_markButton);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupBadgesCollection(UICollectionView badges)
        {
            (badges.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize = new CoreGraphics.CGSize(BadgeCell.WIDTH, BadgeCell.HEIGHT);

            var totalCellWidth = BadgeCell.WIDTH * ((DataContext as IShortInfoProductsPciVm).Badges?.Count ?? 0);

            var horizontalInset = (DeviceInfo.ScreenWidth - (totalCellWidth)) / 2;

            badges.ContentInset = new UIEdgeInsets(0, horizontalInset, 0, horizontalInset);
            badges.RegisterNibForCell(BadgeCell.Nib, BadgeCell.Key);
        }

        protected virtual void SetupExtraPrice(UILabel extraPrice)
        {
            extraPrice.SetupStyle(ThemeConfig.ProductDetails.ExtraPrice);
        }

        protected virtual void SetupOldPrice(UILabel oldPrice)
        {
            oldPrice.SetupStyle(ThemeConfig.ProductDetails.OldPrice);
        }

        protected virtual void SetupPrice(UILabel price)
        {
            price.SetupStyle(ThemeConfig.ProductDetails.Price);
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.Title);
        }

        protected virtual void SetupStatus(UILabel status)
        {
            status.SetupStyle(ThemeConfig.ProductDetails.Status);
        }

        protected virtual void SetupShareButton(UIButton shareButton)
        {
            shareButton.SetupStyle(ThemeConfig.ProductDetails.ShareButton);
        }

        protected virtual void SetupMarkButton(UIButton markButton)
        {
            markButton.SetupStyle(ThemeConfig.ProductDetails.MarkButton);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<ModalShortInfoCell, IShortInfoProductsPciVm>();

            BindName(_name, set);

            BindPrice(_price, set);

            BindOldPrice(_oldPrice, set);

            BindExtraPrice(_extraPrice, set);

            BindBagesCollection(_badges, set);

            BindShareButton(_shareButton, set);

            BindMarkButton(_markButton, set);

            set.Apply();
        }

        protected virtual void BindBagesCollection(UICollectionView badges, MvxFluentBindingDescriptionSet<ModalShortInfoCell, IShortInfoProductsPciVm> set)
        {
            var dataSource = SetupBadgesViewSource(badges);
            badges.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Badges);

            badges.ReloadData();
        }

        protected virtual MvxCollectionViewSource SetupBadgesViewSource(UICollectionView badges)
        {
            return new MvxCollectionViewSource(badges, BadgeCell.Key);
        }

        protected virtual void BindExtraPrice(UILabel extraPrice, MvxFluentBindingDescriptionSet<ModalShortInfoCell, IShortInfoProductsPciVm> set)
        {
            if (Config.UnitNameEnabled)
                set.Bind(extraPrice).ByCombining(new PriceUnitCombiner(), new[] { "ExtraPrice", "UnitNameExtra" });
            else
                set.Bind(extraPrice).To(vm => vm.ExtraPrice).WithConversion("PriceFormat");

            set.Bind(extraPrice).For("Visibility").To(vm => vm.ExtraPrice).WithConversion("Visibility");
        }

        protected virtual void BindOldPrice(UILabel oldPrice, MvxFluentBindingDescriptionSet<ModalShortInfoCell, IShortInfoProductsPciVm> set)
        {
            if (Config.UnitNameEnabled)
                set.Bind(oldPrice).ByCombining(new PriceUnitCombiner(), new[] { "OldPrice", "UnitNameOld" });
            else
                set.Bind(oldPrice).To(vm => vm.OldPrice).WithConversion("PriceFormat");

            set.Bind(oldPrice).For("Visibility").To(vm => vm.OldPrice).WithConversion("Visibility");
        }

        protected virtual void BindPrice(UILabel price, MvxFluentBindingDescriptionSet<ModalShortInfoCell, IShortInfoProductsPciVm> set)
        {
            if (Config.UnitNameEnabled)
                set.Bind(price).ByCombining(new PriceUnitCombiner(), new[] { "Price", "UnitName" });
            else
                set.Bind(price).To(vm => vm.Price).WithConversion("PriceFormat");
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<ModalShortInfoCell, IShortInfoProductsPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindShareButton(UIButton shareButton, MvxFluentBindingDescriptionSet<ModalShortInfoCell, IShortInfoProductsPciVm> set)
        {
            set.Bind(shareButton).To(vm => vm.ShareCommand);
        }

        protected virtual void BindMarkButton(UIButton markButton, MvxFluentBindingDescriptionSet<ModalShortInfoCell, IShortInfoProductsPciVm> set)
        {
            set.Bind(markButton).To(vm => vm.MarkCommand);
            set.Bind(markButton)
               .For(mb => mb.Selected)
               .To(vm => vm.Marked);
        }

        #endregion
    }
}
