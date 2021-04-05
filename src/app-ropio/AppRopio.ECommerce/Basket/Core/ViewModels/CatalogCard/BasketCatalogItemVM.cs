using System.Threading.Tasks;
using System.Windows.Input;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard;
using MvvmCross;

namespace AppRopio.ECommerce.Basket.Core.ViewModels.CatalogCard
{
    public class BasketCatalogItemVM : BasketProductCardViewModel, IBasketCatalogItemVM
    {
        private ICommand _actionCommand;
        public ICommand ActionCommand
        {
            get
            {
                return _actionCommand ?? (_actionCommand = BuyCommand);
            }
        }

        public string ActionText { get; protected set; }

        protected override string AnalyticsPrefix => "catalog_item";

        protected override async Task OnBuyCommandExecute()
        {
            if (Quantity == default(float)) {
                await base.OnBuyCommandExecute();
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

        public BasketCatalogItemVM()
        {
            //init here so the value gets assigned on the catalog loading instead of on accessing (post-loading)
            ActionText = Mvx.IoCProvider.Resolve<ILocalizationService>().GetLocalizableString(BasketConstants.RESX_NAME, "BasketCatalogCard_AddToBasket");
        }
    }
}
