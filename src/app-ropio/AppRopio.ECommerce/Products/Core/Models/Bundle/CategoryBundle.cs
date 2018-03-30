using System.Collections.Generic;
using AppRopio.Base.Core.Attributes;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.ECommerce.Products.Core.Models.Bundle
{
    public class CategoryBundle : BaseBundle
    {
        [DeeplinkProperty("categoryId")]
        public string CategoryId { get; set; }

        [DeeplinkProperty("categoryName")]
        public string CategoryName { get; set; }

        public CategoryBundle()
        {

        }

        public CategoryBundle(string categoryId, string categoryName, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string>
            {
                { nameof(CategoryId), categoryId },
                { nameof(CategoryName), categoryName }
            })
        {

        }
    }
}
