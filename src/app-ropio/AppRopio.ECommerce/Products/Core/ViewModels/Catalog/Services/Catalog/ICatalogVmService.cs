using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Services
{
    public interface ICatalogVmService
    {
        Task<MvxObservableCollection<ICatalogItemVM>> LoadProductsInCategory(string categoryId,
                                                   int offset = 0,
                                                   int count = 10,
                                                   string searchText = null,
                                                   List<ApplyedFilter> filters = null,
                                                   SortType sort = null);

        IMvxViewModel LoadHeaderVm();
    }
}
