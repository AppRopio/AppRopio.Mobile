using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.Core.Messages.Module;
using AppRopio.ECommerce.Basket.API.Services;
using AppRopio.ECommerce.Basket.Core.Messages;
using AppRopio.ECommerce.Basket.Core.Messages.Order;
using AppRopio.ECommerce.Basket.Core.ViewModels;
using MvvmCross;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Basket.Core.Services.Implementation
{
    public class BasketObservableService : IBasketObservableService
    {
        private MvxSubscriptionToken _token;
        private MvxSubscriptionToken _quantityToken;
        private MvxSubscriptionToken _orderFinishedToken;

        protected IBasketService ApiService => Mvx.Resolve<IBasketService>();

        protected IMvxMessenger Messenger => Mvx.Resolve<IMvxMessenger>();

        public BasketObservableService()
        {
            _token = Messenger.Subscribe<ProductAddToBasketMessage>(OnProductAddToBasket);
            _quantityToken = Messenger.Subscribe<ProductQuantityChangedMessage>(OnBasketChanged);
            _orderFinishedToken = Messenger.Subscribe<OrderCreationFinishedMessage>(OnBasketChanged);

            Mvx.CallbackWhenRegistered<IBasketService>(() => OnProductAddToBasket(null));
        }

        private void OnBasketChanged(MvxMessage msg)
        {
            Task.Run(async () =>
            {
                try
                {
                    var quantity = await ApiService.GetQuantity();
                    if (Mvx.CanResolve<IMvxMessenger>() && Messenger.HasSubscriptionsFor<ModulesInteractionMessage<int>>())
                        Messenger.Publish(new ModulesInteractionMessage<int>(this, quantity) { Type = typeof(IBasketViewModel) });
                }
                catch (Exception ex)
                {
                    MvxTrace.TaggedTrace(MvxTraceLevel.Warning, this.GetType().FullName, ex.BuildAllMessagesAndStackTrace());
                }
            });
        }

        private void OnProductAddToBasket(ProductAddToBasketMessage msg)
        {
            Task.Run(async () =>
            {
                try
                {
                    var quantity = await ApiService.GetQuantity();
                    if (Mvx.CanResolve<IMvxMessenger>() && Messenger.HasSubscriptionsFor<ModulesInteractionMessage<int>>())
                        Messenger.Publish(new ModulesInteractionMessage<int>(this, quantity) { Type = typeof(IBasketViewModel) });
                    else
                    {
                        Timer timer = null;
                        timer = new Timer((state) =>
                        {
                            if (timer != null && Mvx.CanResolve<IMvxMessenger>() && Messenger.HasSubscriptionsFor<ModulesInteractionMessage<int>>())
                            {
                                Messenger.Publish(new ModulesInteractionMessage<int>(this, quantity) { Type = typeof(IBasketViewModel) });
                                timer?.Dispose();
                                timer = null;
                            }
                        }, timer, 1000, 1000);
                    }
                }
                catch (Exception ex)
                {
                    MvxTrace.TaggedTrace(MvxTraceLevel.Warning, this.GetType().FullName, ex.BuildAllMessagesAndStackTrace());
                }
            });
        }
    }
}
