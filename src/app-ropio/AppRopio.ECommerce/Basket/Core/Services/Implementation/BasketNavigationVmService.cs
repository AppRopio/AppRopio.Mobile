using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Config;
using AppRopio.Base.Core.Services.Router;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Basket.Core.Models;
using AppRopio.ECommerce.Basket.Core.Models.Bundle;
using AppRopio.ECommerce.Basket.Core.ViewModels;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Partial;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Payment;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Thanks;
using AppRopio.ECommerce.Products.Core.Models.Bundle;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.Models.Basket.Responses.Enums;
using AppRopio.Models.Payments.Responses;
using AppRopio.Payments.Core.Bundle;
using MvvmCross;
using MvvmCross.Logging;

namespace AppRopio.ECommerce.Basket.Core.Services.Implementation
{
    public class BasketNavigationVmService : BaseVmNavigationService, IBasketNavigationVmService
    {
        #region Services

        protected IProductsNavigationVmService ProductNavigationVmService => Mvx.IoCProvider.Resolve<IProductsNavigationVmService>();

        #endregion

        #region Private

        private string GetPaymentModuleType(PaymentOrderBundle bundle, Dictionary<BasketInAppPaymentType, AssemblyElement> inAppPayments)
        {
            if (inAppPayments.TryGetValue(BasketInAppPaymentType.All, out AssemblyElement assembly))
                return assembly.TypeName;

            switch(bundle.PaymentType)
            {
                case PaymentType.CreditCard:
                    return inAppPayments.TryGetValue(BasketInAppPaymentType.CreditCard, out assembly) ? assembly.TypeName : string.Empty;
                case PaymentType.EWallet:
                    return inAppPayments.TryGetValue(BasketInAppPaymentType.EWallet, out assembly) ? assembly.TypeName : string.Empty;
                case PaymentType.Native:
                    return inAppPayments.TryGetValue(BasketInAppPaymentType.Native, out assembly) ? assembly.TypeName : string.Empty;
            }

            return string.Empty;
        }

        #endregion

        #region IBasketNavigationVmService implementation

        public void NavigateToProduct(ProductCardBundle bundle)
        {
            ProductNavigationVmService.NavigateToProduct(bundle);
        }

        public void NavigateToProducts(BaseBundle bundle)
        {
            ProductNavigationVmService.NavigateToMain(bundle);
        }

        public void NavigateToOrder(BasketBundle bundle)
        {
            NavigateTo<IOrderViewModel>(bundle);
        }

        public void NavigateToOrderFieldAutocomplete(OrderFieldAutocompleteBundle bundle)
        {
            NavigateTo<IOrderFieldAutocompleteVM>(bundle);
        }

        public void NavigateToDeliveryTypes(BasketBundle bundle)
        {
            NavigateTo<IDeliveryViewModel>(bundle);
        }

        public void NavigateToDelivery(DeliveryBundle bundle)
        {
            if (bundle.Type == DeliveryType.DeliveryPoint)
                NavigateTo<IDeliveryOnPointVM>(bundle);
            else
                NavigateTo<IDeliveryOnAddressVM>(bundle);
        }

        public void NavigateToDeliveryPointOnMap(DeliveryPointBundle bundle)
        {
            NavigateTo<IDeliveryPointOnMapVM>(bundle);
        }

        public void NavigateToDeliveryPointAdditionalInfo(DeliveryPointBundle bundle)
        {
            NavigateTo<IDeliveryPointAdditionalInfoVM>(bundle);
        }

        public void NavigateToPayment(PaymentBundle bundle)
        {
            NavigateTo<IPaymentViewModel>(bundle);
        }

		public void NavigateToInAppPayment(PaymentOrderBundle bundle)
		{
            var config = Mvx.IoCProvider.Resolve<IBasketConfigService>().Config;
            if (config.InAppPayments != null)
			{
                var type = GetPaymentModuleType(bundle, config.InAppPayments);

                if (!Mvx.IoCProvider.Resolve<IRouterService>().NavigatedTo(type, bundle))
                    Mvx.IoCProvider.Resolve<IMvxLog>().Error($"{this.GetType().FullName}: Can't navigate to ViewModel of type {type}");
			}
		}

        public void NavigateToBasket(BaseBundle bundle)
        {
            NavigateTo<IBasketViewModel>(bundle);
        }

		public void NavigateToThanks(BaseBundle bundle)
		{
			NavigateTo<IThanksViewModel>(bundle);
		}

        #endregion
    }
}
