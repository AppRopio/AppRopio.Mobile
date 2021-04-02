using System;
using AppRopio.Base.iOS;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Header;
using AppRopio.ECommerce.Products.iOS.Models;
using AppRopio.ECommerce.Products.iOS.Services;
using Foundation;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross;
using UIKit;

namespace AppRopio.ECommerce.Products.iOS.Views.Catalog.Header
{
    public class CatalogSearchHeader : MvxCollectionReusableView, IMvxIosView<CatalogSearchHeaderVM>
    {
        #region Fields

        private BindableSearchBar _searchBar;

        #endregion

        #region Properties

        protected ProductsThemeConfig ThemeConfig { get { return Mvx.IoCProvider.Resolve<IProductsThemeConfigService>().ThemeConfig; } }

        public static readonly string ReuseIdentifierString = "CATALOG_SEARCH_HEADER";

        public override NSString ReuseIdentifier
        {
            get
            {
                return new NSString(ReuseIdentifierString);
            }
        }

        public MvxViewModelRequest _request;
        public MvxViewModelRequest Request
        {
            get
            {
                return _request;
            }
            set
            {
                _request = value;
                ViewModel = (CatalogSearchHeaderVM)((value as MvxViewModelInstanceRequest).ViewModelInstance);
                DataContext = ViewModel;
            }
        }

        public CatalogSearchHeaderVM ViewModel { get; set; }
        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (CatalogSearchHeaderVM)value;
            }
        }

        #endregion

        #region Constructor

        public CatalogSearchHeader()
        {
            Initialize();
        }

        public CatalogSearchHeader(IntPtr handle)
            : base(handle)
        {
            Initialize();
            this.DelayBind(OnBindContent);
        }

        #endregion

        #region Private

        private void Initialize()
        {
            Frame = new CoreGraphics.CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 44);
            AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;

            _searchBar = new BindableSearchBar(Bounds);

            SetupSearchBar(_searchBar);

            AddSubview(_searchBar);
        }

        private void OnBindContent()
        {
            var set = this.CreateBindingSet<CatalogSearchHeader, CatalogSearchHeaderVM>();

            set.Bind(_searchBar).To(vm => vm.SearchText);
            set.Bind(_searchBar).For(sb => sb.SearchCommand).To(vm => vm.SearchCommand);
            set.Bind(_searchBar).For(sb => sb.CancelCommand).To(vm => vm.CancelSearchCommand);

            set.Apply();
        }

        #endregion

        #region Protected

        protected virtual void SetupSearchBar(BindableSearchBar searchBar)
        {
            searchBar.SetupStyle(ThemeConfig.SearchBar);
        }

        #endregion
    }
}