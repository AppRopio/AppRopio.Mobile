using System;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Platform;
using UIKit;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.MultiSelection;
using AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.MultiSelection.Cells;

namespace AppRopio.ECommerce.Products.iOS.Views.ProductCard.Cells.Selection.MultiSelection
{
    public partial class PDMultiSelectionCell : MvxTableViewCell
    {
        protected ProductsThemeConfig ThemeConfig => Mvx.Resolve<IProductsThemeConfigService>().ThemeConfig;

        public const float MULTY_SELECTION_TITLE_HEIGHT = 52;
        public const float MULTY_SELECTION_CONTENT_HEIGHT = 30;

        public static readonly NSString Key = new NSString("PDMultiSelectionCell");
        public static readonly UINib Nib;

        static PDMultiSelectionCell()
        {
            Nib = UINib.FromName("PDMultiSelectionCell", NSBundle.MainBundle);
        }

        protected PDMultiSelectionCell(IntPtr handle)
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

            _accessoryImageView.Image = ImageCache.GetImage("Images/Main/accessory_arrow.png");

            _bottomSeparator.BackgroundColor = Theme.ColorPalette.Separator.ToUIColor();
        }

        protected virtual void SetupName(UILabel name)
        {
            name.SetupStyle(ThemeConfig.ProductDetails.DetailsCell.MultiSelection.Title ?? ThemeConfig.ProductDetails.DetailsCell.Title);
        }

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            var flowLayout = collectionView.CollectionViewLayout as UICollectionViewFlowLayout;
            flowLayout.MinimumInteritemSpacing = 8;

            collectionView.ContentInset = new UIEdgeInsets(0, 16, 0, 16);
            collectionView.ShowsHorizontalScrollIndicator = false;

            collectionView.RegisterNibForCell(PDMultiSelectionTextCell.Nib, PDMultiSelectionTextCell.Key);
        }

        #endregion

        #region BindingControls

        protected virtual void BindControls()
        {
            var set = this.CreateBindingSet<PDMultiSelectionCell, IMultiSelectionPciVm>();

            BindName(_name, set);
            BindCollectionView(_collectionView, set);

            set.Apply();
        }

        protected virtual void BindName(UILabel name, MvxFluentBindingDescriptionSet<PDMultiSelectionCell, IMultiSelectionPciVm> set)
        {
            set.Bind(name).To(vm => vm.Name);
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<PDMultiSelectionCell, IMultiSelectionPciVm> set)
        {
            var dataSource = SetupCollectionDataSource(collectionView);
            collectionView.Source = dataSource;

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            set.Bind(collectionView).For("Visibility").To(vm => vm.Items.Count).WithConversion("Visibility");
        }

        protected virtual MvxCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new PDMultiSelectionCollectionViewSource(collectionView, PDMultiSelectionTextCell.Key);
        }

        #endregion
    }
}
