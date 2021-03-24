using System;
using System.Linq;
using System.Threading.Tasks;
using MvvmCross.Core;
using MvvmCross;
using MvvmCross.Exceptions;
using MvvmCross.Platform.Logging;
using MvvmCross.Platform.Platform;

namespace AppRopio.Base.Core
{
    public abstract class MvxAsyncSetup : MvxSetup
    {
        private MvxSetupState _stateWrapper;
        public MvxSetupState StateWrapper
        {
            get
            {
                return _stateWrapper;
            }
            protected set
            {
                _stateWrapper = value;
                var propertyInfo = typeof(MvxSetup).GetProperty("State");
                propertyInfo?.SetValue(this, value);
            }
        } 
        
        public virtual async Task InitializeAsync()
        {
            await InitializePrimaryAsync().ConfigureAwait(false);
            await InitializeSecondaryAsync().ConfigureAwait(false);
        }

        public virtual async Task InitializePrimaryAsync()
        {
            if (StateWrapper != MvxSetupState.Uninitialized)
                throw new MvxException($"Cannot start primary - as state already {StateWrapper}");


            StateWrapper = MvxSetupState.InitializingPrimary;

            InitializeIoC();

            StateWrapper = MvxSetupState.InitializedPrimary;

            if (StateWrapper != MvxSetupState.InitializedPrimary)
                throw new MvxException($"Cannot start seconday - as state is currently {StateWrapper}");
            
            InitializeLoggingServices();
            InitializeDebugServices();

            StateWrapper = MvxSetupState.InitializingSecondary;

            InitializeFirstChance();

            var tasks = new[] {
                Task.Run(() => InitializePlatformServices()),
                Task.Run(() => InitializeSettings()),
                Task.Run(() => InitializeSingletonCache())
            };

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        public virtual async Task InitializeSecondaryAsync()
        {
            await PerformBootstrapActionsAsync();

            var pluginManager = InitializePluginFramework();

            var app = CreateApp();
            Mvx.RegisterSingleton(app);

            InitializeApp(pluginManager, app);

            await Task.Run(() => InitializeViewsContainer()).ConfigureAwait(false);

            var tasks = new[] {
                Task.Run(() => InitializeStringToTypeParser()),
                Task.Run(() => InitializeCommandHelper()),
                Task.Run(() =>
                {
                    InitializeNavigationService(app);
                    LoadNavigationServiceRoutes();
                }),
                Task.Run(() => InitializeViewModelTypeFinder()),
                Task.Run(() => InitializeViewDispatcher()),
                Task.Run(() => InitializeViewLookup()),
                Task.Run(() => InitializeCommandCollectionBuilder()),
                Task.Run(() => InitializeNavigationSerializer()),
                Task.Run(() => InitializeInpcInterception()),
                Task.Run(() => InitializeViewModelCache())
            };

            await Task.WhenAll(tasks).ConfigureAwait(false);

            InitializeLastChance();

            SetupLog?.Trace("Setup: Secondary end");

            StateWrapper = MvxSetupState.Initialized;
        }

        protected virtual Task PerformBootstrapActionsAsync()
        {
            SetupLog?.Trace("Setup: Bootstrap actions");

            var bootstrapRunner = new MvxBootstrapRunner();

            var tasks = GetBootstrapOwningAssemblies()
                .Select(ass => Task.Run(() => bootstrapRunner.Run(ass)));

            return Task.WhenAll(tasks);
        }

        public override void EnsureInitialized(System.Type requiredBy)
        {
            switch (StateWrapper)
            {
                case MvxSetupState.Uninitialized:
                    Initialize();
                    break;

                case MvxSetupState.InitializingPrimary:
                case MvxSetupState.InitializedPrimary:
                case MvxSetupState.InitializingSecondary:
                    throw new MvxException("The default EnsureInitialized method does not handle partial initialization");
                case MvxSetupState.Initialized:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
