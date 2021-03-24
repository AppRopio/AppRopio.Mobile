using System;
using AppRopio.Base.Core.Messages.Analytics;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Analytics.AppsFlyer.Core.Services.Implementation
{
    public class NotificationsSubscriber: INotificationsSubscriber
    {
        #region Fields

        private MvxSubscriptionToken _screenToken;

        private MvxSubscriptionToken _eventToken;

        private MvxSubscriptionToken _orderToken;

        private MvxSubscriptionToken _exceptionToken;

        #endregion

        #region Services

        protected IMvxMessenger Messenger { get { return Mvx.Resolve<IMvxMessenger>(); } }

        protected IAppsFlyerService AppsFlyer { get { return Mvx.Resolve<IAppsFlyerService>(); } }

        #endregion

        #region Constructor

        public NotificationsSubscriber()
        {
            Mvx.CallbackWhenRegistered<IMvxMessenger>(() =>
            {
                _screenToken = Messenger.Subscribe<ScreenAnalyticsMessage>(HandleScreenNotification);
                _eventToken = Messenger.Subscribe<EventAnalyticsMessage>(HandleEventNotification);
                _orderToken = Messenger.Subscribe<OrderAnalyticsMessage>(HandleOrderNotification);
                _exceptionToken = Messenger.Subscribe<ExceptionAnalyticsMessage>(HandleExceptionNotification);
            });
        }

        #endregion

        #region Private

        private void HandleScreenNotification(ScreenAnalyticsMessage msg)
        {
            AppsFlyer.TrackScreen(msg.ScreenName);
        }

        private void HandleEventNotification(EventAnalyticsMessage msg)
        {
            AppsFlyer.TrackEvent(msg.Category, msg.Action, msg.Label, msg.Model);
        }

        private void HandleOrderNotification(OrderAnalyticsMessage msg)
        {
            AppsFlyer.TrackECommerce(msg.FullPrice, msg.Quantity, msg.OrderId, msg.Currency);
        }

        private void HandleExceptionNotification(ExceptionAnalyticsMessage msg)
        {
            AppsFlyer.TrackException(msg.Message, msg.IsFatal);
        }

        #endregion
    }
}
