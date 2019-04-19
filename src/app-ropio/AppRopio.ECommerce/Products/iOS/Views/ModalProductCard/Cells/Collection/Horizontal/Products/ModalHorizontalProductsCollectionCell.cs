using System;
using AppRopio.Base.iOS.FlowLayouts;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Products;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Products;
using CoreGraphics;
using Foundation;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ModalProductCard.Cells.Collection.Horizontal.Products
{
    public partial class ModalHorizontalProductsCollectionCell : PDHorizontalProductsCollectionCell
    {
        public new static readonly NSString Key = new NSString("ModalHorizontalProductsCollectionCell");
        public new static readonly UINib Nib;

        protected override Base.iOS.Controls.ARLabel Name => _name;
        protected override UICollectionView CollectionView => _collectionView;
        protected override NSLayoutConstraint CollectionViewHeightConstraint => _collectionViewHeightConstraint;
        protected override UIView BottomSeparator => _bottomSeparator;

        private int _itemsCount;
        public int ItemsCount
        {
            get => _itemsCount;
            set
            {
                _itemsCount = value;

                if (_itemsCount != 0 && CollectionView.CollectionViewLayout is CardsCollectionViewLayout layout)
                {
                    var count = (DataContext as IHorizontalProductsCollectionPciVm)?.Items?.Count ?? 4;
                    layout.MaximumVisibleItems = count >= 4 ? 4 : (count > 1 ? count + 1 : count);
                }
            }
        }

        static ModalHorizontalProductsCollectionCell()
        {
            Nib = UINib.FromName("ModalHorizontalProductsCollectionCell", NSBundle.MainBundle);
        }

        protected ModalHorizontalProductsCollectionCell(IntPtr handle) 
            : base(handle)
        {
            
        }

        protected override void SetupCollectionView(UICollectionView collectionView)
        {
            base.SetupCollectionView(collectionView);

            var layout = new CardsCollectionViewLayout
            {
                ItemSize = ThemeConfig.Products.CollectionType == CollectionType.Grid ? 
                                      (collectionView.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize 
                                      :
                                      new CGSize(ThemeConfig.Products.ProductCell.Size.Width ?? DeviceInfo.ScreenWidth - 32, ThemeConfig.Products.ProductCell.Size.Height ?? 146),
                ScrollDirection = UICollectionViewScrollDirection.Horizontal,
                MinimumInteritemSpacing = 8
            };
            collectionView.CollectionViewLayout = layout;

            collectionView.PagingEnabled = true;
            collectionView.ShowsHorizontalScrollIndicator = false;
            collectionView.ContentInset = UIEdgeInsets.Zero;

            CollectionViewHeightConstraint.Constant += HEIGHT;
        }

        protected override void BindCollectionView(UICollectionView collectionView, MvvmCross.Binding.BindingContext.MvxFluentBindingDescriptionSet<PDHorizontalProductsCollectionCell, Core.ViewModels.ProductCard.Items.Collection.Horizontal.Products.IHorizontalProductsCollectionPciVm> set)
        {
            base.BindCollectionView(collectionView, set);

            this.CreateBinding().For(c => c.ItemsCount).To<IHorizontalProductsCollectionPciVm>(vm => vm.Items.Count).Apply();
        }

        protected override MvvmCross.Binding.iOS.Views.MvxCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new InfinityCollectionViewSource(collectionView, ThemeConfig.Products.CollectionType == CollectionType.Grid ? CatalogGridCell.Key : CatalogListCell.Key);
        }
    }
}
