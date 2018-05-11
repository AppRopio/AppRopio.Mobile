using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Attributes;
using AppRopio.Base.Core.Extentions;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Categories
{
    [Deeplink("category")]
    public class CCategoriesViewModel : ProductsViewModel, ICCategoriesViewModel
    {
        #region Fields

        private string _currentCategoryId;

        #endregion

        #region Commands

        private ICommand _selectionChangedCommand;
        public override ICommand SelectionChangedCommand
        {
            get
            {
                return _selectionChangedCommand ?? (_selectionChangedCommand = new MvxCommand<ICategoriesItemVM>(OnItemSelected));
            }
        }

        private ICommand _pageChangedCommand;
        public ICommand PageChangedCommand
        {
            get
            {
                return _pageChangedCommand ?? (_pageChangedCommand = new MvxCommand<int>(OnPageChanged));
            }
        }

        #endregion

        #region Properties

        private int _currentPage;
        public int CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                RaisePropertyChanged(() => CurrentPage);
            }
        }

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

        private ObservableCollection<ICatalogViewModel> _pages;
        public ObservableCollection<ICatalogViewModel> Pages
        {
            get
            {
                return _pages;
            }
            set
            {
                _pages = value;
                RaisePropertyChanged(() => Pages);
            }
        }

        #endregion

        #region Services

        private ICategoriesVmService _categoriesVmService;
        public ICategoriesVmService CategoriesVmService => _categoriesVmService ?? (_categoriesVmService = Mvx.Resolve<ICategoriesVmService>());

        private IProductsVmService _productsVmService;
        public IProductsVmService ProductsVmService => _productsVmService ?? (_productsVmService = Mvx.Resolve<IProductsVmService>());

        #endregion

        #region Contructor

        public CCategoriesViewModel()
        {
            Items = new ObservableCollection<ICategoriesItemVM>();
            Pages = new ObservableCollection<ICatalogViewModel>();
        }

        #endregion

        #region Private

        private async Task LoadContent()
        {
            Loading = true;

            var items = await CategoriesVmService.LoadItemsFor(_currentCategoryId);

            InvokeOnMainThread(() => Items = items);

            var pages = new ObservableCollection<ICatalogViewModel>();
            for (int i = 0; i < items.Count(); i++)
            {
                var category = items.ElementAt(i);

                var catalogVm = Activator.CreateInstance(LookupService.Resolve<ICatalogViewModel>()) as ICatalogViewModel;
                catalogVm.Prepare(new ProductsBundle(category.CategoryID, category.Name, null, Base.Core.Models.Navigation.NavigationType.None));
                (catalogVm as Base.Core.ViewModels.IMvxPageViewModel).PageIndex = i;

                pages.Add(catalogVm);
            }

            InvokeOnMainThread(() => Pages = pages);

            Loading = false;
        }

        #endregion

        #region Protected

        protected virtual void OnItemSelected(ICategoriesItemVM item)
        {
            CurrentPage = Items.IndexOf(item);
        }

        protected void OnPageChanged(int index)
        {
            CurrentPage = index;
        }

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
            _currentCategoryId = parameters.CategoryId;
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

        public override void Unbind()
        {
            base.Unbind();

            Pages?.ForEach(x => x.Unbind());
        }

        #endregion
    }
}

