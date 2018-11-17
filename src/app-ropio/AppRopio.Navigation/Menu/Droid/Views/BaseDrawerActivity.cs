using Android.App;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using AppRopio.Base.Droid.Views;
using AppRopio.Navigation.Menu.Droid.Models;
using MvvmCross.Core.ViewModels;

namespace AppRopio.Navigation.Menu.Droid.Views
{
    public abstract class BaseDrawerActivity<TViewModel> : CommonActivity<TViewModel>, View.IOnClickListener
        where TViewModel : class, IMvxViewModel
    {
        #region Properties

        protected DrawerLayout _drawer;
        public DrawerLayout Drawer => _drawer;

        private ActionBarDrawerToggle _toggle;
        public ActionBarDrawerToggle Toggle => _toggle;

        public int DrawerLayoutId { get; }

        #endregion

        #region Constructor

        public BaseDrawerActivity()
        {

        }

        public BaseDrawerActivity(int layoutId, int drawerLayoutId)
            : base(layoutId)
        {
            DrawerLayoutId = drawerLayoutId;
        }

        #endregion

        #region Protected

        protected void InitDrawer(int drawerLayoutId, Android.Support.V7.Widget.Toolbar toolbar, int openDescStrinId, int closeDescStrinId)
        {
            var drawerLayout = FindViewById<DrawerLayout>(drawerLayoutId);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, toolbar, openDescStrinId, closeDescStrinId)
            {
                ToolbarNavigationClickListener = this
            };
			
            drawerLayout.AddDrawerListener(drawerToggle);

            drawerToggle.SyncState();

            _drawer = drawerLayout;
            _toggle = drawerToggle;

            if (CommonPresenter?.CanPop() ?? false)
                _toggle.DrawerIndicatorEnabled = false;
        }

        protected void InitDrawer(int drawerLayoutId, int openDescStrinId, int closeDescStrinId)
        {
            var drawerLayout = FindViewById<DrawerLayout>(drawerLayoutId);
            var drawerToggle = new ActionBarDrawerToggle(this, drawerLayout, openDescStrinId, closeDescStrinId);

            drawerLayout.AddDrawerListener(drawerToggle);

            _drawer = drawerLayout;
            _toggle = drawerToggle;

            if (CommonPresenter?.CanPop() ?? false)
                _toggle.DrawerIndicatorEnabled = false;

            _toggle.SyncState();
        }

        protected override void SetupControls()
        {
            if (Toolbar != null)
                InitDrawer(DrawerLayoutId, Toolbar, 0, 0);
            else
                InitDrawer(DrawerLayoutId, 0, 0);

            base.SetupControls();
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            _toggle?.SyncState();
        }

        protected virtual void OnNavigationItemClick()
        {
            if (CommonPresenter?.CanPop() ?? false)
                OnBackPressed();
            else
                OpenDrawer(GravityFlags.Start);
        }

        #endregion

        #region Public

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            _toggle?.OnConfigurationChanged(newConfig);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (_toggle != null && _toggle.OnOptionsItemSelected(item))
                return true;

            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    OnNavigationItemClick();
                    return base.OnOptionsItemSelected(item);
            }

            return base.OnOptionsItemSelected(item);
        }

        public virtual bool IsDrawerOpen(GravityFlags flag)
        {
            return _drawer.IsDrawerOpen((int)flag);
        }

        public virtual void CloseDrawers()
        {
            _drawer.CloseDrawers();
        }

        public void OpenDrawer(GravityFlags flag)
        {
            _drawer.OpenDrawer((int)flag);
        }

        public void CloseDrawer(GravityFlags flag)
        {
            _drawer.CloseDrawer((int)flag);
        }

        public void OpenDrawer(View drawer)
        {
            _drawer.OpenDrawer(drawer);
        }

        public void CloseDrawer(View drawer)
        {
            _drawer.CloseDrawer(drawer);
        }

        public void SetDrawerLockMode(GravityFlags flag, LockMode lockMode)
        {
            _drawer.SetDrawerLockMode((int)lockMode, (int)flag);
        }

        public void SetDrawerLockMode(View view, LockMode lockMode)
        {
            _drawer.SetDrawerLockMode((int)lockMode, view);
        }

        public void SetScrimColor(Color color)
        {
            _drawer.SetScrimColor(color);
        }

        public override void Close()
        {
            CloseDrawers();
            base.Close();
        }

        public override void OnBackPressed()
        {
            if (IsDrawerOpen(GravityFlags.Right))
            {
                CloseDrawer(GravityFlags.Right);
                return;
            }

            if (IsDrawerOpen(GravityFlags.Left))
            {
                CloseDrawer(GravityFlags.Left);
                return;
            }

            CommonPresenter?.MoveBack();
        }

        #region View.IOnClickListener implementation

        public void OnClick (View v)
        {
            OnNavigationItemClick();
        }

        #endregion

        #endregion
    }
}
