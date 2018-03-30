using AppRopio.Base.iOS;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked;
using AppRopio.ECommerce.Marked.iOS.Models;
using AppRopio.ECommerce.Marked.iOS.Services;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Views.Catalog;
using CoreGraphics;
using MvvmCross.Platform;
using UIKit;

namespace AppRopio.ECommerce.Marked.iOS.Views.Marked
{
    public partial class MarkedViewController : CatalogViewController<IMarkedViewModel>
    {
        protected override UICollectionView CollectionView => _collectionView;

        protected override NSLayoutConstraint CollectionViewTopConstraint => _collectionViewTopConstraint;
        protected override NSLayoutConstraint CollectionViewBottomConstraint => _collectionViewBottomConstraint;

        protected override UIView EmptyView => _emptyView;
        protected override UIImageView EmptyImage => _emptyImage;
        protected override AppRopio.Base.iOS.Controls.ARLabel EmptyTitle => _emptyTitle;
        protected override AppRopio.Base.iOS.Controls.ARLabel EmptyText => _emptyText;
        protected override UIButton GoToButton => _goToButton;

        public MarkedViewController()
            : base("MarkedViewController", null)
        {
        }

        #region Protected

        protected override ProductsThemeConfig ThemeConfig { get { return Mvx.Resolve<IMarkedThemeConfigService>().ThemeConfig; } }

        #region InitializationControls

        protected override void SetupCollectionView(UICollectionView collectionView)
        {
            base.SetupCollectionView(collectionView);

            (collectionView.CollectionViewLayout as UICollectionViewFlowLayout).HeaderReferenceSize = CGSize.Empty;

            if (ThemeConfig.Products.CollectionType == CollectionType.Grid)
                collectionView.RegisterNibForCell(MarkedGridCell.Nib, MarkedGridCell.Key);
            else
                collectionView.RegisterNibForCell(MarkedListCell.Nib, MarkedListCell.Key);
        }

        protected override void SetupEmptyView(UIView emptyView, UIImageView emptyImage, UILabel emptyTitle, UILabel emptyText, UIButton goToButton)
        {
            base.SetupEmptyView(emptyView, emptyImage, emptyTitle, emptyText, goToButton);

            emptyTitle.Text = "Пока в избранном пусто";
            emptyText.Text = "Вернитесь в каталог,\nчтобы добавить товары";
        }

        protected virtual void SetupBasketButton(UIButton button)
        {
            button.SetupStyle((ThemeConfig as MarkedThemeConfig).BasketButton);
        }

        #endregion

        #region BindingControls

        protected override BaseCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new BaseCollectionViewSource(collectionView,ThemeConfig.Products.CollectionType == CollectionType.Grid ? MarkedGridCell.Key : MarkedListCell.Key);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            base.InitializeControls();

            Title = "Избранное";

            SetupBasketButton(_basketButton);
        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();
        }

        #endregion

        #endregion
    }
}