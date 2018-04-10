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
using AppRopio.Base.Core.Services.Permissions;
using AppRopio.Base.Core.Services.Settings;
using AppRopio.Base.Core.Services.Trace;
using AppRopio.Base.Core.Services.UserDialogs;
using AppRopio.Base.Core.Services.ViewFinder;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Core.Services.ViewModelNameMapping;
using AppRopio.Base.iOS.Binding;
using AppRopio.Base.iOS.Helpers;
using AppRopio.Base.iOS.Services.Contacts;
using AppRopio.Base.iOS.Services.Device;
using AppRopio.Base.iOS.Services.Launcher;
using AppRopio.Base.iOS.Services.Log;
using AppRopio.Base.iOS.Services.Permissions;
using AppRopio.Base.iOS.Services.Settings;
using AppRopio.Base.iOS.Services.UserDialogs;
using ModernHttpClient;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Core.Views;
using MvvmCross.iOS.Platform;
using MvvmCross.iOS.Views;
using MvvmCross.iOS.Views.Presenters;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.DownloadCache;
using MvvmCross.Plugins.DownloadCache.iOS;
using MvvmCross.Plugins.Network.Reachability;
using UIKit;

namespace AppRopio.Base.iOS
{
    public abstract class BaseIosSetup : MvxAsyncIosSetup
    {
        private IMvxViewDispatcher _dispatcher;

        protected BaseIosSetup(IMvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
        {
            _dispatcher = new MvxIosViewDispatcher(new MvxIosViewPresenter(applicationDelegate, window));
        }

        protected BaseIosSetup(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
            : base(applicationDelegate, presenter)
        {
            _dispatcher = new MvxIosViewDispatcher(presenter);
        }

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
                var deviceService = Mvx.Resolve<IDeviceService>();

                var data = Mvx.Resolve<ILogService>().Read();

                Mvx.Resolve<IErrorService>().Send(
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
                MvxTrace.TaggedTrace(MvxTraceLevel.Warning, nameof(BaseIosSetup), ex.BuildAllMessagesAndStackTrace());
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
                    if (Mvx.CanResolve<IMvxReachability>())
                        return Mvx.Resolve<IMvxReachability>().IsHostReachable(AppSettings.Host);
                    return true;
                }),
                RequestTimeoutInSeconds = AppSettings.RequestTimeoutInSeconds,
                BaseUrl = new System.Uri(AppSettings.Host),
                CreateHandlerFunc = () => new NativeMessageHandler()
            };

            instance.Headers.Add("ApiKey", AppSettings.ApiKey);
            instance.Headers.Add("Device", Mvx.Resolve<IDeviceService>().DeviceInfo);
            instance.Headers.Add("DeviceToken", Mvx.Resolve<IDeviceService>().Token);
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

        protected override IMvxTrace CreateDebugTrace()
        {
            return new ARTrace();
        }

        protected override void InitializeFirstChance()
        {
            InitalizeExceptionHandler();

            Mvx.RegisterSingleton<ISettingsService>(() => new SettingsService());

            Mvx.RegisterSingleton<IDeviceService>(() => new DeviceService());
            Mvx.RegisterSingleton<ILogService>(() => new LogService());

            Mvx.RegisterSingleton<IPermissionsService>(() => new PermissionsService());
            Mvx.RegisterSingleton<IContactsService>(() => new ContactsService());

            Mvx.RegisterSingleton<IUserDialogs>(() => new UserDialogs());

            Mvx.RegisterSingleton<ILauncherService>(() => new LauncherService());

            var connectionService = SetupConnectionService();
            Mvx.RegisterSingleton<IConnectionService>(connectionService);

            var localizationService = SetupLocalizationService();
            Mvx.RegisterSingleton<ILocalizationService>(localizationService);

            App.Initialize();
        }

        private ILocalizationService SetupLocalizationService()
        {
            var localizationService = new LocalizationService(AppDomain.CurrentDomain.GetAssemblies());
            localizationService.SetCurrentUICulture(AppSettings.SettingsCulture);
            return localizationService;
        }

        protected override void InitializeViewLookup()
        {
            var viewLookupService = Mvx.Resolve<IViewLookupService>();
            if (viewLookupService == null)
                return;

            var viewModelLookupService = Mvx.Resolve<IViewModelLookupService>();
            if (viewModelLookupService == null)
                return;
            
            var container = Mvx.Resolve<IMvxViewsContainer>();
            container.AddSecondary(new ARViewFinder(viewLookupService, viewModelLookupService));
        }

        protected override void InitializeLastChance()
        {
            MvvmCross.Plugins.DownloadCache.PluginLoader.Instance.EnsureLoaded();
            MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();

            var configuration = MvxDownloadCacheConfiguration.Default;
            configuration.MaxInMemoryBytes = 20971520;
            var fileDownloadCache = new MvxFileDownloadCache(
                configuration.CacheName,
                configuration.CacheFolderPath,
                configuration.MaxFiles,
                configuration.MaxFileAge);

            Mvx.RegisterSingleton<IMvxFileDownloadCache>(fileDownloadCache);
            Mvx.RegisterSingleton<IMvxHttpFileDownloader>(new ARMvxHttpFileDownloader());

            base.InitializeLastChance();
        }

        protected override MvvmCross.Core.ViewModels.IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new ARPostfixAwareViewToViewModelNameMapping(Mvx.Resolve<IViewLookupService>(), Mvx.Resolve<IViewModelLookupService>(), "View", "ViewController");
        }

        protected override IEnumerable<Assembly> GetViewModelAssemblies()
        {
            var result = base.GetViewModelAssemblies();

            var viewModelAssemblies = Mvx.Resolve<IViewModelLookupService>().Assemblies;
            var assemblyList = result.ToList();

            viewModelAssemblies.ForEach(x =>
            {
                if (!assemblyList.Any(a => a.FullName == x.FullName))
                    assemblyList.Add(x);
            });

            return assemblyList;
        }

        protected override IEnumerable<Assembly> GetViewAssemblies()
        {
            var result = base.GetViewAssemblies();
            var assemblyList = result.ToList();

            var viewAssemblies = Mvx.Resolve<IViewLookupService>().Assemblies;
            viewAssemblies.ForEach(x =>
            {
                if (!assemblyList.Any(a => a.FullName == x.FullName))
                    assemblyList.Add(x);
            });

            assemblyList.Add(Assembly.GetAssembly((typeof(BaseIosSetup))));

            return assemblyList;
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

                var viewAssemblies = Mvx.Resolve<IViewLookupService>().Assemblies;
                viewAssemblies.ForEach(x =>
                {
                    if (!assemblyList.Any(a => a.FullName == x.FullName))
                        assemblyList.Add(x);
                });

                var viewModelAssemblies = Mvx.Resolve<IViewModelLookupService>().Assemblies;
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

        protected override IMvxViewDispatcher CreateViewDispatcher()
        {
            return _dispatcher;
        }

#endregion
    }
}

