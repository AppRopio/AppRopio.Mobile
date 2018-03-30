using System;
using System.Collections.Generic;
using System.Reflection;
using MvvmCross.Core.Views;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Base.Core.Services.ViewLookup
{
    public interface IViewLookupService
    {
        List<Assembly> Assemblies { get; }

        void Register<TViewModel>(Type concreteType)
            where TViewModel : class, IMvxViewModel;
        void Register<TViewModel, TView>()
            where TViewModel : class, IMvxViewModel
            where TView : class, IMvxView<TViewModel>;

        void Replace<TViewModel>(Type concreteType)
            where TViewModel : class, IMvxViewModel;
        void Replace<TViewModel, TView>()
            where TViewModel : class, IMvxViewModel
            where TView : class, IMvxView<TViewModel>;

        Type Resolve(string concreteType);
        Type Resolve(Type concreteType);
        Type Resolve<TViewModel>()
            where TViewModel : class, IMvxViewModel;
        Type ResolveByView(string viewName);

        bool IsRegisteredView(string name);

        bool IsRegistered(string concreteType);
        bool IsRegistered(Type concreteType);
        bool IsRegistered<TViewModel>()
            where TViewModel : class;

        IDictionary<Type, Type> GetViewModelViewLookup();
    }
}

