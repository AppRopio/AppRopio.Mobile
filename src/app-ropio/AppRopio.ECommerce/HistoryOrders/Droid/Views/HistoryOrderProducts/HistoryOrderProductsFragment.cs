using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;

namespace AppRopio.ECommerce.HistoryOrders.Droid.Views.HistoryOrderProducts
{
    public class HistoryOrderProductsFragment : CommonFragment<IHistoryOrderProductsViewModel>
    {
        public HistoryOrderProductsFragment()
            : base (Resource.Layout.app_historyorders_products, "Состав заказа")
        {
            
        }
    }
}
