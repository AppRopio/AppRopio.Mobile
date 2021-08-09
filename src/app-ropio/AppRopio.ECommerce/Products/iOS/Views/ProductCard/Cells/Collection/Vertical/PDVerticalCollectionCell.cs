using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Vertical;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Vertical.Cells;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Vertical
{
    public partial class PDVerticalCollectionCell : MvxTableViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public const float VERTICAL_COLLECTION_TITLE_HEIGHT = 44;
        public const float VERTICAL_COLLECTION_ITEM_HEIGHT = 44;
        public const float VERTICAL_COLLECTION_BOTTOM_INSET = 15;

        public static readonly NSString Key = new NSString("PDVerticalCollectionCell");
        public static readonly UINib Nib;

        static PDVerticalCollectionCell()
        {
            Nib = UINib.FromName("PDVerticalCollectionCell", NSBundle.MainBundle);
        }

        protected PDVerticalCollectionCell(IntPtr handle)
            : base(handle)
        {
            this.DelayBind(() =>
            {
                InitializeControls();
                BindControls();
            });
        }

        #region Protected

        #region InitializationControls

        protected virtual void InitializeControls()
        {
            SetupName(_name);
            SetupCollectionView(_collectionView);

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Collection.Title ?? ThemeConfig.ProductDetails.DetailsCell.Title);
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            var flowLayout = collectionView.CollectionViewLayout as UICollectionViewFlowLayout;
            flowLayout.SectionInset = new UIEdgeInsets(0, 8, 0, 8);
            flowLayout.MinimumInteritemSpacing = 6;
            flowLayout.MinimumLineSpacing = 6;
            flowLayout.ItemSize = new CoreGraphics.CGSize((DeviceInfo.ScreenWidth - flowLayout.SectionInset.Left - flowLayout.SectionInset.Right - (flowLayout.MinimumInteritemSpacing * 4)) / 4, VERTICAL_COLLECTION_ITEM_HEIGHT);

            collectionView.RegisterNibForCell(PDVerticalTextCell.Nib, PDVerticalTextCell.Key);

            //_bottomCollectionViewConstraint.Constant = VERTICAL_COLLECTION_BOTTOM_INSET;
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDVerticalCollectionCell, IVerticalCollectionPciVm>();

            BindName(_name, set);
            BindCollectionView(_collectionView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDVerticalCollectionCell, IVerticalCollectionPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<PDVerticalCollectionCell, IVerticalCollectionPciVm> set)
        {
            var dataSource = SetupCollectionViewDataSource(collectionView);

            collectionView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            collectionView.ReloadData();
        }

        protected virtual MvxCollectionViewSource SetupCollectionViewDataSource(UICollectionView collectionView)
        {
            return new MvxCollectionViewSource(collectionView, PDVerticalTextCell.Key);
        }

        #endregion

        #endregion
    }
}
