using System;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;

namespace AppRopio.ECommerce.Basket.Droid.Views.Order.Delivery
{
    public class DeliveryOnPointFragment : CommonFragment<IDeliveryOnPointVM>, View.IOnKeyListener
    {
        protected const int MENU_SEARCH_ID = 1;
        protected const int MENU_CLEAR_ID = 2;

		private View _statusBar;
        private Toolbar _toolbar;
        private Toolbar _toolbarInSearchState;
        private Android.Widget.EditText _searchEditText;

        public DeliveryOnPointFragment()
            : base (Resource.Layout.app_basket_delivery_on_point, "Выбор адреса")
        {
        }

        private int GetStatusBarHeight()
        {
            int result = 0;
            int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
            if (resourceId > 0)
                result = Resources.GetDimensionPixelSize(resourceId);

            return result;
        }

        private void SetupStatusBar()
        {
            _statusBar = View.FindViewById<View>(Resource.Id.app_basket_delivery_on_point_status_bar_background);

            var statusBarColor = new TypedValue();
            Context.Theme.ResolveAttribute(Android.Resource.Attribute.StatusBarColor, statusBarColor, true);
            int color = statusBarColor.Data;

            if (_statusBar != null)
            {
                var layoutParameters = _statusBar.LayoutParameters;
                layoutParameters.Height = GetStatusBarHeight();
                _statusBar.LayoutParameters = layoutParameters;

                if (color != Color.Transparent.ToArgb())
                    _statusBar.Visibility = ViewStates.Gone;
            }
        }

        private void SetupBackground()
        {
            var appBackground = new TypedValue();
            Context.Theme.ResolveAttribute(Resource.Attribute.app_color_background, appBackground, true);
            var backgroundColor = new Color(appBackground.Data);

            View?.SetBackgroundColor(backgroundColor);
        }

        private void OnBackClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
            ViewModel.CloseCommand.Execute(null);
        }

        private void OnSearchMenuItemClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            _toolbar.Visibility = ViewStates.Gone;
            _toolbarInSearchState.Visibility = ViewStates.Visible;

            ShowKeyboard(_searchEditText);
        }

        private void OnClearSearchClick(object sender, Toolbar.MenuItemClickEventArgs e)
        {
            _searchEditText.Text = string.Empty;
        }

        private void OnBackToToolbarClick(object sender, Toolbar.NavigationClickEventArgs e)
        {
            ViewModel.CancelSearchCommand.Execute(null);

            _toolbarInSearchState.Visibility = ViewStates.Gone;
            _toolbar.Visibility = ViewStates.Visible;

            HideKeyboard();
        }

        protected override void SetToolbar()
        {
            _toolbar = View.FindViewById<Toolbar>(Resource.Id.app_basket_delivery_on_point_toolbar);
            _toolbar.Title = Title;
            _toolbar.SetNavigationIcon(Resource.Drawable.abc_ic_clear_material);
            _toolbar.NavigationClick += OnBackClick;
            _toolbar.MenuItemClick += OnSearchMenuItemClick;

            var searchMenuItem = _toolbar.Menu.Add(0, MENU_SEARCH_ID, 0, "Поиск");

            var typedValue = new TypedValue();
            Activity.Theme.ResolveAttribute(Resource.Attribute.app_basket_delivery_on_point_ic_toolbar_search, typedValue, true);

            searchMenuItem.SetIcon(Resources.GetDrawable(typedValue.ResourceId, Context.Theme));
            searchMenuItem.SetShowAsAction(ShowAsAction.Always);


            _toolbarInSearchState = View.FindViewById<Toolbar>(Resource.Id.app_basket_delivery_on_point_toolbar_onSearch);
            _toolbarInSearchState.SetNavigationIcon(Resource.Drawable.abc_ic_ab_back_material);
            _toolbarInSearchState.NavigationClick += OnBackToToolbarClick;
            _toolbarInSearchState.MenuItemClick += OnClearSearchClick;

            var clearMenuItem = _toolbarInSearchState.Menu.Add(0, MENU_CLEAR_ID, 0, "Поиск");
            clearMenuItem.SetIcon(Resource.Drawable.abc_ic_clear_material);
            clearMenuItem.SetShowAsAction(ShowAsAction.Always);

            _searchEditText = View.FindViewById<Android.Widget.EditText>(Resource.Id.app_basket_delivery_on_point_toolbar_onSearch_input);
            _searchEditText.SetOnKeyListener(this);
        }

        public override void OnViewCreated(View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SetupStatusBar();

            SetupBackground();
        }

        public override void OnDestroy()
        {
            if (_toolbar != null)
            {
                _toolbar.NavigationClick -= OnBackClick;
                _toolbar.MenuItemClick -= OnSearchMenuItemClick;
            }

            if (_toolbarInSearchState != null)
            {
                _toolbarInSearchState.NavigationClick -= OnBackToToolbarClick;
                _toolbarInSearchState.MenuItemClick -= OnClearSearchClick;
            }
            
            base.OnDestroy();
        }

        public bool OnKey(View v, [GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            var handled = false;

            if (e.Action == KeyEventActions.Up && keyCode == Keycode.Enter)
            {
                HideKeyboard();

                ViewModel.SearchCommand.Execute(null);

                handled = true;
            }

            return handled;
        }
    }
}
