using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppRopio.Base.Core.Attributes;

namespace AppRopio.Base.Core.Services.ViewModelLookup
{
    public class ViewModelLookupService : IViewModelLookupService
    {
        private readonly Dictionary<Type, Type> _viewModelLookups = new Dictionary<Type, Type>();

        private readonly List<KeyValuePair<Type, Action<Type>>> _viewModelLookupsCallbacks = new List<KeyValuePair<Type, Action<Type>>>();

        private readonly Dictionary<string, Type> _deeplinks = new Dictionary<string, Type>();

        private readonly List<KeyValuePair<string, Action<Type>>> _deeplinkCallbacks = new List<KeyValuePair<string, Action<Type>>>();

        public List<Assembly> Assemblies { get; private set; } = new List<Assembly>();

        public ViewModelLookupService()
        {
            if (AppSettings.ApiKey.IsNullOrEmtpy())
                throw new ArgumentNullException(nameof(AppSettings.ApiKey));
        }

        #region IViewModelLookupService implementation

        #region Registration

        private void RegisterDeeplinkInDictionary(Type concreteType, TypeInfo typeInfo)
        {
            var deeplinkAttribute = typeInfo.GetCustomAttribute<DeeplinkAttribute>();
            if (deeplinkAttribute != null && !_deeplinks.Any(d => d.Key == deeplinkAttribute.Scheme))
            {
                lock (_deeplinks)
                {
                    _deeplinks.Add(deeplinkAttribute.Scheme, concreteType);

                    lock (_deeplinkCallbacks)
                    {
                        if (_deeplinkCallbacks.Any(x => x.Key == deeplinkAttribute.Scheme))
                        {
                            _deeplinkCallbacks.Where(x => x.Key == deeplinkAttribute.Scheme).ForEach(x =>
                            {
                                x.Value?.Invoke(concreteType);
                            });
                        }
                    }
                }
            }
        }

        public void Register<TInterface, TModel>() where TInterface : class where TModel : class, TInterface
        {
            Register<TInterface>(typeof(TModel));
        }

        public void Register<TInterface>(Type concreteType)
            where TInterface : class
        {
            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));
            if (IsRegistered<TInterface>())
                throw new Exception(string.Format("Lookup для интерфейса '{0}' уже зарегистрирован! Используй Replace.", typeof(TInterface).FullName));

            lock (_viewModelLookups)
            {
                _viewModelLookups.Add(typeof(TInterface), concreteType);

                lock (_viewModelLookupsCallbacks)
                {
                    if (_viewModelLookupsCallbacks.Any(x => x.Key == concreteType))
                    {
                        _viewModelLookupsCallbacks.Where(x => x.Key == concreteType).ForEach(x => 
                        {
                            x.Value?.Invoke(concreteType);
                        });
                    }
                }
            }

            var typeInfo = concreteType.GetTypeInfo();
            var vmAssembly = typeInfo.Assembly;

            if (!Assemblies.Any(a => a.FullName == vmAssembly.FullName))
                Assemblies.Add(vmAssembly);

