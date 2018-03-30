using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Models.Filters.Responses;
using AppRopio.Models.Products.Requests;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.API.Services.Implementation
{
    public class ProductService : BaseService, IProductService
    {
        protected string PRODUCTS_URL = "products";
        protected string PRODUCT_DETAILS_URL = "product";
        protected string PRODUCT_MARK = "product/marked";
        protected string PRODUCT_SHARE = "product/share";
        protected string PRODUCT_RELOAD = "product/parameters";
        protected string PRODUCT_COMPILATIONS = "product/parameter_products";
        protected string PRODUCT_SHOPS_COMPILATIONS = "product/parameter_shops";
        protected string PRODUCT_SHORT = "product/shortInfo";

        public async Task<List<Product>> LoadProductsInCategory(string categoryId, int offset = 0, int count = 10, string searchText = null, List<ApplyedFilter> filters = null, SortType sort = null)
        {
            return await Post<List<Product>>(PRODUCTS_URL,
                       ToStringContent(
                           new ProductsRequest { CategoryId = categoryId, Offset = offset, Count = count, SearchText = searchText, Filters = filters, SortType = sort })
                      );
        }

        public async Task<ProductDetails> LoadProductDetails(string groupId, string productId)
        {
            var groupIdStr = string.IsNullOrEmpty(groupId) ? string.Empty : $"groupId={groupId}";
            var productIdStr = string.IsNullOrEmpty(productId) ? string.Empty : $"{(string.IsNullOrEmpty(groupIdStr) ? string.Empty : "&")}productId={productId}";

            return await Get<ProductDetails>($"{PRODUCT_DETAILS_URL}?{groupIdStr}{productIdStr}");
        }

        public async Task<Product> ReloadProductByParameter(string groupId, string productId, IEnumerable<ApplyedProductParameter> applyedParameters)
        {
            return await Post<Product>(PRODUCT_RELOAD, ToStringContent(new ApplyedParametersRequest { GroupId = groupId, ProductId = productId, Parameters = applyedParameters.ToList() }));
        }

        public async Task<List<Product>> LoadProductsCompilationForParameter(string groupId, string productId, string parameterId)
        {
            return await Post<List<Product>>(PRODUCT_COMPILATIONS, ToStringContent(new ProductsCompilationRequest { GroupId = groupId, ProductId = productId, ParameterId = parameterId }));
        }

        public async Task<List<Shop>> LoadShopsCompilationForParameter(string groupId, string productId, string parameterId)
        {
            return await Post<List<Shop>>(PRODUCT_SHOPS_COMPILATIONS, ToStringContent(new ProductsCompilationRequest { GroupId = groupId, ProductId = productId, ParameterId = parameterId }));
        }

        public async Task MarkProduct(string groupId, string productId, bool isMarked)
        {
            await Post(PRODUCT_MARK, ToStringContent(new ProductMarkRequest { GroupId = groupId, ProductId = productId, IsMarked = isMarked }));
        }

        public async Task<ProductShare> GetProductForShare(string groupId, string productId)
        {
            return await Post<ProductShare>(PRODUCT_SHARE, ToStringContent(new ProductRequest { GroupId = groupId, ProductId = productId }));
        }

        public async Task<Product> LoadProductShortInfo(string groupId, string productId)
        {
            var groupIdStr = string.IsNullOrEmpty(groupId) ? string.Empty : $"groupId={groupId}";
            var productIdStr = string.IsNullOrEmpty(productId) ? string.Empty : $"{(string.IsNullOrEmpty(groupIdStr) ? string.Empty : "&")}productId={productId}";

            return await Get<Product>($"{PRODUCT_SHORT}?{groupIdStr}{productIdStr}");
        }
    }
}
