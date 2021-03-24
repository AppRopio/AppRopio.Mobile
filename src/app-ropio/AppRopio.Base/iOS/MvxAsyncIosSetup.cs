using System;
using System.Collections.Generic;
using System.Reflection;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Core.Services.ViewModelNameMapping;
using MvvmCross.Binding;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Platforms.Ios.Binding;
using MvvmCross.Core;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Platforms.Ios.Views;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross;
using MvvmCross.Converters;
using MvvmCross.Platforms.Ios;
using MvvmCross.Platforms.Ios.Views.Base;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin;
using UIKit;

namespace AppRopio.Base.iOS
{
    public abstract class MvxAsyncIosSetup
        : MvxAsyncSetup
    {
        private readonly IMvxApplicationDelegate _applicationDelegate;
        private readonly UIWindow _window;

        private IMvxIosViewPresenter _presenter;

        protected MvxAsyncIosSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            _window = window;
            _applicationDelegate = applicationDelegate;
        }

        protected MvxAsyncIosSetup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
        {
            _applicationDelegate = applicationDelegate;
            _presenter = presenter;
        }

        protected UIWindow Window => _window;

        protected IMvxApplicationDelegate ApplicationDelegate => _applicationDelegate;

        protected override IMvxTrace CreateDebugTrace()
        {
            return new MvxDebugTrace();
        }

        protected override IMvxPluginManager CreatePluginManager()
        {
            return new MvxPluginManager();
        }

        protected sealed override IMvxViewsContainer CreateViewsContainer()
        {
            var container = CreateIosViewsContainer();
            RegisterIosViewCreator(container);
            return container;
        }

        protected virtual IMvxIosViewsContainer CreateIosViewsContainer()
        {
            return new MvxIosViewsContainer();
        }

        protected virtual void RegisterIosViewCreator(IMvxIosViewsContainer container)
        {
            Mvx.RegisterSingleton<IMvxIosViewCreator>(container);
            Mvx.RegisterSingleton<IMvxCurrentRequest>(container);
        }

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return new MvxIosViewDispatcher(Presenter);
        }

        protected override void InitializePlatformServices()
        {
            RegisterPlatformProperties();
            RegisterPresenter();
            RegisterLifetime();
        }

        protected virtual void RegisterPlatformProperties()
        {
            Mvx.RegisterSingleton<IMvxIosSystem>(CreateIosSystemProperties());
        }

        protected virtual MvxIosSystem CreateIosSystemProperties()
        {
            return new MvxIosSystem();
        }

        protected virtual void RegisterLifetime()
        {
            Mvx.RegisterSingleton<IMvxLifetime>(_applicationDelegate);
        }

        protected IMvxIosViewPresenter Presenter
        {
            get
            {
                _presenter = _presenter ?? CreatePresenter();
                return _presenter;
            }
        }

        protected virtual IMvxIosViewPresenter CreatePresenter()
        {
            return new MvxIosViewPresenter(_applicationDelegate, _window);
        }

        protected virtual void RegisterPresenter()
        {
            var presenter = Presenter;
            Mvx.RegisterSingleton(presenter);
            Mvx.RegisterSingleton<IMvxIosModalHost>(presenter);
        }

        protected override void InitializeLastChance()
        {
            InitializeBindingBuilder();
            base.InitializeLastChance();
        }

        protected override IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new ARPostfixAwareViewToViewModelNameMapping(Mvx.Resolve<IViewLookupService>(), Mvx.Resolve<IViewModelLookupService>(), "View", "ViewController", "ViCo");
        }

        protected virtual void InitializeBindingBuilder()
        {
            RegisterBindingBuilderCallbacks();
            var bindingBuilder = CreateBindingBuilder();
            bindingBuilder.DoRegistration();
        }

        protected virtual void RegisterBindingBuilderCallbacks()
        {
            Mvx.CallbackWhenRegistered<IMvxValueConverterRegistry>(FillValueConverters);
            Mvx.CallbackWhenRegistered<IMvxTargetBindingFactoryRegistry>(FillTargetFactories);
            Mvx.CallbackWhenRegistered<IMvxBindingNameRegistry>(FillBindingNames);
        }

        protected virtual MvxBindingBuilder CreateBindingBuilder()
        {
            return new MvxIosBindingBuilder();
        }

        protected virtual void FillBindingNames(IMvxBindingNameRegistry obj)
        {
            // this base class does nothing
        }

        protected virtual void FillValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);
        }

        protected virtual List<Type> ValueConverterHolders => new List<Type>();

        protected virtual IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var toReturn = new List<Assembly>();
                toReturn.AddRange(GetViewModelAssemblies());
                toReturn.AddRange(GetViewAssemblies());
                return toReturn;
            }
        }

        protected virtual void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            // this base class does nothing
        }
    }
}
