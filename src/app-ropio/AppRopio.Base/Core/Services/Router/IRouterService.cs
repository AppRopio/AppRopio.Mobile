using System;
using AppRopio.Base.Core.Models.Bundle;

namespace AppRopio.Base.Core.Services.Router
{
    public interface IRouterService
    {
        bool NavigatedTo(string type, BaseBundle bundle = null);

        void Register<TEntryPoint>(IRouterSubscriber subscriber)
            where TEntryPoint : class;

        void Register(IRouterSubscriber subscriber, params Type[] types);
    }
}
