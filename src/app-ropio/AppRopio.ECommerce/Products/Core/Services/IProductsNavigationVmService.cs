using System;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Filters.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.Models.Bundle;

namespace AppRopio.ECommerce.Products.Core.Services
{
    public interface IProductsNavigationVmService : IBaseVmNavigationService
    {
        void NavigateToContentSearch(BaseBundle bundle);

        void NavigateToMain(BaseBundle bundle);

        void NavigateToSSCategory(CategoryBundle bundle);

        void NavigateToCategory(CategoryBundle bundle);

        void NavigateToProducts(ProductsBundle bundle);

        void NavigateToFilters(FiltersBundle bundle);

        void NavigateToSelection(Base.Filters.Core.Models.Bundle.SelectionBundle bundle);

        void NavigateToSelection(Models.Bundle.SelectionBundle bundle);

        void NavigateToSort(SortBundle bundle);

        void NavigateToProduct(ProductCardBundle bundle);

        void NavigateToTextContent(BaseTextContentBundle bundle);

        void NavigateToWebContent(BaseWebContentBundle bundle);

        void NavigateToCustomType(Type customVmType, BaseBundle bundle);
    }
}
