using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Images.Cells
{
    public partial class ImageCollectionCell : MvxCollectionViewCell
    {
        protected ProductsThemeConfig ThemeConfig => Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig;

        public static readonly NSString Key = new NSString("ImageCollectionCell");
        public static readonly UINib Nib;

        static ImageCollectionCell()
        {
            Nib = UINib.FromName("ImageCollectionCell", NSBundle.MainBundle);
        }

        protected ImageCollectionCell(IntPtr handle) 
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
            //_image.ChangeFrame(0, 0, DeviceInfo.ScreenWidth, DeviceInfo.ScreenWidth);

            _image.SetupStyle(ThemeConfig.ProductDetails.Image);

            Layer.SetupStyle(ThemeConfig.ProductDetails.Image.Layer);
            if (ContentView != null)
            {
                ContentView.Layer.SetupStyle(ThemeConfig.ProductDetails.Image.Layer);
                ContentView.Layer.MasksToBounds = true;
            }
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var imageLoader = new MvxImageViewLoader(() => _image)
            {
                DefaultImagePath = $"res:{ThemeConfig.ProductDetails.Image.Path}",
                ErrorImagePath = $"res:{ThemeConfig.ProductDetails.Image.Path}"
            };

            this.CreateBindingSet<ImageCollectionCell, string>().Bind(imageLoader).To(".").Apply();
        }

        #endregion
    }
}
