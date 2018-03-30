using System;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Platform;
using AppRopio.Base.Core.Messages.Analytics;

namespace AppRopio.Analytics.GoogleAnalytics.Core.Services.Implementation
{
    public class NotificationsSubscriber : INotificationsSubscriber
    {
        #region Fields

        private MvxSubscriptionToken _screenToken;

        private MvxSubscriptionToken _eventToken;

        private MvxSubscriptionToken _orderToken;

        private MvxSubscriptionToken _exceptionToken;

        #endregion

        #region Services

        protected IMvxMessenger Messenger { get { return Mvx.Resolve<IMvxMessenger>(); } }

        protected IGoogleAnalytics GoogleAnalytics { get { return Mvx.CanResolve<IGoogleAnalytics>() ? Mvx.Resolve<IGoogleAnalytics>() : null; } }

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
            try
            {
                GoogleAnalytics?.TrackScreen(msg.ScreenName);
            }
            catch {}
        }

        private void HandleEventNotification(EventAnalyticsMessage msg)
        {
            try
            {
                GoogleAnalytics?.TrackEvent(msg.Category, msg.Action, msg.Label);
            }
            catch {}
        }

        private void HandleOrderNotification(OrderAnalyticsMessage msg)
        {
            try
            {
                GoogleAnalytics?.TrackECommerce(msg.FullPrice, msg.OrderId, msg.Currency);
            }
            catch { }
        }

        private void HandleExceptionNotification(ExceptionAnalyticsMessage msg)
        {
            try
            {
                GoogleAnalytics?.TrackException(msg.Message, msg.IsFatal);
            }
            catch {}
        }

        #endregion
   }
}
