using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AppRopio.Base.API.Services;
using AppRopio.Base.Core;
using AppRopio.Base.Core.Services.Contacts;
using AppRopio.Base.Core.Services.Device;
using AppRopio.Base.Core.Services.Launcher;
using AppRopio.Base.Core.Services.Localization;
using AppRopio.Base.Core.Services.Log;
using AppRopio.Base.Core.Services.LogProvider;
using AppRopio.Base.Core.Services.Permissions;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Base.Core.Services.UserDialogs;
using AppRopio.Base.Core.Services.ViewFinder;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Core.Services.ViewModelNameMapping;
using AppRopio.Base.iOS.Binding;
using AppRopio.Base.iOS.Services.Contacts;
using AppRopio.Base.iOS.Services.Device;
using AppRopio.Base.iOS.Services.Launcher;
using AppRopio.Base.iOS.Services.Log;
using AppRopio.Base.iOS.Services.Permissions;
using AppRopio.Base.iOS.Services.Settings;
using AppRopio.Base.iOS.Services.UserDialogs;
using ModernHttpClient;
using MvvmCross;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Logging;
using MvvmCross.Platforms.Ios.Core;
using MvvmCross.Plugin.Network.Reachability;
using MvvmCross.Views;
using UIKit;

namespace AppRopio.Base.iOS
{
    public abstract class BaseIosSetup : MvxIosSetup
    {
        private readonly TaskCompletionSource<bool> _initializePrimaryTCS = new TaskCompletionSource<bool>();

        #region Private

