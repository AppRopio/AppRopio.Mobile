using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Controls;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Products;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Products
{
    public partial class PDHorizontalProductsCollectionCell : MvxTableViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        protected virtual ARLabel Name => _name;
        protected virtual UICollectionView CollectionView => _collectionView;
        protected virtual NSLayoutConstraint CollectionViewHeightConstraint => _collectionViewHeightConstraint;
        protected virtual UIView BottomSeparator => _bottomSeparator;

        public const float HEIGHT = 60f;
        public const float DEFAULT_INSET = 16.0f;

        public static readonly NSString Key = new NSString("PDHorizontalProductsCollectionCell");
        public static readonly UINib Nib;

        static PDHorizontalProductsCollectionCell()
        {
            Nib = UINib.FromName("PDHorizontalProductsCollectionCell", NSBundle.MainBundle);
        }

        protected PDHorizontalProductsCollectionCell(IntPtr handle) 
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
            SetupName(Name);

            SetupCollectionView(CollectionView);

            BottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.ProductsCompilation.Title);
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            var flowLayout = (collectionView.CollectionViewLayout as UICollectionViewFlowLayout);

            flowLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            flowLayout.MinimumInteritemSpacing = DEFAULT_INSET / 2;
            flowLayout.MinimumLineSpacing = DEFAULT_INSET / 2;
            flowLayout.SectionInset = UIEdgeInsets.Zero;

            collectionView.ContentInset = new UIEdgeInsets(0, DEFAULT_INSET, 0, DEFAULT_INSET);

            var width = ThemeConfig.Products.ProductCell.Size.Width ?? (DeviceInfo.ScreenWidth - DEFAULT_INSET - (DEFAULT_INSET / 2)) / 2;

            if (ThemeConfig.Products.CollectionType == CollectionType.Grid)
            {
                collectionView.RegisterNibForCell(CatalogGridCell.Nib, CatalogGridCell.Key);

                flowLayout.ItemSize = new CoreGraphics.CGSize(
                    width,
                    ThemeConfig.Products.ProductCell.Size.Height.HasValue ?
                        (nfloat)ThemeConfig.Products.ProductCell.Size.Height :
                        width * 1.69
                );
            }
            else
            {
                collectionView.RegisterNibForCell(CatalogListCell.Nib, CatalogListCell.Key);

                flowLayout.ItemSize = new CoreGraphics.CGSize(
                    width,
                    ThemeConfig.Products.ProductCell.Size.Height.HasValue ?
                        (nfloat)ThemeConfig.Products.ProductCell.Size.Height :
                        146
                );
            }

            CollectionViewHeightConstraint.Constant = flowLayout.ItemSize.Height;

            collectionView.ShowsVerticalScrollIndicator = false;
            collectionView.ShowsHorizontalScrollIndicator = false;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDHorizontalProductsCollectionCell, IHorizontalProductsCollectionPciVm>();

            BindName(Name, set);
            BindCollectionView(CollectionView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDHorizontalProductsCollectionCell, IHorizontalProductsCollectionPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<PDHorizontalProductsCollectionCell, IHorizontalProductsCollectionPciVm> set)
        {
            var dataSource = SetupCollectionDataSource(collectionView);
            collectionView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
        }

        protected virtual MvxCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new MvxCollectionViewSource(collectionView, ThemeConfig.Products.CollectionType == CollectionType.Grid ? CatalogGridCell.Key : CatalogListCell.Key);
        }

        #endregion
    }
}
