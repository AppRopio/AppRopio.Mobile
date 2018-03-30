using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;

namespace AppRopio.Base.Core.Services.Router
{
    public class RouterService : IRouterService
    {
        #region Fields

        private readonly Dictionary<string, IRouterSubscriber> _subscribers = new Dictionary<string, IRouterSubscriber>();

        #endregion

        #region Services

        #endregion

        #region Constructor

        public RouterService()
        {
            if (AppSettings.ApiKey.IsNullOrEmtpy())
                throw new ArgumentNullException(nameof(AppSettings.ApiKey));
        }

        #endregion

        #region IRouterService implementation

        public bool NavigatedTo(string type, BaseBundle bundle = null)
        {
            IRouterSubscriber subscription = null;
            lock (this)
            {
                if (!_subscribers.TryGetValue(type, out subscription))
                    return false;
            }

            var succeeded = subscription.CanNavigatedTo(type, bundle);

            if (!succeeded)
                subscription.FailedNavigatedTo(type, bundle);

            return succeeded;
        }

        public void Register<TEntryPoint>(IRouterSubscriber subscriber)
            where TEntryPoint : class
        {
            Register(subscriber, typeof(TEntryPoint));
        }

        public void Register(IRouterSubscriber subscriber, params Type[] types)
        {
            if (subscriber == null)
                throw new ArgumentNullException(nameof(subscriber));
            if (types == null)
                throw new ArgumentNullException(nameof(types));

            lock (this)
            {
                foreach (var type in types)
                {
                    _subscribers[type.FullName] = subscriber;
                }
            }
        }

        #endregion
    }
}
