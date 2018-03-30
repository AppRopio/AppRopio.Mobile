using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppRopio.Base.Core.Services.ViewModelLookup
{
    public interface IViewModelLookupService
    {
        List<Assembly> Assemblies { get; }

        void Register<TInterface>(Type concreteType) where TInterface : class;
        void Register<TInterface, TModel>() where TInterface : class where TModel : class, TInterface;
        void RegisterDeeplink(Type concreteType);

        void Replace<TInterface>(Type concreteType) where TInterface : class;
        void Replace<TInterface, TModel>() where TInterface : class where TModel : class, TInterface;
        void ReplaceDeeplink(Type concreteType);

        Type Resolve(string concreteType);
        Type Resolve(Type concreteType);
        Type Resolve<TInterface>() where TInterface : class;
        Type ResolveDeeplink(string scheme);

        bool IsRegistered(string concreteType);
        bool IsRegistered(Type concreteType);
        bool IsRegistered<TInterface>() where TInterface : class;
        bool IsRegisteredDeeplink(string scheme);

        void CallbackWhenDeeplinkRegistered(string scheme, Action<Type> callback);
        void CallbackWhenRegistered(Type concreteType, Action<Type> callback);

        IDictionary<Type, Type> GetInterfaceViewModelLookup();
    }
}

