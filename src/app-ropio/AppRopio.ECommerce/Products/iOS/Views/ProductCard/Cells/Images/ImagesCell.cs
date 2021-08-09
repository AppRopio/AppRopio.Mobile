using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Images;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Images.Cells;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Images
{
    public partial class ImagesCell : MvxTableViewCell
    {
        public static readonly NSString Key = new NSString("ImagesCell");
        public static readonly UINib Nib;

        static ImagesCell()
        {
            Nib = UINib.FromName("ImagesCell", NSBundle.MainBundle);
        }

        protected ImagesCell(IntPtr handle) 
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
            SetupCollectionView(_collectionView);

            SetupPageControl(_pageControl);
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            collectionView.ChangeFrame(0, 0, DeviceInfo.ScreenWidth, DeviceInfo.ScreenWidth);
            
            (collectionView.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize = new CGSize(DeviceInfo.ScreenWidth, DeviceInfo.ScreenWidth);

            collectionView.PagingEnabled = true;
            collectionView.ShowsHorizontalScrollIndicator = false;

            collectionView.RegisterNibForCell(ImageCollectionCell.Nib, ImageCollectionCell.Key);
        }

        protected virtual void SetupPageControl(UIPageControl pageControl)
        {
            var mainTextColor = Theme.ColorPalette.TextBase.ToUIColor();

            nfloat r, g, b, a = 0;
            mainTextColor.GetRGBA(out r, out g, out b, out a);

            pageControl.PageIndicatorTintColor = UIColor.FromRGBA(r, g, b, 0.5f);
            pageControl.CurrentPageIndicatorTintColor = mainTextColor;
            pageControl.HidesForSinglePage = true;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<ImagesCell, IImagesProductsPciVm>();

            BindCollectionView(_collectionView, _pageControl, set);

            BindPageControl(_pageControl, set);

            set.Apply();
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, UIPageControl pageControl, MvxFluentBindingDescriptionSet<ImagesCell, IImagesProductsPciVm> set)
        {
            var dataSource = SetupCollectionDataSource(collectionView);
            dataSource.PageChanged += (sender, e) => pageControl.CurrentPage = (sender as HorizontalPagingCollectionViewSource).Page;

            set.Bind(dataSource).To(vm => vm.ImagesUrls);

            collectionView.Source = dataSource;
            collectionView.ReloadData();
        }

        protected virtual HorizontalPagingCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new HorizontalPagingCollectionViewSource(collectionView, ImageCollectionCell.Key);
        }

        protected virtual void BindPageControl(UIPageControl pageControl, MvxFluentBindingDescriptionSet<ImagesCell, IImagesProductsPciVm> set)
        {
            set.Bind(pageControl).For(v => v.Pages).To(vm => vm.ImagesUrls.Count);
        }

        #endregion
    }
}
