using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Android.Widget;
using AppRopio.Base.API.Services;
using AppRopio.Base.Core;
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
using AppRopio.Base.Droid.Bindings;
using AppRopio.Base.Droid.Converters;
using AppRopio.Base.Droid.Services.Device;
using AppRopio.Base.Droid.Services.Launcher;
using AppRopio.Base.Droid.Services.Log;
using AppRopio.Base.Droid.Services.MultipleViewModelCache;
using AppRopio.Base.Droid.Services.Permissions;
using AppRopio.Base.Droid.Services.Settings;
using AppRopio.Base.Droid.Services.UserDialogs;
using MvvmCross;
using MvvmCross.Binding.Binders;
using MvvmCross.Binding.Bindings.Target.Construction;
using MvvmCross.Binding.Combiners;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.Platforms.Android.Views;
using MvvmCross.Plugin.Network.Reachability;
using MvvmCross.Views;
using Xamarin.Android.Net;

namespace AppRopio.Base.Droid
{
	public abstract class BaseAndroidSetup : MvxAppCompatSetup
    {
        protected BaseAndroidSetup()
            : base()
        {
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
                Mvx.IoCProvider.Resolve<IMvxLog>().Warn($"{nameof(BaseAndroidSetup)}: {ex.BuildAllMessagesAndStackTrace()}");
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
                CreateHandlerFunc = () => new AndroidClientHandler()
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
            InitalizeExceptionHandler();

            Mvx.IoCProvider.RegisterSingleton<ISettingsService>(() => new SettingsService());

            Mvx.IoCProvider.RegisterSingleton<IDeviceService>(() => new DeviceService());
            Mvx.IoCProvider.RegisterSingleton<ILogService>(() => new LogService());

            Mvx.IoCProvider.RegisterSingleton<IUserDialogs>(() => new UserDialogs());

            Mvx.IoCProvider.RegisterSingleton<IPermissionsService>(() => new PermissionsService());

            Mvx.IoCProvider.RegisterSingleton<ILauncherService>(() => new LauncherService());

            var connectionService = SetupConnectionService();
            Mvx.IoCProvider.RegisterSingleton<IConnectionService>(connectionService);

            var localizationService = SetupLocalizationService();
            Mvx.IoCProvider.RegisterSingleton<ILocalizationService>(localizationService);

            App.Initialize();
        }

        private ILocalizationService SetupLocalizationService()
        {
            return new LocalizationService(AppDomain.CurrentDomain.GetAssemblies());
        }

        protected override IMvxViewsContainer InitializeViewLookup(IDictionary<Type, Type> viewModelViewLookup)
        {
            if (viewModelViewLookup == null)
                return null;

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();
            if (viewLookupService == null)
                return null;

            var viewModelLookupService = Mvx.IoCProvider.Resolve<IViewModelLookupService>();
            if (viewModelLookupService == null)
                return null;

            var container = Mvx.IoCProvider.Resolve<IMvxViewsContainer>();
            container.AddAll(viewModelViewLookup);
            container.AddSecondary(new ARViewFinder(viewLookupService, viewModelLookupService));
            return container;
        }

        protected override void InitializeLastChance()
        {
            base.InitializeLastChance();

            var presenter = Mvx.IoCProvider.Resolve<IMvxAndroidViewPresenter>() as IMvxMultipleViewModelCache;
            Mvx.IoCProvider.RegisterSingleton<MvvmCross.Platforms.Android.Views.IMvxMultipleViewModelCache>(presenter ?? new ARMultipleViewModelCache());

            Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueCombinerRegistry>(() =>
            {
                Mvx.IoCProvider.Resolve<IMvxValueCombinerRegistry>().Fill(ValueConverterAssemblies);
            });
        }

        protected override MvvmCross.ViewModels.IMvxNameMapping CreateViewToViewModelNaming()
        {
            return new ARPostfixAwareViewToViewModelNameMapping(Mvx.IoCProvider.Resolve<IViewLookupService>(), Mvx.IoCProvider.Resolve<IViewModelLookupService>(), "View", "Activity", "Fragment");
        }

        protected override void FillTargetFactories(IMvxTargetBindingFactoryRegistry registry)
        {
            registry.RegisterCustomBindingFactory<EditText>("PhoneBinding", view => new PhoneEditTextBinding(view));
            registry.RegisterCustomBindingFactory<EditText>("UpdatedText", view => new UpdatedTextBinding(view));

            base.FillTargetFactories(registry);
        }

        protected override void FillValueConverters(MvvmCross.Converters.IMvxValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("ErrorFromBoolean", new ErrorFromBooleanValueConverter());
            registry.AddOrOverwrite("Resx", new ResxValueConverter());

            registry.Fill(ValueConverterAssemblies);
            registry.Fill(ValueConverterHolders);

            base.FillValueConverters(registry);
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

            assemblyList.Add(Assembly.GetAssembly((typeof(BaseAndroidSetup))));

            return assemblyList;
        }

        protected override IEnumerable<Assembly> GetBootstrapOwningAssemblies()
        {
            var result = base.GetBootstrapOwningAssemblies();

            var bootstrapAssemblies = result.ToList();
            if (!bootstrapAssemblies.Contains(ApplicationContext.GetType().GetTypeInfo().Assembly))
                bootstrapAssemblies.Add(ApplicationContext.GetType().GetTypeInfo().Assembly);

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

                assemblyList.Add(Assembly.GetAssembly((typeof(BaseAndroidSetup))));
                assemblyList.Add(Assembly.GetAssembly((typeof(App))));

                return assemblyList;
            }
        }

        protected override IEnumerable<Assembly> AndroidViewAssemblies
        {
            get
            {
                var result = base.AndroidViewAssemblies;

                var assemblyList = result.ToList();

                var viewAssemblies = Mvx.IoCProvider.Resolve<IViewLookupService>().Assemblies;
                viewAssemblies.ForEach(x =>
                {
                    if (!assemblyList.Any(a => a.FullName == x.FullName))
                        assemblyList.Add(x);
                });

                assemblyList.Add(Assembly.GetAssembly((typeof(BaseAndroidSetup))));

                return assemblyList;
            }
        }
        #endregion
    }
}
