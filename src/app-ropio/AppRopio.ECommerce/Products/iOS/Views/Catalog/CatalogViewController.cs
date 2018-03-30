using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.iOS;
using AppRopio.Base.iOS.UIExtentions;
using AppRopio.Base.iOS.ViewSources;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Cells;
using AppRopio.ECommerce.Products.iOS.Views.Catalog.Header;
using CoreGraphics;
using MvvmCross.Binding.BindingContext;
using MvvmCross.iOS.Views;
using MvvmCross.Platform;
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

            if (ThemeConfig.Products.CollectionType == Models.CollectionType.Grid)
            {
                collectionView.RegisterNibForCell(CatalogGridCell.Nib, CatalogGridCell.Key);

                var width = ThemeConfig.Products.ProductCell.Size.Width.HasValue ?
                                        ThemeConfig.Products.ProductCell.Size.Width.Value :
                                        (DeviceInfo.ScreenWidth - flowLayout.SectionInset.Left - flowLayout.SectionInset.Right - flowLayout.MinimumInteritemSpacing) / 2;

                flowLayout.ItemSize = new CoreGraphics.CGSize(
                    width,
                    ThemeConfig.Products.ProductCell.Size.Height.HasValue ?
                        (nfloat)ThemeConfig.Products.ProductCell.Size.Height :
                        width * 1.78
                );
            }
            else
            {
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
            var config = Mvx.Resolve<IProductConfigService>().Config;
            if (config.Header != null && Mvx.Resolve<IViewLookupService>().IsRegistered(config.Header.TypeName))
            {
                var type = Mvx.Resolve<IViewLookupService>().Resolve(config.Header.TypeName);
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

            emptyTitle.Text = "Ничего не найдено";
            emptyText.Text = "Попробуйте воспользоваться\nпоиском или каталогом";
        }

        protected override void SetupRightBarButtonItems()
        {
            base.SetupRightBarButtonItems();

            if (ViewModel.VmNavigationType != Base.Core.Models.Navigation.NavigationType.InsideScreen)
                SetupBasketCartIndicator();
        }

        protected void SetupBasketCartIndicator()
        {
            var config = Mvx.Resolve<IProductConfigService>().Config;
            if (config.Basket?.CartIndicator != null && Mvx.Resolve<IViewLookupService>().IsRegistered(config.Basket?.CartIndicator.TypeName))
            {
                var cartIndicatorView = ViewModel.CartIndicatorVM == null ? null : Mvx.Resolve<IMvxIosViewCreator>().CreateView(ViewModel.CartIndicatorVM) as UIView;

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
            var config = Mvx.Resolve<IProductConfigService>().Config;

            var dataSource = new SupplementaryCollectionViewSource(
                collectionView,
                ThemeConfig.Products.CollectionType == Models.CollectionType.Grid ? CatalogGridCell.Key : CatalogListCell.Key,
                ViewModel.HeaderVm)
            {
                HeaderReuseID = config.Header?.TypeName
            };

            return dataSource;
        }

        protected virtual void BindEmptyView(UIView emptyView, UIButton goToButton, MvxFluentBindingDescriptionSet<CatalogViewController<T>, T> set)
        {
            set.Bind(emptyView).For("Visibility").To(vm => vm.Empty).WithConversion("Visibility");
            set.Bind(goToButton).To(vm => vm.CatalogCommand);
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
            BindEmptyView(EmptyView, GoToButton, set);

            set.Apply();
        }

        #endregion

        #endregion
    }
}

