using System;
using AppRopio.Base.iOS.FlowLayouts;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Images;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Images.Cells;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ModalProductCard.Cells.Images
{
    public partial class ModalImagesCell : MvxTableViewCell
    {
        protected ProductsThemeConfig ThemeConfig => Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig;

        public static readonly UINib Nib;

        static ModalImagesCell()
        {
            Nib = UINib.FromName("ModalImagesCell", NSBundle.MainBundle);
        }

        protected ModalImagesCell(IntPtr handle)
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
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            collectionView.ChangeFrame(0, 0, DeviceInfo.ScreenWidth, DeviceInfo.ScreenWidth);

            var layout = new CardsCollectionViewLayout
            {
                ItemSize = new CGSize(DeviceInfo.ScreenWidth - 32, DeviceInfo.ScreenWidth - 32),
                ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                MinimumInteritemSpacing = 8
            };
            collectionView.CollectionViewLayout = layout;

            collectionView.PagingEnabled = true;
            collectionView.ShowsHorizontalScrollIndicator = false;

            collectionView.RegisterNibForCell(ImageCollectionCell.Nib, ImageCollectionCell.Key);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<ModalImagesCell, IImagesProductsPciVm>();

            BindCollectionView(_collectionView, set);

            set.Apply();
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<ModalImagesCell, IImagesProductsPciVm> set)
        {
            if (collectionView.CollectionViewLayout is CardsCollectionViewLayout layout)
            {
                var count = (DataContext as IImagesProductsPciVm)?.ImagesUrls?.Count ?? 4;
                layout.MaximumVisibleItems = count >= 4 ? 4 : (count > 1 ? count + 1 : count);
            }

            var dataSource = SetupCollectionDataSource(collectionView);

            set.Bind(dataSource).To(vm => vm.ImagesUrls);

            collectionView.Source = dataSource;
            collectionView.ReloadData();
        }

        protected virtual HorizontalPagingCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new InfinityCollectionViewSource(collectionView, ImageCollectionCell.Key);
        }

        #endregion
    }
}
