using System;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Views.InputMethods;
using AppRopio.Base.Droid.Navigation;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Views;
using MvvmCross;
using AppRopio.Base.Core.Services.Localization;

namespace AppRopio.Base.Droid.Views
{
    public abstract class CommonFragment<TViewModel> : MvxFragment<TViewModel>
        where TViewModel : class, IMvxViewModel
    {
        #region Fields

        #endregion

        #region Properties

        protected ICommonAndroidViewPresenter CommonPresenter => Mvx.Resolve<IMvxAndroidViewPresenter>() as ICommonAndroidViewPresenter;

        public bool SetupToolbar { get; protected set; }

        public int LayoutId { get; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                SetToolbarTitle(value);
            }
        }

        #endregion

        #region Services

        protected ILocalizationService LocalizationService => Mvx.Resolve<ILocalizationService>();

        #endregion

        #region Constructor

        protected CommonFragment()
        {

        }

        protected CommonFragment(int layoutId)
        {
            LayoutId = layoutId;
        }

        protected CommonFragment(int layoutId, string title)
            : this(layoutId)
        {
            Title = title;
        }

        #endregion

        #region Protected

        protected virtual void SetToolbar()
        {
            if (SetupToolbar)
            {
                var toolbar = this.View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_toolbar);

                if (toolbar != null)
                {
                    SetSupportActionBar(toolbar);
                }
                else
                {
                    HideSupportActionBar();
                }
            }

            if (!Title.IsNullOrEmtpy())
                SetToolbarTitle(Title);
        }

        protected virtual void SetSupportActionBar(Android.Support.V7.Widget.Toolbar toolbar)
        {
            if (Activity is AppCompatActivity activity)
            {
                activity.SetSupportActionBar(toolbar);
                activity.SupportActionBar.Show();
            }
        }

        protected virtual void SetDisplayHomeAsUpEnabled(bool enabled)
        {
            if (Activity is AppCompatActivity activity)
                activity.SupportActionBar.SetDisplayHomeAsUpEnabled(enabled);
        }

        protected virtual void SetDisplayShowHomeEnabled(bool enabled)
        {
            if (Activity is AppCompatActivity activity)
                activity.SupportActionBar.SetDisplayShowHomeEnabled(enabled);
        }

        protected virtual void SetHomeButtonEnabled(bool enabled)
        {
            if (Activity is AppCompatActivity activity)
                activity.SupportActionBar.SetHomeButtonEnabled(enabled);
        }

        protected virtual void SetToolbarTitle(string title)
        {
            if (Activity is AppCompatActivity activity && activity.SupportActionBar != null)
                activity.SupportActionBar.Title = title;
        }

        protected virtual void SetHomeIndicator(int backResId, int menuResId)
        {
            if (Activity is AppCompatActivity activity && activity.SupportActionBar != null)
            {
                if (CommonPresenter?.CanPop() ?? false)
                    activity.SupportActionBar.SetHomeAsUpIndicator(backResId);
                else
                    activity.SupportActionBar.SetHomeAsUpIndicator(menuResId);
            }
        }

        protected virtual void HideSupportActionBar()
        {
            if (Activity is AppCompatActivity activity && activity.SupportActionBar != null)
                activity.SupportActionBar.Hide();
        }

        protected virtual void HideKeyboard()
        {
            View.Post(() =>
            {
                var imm = (InputMethodManager)Context?.GetSystemService(Android.Content.Context.InputMethodService);
                if (imm != null && Activity?.Window?.CurrentFocus?.WindowToken != null)
                    imm.HideSoftInputFromWindow(Activity.Window.CurrentFocus.WindowToken, 0);
            });
        }

        protected virtual void ShowKeyboard(View view)
        {
            View.Post(() =>
            {
                var imm = (InputMethodManager)Context.GetSystemService(Android.Content.Context.InputMethodService);
                if (imm != null)
                    imm.ToggleSoftInputFromWindow(view.ApplicationWindowToken, ShowSoftInputFlags.Forced, HideSoftInputFlags.None);
                
                view.RequestFocus();
            });
        }

        #endregion

        #region Public

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return CreateView(inflater, container, savedInstanceState);
        }

        public virtual View CreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            this.EnsureBindingContextIsSet(inflater);

            var view = this.BindingInflate(LayoutId, null);
            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            SetToolbar();
        }


        #endregion
    }
}
