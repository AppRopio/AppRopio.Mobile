using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Collection.Horizontal;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal.Cells;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Collection.Horizontal
{
    public partial class PDHorizontalCollectionCell : MvxTableViewCell
    {
        protected ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public const float HORIZONTAL_COLLECTION_HEIGHT = 98;

        public static readonly NSString Key = new NSString("PDHorizontalCollectionCell");
        public static readonly UINib Nib;

        static PDHorizontalCollectionCell()
        {
            Nib = UINib.FromName("PDHorizontalCollectionCell", NSBundle.MainBundle);
        }

        protected PDHorizontalCollectionCell(IntPtr handle)
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

            _bottonSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.Collection.Title ?? ThemeConfig.ProductDetails.DetailsCell.Title);
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            var flowLayout = collectionView.CollectionViewLayout as UICollectionViewFlowLayout;

            collectionView.ContentInset = new UIEdgeInsets(0, 16, 0, 16);
            collectionView.ShowsHorizontalScrollIndicator = false;

            collectionView.RegisterNibForCell(PDHorizontalColorCell.Nib, PDHorizontalColorCell.Key);
            collectionView.RegisterNibForCell(PDHorizontalTextCell.Nib, PDHorizontalTextCell.Key);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDHorizontalCollectionCell, IHorizontalCollectionPciVm>();

            BindName(_name, set);
            BindCollectionView(_collectionView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDHorizontalCollectionCell, IHorizontalCollectionPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<PDHorizontalCollectionCell, IHorizontalCollectionPciVm> set)
        {
            var dataSource = SetupCollectionDataSource(collectionView);
            collectionView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
        }

        protected virtual MvxCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new PDHorizontalCollectionViewSource(collectionView);
        }

        #endregion
    }
}
