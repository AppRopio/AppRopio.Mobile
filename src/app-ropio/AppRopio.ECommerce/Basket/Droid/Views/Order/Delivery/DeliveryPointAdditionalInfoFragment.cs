using System;
using Android.Graphics;
using Android.Util;
using Android.Views;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;

namespace AppRopio.ECommerce.Basket.Droid.Views.Order.Delivery
{
    public class DeliveryPointAdditionalInfoFragment : CommonFragment<IDeliveryPointAdditionalInfoVM>
    {
        private Android.Support.V7.Widget.Toolbar _toolbar;
        private View _statusBar;

        public DeliveryPointAdditionalInfoFragment()
            : base (Resource.Layout.app_basket_delivery_on_point_info, "Подробнее")
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

        protected override void SetToolbar()
        {
            _toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_basket_delivery_on_point_toolbar);

            _toolbar.Title = Title;

            _toolbar.SetNavigationIcon(Resource.Drawable.abc_ic_ab_back_material);
            _toolbar.NavigationClick += (sender, e) => ViewModel.CloseCommand.Execute(null);
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SetupStatusBar();

            SetupBackground();
        }
    }
}
