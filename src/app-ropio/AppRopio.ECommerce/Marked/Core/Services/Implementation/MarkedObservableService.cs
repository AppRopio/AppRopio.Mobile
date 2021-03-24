using System;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.Core.Messages.Module;
using AppRopio.ECommerce.Marked.API.Services;
using AppRopio.ECommerce.Marked.Core.ViewModels.Marked;
using AppRopio.ECommerce.Products.Core.Messages;
using MvvmCross;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Marked.Core.Services.Implementation
{
    public class MarkedObservableService : IMarkedObservableService
    {
        private MvxSubscriptionToken _listToken;
        private MvxSubscriptionToken _cardToken;

        private IMarkedService _apiService;
        protected IMarkedService ApiService => _apiService ?? (_apiService = Mvx.Resolve<IMarkedService>());

        protected IMvxMessenger Messenger => Mvx.Resolve<IMvxMessenger>();

        public MarkedObservableService()
        {
            _listToken = Messenger.Subscribe<ProductMarkedQuantityChangedMessage>(OnMarkedChanged);
            _cardToken = Messenger.Subscribe<ProductCardMarkedMessage>(OnMarkedChanged);

            Mvx.CallbackWhenRegistered<IMarkedService>(() => OnMarkedChanged(null));
        }

        private void OnMarkedChanged(MvxMessage msg)
        {
            Task.Run(async () =>
            {
                try
                {
                    var quantity = await ApiService.GetQuantity();
                    if (Mvx.CanResolve<IMvxMessenger>() && Messenger.HasSubscriptionsFor<ModulesInteractionMessage<int>>())
                        Messenger.Publish(new ModulesInteractionMessage<int>(this, quantity) { Type = typeof(IMarkedViewModel) });
                    else
                    {
                        Timer timer = null;
                        timer = new Timer((state) =>
                        {
                            if (timer != null && Mvx.CanResolve<IMvxMessenger>() && Messenger.HasSubscriptionsFor<ModulesInteractionMessage<int>>())
                            {
                                Messenger.Publish(new ModulesInteractionMessage<int>(this, quantity) { Type = typeof(IMarkedViewModel) });
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
