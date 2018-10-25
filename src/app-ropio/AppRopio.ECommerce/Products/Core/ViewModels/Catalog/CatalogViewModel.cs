using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Attributes;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Core.ViewModels.Search;
using AppRopio.Base.Filters.Core.Messages;
using AppRopio.Base.Filters.Core.Models.Bundle;
using AppRopio.Base.Filters.Core.ViewModels.Sort;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Header;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Services;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog
{
    [Deeplink("products")]
    public class CatalogViewModel : SearchViewModel, ICatalogViewModel
    {
        #region Fields

        protected MvxSubscriptionToken _productCardMarkedToken;

        protected MvxSubscriptionToken _sortChangedToken;
        protected MvxSubscriptionToken _filtersChangedToken;

        protected MvxSubscriptionToken _markChangedToken;

        protected int LOADING_PRODUCTS_COUNT = 10;

        #endregion

        #region Commands

        private IMvxCommand _catalogCommand;
        public IMvxCommand CatalogCommand => _catalogCommand ?? (_catalogCommand = new MvxCommand(OnCatalogExecute));

        private ICommand _selectionChangedCommand;
        public ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<ICatalogItemVM>(OnItemSelected));
            }
        }

        private IMvxCommand _loadMoreCommand;
        public IMvxCommand LoadMoreCommand
        {
            get
            {
                return _loadMoreCommand ?? (_loadMoreCommand = new MvxCommand(OnLoadMoreExecute, () => CanLoadMore && !LoadingMore));
            }
        }

        private ICommand _showSortCommand;
        public ICommand ShowSortCommand
        {
            get
            {
                return _showSortCommand ?? (_showSortCommand = new MvxCommand(OnShowSortExecute));
            }
        }

        private ICommand _showFiltersCommand;
        public ICommand ShowFiltersCommand
        {
            get
            {
                return _showFiltersCommand ?? (_showFiltersCommand = new MvxCommand(OnShowFiltersExecute));
            }
        }

        private IMvxCommand _reloadCommand;
        public IMvxCommand ReloadCommand
        {
            get
            {
                return _reloadCommand ?? (_reloadCommand = new MvxCommand(OnReloadExecute));
            }
        }

        private ICommand _searchCommand;
        public ICommand ShowSearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new MvxCommand(OnShowSearchExecute));
            }
        }


        #endregion

        #region Properties

        public virtual bool SearchBar => ConfigService.Config.SearchType == SearchType.Bar;

        protected string CategoryId { get; private set; }

        protected ISortViewModel SortVm { get; set; }

        protected List<ApplyedFilter> ApplyedFilters { get; set; }

        protected SortType Sort { get; set; }

        private bool _canLoadMore;
        public bool CanLoadMore 
        {
            get => _canLoadMore;
            set => SetProperty(ref _canLoadMore, value, nameof(CanLoadMore));
        }

        private bool _loadingMore;
        public bool LoadingMore
        {
            get
            {
                return _loadingMore;
            }
            set
            {
                _loadingMore = value;
                RaisePropertyChanged(() => LoadingMore);
            }
        }

        private bool _reloading;
        public bool Reloading
        {
            get
            {
                return _reloading;
            }
            set
            {
                _reloading = value;
                RaisePropertyChanged(() => Reloading);
            }
        }

        private IMvxViewModel _headerVm;
        public IMvxViewModel HeaderVm
        {
            get
            {
                return _headerVm;
            }
            set
            {
                _headerVm = value;
                RaisePropertyChanged(() => HeaderVm);
            }
        }

        private MvxObservableCollection<ICatalogItemVM> _items;
        public MvxObservableCollection<ICatalogItemVM> Items
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

        public virtual bool SearchEnabled
        {
            get
            {
                return true;
            }
        }

        private bool _empty;
        public bool Empty
        {
            get => _empty;
            set => SetProperty(ref _empty, value, nameof(Empty));
        }

        private IMvxViewModel _cartIndicatorVM;
        public IMvxViewModel CartIndicatorVM
        {
            get => _cartIndicatorVM;
            set => SetProperty(ref _cartIndicatorVM, value, nameof(CartIndicatorVM));
        }

        public virtual string CatalogTitle => LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "ContentSearch_NoResultsCatalog");

        public virtual string NoResultsTitle => LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "ContentSearch_NoResultsTitle");

        public virtual string NoResultsText => LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "ContentSearch_NoResultsText");

        #endregion

        #region Services

        protected ICatalogVmService VmService { get { return Mvx.Resolve<ICatalogVmService>(); } }

        protected IMarkProductVmService MarkProductVmService { get { return Mvx.Resolve<IMarkProductVmService>(); } }

        protected IProductsNavigationVmService NavigationVmService { get { return Mvx.Resolve<IProductsNavigationVmService>(); } }

        private IProductsVmService _productsVmService;
        public IProductsVmService ProductsVmService => _productsVmService ?? (_productsVmService = Mvx.Resolve<IProductsVmService>());

        protected IProductConfigService ConfigService { get { return Mvx.Resolve<IProductConfigService>(); } }

        #endregion

        #region Constructor

        public CatalogViewModel()
        {
            VmNavigationType = ConfigService.Config.CategoriesType == Models.CategoriesType.Disabled ?
                                  Base.Core.Models.Navigation.NavigationType.ClearAndPush :
                                  Base.Core.Models.Navigation.NavigationType.Push;

            Items = new MvxObservableCollection<ICatalogItemVM>();
        }

        #endregion

        #region Private

        private async Task LoadContent()
        {
            Loading = true;

            await LoadItemsOnStart();

            SubscribeOnItemMarkChanged();

            Loading = false;
        }

        private void OnItemSelected(ICatalogItemVM item)
        {
            if (_productCardMarkedToken == null)
                _productCardMarkedToken = Messenger.Subscribe<ProductCardMarkedMessage>(OnProductCardMarkedMessage);

            NavigationVmService.NavigateToProduct(new ProductCardBundle(item.Model, Base.Core.Models.Navigation.NavigationType.DoublePush));
        }

        private void InitializeSearchHeader(CatalogSearchHeaderVM headerVM)
        {
            headerVM.SearchTextChanged = OnSearchTextChanged;
            headerVM.SearchExecute = SearchCommandExecute;
            headerVM.CancelExecute = CancelSearchExecute;
        }

        private void InitializeSortHeader(CatalogSortFiltersHeaderVM headerVM)
        {
            headerVM.SortExecute = OnShowSortExecute;
            headerVM.FiltersExecute = OnShowFiltersExecute;
        }

        private void ReleaseSubscriptionTokens()
        {
            if (_sortChangedToken != null)
            {
                Messenger.Unsubscribe<SortChangedMessage>(_sortChangedToken);
                _sortChangedToken = null;
            }

            if (_filtersChangedToken != null)
            {
                Messenger.Unsubscribe<FiltersChangedMessage>(_filtersChangedToken);
                _filtersChangedToken = null;
            }

            if (_markChangedToken != null)
            {
                Messenger.Unsubscribe<ProductMarkChangedMessage>(_markChangedToken);
                _markChangedToken = null;
            }

            if (_productCardMarkedToken != null)
            {
                Messenger.Unsubscribe<ProductCardMarkedMessage>(_productCardMarkedToken);
                _productCardMarkedToken = null;
            }
        }

        #endregion

        #region Protected

        protected virtual void LoadHeaderVm()
        {
            HeaderVm = VmService.LoadHeaderVm();

            if (HeaderVm != null)
            {
                var catalogSearchHeader = HeaderVm as CatalogSearchHeaderVM;
                var catalogSortHeader = HeaderVm as CatalogSortFiltersHeaderVM;

                if (catalogSearchHeader != null)
                    InitializeSearchHeader(catalogSearchHeader);
                else if (catalogSortHeader != null)
                    InitializeSortHeader(catalogSortHeader);

                InitializeHeaderVm();
            }
        }

        protected virtual async Task LoadItemsOnStart()
        {
            await LoadItems();
        }

        protected virtual void SubscribeOnItemMarkChanged()
        {
            if (_markChangedToken != null)
            {
                Messenger.Unsubscribe<ProductMarkChangedMessage>(_markChangedToken);
                _markChangedToken = null;
            }

            _markChangedToken = Messenger.Subscribe<ProductMarkChangedMessage>(OnMarkChangedMsgRecieved);
        }

        protected virtual void InitializeHeaderVm()
        {

        }

        protected virtual async Task LoadItems()
        {
            Loading = true;

            var dataSource = await VmService.LoadProductsInCategory(
                    CategoryId,
                    Items?.Count ?? 0,
                    LOADING_PRODUCTS_COUNT,
                    SearchText,
                    ApplyedFilters,
                    Sort
                );

            InvokeOnMainThread(() =>
            {
                Items = dataSource;
                Empty = Items.IsNullOrEmpty();
            });

            CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_PRODUCTS_COUNT;
            LoadMoreCommand.RaiseCanExecuteChanged();

            Loading = false;
        }

        protected virtual void OnReloadExecute()
        {
            if (Loading || Reloading)
                return;

            Task.Run(async () =>
            {
                Reloading = true;

                var dataSource = await VmService.LoadProductsInCategory(
                    CategoryId,
                    Items?.Count ?? 0,
                    LOADING_PRODUCTS_COUNT,
                    SearchText,
                    ApplyedFilters,
                    Sort
                );

                InvokeOnMainThread(() =>
                {
                    Items = dataSource;
                    Empty = Items.IsNullOrEmpty();
                });

                CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_PRODUCTS_COUNT;
                LoadMoreCommand.RaiseCanExecuteChanged();

                Reloading = false;
            });
        }

        protected virtual void OnLoadMoreExecute()
        {
            if (Loading || LoadingMore)
                return;
            
            LoadingMore = true;
            LoadMoreCommand.RaiseCanExecuteChanged();

            Task.Run(async () =>
            {
                var dataSource = await VmService.LoadProductsInCategory(
                    CategoryId,
                    Items.Count,
                    LOADING_PRODUCTS_COUNT,
                    SearchText,
                    ApplyedFilters,
                    Sort
                );

                CanLoadMore = !dataSource.IsNullOrEmpty() && dataSource.Count == LOADING_PRODUCTS_COUNT;

                if (!dataSource.IsNullOrEmpty())
                    InvokeOnMainThread(() =>
                    {
                        Items.AddRange(dataSource);
                    });

                LoadingMore = false;
                LoadMoreCommand.RaiseCanExecuteChanged();
            });
        }

        protected virtual void OnShowSortExecute()
        {
            if (_sortChangedToken == null)
                _sortChangedToken = Messenger.Subscribe<SortChangedMessage>(OnSortChangedMsgRecieved);

            NavigationVmService.NavigateToSort(new SortBundle(CategoryId, Base.Core.Models.Navigation.NavigationType.PresentModal, Sort?.Id));
        }

        protected virtual void OnShowFiltersExecute()
        {
            if (_filtersChangedToken == null)
                _filtersChangedToken = Messenger.Subscribe<FiltersChangedMessage>(OnFiltersChangedMsgRecieved);

            NavigationVmService.NavigateToFilters(new FiltersBundle(CategoryId, Base.Core.Models.Navigation.NavigationType.Push, ApplyedFilters));
        }

        protected virtual void OnShowSearchExecute()
        {
            NavigationVmService.NavigateToContentSearch(new BaseBundle(Base.Core.Models.Navigation.NavigationType.Push));
        }

        protected virtual void OnCatalogExecute()
        {
            if (VmNavigationType == NavigationType.InsideScreen)
                NavigationVmService.NavigateToMain(new BaseBundle(NavigationType.ClearAndPush));
            else
            {
                SearchText = string.Empty;

                ApplyedFilters?.Clear();

                Items?.Clear();

                Task.Run(LoadItems);
            }
        }

        protected virtual void OnSortChangedMsgRecieved(SortChangedMessage msg)
        {
            if (Loading || msg.CategoryId != CategoryId)
                return;

            Sort = msg.Sort;

            Items?.Clear();

            Task.Run(LoadItems);
        }

        protected virtual void OnFiltersChangedMsgRecieved(FiltersChangedMessage msg)
        {
            if (Loading || msg.CategoryId != CategoryId)
                return;

            ApplyedFilters = msg.ApplyedFilters;

            Items?.Clear();

            Task.Run(LoadItems);
        }

        protected virtual void OnMarkChangedMsgRecieved(ProductMarkChangedMessage msg)
        {
            MarkProductVmService.MarkProductAsFavorite(msg, onSuccess: (groupId, productId, isMarked) => 
            {
                Messenger.Publish(new ProductMarkedQuantityChangedMessage(this));
            }, onFailure: (groupId, productId, isMarked) =>
            {
                var item = Items.FirstOrDefault(i => (!i.Model.GroupId.IsNullOrEmpty() && i.Model.GroupId == groupId) || i.Model.Id == productId);
                if (item != null)
                    item.Marked = !isMarked;
            });
        }

        protected virtual void OnProductCardMarkedMessage(ProductCardMarkedMessage msg)
        {
            var item = Items.FirstOrDefault(i => (!i.Model.GroupId.IsNullOrEmpty() && i.Model.GroupId == msg.Model.GroupId) || i.Model.Id == msg.Model.Id);
            if (item != null)
                item.Marked = msg.Marked;
        }

        #region Init

        public override void Prepare(IMvxBundle parameters)
        {
            base.Prepare(parameters);

            var productsBundle = parameters.ReadAs<ProductsBundle>();
            this.InitFromBundle(productsBundle);

            LoadHeaderVm();
        }

        protected virtual async void InitFromBundle(ProductsBundle parameters)
        {
            Title = parameters.CategoryName;
            CategoryId = parameters.CategoryId;
            SearchText = parameters.SearchText;
            VmNavigationType = parameters.NavigationType;

            if (VmNavigationType != NavigationType.InsideScreen)
            {
                CartIndicatorVM = await ProductsVmService.LoadCartIndicatorViewModel(); 

                if (CartIndicatorVM != null)
                    await CartIndicatorVM.Initialize();
            }
        }

        #endregion

        #region Search

        protected override void OnSearchTextChanged(string searchText)
        {
            SearchText = searchText;
        }

        protected override void SearchCommandExecute()
        {
            if (Loading)
                return;

            Items?.Clear();

            Task.Run(LoadItems);
        }

        protected override void CancelSearchExecute()
        {
            if (Loading)
                return;

            SearchText = string.Empty;

            var searchHeader = HeaderVm as ISearchViewModel;
            if (searchHeader != null)
                searchHeader.SearchText = string.Empty;

            Task.Run(LoadItems);
        }

        #endregion

        #endregion

        #region Public

        public override Task Initialize()
        {
            return LoadContent();
        }

        public override void Unbind()
        {
            base.Unbind();

            if (!Items.IsNullOrEmpty()) {
                foreach (var item in Items) {
                    (item as BaseViewModel)?.Unbind();
                }
            }

            ReleaseSubscriptionTokens();
        }

        #endregion
    }
}

