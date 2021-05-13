using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Catalog
{
    public partial class CatalogViewController : CatalogViewController<ICatalogViewModel>
    {
        protected override NSLayoutConstraint CollectionViewTopConstraint => _collectionViewTopConstraint;
        protected override NSLayoutConstraint CollectionViewBottomConstraint => _collectionViewBottomConstraint;

        protected override UICollectionView CollectionView => _collectionView;
        protected override UIView EmptyView => _emptyView;
        protected override UIImageView EmptyImage => _emptyImage;
        protected override AppRopio.Base.iOS.Controls.ARLabel EmptyTitle => _epmtyTitle;
        protected override AppRopio.Base.iOS.Controls.ARLabel EmptyText => _emptyText;
        protected override UIButton GoToButton => _goToButton;

        public CatalogViewController()
        {

        }

        protected CatalogViewController(IntPtr handle)
            : base(handle)
        {

        }

        protected CatalogViewController(string nibName, Foundation.NSBundle bundle)
            : base(nibName, bundle)
        {

        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();
            base.CleanUp();
        }

        protected virtual void SetupSearchView(UIView searchView)
        {
            if (Config.SearchType == SearchType.Bar)
            {
                searchView.RemoveFromSuperview();
                searchView.ChangeFrame(0.0f, 44.0f + DeviceInfo.SafeAreaInsets.Top, DeviceInfo.ScreenWidth, 56.0f);
                searchView.TranslatesAutoresizingMaskIntoConstraints = true;
                View.AddSubview(searchView);
            }
        }

        protected virtual void SetupSearchBar(UISearchBar searchBar)
        {
            searchBar.SetupStyle(ThemeConfig.ContentSearch.SearchBar);
        }

        protected virtual void SetupStackView(UIStackView stackView)
        {
            stackView.Hidden = true;
        }

        protected override void InitializeControls()
        {
            base.InitializeControls();
            SetupSearchView(_searchView);
            SetupSearchBar(_searchBar);
            SetupStackView(_stackView);
        }

        protected virtual void BindSearchView(UIView searchView, MvxFluentBindingDescriptionSet<CatalogViewController, ICatalogViewModel> set)
        {
            set.Bind(searchView).For("Visibility").To(vm => vm.SearchBar).WithConversion("Visibility");
        }

        protected virtual void BindSearchButton(UIButton searchButton, MvxFluentBindingDescriptionSet<CatalogViewController, ICatalogViewModel> set)
        {
            set.Bind(searchButton).To(vm => vm.ShowSearchCommand);
        }

        protected override void BindControls()
        {
            base.BindControls();

            var set = this.CreateBindingSet<CatalogViewController, ICatalogViewModel>();

            BindSearchView(_searchView, set);
            BindSearchButton(_searchButton, set);

            set.Apply();
        }
    }

    public abstract class CatalogViewController<T> : ProductsViewController<T>
        where T : class, ICatalogViewModel
    {
        protected abstract NSLayoutConstraint CollectionViewTopConstraint { get; }
        protected abstract NSLayoutConstraint CollectionViewBottomConstraint { get; }

        protected abstract UICollectionView CollectionView { get; }
        protected abstract UIView EmptyView { get; }
        protected abstract UIImageView EmptyImage { get; }
        protected abstract AppRopio.Base.iOS.Controls.ARLabel EmptyTitle { get; }
        protected abstract AppRopio.Base.iOS.Controls.ARLabel EmptyText { get; }
        protected abstract UIButton GoToButton { get; }

        public CatalogViewController()
            : base("CatalogViewController", null)
        {

        }

        protected CatalogViewController(IntPtr handle)
            : base(handle)
        {

        }

        protected CatalogViewController(string nibName, Foundation.NSBundle bundle)
            : base(nibName, bundle)
        {

        }

        #region Protected

        #region InitializationControls

        protected virtual void SetupCollectionView(UICollectionView collectionView)
        {
            var flowLayout = (collectionView.CollectionViewLayout as UICollectionViewFlowLayout);

            flowLayout.MinimumInteritemSpacing = 8;
            flowLayout.MinimumLineSpacing = 8;
            flowLayout.SectionInset = new UIEdgeInsets(8, 8, 8, 8);

            if (ThemeConfig.Products.CollectionType == CollectionType.Grid)
            {
                collectionView.RegisterNibForCell(CatalogGridCell.Nib, CatalogGridCell.Key);

                var width = ThemeConfig.Products.ProductCell.Size.Width
                    ?? (DeviceInfo.ScreenWidth - flowLayout.SectionInset.Left - flowLayout.SectionInset.Right - flowLayout.MinimumInteritemSpacing) / 2;

                var height = (ThemeConfig.Products.ProductCell.Size.Height.HasValue ?
                        (nfloat)ThemeConfig.Products.ProductCell.Size.Height :
                        (width + 115.0f));

                var config = Mvx.IoCProvider.Resolve<IProductConfigService>().Config;
                if (config.Basket?.ItemAddToCart != null
                    && Mvx.IoCProvider.Resolve<IViewModelLookupService>().IsRegistered(config.Basket?.ItemAddToCart.TypeName)) {
                    height += ThemeConfig.Products.ProductCell.ActionButtonHeight;
                }

                flowLayout.ItemSize = new CGSize(width, height);
            }
            else
            {
                //TODO: adjust size of the list cell when adding Add To Card button if necessarily

                collectionView.RegisterNibForCell(CatalogListCell.Nib, CatalogListCell.Key);

                var width = DeviceInfo.ScreenWidth - flowLayout.SectionInset.Left - flowLayout.SectionInset.Right - flowLayout.MinimumInteritemSpacing;

                flowLayout.ItemSize = new CoreGraphics.CGSize(
                    width,
                    ThemeConfig.Products.ProductCell.Size.Height.HasValue ?
                        (nfloat)ThemeConfig.Products.ProductCell.Size.Height :
                        146
                );
            }

            SetupCollectionHeader(collectionView);

            flowLayout.HeaderReferenceSize = new CoreGraphics.CGSize(DeviceInfo.ScreenWidth, 44);
        }

        protected void SetupCollectionHeader(UICollectionView collectionView)
        {
            var config = Mvx.IoCProvider.Resolve<IProductConfigService>().Config;
            if (config.Header != null && Mvx.IoCProvider.Resolve<IViewLookupService>().IsRegistered(config.Header.TypeName))
            {
                var type = Mvx.IoCProvider.Resolve<IViewLookupService>().Resolve(config.Header.TypeName);
                collectionView.RegisterClassForSupplementaryView(type, UICollectionElementKindSection.Header, config.Header.TypeName);
            }
            else
                (collectionView.CollectionViewLayout as UICollectionViewFlowLayout).HeaderReferenceSize = CGSize.Empty;
        }

        protected virtual void SetupEmptyView(UIView emptyView, UIImageView emptyImage, UILabel emptyTitle, UILabel emptyText, UIButton goToButton)
        {
            if (emptyView == null)
                return;

            emptyView.Hidden = true;
            emptyView.BackgroundColor = ThemeConfig.Products.EmptyView.Background.ToUIColor();

            emptyImage.SetupStyle(ThemeConfig.Products.EmptyView.Image);
            emptyTitle.SetupStyle(ThemeConfig.Products.EmptyView.Title);
            emptyText.SetupStyle(ThemeConfig.Products.EmptyView.Text);

            goToButton.SetupStyle(ThemeConfig.Products.EmptyView.CatalogButton);
        }

        protected override void SetupRightBarButtonItems()
        {
            base.SetupRightBarButtonItems();

            if (ViewModel.VmNavigationType != Base.Core.Models.Navigation.NavigationType.InsideScreen)
                SetupBasketCartIndicator();
        }

        protected void SetupBasketCartIndicator()
        {
            var config = Mvx.IoCProvider.Resolve<IProductConfigService>().Config;
            if (config.Basket?.CartIndicator != null && Mvx.IoCProvider.Resolve<IViewLookupService>().IsRegistered(config.Basket?.CartIndicator.TypeName))
            {
                var cartIndicatorView = ViewModel.CartIndicatorVM == null ? null : Mvx.IoCProvider.Resolve<IMvxIosViewCreator>().CreateView(ViewModel.CartIndicatorVM) as UIView;

                if (cartIndicatorView != null)
                {
                    var list = new List<UIBarButtonItem>();

                    var cartIndicatorBarButton = new UIBarButtonItem(cartIndicatorView);

                    if (!NavigationItem.RightBarButtonItems.IsNullOrEmpty())
                        list.AddRange(NavigationItem.RightBarButtonItems);

                    list.Add(cartIndicatorBarButton);

                    NavigationItem.SetRightBarButtonItems(list.ToArray(), false);
                }
            }
        }

        #endregion

        #region BindingControls

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<CatalogViewController<T>, T> set)
        {
            var dataSource = SetupCollectionDataSource(collectionView);

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);
            set.Bind(dataSource).For(ds => ds.MoreCommand).To(vm => vm.LoadMoreCommand);

            collectionView.Source = dataSource;
            collectionView.ReloadData();
        }

        protected virtual BaseCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            var config = Mvx.IoCProvider.Resolve<IProductConfigService>().Config;

            var dataSource = new SupplementaryCollectionViewSource(
                collectionView,
                ThemeConfig.Products.CollectionType == CollectionType.Grid ? CatalogGridCell.Key : CatalogListCell.Key,
                ViewModel.HeaderVm)
            {
                HeaderReuseID = config.Header?.TypeName
            };

            return dataSource;
        }

        protected virtual void BindEmptyView(UIView emptyView, UIImageView emptyImage, UILabel emptyTitle, UILabel emptyText, UIButton goToButton, MvxFluentBindingDescriptionSet<CatalogViewController<T>, T> set)
        {
            set.Bind(emptyView).For("Visibility").To(vm => vm.Empty).WithConversion("Visibility");

            set.Bind(goToButton).To(vm => vm.CatalogCommand);
            set.Bind(goToButton).For("Title").To(vm => vm.CatalogTitle);
            set.Bind(goToButton).For("HighlightedTitle").To(vm => vm.CatalogTitle);

            set.Bind(emptyTitle).To(vm => vm.NoResultsTitle);
            set.Bind(emptyText).To(vm => vm.NoResultsText);
        }

        #endregion

        #region CommonViewController implementation

        protected override void InitializeControls()
        {
            SetupCollectionView(CollectionView);
            SetupEmptyView(EmptyView, EmptyImage, EmptyTitle, EmptyText, GoToButton);
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<CatalogViewController<T>, T>();

            BindCollectionView(CollectionView, set);
            BindEmptyView(EmptyView, EmptyImage, EmptyTitle, EmptyText, GoToButton, set);

            set.Apply();
        }

        #endregion

        #endregion
    }
}

