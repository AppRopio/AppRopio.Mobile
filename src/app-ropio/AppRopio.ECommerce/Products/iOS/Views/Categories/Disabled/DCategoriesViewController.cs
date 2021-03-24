using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Disabled;
using AppRopio.ECommerce.Products.iOS.Views.Catalog;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Header;
using AppRopio.ECommerce.Products.iOS.Views.Categories.SupplementaryView;
using AppRopio.ECommerce.Products.iOS.Views.Categories.Disabled.ViewSources;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.Disabled
{
    public partial class DCategoriesViewController : CatalogViewController<IDCategoriesViewModel>
    {
        protected override NSLayoutConstraint CollectionViewTopConstraint => _collectionViewTopConstraint;
        protected override NSLayoutConstraint CollectionViewBottomConstraint => _collectionViewBottomConstraint;

        protected override UICollectionView CollectionView => _collectionView;
        protected override UIView EmptyView => _emptyView;
        protected override UIImageView EmptyImage => _emptyImage;
        protected override AppRopio.Base.iOS.Controls.ARLabel EmptyTitle => _epmtyTitle;
        protected override AppRopio.Base.iOS.Controls.ARLabel EmptyText => _emptyText;
        protected override UIButton GoToButton => _goToButton;

        public DCategoriesViewController() 
            : base("DCategoriesViewController", null)
        {
            
        }

        #region Protected

        protected override void SetupCollectionView(UICollectionView collectionView)
        {
            base.SetupCollectionView(collectionView);

            var flowLayout = (collectionView.CollectionViewLayout as UICollectionViewFlowLayout);

            flowLayout.SectionInset = new UIEdgeInsets(8, 8, 0, 8);

            collectionView.RegisterClassForSupplementaryView(typeof(BannersSupplementaryView), UICollectionElementKindSection.Header, BannersSupplementaryView.ReuseIdentifierString_Header);
            collectionView.RegisterClassForSupplementaryView(typeof(BannersSupplementaryView), UICollectionElementKindSection.Footer, BannersSupplementaryView.ReuseIdentifierString_Footer);
        }

        protected override Base.iOS.ViewSources.BaseCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            var config = Mvx.Resolve<IProductConfigService>().Config;
            var dataSource = new DCategoriesCollectionViewSource(collectionView, CatalogGridCell.Key, ViewModel, ViewModel.HeaderVm)
            {
                BannersHeaderReuseID = BannersSupplementaryView.ReuseIdentifierString_Header,
                HeaderReuseID = config.Header?.TypeName,
                FooterReuseID = BannersSupplementaryView.ReuseIdentifierString_Footer
            };

            if (ViewModel.VmNavigationType == Base.Core.Models.Navigation.NavigationType.InsideScreen)
            {
                dataSource.BannersHeaderReuseID = null;
                dataSource.FooterReuseID = null;
            }

            return dataSource;
        }

        #endregion

        #region Public

        public override void ViewWillAppear(bool animated)
        {
            NavigationController.SetNavigationBarHidden(false, true);

            base.ViewWillAppear(animated);
        }

        #endregion
    }
}

