using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Attributes;
using AppRopio.Base.Core.Extentions;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items.Banners;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services.Banners;
using MvvmCross.ViewModels;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories
{
    [Deeplink("category")]
    public class SSCategoriesViewModel : ProductsViewModel, ISSCategoriesViewModel
    {
        #region Commands

        private ICommand _selectionChangedCommand;
        public override ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<ICategoriesItemVM>(OnItemSelected));
            }
        }

        private ICommand _bannerSelectionChangedCommand;
        public ICommand BannerSelectionChangedCommand
        {
            get
            {
                return _bannerSelectionChangedCommand ?? (_bannerSelectionChangedCommand = new MvxCommand<IBannerItemVM>(OnBannerItemSelected));
            }
        }

        #endregion

        #region Properties

        protected string ParentCategoryId { get; set; }

        public override bool SearchBar => ConfigService.Config.SearchType == SearchType.Bar
                                      || (ConfigService.Config.SearchType == SearchType.BarOnFirstScreen
                                          && string.IsNullOrEmpty(ParentCategoryId));

        private ObservableCollection<ICategoriesItemVM> _items;
        public ObservableCollection<ICategoriesItemVM> Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                RaisePropertyChanged(() => Items);
            }
        }

        private ObservableCollection<IBannerItemVM> _topBanners;
        public ObservableCollection<IBannerItemVM> TopBanners
        {
            get
            {
                return _topBanners;
            }
            set
            {
                _topBanners = value;
                RaisePropertyChanged(() => TopBanners);
            }
        }

        private ObservableCollection<IBannerItemVM> _bottomBanners;
        public ObservableCollection<IBannerItemVM> BottomBanners
        {
            get
            {
                return _bottomBanners;
            }
            set
            {
                _bottomBanners = value;
                RaisePropertyChanged(() => BottomBanners);
            }
        }

        #endregion

        #region Constructor

        public SSCategoriesViewModel()
        {
            VmNavigationType = Base.Core.Models.Navigation.NavigationType.ClearAndPush;
        }

        #endregion

        #region Services

        private ICategoriesVmService _categoriesVmService;
        public ICategoriesVmService CategoriesVmService => _categoriesVmService ?? (_categoriesVmService = Mvx.Resolve<ICategoriesVmService>());

        private IBannersVmService _bannersVmService;
        public IBannersVmService BannersVmService => _bannersVmService ?? (_bannersVmService = Mvx.Resolve<IBannersVmService>());

        private IProductsVmService _productsVmService;
        public IProductsVmService ProductsVmService => _productsVmService ?? (_productsVmService = Mvx.Resolve<IProductsVmService>());

        #endregion

        #region Private

        private async void OnItemSelected(ICategoriesItemVM item)
        {
            var categoriesType = ConfigService.Config.CategoriesType;
            if (categoriesType == CategoriesType.Mixed && item.IsFolder)
            {
                Loading = true;

                var categories = await CategoriesVmService.LoadItemsFor(item.CategoryID);

                if (!categories.IsNullOrEmpty() && !categories.All(x => x != null && !x.IsFolder))
                {
                    NavigationVmService.NavigateToSSCategory(new CategoryBundle(item.CategoryID, item.Name, Base.Core.Models.Navigation.NavigationType.DoublePush));
                    Loading = false;
                    return;
                }

                Loading = false;
            }
            
            if (item.IsFolder)
                NavigationVmService.NavigateToCategory(new CategoryBundle(item.CategoryID, item.Name, Base.Core.Models.Navigation.NavigationType.DoublePush));
            else
                NavigationVmService.NavigateToProducts(new ProductsBundle(item.CategoryID, item.Name, string.Empty, Base.Core.Models.Navigation.NavigationType.Push));
        }

        private void OnBannerItemSelected(IBannerItemVM item)
        {
            NavigationVmService.NavigateTo(item.Deeplink);
        }

        private async Task LoadContent()
        {
            Loading = true;

            TopBanners = await BannersVmService.LoadTopBannersFor(ParentCategoryId);
            Items = await CategoriesVmService.LoadItemsFor(ParentCategoryId);
            BottomBanners = await BannersVmService.LoadBottomBannersFor(ParentCategoryId);

            if (CartIndicatorVM != null)
                await CartIndicatorVM.Initialize();
            
            Loading = false;

            if (Items.IsNullOrEmpty())
                ContentState = Base.Core.Models.Content.ContentState.NoData;
            else
                ContentState = Base.Core.Models.Content.ContentState.HasData;
        }

        #endregion

        #region Protected

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var categoryBundle = parameters.ReadAs<CategoryBundle>();
            this.InitFromBundle(categoryBundle);
        }

        protected virtual async void InitFromBundle(CategoryBundle parameters)
        {
            Title = parameters.CategoryName ?? LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "CategoriesEmptyTitle");
            ParentCategoryId = parameters.CategoryId;
            VmNavigationType = parameters.NavigationType == Base.Core.Models.Navigation.NavigationType.None ? Base.Core.Models.Navigation.NavigationType.ClearAndPush : parameters.NavigationType;

            CartIndicatorVM = await ProductsVmService.LoadCartIndicatorViewModel();
        }

        #endregion

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        #endregion
    }
}

