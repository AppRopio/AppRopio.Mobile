using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal.Shops;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Shops.Cells;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Shops
{
    public partial class PDHorizontalShopsCollectionCell : MvxTableViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public const float HEIGHT = 52f;
        public const float CELL_WIDTH = 180f;
        public const float INDICATOR_HEIGHT = 133f;
        public const float COUNT_HEIGHT = 163f;
        public const float DEFAULT_INSET = 16f;

        public static readonly NSString Key = new NSString("PDHorizontalShopsCollectionCell");
        public static readonly UINib Nib;

        static PDHorizontalShopsCollectionCell()
        {
            Nib = UINib.FromName("PDHorizontalShopsCollectionCell", NSBundle.MainBundle);
        }

        protected PDHorizontalShopsCollectionCell(IntPtr handle)
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

            SetupCollectionView(_collectionView);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.ShopsCompilation.Title);
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            var viewModel = (DataContext as IHorizontalShopsCollectionPciVm);

            _collectionViewHeightConstraint.Constant = viewModel.DataType == AppRopio.Models.Products.Responses.ProductDataType.ShopsAvailability_Count ? COUNT_HEIGHT : INDICATOR_HEIGHT;

            collectionView.RegisterNibForCell(PDHShopsCountCell.Nib, PDHShopsCountCell.Key);
            collectionView.RegisterNibForCell(PDHShopIndicatorCell.Nib, PDHShopIndicatorCell.Key);

            var flowLayout = (collectionView.CollectionViewLayout as UICollectionViewFlowLayout);

            flowLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            flowLayout.MinimumInteritemSpacing = DEFAULT_INSET / 2;
            flowLayout.MinimumLineSpacing = DEFAULT_INSET / 2;
            flowLayout.SectionInset = UIEdgeInsets.Zero;

            collectionView.ContentInset = new UIEdgeInsets(0, DEFAULT_INSET, 0, DEFAULT_INSET);

            collectionView.ShowsVerticalScrollIndicator = false;
            collectionView.ShowsHorizontalScrollIndicator = false;
        }

#endregion

#region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDHorizontalShopsCollectionCell, IHorizontalShopsCollectionPciVm>();

            BindName(_name, set);
            BindCollectionView(_collectionView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDHorizontalShopsCollectionCell, IHorizontalShopsCollectionPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<PDHorizontalShopsCollectionCell, IHorizontalShopsCollectionPciVm> set)
        {
            var dataSource = SetupCollectionDataSource(collectionView);

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

			collectionView.Source = dataSource;
            collectionView.ReloadData();

            collectionView.ContentInset = new UIEdgeInsets(0, DEFAULT_INSET, 0, DEFAULT_INSET);
        }

        protected virtual MvxCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new PDHorizontalShopsCollectionViewSource(collectionView);
        }

#endregion
    
        public override void PrepareForReuse()
        {
            base.PrepareForReuse();
        }
    }
}
