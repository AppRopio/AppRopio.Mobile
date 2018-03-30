using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Items;
using AppRopio.Models.Basket.Responses.Basket;
using MvvmCross.Core.ViewModels;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.Basket.Services
{
    public interface IBasketVmService
    {
        BasketModel LoadedBasket { get; }

        Task<ObservableCollection<IBasketItemVM>> LoadItemsIfNeeded(string basketVersionId = null);

        Task<bool> DeleteItem(string productId);

        IMvxViewModel LoadLoyaltyVmIfExist();

        Task<bool> IsBasketValid(string versionId, CancellationToken token);

        Task<decimal> LoadBasketSummaryAmount();
    }
}
