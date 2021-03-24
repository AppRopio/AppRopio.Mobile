using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvvmCross.ViewModels;
using MvvmCross.Views;

namespace AppRopio.Base.Core.Services.ViewLookup
{
    public class ViewLookupService : IViewLookupService
    {
        private readonly Dictionary<Type, Type> _viewModelViewLookups = new Dictionary<Type, Type>();

        public List<Assembly> Assemblies { get; private set; } = new List<Assembly>();

        #region IViewModelLookupService implementation

        public void Register<TViewModel, TView>()
            where TViewModel : class, IMvxViewModel
            where TView : class, IMvxView<TViewModel>
        {
            Register<TViewModel>(typeof(TView));
        }

        public void Register<TViewModel>(Type concreteType)
            where TViewModel : class, IMvxViewModel
        {
            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));

            lock (_viewModelViewLookups)
            {
                if (IsRegistered<TViewModel>())
                    throw new Exception(string.Format("Lookup для модели '{0}' уже зарегистрирован! Используй Replace.", typeof(TViewModel).FullName));

                _viewModelViewLookups[typeof(TViewModel)] = concreteType;

                var viewAssembly = concreteType.GetTypeInfo().Assembly;

                if (!Assemblies.Any(a => a.FullName == viewAssembly.FullName))
                    Assemblies.Add(viewAssembly);
            }
        }

        public void Replace<TViewModel, TView>()
            where TViewModel : class, IMvxViewModel
            where TView : class, IMvxView<TViewModel>
        {
            Replace<TViewModel>(typeof(TView));
        }

        public void Replace<TViewModel>(Type concreteType)
            where TViewModel : class, IMvxViewModel
        {
            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));

            lock (_viewModelViewLookups)
            {
                if (!IsRegistered<TViewModel>())
                    throw new Exception(string.Format("Lookup для модели '{0}' еще не зарегистрирован! Используй Register.", typeof(TViewModel).FullName));

                Assemblies.Remove(_viewModelViewLookups[typeof(TViewModel)].GetTypeInfo().Assembly);

                _viewModelViewLookups[typeof(TViewModel)] = concreteType;

                Assemblies.Add(concreteType.GetTypeInfo().Assembly);
            }
        }

        public Type Resolve(string concreteType)
        {
            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));
            if (!IsRegistered(concreteType))
                throw new KeyNotFoundException(string.Format("Lookup для модели '{0}' еще не зарегистрирован! Используй Register.", concreteType));

            return _viewModelViewLookups.First(x => x.Key.FullName == concreteType).Value;
        }

        public Type Resolve(Type concreteType)
        {
            if (concreteType == null)
                throw new ArgumentNullException(nameof(concreteType));
            if (!IsRegistered(concreteType))
                throw new KeyNotFoundException(string.Format("Lookup для модели '{0}' еще не зарегистрирован! Используй Register.", concreteType.FullName));

            return _viewModelViewLookups[concreteType];
        }

        public Type Resolve<TViewModel>()
            where TViewModel : class, IMvxViewModel
        {
            return Resolve(typeof(TViewModel));
        }

        public Type ResolveByView(string viewName)
        {
            if (viewName == null)
                throw new ArgumentNullException(nameof(viewName));
            if (!IsRegisteredView(viewName))
                throw new KeyNotFoundException(string.Format("Lookup для View '{0}' еще не зарегистрирован! Используй Register.", viewName));

            return _viewModelViewLookups.First(x => x.Value.Name == viewName).Key;
        }

        public bool IsRegisteredView(string name)
        {
            return _viewModelViewLookups.Any(x => x.Value.Name == name);
        }

        public bool IsRegistered(string concreteType)
        {
            return _viewModelViewLookups.Any(x => x.Key.FullName == concreteType);
        }

        public bool IsRegistered(Type concreteType)
        {
            return _viewModelViewLookups.ContainsKey(concreteType);
        }

        public bool IsRegistered<TViewModel>()
            where TViewModel : class
        {
            return IsRegistered(typeof(TViewModel));
        }

        public IDictionary<Type, Type> GetViewModelViewLookup()
        {
            return _viewModelViewLookups;
        }

        #endregion
    }
}

