using System.Collections;
using System.Linq;
using Android.Graphics;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Basket.Core;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Delivery;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace AppRopio.ECommerce.Basket.Droid.Views.Order.Delivery
{
    public class DeliveryOnAddressFragment : CommonFragment<IDeliveryOnAddressVM>
    {
        private Android.Support.V7.Widget.Toolbar _toolbar;
        private View _statusBar;

        public DeliveryOnAddressFragment()
            : base (Resource.Layout.app_basket_delivery_on_address)
        {
            Title = LocalizationService.GetLocalizableString(BasketConstants.RESX_NAME, "DeliveryAddress_Title");
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
            _statusBar = View.FindViewById<View>(Resource.Id.app_basket_delivery_on_address_status_bar_background);

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
            _toolbar = View.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.app_basket_delivery_on_address_toolbar);

            _toolbar.Title = Title;

            var navigationIcon = new TypedValue();
            Context.Theme.ResolveAttribute(Resource.Attribute.app_basket_delivery_on_address_ic_close, navigationIcon, false);

            _toolbar.SetNavigationIcon(navigationIcon.Data);
            _toolbar.NavigationClick += (sender, e) => ViewModel.CloseCommand.Execute(null);
        }

        protected virtual void SetupRecyclerView(Android.Views.View view, MvxRecyclerView recyclerView)
        {
            recyclerView.Adapter = SetupAdapter(recyclerView);
        }

        #region RecyclerView adapter

        protected virtual IMvxRecyclerAdapter SetupAdapter(MvxRecyclerView recyclerView)
        {
            return new ARFlatGroupAdapter(GetInnerItems, SetupTemplateSelector(), BindingContext)
            {
                HasFooter = CheckSectionHasFooter,
                TuneViewHolderOnCreate = TuneItem,
            };
        }

        protected virtual bool CheckSectionHasFooter(object item)
        {
            return ViewModel?.AddressFieldsItems?.LastOrDefault()?.Equals(item) ?? false;
        }

        protected virtual IARFlatGroupTemplateSelector SetupTemplateSelector()
        {
            return new DeliveryOnAddressTemplateSelector();
        }

        protected virtual IEnumerable GetInnerItems(object item)
        {
            return new[] { item };
        }

        protected virtual void TuneItem(RecyclerView.ViewHolder viewHolder, int viewType)
        {
            var spinner = viewHolder.ItemView.FindViewById<MvxAppCompatSpinner>(Resource.Id.app_basket_full_order_item_spinner);
            if (spinner != null)
            {
                spinner.ItemTemplateId = Resource.Layout.app_basket_full_order_item_orderField_counter_spinner;
                spinner.DropDownItemTemplateId = Resource.Layout.app_basket_full_order_item_orderField_counter_spinner_item;
            }
        }

        #endregion

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SetupStatusBar();

            SetupBackground();

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_basket_delivery_on_address_recyclerView);
            SetupRecyclerView(view, recyclerView);
        }

        private class DeliveryOnAddressTemplateSelector : IARFlatGroupTemplateSelector
        {
			public int ItemTemplateId { get; set; }

			public int GetFooterViewType(object forItemObject)
            {
                return Resource.Layout.app_basket_full_order_footer;
            }

            public int GetHeaderViewType(object forItemObject)
            {
                return Resource.Layout.app_basket_full_order_header;
            }

            public int GetItemLayoutId(int fromViewType)
            {
                return fromViewType;
            }

            public int GetItemViewType(object forItemObject)
            {
                if (forItemObject is IOrderFieldItemVM orderField)
                {
                    if (orderField.IsOptional)
                        return Resource.Layout.app_basket_full_order_item_orderField_optional;

                    switch (orderField.Type)
                    {
                        case Models.Basket.Responses.Enums.OrderFieldType.Counter:
                            return Resource.Layout.app_basket_full_order_item_orderField_counter;
                        case Models.Basket.Responses.Enums.OrderFieldType.Phone:
                            return Resource.Layout.app_basket_full_order_item_orderField_phone;
                        default:
                            return Resource.Layout.app_basket_full_order_item_orderField_text;
                    }
                }

                return -1;
            }
        }
    }
}
