using System;
using System.Threading;
using AppRopio.Base.Core.Models.Content;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Base.Core.Services.Analytics;
using AppRopio.Base.Core.Services.Device;
using AppRopio.Base.Core.Services.UserDialogs;
using AppRopio.Base.Core.Services.ViewModelLookup;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Messenger;
using System.Threading.Tasks;
using AppRopio.Base.Core.Services.Localization;

namespace AppRopio.Base.Core.ViewModels
{
    /// <summary>
    /// Базовая ViewModel
    /// </summary>
    public abstract class BaseViewModel : MvxViewModel<IMvxBundle>, IBaseViewModel
    {
        #region Properties

        /// <summary>
        /// Задает или возвращает тип навигации
        /// </summary>
        /// <value>Тип навигации</value>
        public NavigationType VmNavigationType { get; set; }

        protected CancellationTokenSource OnUnbindCTS = new CancellationTokenSource();

        protected bool TrackInAnalytics { get; set; } = true;

        private bool _loading;
        /// <summary>
        /// Задает или возвращает величину показывающую, что эта
        /// <see cref="T:AppRopio.Base.Core.ViewModels.BaseViewModel"/> загружается.
        /// </summary>
        /// <value><c>true</c> если загружается; иначе, <c>false</c>.</value>
        public virtual bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                _loading = value;
                InvokeOnMainThread(() => RaisePropertyChanged(() => Loading));
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value, nameof(Title));
        }

        private string _pageTitle;
        /// <summary>
        /// Задает или возвращает название ViewModel
        /// </summary>
        /// <value>Название ViewModel</value>
        public string ViewModelTitle
        {
            get
            {
                return _pageTitle;
            }
            set
            {
                _pageTitle = value;
                RaisePropertyChanged(() => ViewModelTitle);
            }
        }

        private ContentState _contentState;
        public ContentState ContentState
        {
            get
            {
                return _contentState;
            }
            set
            {
                _contentState = value;
                RaisePropertyChanged(() => ContentState);
            }
        }

        #endregion

        #region Services

        protected IUserDialogs UserDialogs => Mvx.Resolve<IUserDialogs>();

        protected IMvxTrace MvxTrace => Mvx.Resolve<IMvxTrace>();

        protected IViewModelLookupService LookupService => Mvx.Resolve<IViewModelLookupService>();

        protected IAnalyticsNotifyingService AnalyticsNotifyingService => Mvx.CanResolve<IAnalyticsNotifyingService>() ? Mvx.Resolve<IAnalyticsNotifyingService>() : new AnalyticsNotifyingService();

        protected IDeviceService DeviceService => Mvx.Resolve<IDeviceService>();

        protected IMvxMessenger Messenger => Mvx.Resolve<IMvxMessenger>();

        protected ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();

        #endregion

        #region Constructor

        protected BaseViewModel()
        {
            if (AppSettings.ApiKey.IsNullOrEmtpy())
                throw new ArgumentNullException(nameof(AppSettings.ApiKey));

            ViewModelTitle = GetType().Name;

            ShouldAlwaysRaiseInpcOnUserInterfaceThread(true);
        }

        #endregion

        #region Public

        public override void Prepare(IMvxBundle parameter)
        {
            if (TrackInAnalytics)
                AnalyticsNotifyingService?.NotifyScreen(ViewModelTitle, Models.Analytics.ScreenState.Opened);
        }

        public virtual void Pause()
        {

        }

        public override void ViewAppeared()
        {
            if (TrackInAnalytics)
                AnalyticsNotifyingService?.NotifyScreen(ViewModelTitle, Models.Analytics.ScreenState.Appeared);

            base.ViewAppeared();
        }

        public override void ViewDisappeared()
        {
            if (TrackInAnalytics)
                AnalyticsNotifyingService?.NotifyScreen(ViewModelTitle, Models.Analytics.ScreenState.Disappeared);

            base.ViewDisappeared();
        }

        /// <summary>
        /// Unbind этого экземпляра ViewModel
        /// </summary>
        public virtual void Unbind()
        {
            if (TrackInAnalytics)
                AnalyticsNotifyingService?.NotifyScreen(ViewModelTitle, Models.Analytics.ScreenState.Closed);

            if (OnUnbindCTS != null)
            {
                lock (OnUnbindCTS)
                {
                    OnUnbindCTS.Cancel();
                    OnUnbindCTS.Dispose();
                    OnUnbindCTS = null;
                }
            }
        }

        #endregion
    }
}

