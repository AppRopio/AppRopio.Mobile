using System.Collections.Generic;
using System.Threading.Tasks;
using AppRopio.Models.Filters.Responses;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> LoadProductsInCategory(string categoryId,
                                                   int offset = 0,
                                                   int count = 10,
                                                   string searchText = null,
                                                   List<ApplyedFilter> filters = null,
                                                   SortType sort = null);

        Task<ProductDetails> LoadProductDetails(string groupId, string productId);

        Task<Product> ReloadProductByParameter(string groupId, string productId, IEnumerable<ApplyedProductParameter> applyedParameters);

        Task MarkProduct(string groupId, string productId, bool isMarked);

        Task<ProductShare> GetProductForShare(string groupId, string productId);

        Task<List<Product>> LoadProductsCompilationForParameter(string groupId, string productId, string parameterId);

        Task<List<Shop>> LoadShopsCompilationForParameter(string groupId, string productId, string parameterId);

        Task<Product> LoadProductShortInfo(string groupId, string productId);
    }
}
