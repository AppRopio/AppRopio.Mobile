using System;
using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.Services.Push;
using AppRopio.Base.Core.Services.ViewModelLookup;
using AppRopio.Base.Droid.Navigation;
using MvvmCross;
using MvvmCross.Droid.Support.V7.AppCompat;
using MvvmCross.Logging;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Presenters;
using MvvmCross.ViewModels;
using Xamarin.Essentials;

namespace AppRopio.Base.Droid.Views {
	public abstract class CommonActivity<TViewModel> : MvxAppCompatActivity<TViewModel>, ICommonActivity
        where TViewModel : class, IMvxViewModel
    {
        #region Fields

        private Intent _notifyIntent;

        #endregion

        #region Properties

        public virtual bool IsActive { get; private set; }

        private ICommonAndroidViewPresenter _customPresenter;
        protected ICommonAndroidViewPresenter CommonPresenter
        {
            get { return _customPresenter ?? (_customPresenter = Mvx.IoCProvider.Resolve<IMvxAndroidViewPresenter>() as ICommonAndroidViewPresenter); }
        }

        protected Color BackgroundColor { get; private set; }

        private Android.Support.V7.Widget.Toolbar _toolbar;
        protected Android.Support.V7.Widget.Toolbar Toolbar => _toolbar;

        public View View { get; private set; }

        public View StatusBar { get; private set; }

        public int LayoutId { get; }

        #endregion

        #region Constructor

        public CommonActivity()
        {

        }

        public CommonActivity(int layoutId)
        {
            LayoutId = layoutId;
        }

        #endregion

        #region Private

        private View CreateView()
        {
            //View = Inflate(LayoutId);

            View = this.BindingInflate(LayoutId, null);

            return View;
        }

        private void SetupToolbar()
        {
            _toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_toolbar);
            if (_toolbar != null)
            {
                SetSupportActionBar(_toolbar);
                SupportActionBar.SetDisplayShowHomeEnabled(true);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
            }
        }

        private void SetupStatusBar()
        {
            var statusBarColor = new TypedValue();
            Theme.ResolveAttribute(Android.Resource.Attribute.StatusBarColor, statusBarColor, true);
            int color = statusBarColor.Data;

            if (color == Color.Transparent.ToArgb())
                Window.SetFlags(WindowManagerFlags.TranslucentStatus, WindowManagerFlags.TranslucentStatus);

            StatusBar = FindViewById<View>(Resource.Id.app_status_bar_background);
            if (StatusBar != null)
            {
                var layoutParameters = StatusBar.LayoutParameters;
                layoutParameters.Height = GetStatusBarHeight();
                StatusBar.LayoutParameters = layoutParameters;

                if (color != Color.Transparent.ToArgb())
                    StatusBar.Visibility = ViewStates.Gone;
            }
        }

        private void SetupBackground()
        {
            var appBackground = new TypedValue();
            Theme.ResolveAttribute(Resource.Attribute.app_color_background, appBackground, true);
            BackgroundColor = new Color(appBackground.Data);

            View?.SetBackgroundColor(BackgroundColor);
        }

        #endregion

        #region Protected

        protected virtual void SetupControls()
        {

        }

        protected virtual void InitialSetup()
        {
            //FacebookSdk.SdkInitialize(ApplicationContext);

            InitStartPage();
        }

        protected virtual void InitStartPage()
        {

        }

        protected int GetStatusBarHeight()
        {
            int result = 0;
            int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
                result = Resources.GetDimensionPixelSize(resourceId);

            return result;
        }

        protected override void OnCreate(Bundle bundle)
        {
            IsActive = true;

            base.OnCreate(bundle);

            Platform.Init(this, bundle);

            if (_notifyIntent == null)
                _notifyIntent = Intent;

            SetContentView(View ?? CreateView());

            SetupBackground();

            SetupStatusBar();

            SetupToolbar();

            SetupControls();

            if (bundle == null)
                InitialSetup();
        }

        protected override void OnResume()
        {
            IsActive = true;

            base.OnResume();

            HandlePushNotificationIntent();
        }

        protected virtual void HandlePushNotificationIntent()
        {
            if (_notifyIntent != null)
            {
                Log.Verbose(PackageName, $"Notify Intent");

                System.Diagnostics.Debug.WriteLine(message: $"Notify Intent", category: PackageName);

                if (_notifyIntent.HasExtra(PushConstants.PUSH_DEEPLINK_KEY))
                {
                    var deeplink = _notifyIntent.GetStringExtra(PushConstants.PUSH_DEEPLINK_KEY);

                    Log.Verbose(PackageName, $"Try navigate to deeplink");

                    System.Diagnostics.Debug.WriteLine(message: $"Try navigate to deeplink {deeplink}", category: PackageName);

                    Mvx.IoCProvider.CallbackWhenRegistered<IViewModelLookupService>(() => NavigateToDeeplinkFromPush(Mvx.IoCProvider.Resolve<IViewModelLookupService>(), deeplink));
                }
            }

            _notifyIntent = null;
        }

        protected virtual void NavigateToDeeplinkFromPush(IViewModelLookupService vmLS, string deeplink)
        {
            deeplink.ParseDeeplink(out var urlScheme, out var routing, out var urlParameters);

            if (vmLS.IsRegisteredDeeplink(urlScheme))
            {
                Log.Verbose(PackageName, $"Deeplink was registered");

                System.Diagnostics.Debug.WriteLine(message: $"Deeplink was registered", category: PackageName);

                Mvx.IoCProvider.CallbackWhenRegistered<IPushNotificationsService>(() =>
                {
                    Mvx.IoCProvider.Resolve<IPushNotificationsService>().NavigateTo(deeplink: deeplink);
                });
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            IsActive = false;
            base.OnSaveInstanceState(outState);
        }

        protected override void OnPause()
        {
            IsActive = false;
            base.OnPause();
        }

        protected override void OnNewIntent(Intent intent)
        {
            _notifyIntent = intent;

            base.OnNewIntent(intent);
        }

        #endregion

        #region Public

        async Task CheckLocationPermission()
        {
            try
            {
                var statusLocation = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (statusLocation != PermissionStatus.Granted)
                {
                    statusLocation = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                }
            }
            catch (Exception ex)
            {
                Mvx.IoCProvider.Resolve<IMvxLog>().Info("MainActivity: Request permission exception: " + ex);
            }
        }

        public void ShowToast(string message, ToastLength length = ToastLength.Short)
        {
            Toast.MakeText(this, message, length).Show();
        }

        public void HideKeyboard()
        {
            var imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            if (imm != null && Window.CurrentFocus != null && Window.CurrentFocus.WindowToken != null)
                imm.HideSoftInputFromWindow(Window.CurrentFocus.WindowToken, 0);
        }

        public void ShowKeyboard(View view)
        {
            var imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
            imm.ShowSoftInput(view, ShowFlags.Forced);
        }

        public virtual void Close()
        {
            Finish();
        }

        public View Inflate(int id)
        {
            return View.Inflate(this, id, null);
        }

        #endregion
    }
}
