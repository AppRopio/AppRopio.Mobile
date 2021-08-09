using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items;
using AppRopio.Models.Products.Responses;
using MvvmCross.ViewModels;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Services
{
    public interface IProductCardVmService
    {
        Task<Product> LoadProductShortInfo(string groupId, string productId);

        Task<ObservableCollection<IProductBasicItemVM>> LoadBasicProductCardItems(Product model);

        Task<ObservableCollection<IProductDetailsItemVM>> LoadDetailsProductCardItems(string groupId, string productId);

        Task<Product> ReloadProductByParameters(string groupId, string productId, IEnumerable<ApplyedProductParameter> applyedParameters);

        IMvxViewModel LoadBasketBlock();
    }
}
