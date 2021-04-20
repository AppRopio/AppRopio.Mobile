using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.Models.ValueConverters;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories;
using AppRopio.ECommerce.Products.iOS.Views.Categories.Cells;
using AppRopio.ECommerce.Products.iOS.Views.Categories.StepByStep.Cells;
using AppRopio.ECommerce.Products.iOS.Views.Categories.SupplementaryView;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Categories.StepByStep
{
    public partial class SSCategoriesViewController : ProductsViewController<ISSCategoriesViewModel>
    {
        public SSCategoriesViewController()
            : base("SSCategoriesViewController", null)
        {
            RegisterKeyboardActions = true;
        }

        #region Protected

        #region InitializationControls

        protected virtual void SetupSearchViewForScrollView(UIView searchView, UIScrollView scrollView)
        {
            if (Config.SearchType == SearchType.Bar)
            {
                searchView.RemoveFromSuperview();
                searchView.ChangeFrame(0.0f, -56.0f, DeviceInfo.ScreenWidth, 56.0f);
                searchView.TranslatesAutoresizingMaskIntoConstraints = true;

                var insets = scrollView.ContentInset;
                scrollView.ContentInset = insets.ChangeInsets(t: insets.Top + 56.0f);
                scrollView.AddSubview(searchView);
            }
        }

        protected virtual void SetupTopCollection(UICollectionView topCollection)
        {
            topCollection.RegisterNibForCell(BannerCell.Nib, BannerCell.Key);

            var flowLayout = (topCollection.CollectionViewLayout as UICollectionViewFlowLayout);
            flowLayout.ItemSize = new CoreGraphics.CGSize(DeviceInfo.ScreenWidth, DeviceInfo.ScreenWidth * 9 / 16);
            flowLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
        }

        protected virtual void SetupTableView(UITableView tableView, UIView searchView)
        {
            tableView.RegisterNibForCellReuse(SSCategoryCell.Nib, SSCategoryCell.Key);

            tableView.TableHeaderView.ChangeFrame(h: (DeviceInfo.ScreenWidth * 9 / 16));
            tableView.TableFooterView.ChangeFrame(h: DeviceInfo.ScreenWidth * 9 / 16);
        }

        protected virtual void SetupBottomCollection(UICollectionView bottomCollection)
        {
            bottomCollection.RegisterNibForCell(BannerCell.Nib, BannerCell.Key);

            var flowLayout = (bottomCollection.CollectionViewLayout as UICollectionViewFlowLayout);
            flowLayout.ItemSize = new CoreGraphics.CGSize(DeviceInfo.ScreenWidth, DeviceInfo.ScreenWidth * 9 / 16);
            flowLayout.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
        }

        protected virtual void SetupBasketCartIndicator()
        {
            if (ViewModel == null)
                return;

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

        protected virtual void SetupCollectionView(UICollectionView collectionView, UIView searchView)
        {
            var flowLayout = (collectionView.CollectionViewLayout as UICollectionViewFlowLayout);

            flowLayout.MinimumInteritemSpacing = 8;
            flowLayout.MinimumLineSpacing = 8;
            flowLayout.SectionInset = new UIEdgeInsets(8, 8, 8, 8);

            collectionView.RegisterNibForCell(CategoryGridCell.Nib, CategoryGridCell.Key);

            var width = (DeviceInfo.ScreenWidth - flowLayout.SectionInset.Left - flowLayout.SectionInset.Right - (flowLayout.MinimumInteritemSpacing * ThemeConfig.Categories.CollectionColumns - 1)) / ThemeConfig.Categories.CollectionColumns;

            flowLayout.ItemSize = new CoreGraphics.CGSize(width, width);

            collectionView.RegisterClassForSupplementaryView(typeof(BannersSupplementaryView), UICollectionElementKindSection.Header, BannersSupplementaryView.ReuseIdentifierString_Header);
            collectionView.RegisterClassForSupplementaryView(typeof(BannersSupplementaryView), UICollectionElementKindSection.Footer, BannersSupplementaryView.ReuseIdentifierString_Footer);
        }

        #endregion

        #region BindingContols

        protected virtual void BindTopCollection(UICollectionView topCollection, MvxFluentBindingDescriptionSet<SSCategoriesViewController, ISSCategoriesViewModel> set)
        {
            var topDataSource = new MvxCollectionViewSource(topCollection, BannerCell.Key);

            set.Bind(topDataSource).To(vm => vm.TopBanners);
            set.Bind(topDataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.BannerSelectionChangedCommand);

            topCollection.Source = topDataSource;
            topCollection.ReloadData();
        }

        protected virtual void BindTableView(UITableView tableView, MvxFluentBindingDescriptionSet<SSCategoriesViewController, ISSCategoriesViewModel> set)
        {
            var dataSource = new SSCategoriesTableViewSource(tableView, SSCategoryCell.Key, SSCategoryCell.Key);

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            tableView.Source = dataSource;
            tableView.ReloadData();

            set.Bind(tableView)
                .For(v => v.TableHeaderView)
                .To(vm => vm.TopBanners.Count)
                .WithConversion(
                    "SizeVisibility",
                    new SizeVisibilityParameter() {
                        View = tableView.TableHeaderView,
                        MinimumHeight = () => 0
                    }
                );
            set.Bind(tableView)
                .For(v => v.TableFooterView)
                .To(vm => vm.BottomBanners.Count)
                .WithConversion(
                    "SizeVisibility",
                    new SizeVisibilityParameter() {
                        View = tableView.TableFooterView,
                        MinimumHeight = () => 0
                    }
                );
        }

        protected virtual void BindBottomCollection(UICollectionView bottomCollection, MvxFluentBindingDescriptionSet<SSCategoriesViewController, ISSCategoriesViewModel> set)
        {
            var bottomDataSource = new MvxCollectionViewSource(bottomCollection, BannerCell.Key);

            set.Bind(bottomDataSource).To(vm => vm.BottomBanners);
            set.Bind(bottomDataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.BannerSelectionChangedCommand);

            bottomCollection.Source = bottomDataSource;
            bottomCollection.ReloadData();
        }

        protected virtual void BindCollectionView(UICollectionView collectionView, MvxFluentBindingDescriptionSet<SSCategoriesViewController, ISSCategoriesViewModel> set)
        {
            var dataSource = SetupCollectionDataSource(collectionView);

            set.Bind(dataSource).To(vm => vm.Items);
            set.Bind(dataSource).For(ds => ds.SelectionChangedCommand).To(vm => vm.SelectionChangedCommand);

            collectionView.Source = dataSource;
            collectionView.ReloadData();
        }

        protected virtual BaseCollectionViewSource SetupCollectionDataSource(UICollectionView collectionView)
        {
            return new SSCategoriesCollectionViewSource(ViewModel, collectionView, CategoryGridCell.Key)
            {
                DeselectAutomatically = true,
                HeaderReuseID = BannersSupplementaryView.ReuseIdentifierString_Header,
                FooterReuseID = BannersSupplementaryView.ReuseIdentifierString_Footer
            };
        }

        #endregion

        #region CommonViewController implementation

        protected override void SetupRightBarButtonItems()
        {
            base.SetupRightBarButtonItems();

            SetupBasketCartIndicator();
        }

        protected override void BindControls()
        {
            var set = this.CreateBindingSet<SSCategoriesViewController, ISSCategoriesViewModel>();

            if (ThemeConfig.Categories.CollectionType == CollectionType.List)
            {
                BindTopCollection(_topCollection, set);

                BindTableView(_tableView, set);

                BindBottomCollection(_bottomCollection, set);
            }
            else
            {
                BindCollectionView(_collectionView, set);
            }

            BindSearchView(_searchView, set);
            BindSearchButton(_searchButton, set);

            set.Apply();
        }

        protected override void InitializeControls()
        {
            Title = LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "CategoriesEmptyTitle");

            if (ThemeConfig.Categories.CollectionType == CollectionType.List)
            {
                _collectionView.RemoveFromSuperview();
                SetupTopCollection(_topCollection);
                SetupTableView(_tableView, _searchView);
                SetupBottomCollection(_bottomCollection);
                SetupSearchViewForScrollView(_searchView, _tableView);
            }
            else
            {
                _tableView.RemoveFromSuperview();
                SetupCollectionView(_collectionView, _searchView);
                SetupSearchViewForScrollView(_searchView, _collectionView);
            }
            SetupSearchBar(_searchBar);
            SetupStackView(_stackView);
        }

        protected override void CleanUp()
        {
            ReleaseDesignerOutlets();
        }

        #endregion

        #endregion

        #region Public

        public override void ViewWillAppear(bool animated)
        {
            NavigationController.SetNavigationBarHidden(false, true);

            base.ViewWillAppear(animated);
        }

        #endregion

        protected virtual void SetupSearchBar(UISearchBar searchBar)
        {
            searchBar.SetupStyle(ThemeConfig.ContentSearch.SearchBar);
        }

        protected virtual void SetupStackView(UIStackView stackView)
        {
//            stackView.LayoutMarginsRelativeArrangement = true;
        }

        protected virtual void BindSearchView(UIView searchView, MvxFluentBindingDescriptionSet<SSCategoriesViewController, ISSCategoriesViewModel> set)
        {
            set.Bind(searchView).For("Visibility").To(vm => vm.SearchBar).WithConversion("Visibility");
        }

        protected virtual void BindSearchButton(UIButton searchButton, MvxFluentBindingDescriptionSet<SSCategoriesViewController, ISSCategoriesViewModel> set)
        {
            set.Bind(searchButton).To(vm => vm.ShowSearchCommand);
        }
    }
}

