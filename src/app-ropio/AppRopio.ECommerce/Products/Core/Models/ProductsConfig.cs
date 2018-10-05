using System;
using AppRopio.Base.Core.Models.Config;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.ECommerce.Products.Core.Models
{
    public class ProductsConfig
    {
        public CategoriesType CategoriesType { get; set; }

        public SearchType SearchType { get; set; }

        public NavigationType ProductCardNavigationType { get; set; }

        public BasketReference Basket { get; set; }

        public bool MarkedEnabled { get; set; }

        public bool UnitNameEnabled { get; set; }

        public AssemblyElement Header { get; set; }

        public ProductDetailsConfig ProductDetails { get; set; }

        public ProductsConfig() {
            SearchType = SearchType.Screen;
        }
    }

    public class BasketReference
    {
        public AssemblyElement AddToCart { get; set; }

        public AssemblyElement ItemAddToCart { get; set; }

        public AssemblyElement CartIndicator { get; set; }
    }

    public class ProductDetailsConfig
    {
        public List<ParameterCustomType> CustomTypes { get; set; }
    }

    public class ParameterCustomType
    {
        public string Name { get; set; }

        public AssemblyElement Assembly { get; set; }
    }
}

