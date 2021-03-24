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
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.ShortInfo
{
    public partial class ShortInfoCell : MvxTableViewCell
    {
        protected ProductsConfig Config { get { return Mvx.Resolve<IProductConfigService>().Config; } }

        protected ProductsThemeConfig ThemeConfig => Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig;

        public static readonly NSString Key = new NSString("ShortInfoCell");
        public static readonly UINib Nib;

        public static float INFO_HEIGHT = 120;

        static ShortInfoCell()
        {
            Nib = UINib.FromName("ShortInfoCell", NSBundle.MainBundle);
        }

        protected ShortInfoCell(IntPtr handle) 
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
            SetupStatus(_status);

            SetupName(_name);

            SetupPrice(_fromHintLabel, _price);

            SetupOldPrice(_oldPrice);

            SetupExtraPrice(_extraPrice);

            SetupBadgesCollection(_badges);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupBadgesCollection(UICollectionView badges)
        {
            (badges.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize = new CoreGraphics.CGSize(BadgeCell.WIDTH, BadgeCell.HEIGHT);

            var viewModel = DataContext as IShortInfoProductsPciVm;
            if (viewModel != null)
                _badgesWidthContraint.Constant = viewModel.Badges.IsNullOrEmpty() ? 0 : Math.Min(3f * BadgeCell.WIDTH, viewModel.Badges.Count * BadgeCell.WIDTH);

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

        protected virtual void SetupPrice(UILabel fromHintLabel, UILabel price)
        {
            fromHintLabel.SetupStyle(ThemeConfig.ProductDetails.Price);
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

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<ShortInfoCell, IShortInfoProductsPciVm>();

            BindStatus(_status, set);

            BindName(_name, set);

            BindPrice(_fromHintLabel, _price, set);

            BindOldPrice(_oldPrice, set);

            BindExtraPrice(_extraPrice, set);

            BindBagesCollection(_badges, set);

            set.Apply();
        }

        protected virtual void BindBagesCollection(UICollectionView badges, MvxFluentBindingDescriptionSet<ShortInfoCell, IShortInfoProductsPciVm> set)
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

        protected virtual void BindExtraPrice(UILabel extraPrice, MvxFluentBindingDescriptionSet<ShortInfoCell, IShortInfoProductsPciVm> set)
        {
            if (Config.UnitNameEnabled)
                set.Bind(extraPrice).ByCombining(new PriceUnitCombiner(), new[] { "ExtraPrice", "UnitNameExtra" });
            else
                set.Bind(extraPrice).To(vm => vm.ExtraPrice).WithConversion("PriceFormat");

            set.Bind(extraPrice).For("Visibility").To(vm => vm.ExtraPrice).WithConversion("Visibility");
        }

        protected virtual void BindOldPrice(UILabel oldPrice, MvxFluentBindingDescriptionSet<ShortInfoCell, IShortInfoProductsPciVm> set)
        {
            if (Config.UnitNameEnabled)
                set.Bind(oldPrice).ByCombining(new PriceUnitCombiner(), new[] { "OldPrice", "UnitNameOld" });
            else
                set.Bind(oldPrice).To(vm => vm.OldPrice).WithConversion("PriceFormat");

            set.Bind(oldPrice).For("Visibility").To(vm => vm.OldPrice).WithConversion("Visibility");
        }

        protected virtual void BindPrice(UILabel price, UILabel fromHintLabel, MvxFluentBindingDescriptionSet<ShortInfoCell, IShortInfoProductsPciVm> set)
        {
            if (Config.UnitNameEnabled)
                set.Bind(price).ByCombining(new PriceUnitCombiner(), new[] { "Price", "UnitName" });
            else
                set.Bind(price).To(vm => vm.Price).WithConversion("PriceFormat");

            set.Bind(fromHintLabel).For("Visibility").To(vm => vm.IsPriceDependsOnParams).WithConversion("Visibility");
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<ShortInfoCell, IShortInfoProductsPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindStatus(UILabel status, MvxFluentBindingDescriptionSet<ShortInfoCell, IShortInfoProductsPciVm> set)
        {
            set.Bind(status).To(vm => vm.StateName);
        }

        #endregion
    }
}
