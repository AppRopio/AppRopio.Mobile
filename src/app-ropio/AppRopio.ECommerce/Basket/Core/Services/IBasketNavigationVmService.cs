using AppRopio.Base.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.Payments.Core.Bundle;

namespace AppRopio.ECommerce.Basket.Core.Services
{
    public interface IBasketNavigationVmService
    {
        void NavigateToProduct(ProductCardBundle bundle);

        void NavigateToProducts(BaseBundle bundle);

        void NavigateToOrder(BasketBundle bundle);

        void NavigateToOrderFieldAutocomplete(OrderFieldAutocompleteBundle bundle);

        void NavigateToDeliveryTypes(BasketBundle bundle);

        void NavigateToDelivery(DeliveryBundle bundle);

        void NavigateToDeliveryPointOnMap(DeliveryPointBundle bundle);

        void NavigateToDeliveryPointAdditionalInfo(DeliveryPointBundle bundle);

        void NavigateToPayment(PaymentBundle bundle);

        void NavigateToInAppPayment(PaymentOrderBundle bundle);

        void NavigateToBasket(BaseBundle bundle);

        void NavigateToThanks(BaseBundle bundle);
    }
}
