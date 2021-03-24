using System;
using AppRopio.Base.Core.Messages.Analytics;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Analytics.MobileCenter.Core.Services.Implementation
{
    public class NotificationsSubscriber : INotificationsSubscriber
    {
        #region Fields

        private MvxSubscriptionToken _screenToken;

        private MvxSubscriptionToken _eventToken;

        private MvxSubscriptionToken _appToken;

        private MvxSubscriptionToken _exceptionToken;

        #endregion

        #region Services

        protected IMvxMessenger Messenger { get { return Mvx.Resolve<IMvxMessenger>(); } }

        protected IMobileCenter MobileCenter { get { return Mvx.CanResolve<IMobileCenter>() ? Mvx.Resolve<IMobileCenter>() : null; } }

        #endregion

        #region Constructor

        public NotificationsSubscriber()
        {
            Mvx.CallbackWhenRegistered<IMvxMessenger>(() =>
            {
                _appToken = Messenger.Subscribe<AppAnalyticsMessage>(HandleAppNotification);
                _screenToken = Messenger.Subscribe<ScreenAnalyticsMessage>(HandleScreenNotification);
                _eventToken = Messenger.Subscribe<EventAnalyticsMessage>(HandleEventNotification);
                _exceptionToken = Messenger.Subscribe<ExceptionAnalyticsMessage>(HandleExceptionNotification);
            });
        }

        #endregion

        #region Private

        private void HandleAppNotification(AppAnalyticsMessage msg)
        {
            try
            {
                if (MobileCenter == null)
                    Mvx.CallbackWhenRegistered<IMobileCenter>(() => MobileCenter.TrackApp(msg.State, msg.Data));
                else
                    MobileCenter.TrackApp(msg.State, msg.Data);
            }
            catch {}
        }

        private void HandleScreenNotification(ScreenAnalyticsMessage msg)
        {
            try
            {
                if (MobileCenter == null)
                    Mvx.CallbackWhenRegistered<IMobileCenter>(() => MobileCenter.TrackScreen(msg.ScreenName, msg.ScreenState, msg.Data));
                else
                    MobileCenter.TrackScreen(msg.ScreenName, msg.ScreenState, msg.Data);
            }
            catch {}
        }

        private void HandleEventNotification(EventAnalyticsMessage msg)
        {
            try
            {
                if (MobileCenter == null)
                    Mvx.CallbackWhenRegistered<IMobileCenter>(() => MobileCenter.TrackEvent(msg.Category, msg.Action, msg.Label, msg.Data));
                else
                    MobileCenter.TrackEvent(msg.Category, msg.Action, msg.Label, msg.Data);
            }
            catch {}
        }

        private void HandleExceptionNotification(ExceptionAnalyticsMessage msg)
        {
            try
            {
                if (MobileCenter == null)
                    Mvx.CallbackWhenRegistered<IMobileCenter>(() => MobileCenter.TrackException(msg.Message, msg.StackTrace, msg.Data));
                else
                    MobileCenter.TrackException(msg.Message, msg.StackTrace, msg.Data);
            }
            catch {}
        }

        #endregion
    }
}
