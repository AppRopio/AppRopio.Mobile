using System;
using System.Reflection;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Filters.Core.Models.Bundle;
using AppRopio.Base.Filters.Core.Services;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.ViewModels;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductTextContent;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductWebContent;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Core.Services.Implementation
{
    public class ProductsNavigationVmService : BaseVmNavigationService, IProductsNavigationVmService
    {
        protected IFiltersNavigationVmService FiltersNavigationVmService { get { return Mvx.IoCProvider.CanResolve<IFiltersNavigationVmService>() ? Mvx.IoCProvider.Resolve<IFiltersNavigationVmService>() : null; } }

        public async void NavigateToContentSearch(BaseBundle bundle)
        {
            await NavigateTo<IContentSearchViewModel>(bundle);
        }

        public async void NavigateToMain(BaseBundle bundle)
        {
            await NavigateTo<IProductsViewModel>(bundle);
        }

        public async void NavigateToSSCategory(CategoryBundle bundle)
        {
            await NavigateTo<ISSCategoriesViewModel>(bundle);
        }

        public async void NavigateToCategory(CategoryBundle bundle)
        {
            await NavigateTo<ICategoriesViewModel>(bundle);
        }

        public async void NavigateToProducts(ProductsBundle bundle)
        {
            await NavigateTo<ICatalogViewModel>(bundle);
        }

        public async void NavigateToProduct(ProductCardBundle bundle)
        {
            await NavigateTo<IProductCardViewModel>(bundle);
        }

        public void NavigateToFilters(FiltersBundle bundle)
        {
            FiltersNavigationVmService?.NavigateToFilters(bundle);
        }

        public void NavigateToSelection(Base.Filters.Core.Models.Bundle.SelectionBundle bundle)
        {
            FiltersNavigationVmService?.NavigateToSelection(bundle);
        }

        public void NavigateToSort(SortBundle bundle)
        {
            FiltersNavigationVmService?.NavigateToSort(bundle);
        }

        public async void NavigateToSelection(Models.Bundle.SelectionBundle bundle)
        {
            await NavigateTo<IProductDetailsSelectionViewModel>(bundle);
        }

        public async void NavigateToTextContent(BaseTextContentBundle bundle)
        {
            await NavigateTo<IProductTextContentViewModel>(bundle);
        }

        public async void NavigateToWebContent(BaseWebContentBundle bundle)
        {
            await NavigateTo<IProductWebContentViewModel>(bundle);
        }

        public async void NavigateToCustomType(Type customVmType, BaseBundle bundle)
        {
            await NavigateTo(customVmType, bundle);
        }
    }
}