            RegisterDeeplinkInDictionary(concreteType, typeInfo);
        }

        public void RegisterDeeplink(Type concreteType)
        {
            var typeInfo = concreteType.GetTypeInfo();

            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));
            if (IsRegisteredDeeplink(concreteType))
                throw new Exception(string.Format("Deeplink для '{0}' уже зарегистрирован! Используй ReplaceDeeplink.", concreteType.FullName));

            RegisterDeeplinkInDictionary(concreteType, typeInfo);
        }

        #endregion

        #region Replacing

        public void Replace<TInterface, TModel>() where TInterface : class where TModel : class, TInterface
        {
            Replace<TInterface>(typeof(TModel));
        }

        public void Replace<TInterface>(Type concreteType)
            where TInterface : class
        {
            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));
            if (!IsRegistered<TInterface>())
                throw new Exception(string.Format("Lookup для интерфейса '{0}' еще не зарегистрирован! Используй Register.", typeof(TInterface).FullName));

            Assemblies.Remove(_viewModelLookups[typeof(TInterface)].GetTypeInfo().Assembly);

            _viewModelLookups[typeof(TInterface)] = concreteType;
            Assemblies.Add(concreteType.GetTypeInfo().Assembly);
        }

        public void ReplaceDeeplink(Type concreteType)
        {
            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));
            if (!IsRegisteredDeeplink(concreteType))
                throw new Exception(string.Format("Deeplink для '{0}' не зарегистрирован", concreteType.FullName));

            var deeplinkAttribute = concreteType.GetTypeInfo().GetCustomAttribute<DeeplinkAttribute>();
            if (deeplinkAttribute != null)
                _deeplinks[deeplinkAttribute.Scheme] = concreteType;
        }

        #endregion

        #region Resolving

        public Type Resolve(string concreteType)
        {
            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));
            if (!IsRegistered(concreteType))
                throw new Exception(string.Format("Lookup для интерфейса '{0}' еще не зарегистрирован! Используй Register.", concreteType));

            return _viewModelLookups.First(x => x.Key.FullName == concreteType).Value;
        }

        public Type Resolve(Type concreteType)
        {
            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));
            if (!IsRegistered(concreteType))
                throw new Exception(string.Format("Lookup для интерфейса '{0}' еще не зарегистрирован! Используй Register.", concreteType.FullName));

            return _viewModelLookups[concreteType];
        }

        public Type Resolve<TInterface>()
            where TInterface : class
        {
            return Resolve(typeof(TInterface));
        }

        public Type ResolveDeeplink(string scheme)
        {
            if (scheme == null)
                throw new ArgumentNullException(nameof(scheme));
            if (!IsRegisteredDeeplink(scheme))
                throw new Exception(string.Format("Deeplink для ссылки '{0}' еще не зарегистрирован! Необходимо прописать аттрибут DeeplinkAttribute у ViewModel'и!", scheme));

            return _deeplinks[scheme];
        }

        #endregion

        #region Checks

        public bool IsRegistered(string concreteType)
        {
            return _viewModelLookups.Any(x => x.Key.FullName == concreteType);
        }

        public bool IsRegistered(Type concreteType)
        {
            return _viewModelLookups.ContainsKey(concreteType);
        }

        public bool IsRegistered<TInterface>()
            where TInterface : class
        {
            return IsRegistered(typeof(TInterface));
        }

        public bool IsRegisteredDeeplink(string scheme)
        {
            return _deeplinks.ContainsKey(scheme);
        }

        public bool IsRegisteredDeeplink(Type concreteType)
        {
            var deeplinkAttribute = concreteType.GetTypeInfo().GetCustomAttribute<DeeplinkAttribute>();
            return deeplinkAttribute != null && _deeplinks.ContainsKey(deeplinkAttribute.Scheme);
        }

        #endregion

        #region InterfaceViewModelLookup

        public IDictionary<Type, Type> GetInterfaceViewModelLookup()
        {
            return _viewModelLookups;
        }

        public void CallbackWhenDeeplinkRegistered(string scheme, Action<Type> callback)
        {
            lock (_deeplinks)
            {
                if (IsRegisteredDeeplink(scheme))
                    callback?.Invoke(ResolveDeeplink(scheme));
                else
                    _deeplinkCallbacks.Add(new KeyValuePair<string, Action<Type>>(scheme, callback));
            }
        }

        public void CallbackWhenRegistered(Type concreteType, Action<Type> callback)
        {
            lock (_viewModelLookups)
            {
                if (IsRegistered(concreteType))
                    callback?.Invoke(Resolve(concreteType));
                else
                    _viewModelLookupsCallbacks.Add(new KeyValuePair<Type, Action<Type>>(concreteType, callback));
            }
        }

        #endregion

        #endregion
    }
}

