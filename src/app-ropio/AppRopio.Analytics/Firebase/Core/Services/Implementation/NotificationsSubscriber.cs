using System;
using AppRopio.Base.Core.Messages.Analytics;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace AppRopio.Analytics.Firebase.Core.Services.Implementation
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

        protected IFirebaseService Firebase { get { return Mvx.CanResolve<IFirebaseService>() ? Mvx.Resolve<IFirebaseService>() : null; } }

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
                Firebase?.TrackScreen(msg.ScreenName);
            }
            catch {}
        }

        private void HandleEventNotification(EventAnalyticsMessage msg)
        {
            try
            {
                Firebase?.TrackEvent(msg.Category, msg.Action, msg.Label, msg.Model);
            }
            catch {}
        }

        private void HandleOrderNotification(OrderAnalyticsMessage msg)
        {
            try
            {
                Firebase?.TrackECommerce(msg.FullPrice, msg.OrderId, msg.Currency);
            }
            catch { }
        }

        private void HandleExceptionNotification(ExceptionAnalyticsMessage msg)
        {
            try
            {
                Firebase?.TrackException(msg.Message, msg.IsFatal);
            }
            catch {}
        }

        #endregion
    }
}
