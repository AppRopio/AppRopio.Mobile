using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using AppRopio.Models.Marked.Responses;

namespace AppRopio.ECommerce.Marked.Core.ViewModels.Marked.Items
{
    public class MarkedProductVM : CatalogItemVM
    {
        public MarkedProductVM(MarkedProduct product) 
            : base(product)
        {

        }
    }
}