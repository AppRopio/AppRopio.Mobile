using System.Collections.Generic;
using AppRopio.Base.Core.Attributes;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.ECommerce.Products.Core.Models.Bundle
{
    public class ProductsBundle : BaseBundle
    {
        [DeeplinkProperty("categoryId")]
        public string CategoryId { get; set; }

        [DeeplinkProperty("categoryName")]
        public string CategoryName { get; set; }

        [DeeplinkProperty("searchText")]
        public string SearchText { get; set; }

        public ProductsBundle()
        {

        }

        public ProductsBundle(string categoryId, string categoryName, string searchText, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string>
            {
                { nameof(categoryId), categoryId },
                { nameof(categoryName), categoryName },
                { nameof(searchText), searchText }
            })
        {
        }
    }
}
