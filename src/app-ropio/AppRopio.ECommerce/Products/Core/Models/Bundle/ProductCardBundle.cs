using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Attributes;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Models.Products.Responses;
using Newtonsoft.Json;

namespace AppRopio.ECommerce.Products.Core.Models.Bundle
{
    public class ProductCardBundle : BaseBundle
    {
        [DeeplinkProperty("groupId")]
        public string GroupId { get; set; }

        [DeeplinkProperty("productId")]
        public string ProductId { get; set; }

        public string ProductRawString { get; set; }

        public Product Product { get { return ProductRawString.IsNullOrEmtpy() ? null : JsonConvert.DeserializeObject<Product>(ProductRawString); } }

        public ProductCardBundle()
        {

        }

        public ProductCardBundle(string groupId, string productId, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string>
            {
                { nameof(GroupId), groupId },
                { nameof(ProductId), productId }
            })
        {

        }

        public ProductCardBundle(Product product, NavigationType navigationType)
            : base(navigationType, new Dictionary<string, string>
            {
                { nameof(ProductRawString), JsonConvert.SerializeObject(product) }
            })
        {

        }
    }
}
