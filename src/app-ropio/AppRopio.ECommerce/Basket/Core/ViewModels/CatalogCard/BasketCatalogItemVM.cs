using System.Threading.Tasks;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.CatalogCard
{
    public class BasketCatalogItemVM : BasketProductCardViewModel, IBasketCatalogItemVM
    {
        protected override string AnalyticsPrefix => "catalog_item";

        protected override void OnBuyCommandExecute()
        {
            if (Quantity == default(float)) {
                base.OnBuyCommandExecute();
            } else if (IncrementCommand != null && IncrementCommand.CanExecute()) {
                IncrementCommand.Execute();
            }
        }

        protected override async Task OnIncrementExecute()
        {
            float quantity = Quantity;
            await base.OnIncrementExecute();
            if (Quantity != quantity) {
                await OnProductAdded();
            }
        }
    }
}