        private void InitalizeExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) => LogUncaughtException(args.ExceptionObject);
            //System.Threading.Tasks.TaskScheduler.UnobservedTaskException += (sender, args) => LogUncaughtException(args.Exception);
        }

        private static void LogUncaughtException(object handle)
        {
            var exception = handle as Exception;
            try
            {
                var deviceService = Mvx.IoCProvider.Resolve<IDeviceService>();

                var data = Mvx.IoCProvider.Resolve<ILogService>().Read();

                Mvx.IoCProvider.Resolve<IErrorService>().Send(
                    exception?.Message,
                    exception?.BuildAllMessagesAndStackTrace(),
                    deviceService.PackageName,
                    deviceService.AppVersion,
                    deviceService.OSVersion,
                    data
                );
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn($"{nameof(BaseIosSetup)}: {ex.BuildAllMessagesAndStackTrace()}");
            }
        }

        private static IConnectionService SetupConnectionService()
        {
            var instance = new ConnectionService
            {
                ErrorWhenConnectionFailed = AppSettings.ErrorWhenConnectionFailed,
                ErrorWhenTaskCanceled = AppSettings.ErrorWhenTaskCanceled,
                IsConnectionAvailable = () => Task<bool>.Factory.StartNew(() =>
                {
                    if (Mvx.IoCProvider.CanResolve<IMvxReachability>())
                        return Mvx.IoCProvider.Resolve<IMvxReachability>().IsHostReachable(AppSettings.Host);
                    return true;
                }),
                RequestTimeoutInSeconds = AppSettings.RequestTimeoutInSeconds,
                BaseUrl = new System.Uri(AppSettings.Host),
                CreateHandlerFunc = () => new NativeMessageHandler()
            };

            instance.Headers.Add("ApiKey", AppSettings.ApiKey);
            instance.Headers.Add("Device", Mvx.IoCProvider.Resolve<IDeviceService>().DeviceInfo);
            instance.Headers.Add("DeviceToken", Mvx.IoCProvider.Resolve<IDeviceService>().Token);
            instance.Headers.Add("Company", AppSettings.CompanyID);
            instance.Headers.Add("Region", AppSettings.RegionID ?? AppSettings.DefaultRegionID);
            instance.Headers.Add("Accept-Language", AppSettings.SettingsCulture.Name);
#if DEBUG
            instance.Headers.Add("Debug", "true");
#endif

            return instance;
        }

        #endregion

        #region Protected

		protected override IMvxLogProvider CreateLogProvider()
        {
            return new ARLogProvider();
		}

        protected override void InitializeFirstChance()
        {
            base.InitializeFirstChance();

            InitalizeExceptionHandler();

            Mvx.IoCProvider.RegisterSingleton<ISettingsService>(() => new SettingsService());

            Mvx.IoCProvider.RegisterSingleton<IDeviceService>(() => new DeviceService());
            Mvx.IoCProvider.RegisterSingleton<ILogService>(() => new LogService());

            Mvx.IoCProvider.RegisterSingleton<IPermissionsService>(() => new PermissionsService());
            Mvx.IoCProvider.RegisterSingleton<IContactsService>(() => new ContactsService());

            Mvx.IoCProvider.RegisterSingleton<IUserDialogs>(() => new UserDialogs());

            Mvx.IoCProvider.RegisterSingleton<ILauncherService>(() => new LauncherService());

            var connectionService = SetupConnectionService();
            Mvx.IoCProvider.RegisterSingleton<IConnectionService>(connectionService);

            var localizationService = SetupLocalizationService();
            Mvx.IoCProvider.RegisterSingleton<ILocalizationService>(localizationService);

            App.Initialize();
        }

        private ILocalizationService SetupLocalizationService()
        {
            var localizationService = new LocalizationService(AppDomain.CurrentDomain.GetAssemblies());
            localizationService.SetCurrentUICulture(AppSettings.SettingsCulture);
            return localizationService;
        }

        protected override IDictionary<Type, Type> InitializeLookupDictionary()
        {
            return null;
        }

        protected override IMvxViewsContainer InitializeViewLookup(IDictionary<Type, Type> viewModelViewLookup)
        {
            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();
            if (viewLookupService == null)
                return null;

            var viewModelLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
            if (viewModelLookupService == null)
                return null;

            var container = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
            container.AddSecondary(new ARViewFinder(viewLookupService, viewModelLookupService));
            return container;
        }

        public override void InitializePrimary()
        {
            UIApplication.SharedApplication.BeginInvokeOnMainThread(() =>
            {
                base.InitializePrimary();
                _initializePrimaryTCS.TrySetResult(true);
            });
        }

        public override async void InitializeSecondary()
        {
            await _initializePrimaryTCS.Task;
            base.InitializeSecondary();
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();
        }

        protected override MvvmCross.ViewModels.IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new ARPostfixAwareViewToViewModelNameMapping(Mvx.IoCProvider.Resolve<IViewLookupService>(), Mvx.IoCProvider.Resolve<IViewModelLookupService>(), "View", "ViewController");
        }

        public override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var result = base.GetViewModelAssemblies();

            var viewModelAssemblies = Mvx.IoCProvider.Resolve<IViewModelLookupService>().Assemblies;
            var assemblyList = result.ToList();

            viewModelAssemblies.ForEach(x =>
            {
                if (!assemblyList.Any(a => a.FullName == x.FullName))
                    assemblyList.Add(x);
            });

            return assemblyList;
        }

        public override IEnumerable<Assembly> GetViewAssemblies()
        {
            var result = base.GetViewAssemblies();
            var assemblyList = result.ToList();

            var viewAssemblies = Mvx.IoCProvider.Resolve<IViewLookupService>().Assemblies;
            viewAssemblies.ForEach(x =>
            {
                if (!assemblyList.Any(a => a.FullName == x.FullName))
                    assemblyList.Add(x);
            });

            assemblyList.Add(Assembly.GetAssembly((typeof(BaseIosSetup))));

            return assemblyList;
        }

        protected abstract IEnumerable<Type> GetPluginTypes();

        public override IEnumerable<Assembly> GetPluginAssemblies()
        {
            var assemblies = base.GetPluginAssemblies();
            var pluginTypes = GetPluginTypes();

            if (!pluginTypes.IsNullOrEmpty())
                assemblies.Concat(pluginTypes.Select(t => t.Assembly));

            return assemblies;
        }

        protected override IEnumerable<Assembly> GetBootstrapOwningAssemblies()
        {
            var result = base.GetBootstrapOwningAssemblies();

            var bootstrapAssemblies = result.ToList();
            if (!bootstrapAssemblies.Contains(ApplicationDelegate.GetType().GetTypeInfo().Assembly))
                bootstrapAssemblies.Add(ApplicationDelegate.GetType().GetTypeInfo().Assembly);

            return bootstrapAssemblies;
        }

        protected override IEnumerable<Assembly> ValueConverterAssemblies
        {
            get
            {
                var result = base.GetViewAssemblies();

                var assemblyList = result.ToList();

                var viewAssemblies = Mvx.IoCProvider.Resolve<IViewLookupService>().Assemblies;
                viewAssemblies.ForEach(x =>
                {
                    if (!assemblyList.Any(a => a.FullName == x.FullName))
                        assemblyList.Add(x);
                });

                var viewModelAssemblies = Mvx.IoCProvider.Resolve<IViewModelLookupService>().Assemblies;
                viewModelAssemblies.ForEach(x =>
                {
                    if (!assemblyList.Any(a => a.FullName == x.FullName))
                        assemblyList.Add(x);
                });

                assemblyList.Add(Assembly.GetAssembly((typeof(BaseIosSetup))));
                assemblyList.Add(Assembly.GetAssembly((typeof(App))));

                return assemblyList;
            }
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<UITextField>("PhoneBinding", view => new PhoneBinding(view));
            registry.RegisterCustomBindingFactory<UIView>("AnimatedVisibility", view => new AnimatedVisibilityBinding(view));

            registry.RegisterCustomBindingFactory<UIButton>("TitleColor", view => new UIButtonTitleColorTargetBinding(view));
            registry.RegisterCustomBindingFactory<UIButton>("DisabledTitleColor", view => new UIButtonTitleColorTargetBinding(view, UIControlState.Disabled));
            registry.RegisterCustomBindingFactory<UIButton>("HighlightedTitleColor", view => new UIButtonTitleColorTargetBinding(view, UIControlState.Highlighted));
            registry.RegisterCustomBindingFactory<UIButton>("SelectedTitleColor", view => new UIButtonTitleColorTargetBinding(view, UIControlState.Selected));

            base.FillTargetFactories(registry);
        }

#endregion
    }
}

