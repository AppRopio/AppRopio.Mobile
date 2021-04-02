using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ThemeConfigs;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items.Banners;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using FFImageLoading.Cross;
using Foundation;
using MvvmCross;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Cells
{
    public partial class BannerCell : MvxCollectionViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly NSString Key = new NSString("BannerCell");
        public static readonly UINib Nib = UINib.FromName("BannerCell", NSBundle.MainBundle);

        protected BannerCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindContols();
            });
        }

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupImage(_image, ThemeConfig.BannersCell.Image);

            this.SetupStyle(ThemeConfig.BannersCell);
        }

        protected virtual void SetupImage(UIImageView image, Image model)
        {
            image.SetupStyle(model);
        }

        #endregion

        #region BindingControls

        protected virtual void BindContols()
        {
            var set = this.CreateBindingSet<BannerCell, IBannerItemVM>();

            BindImage(_image, set);

            set.Apply();
        }

        protected virtual void BindImage(UIImageView image, MvxFluentBindingDescriptionSet<BannerCell, IBannerItemVM> set)
        {
            if (image is MvxCachedImageView imageView)
            {
                set.Bind(imageView).For(i => i.ImagePath).To(vm => vm.ImageUrl);
            }
        }

        #endregion
    }
}
