using System;
using System.Collections;
using System.Linq;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Widget;
using AppRopio.Base.Core.Converters;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Controls;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Basket.Core.Services;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Full;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items.Delivery;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Droid.Support.V7.AppCompat.Widget;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Platform;
using AppRopio.Base.Droid.Listeners;

namespace AppRopio.ECommerce.Basket.Droid.Views.Order.Full
{
    public class FullOrderFragment : CommonFragment<IFullOrderViewModel>
    {
        private MvxRecyclerView _recyclerView;

        public FullOrderFragment()
            : base(Resource.Layout.app_basket_full_order, "Оформление заказа")
        {
            
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.DeliveryViewModel.IsShowDeliveryTimePicker) &&
                _recyclerView != null)
            {
                if (ViewModel.DeliveryViewModel.IsShowDeliveryTimePicker && ViewModel.CanGoNext)
                    _recyclerView.SmoothScrollToPosition((_recyclerView.Adapter as RecyclerView.Adapter).ItemCount);
            }
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
                HasHeader = CheckSectionHasHeader,
                TuneSectionHeaderOnBind = TuneSectionHeader,
                TuneSectionFooterOnBind = TuneSectionFooter,
                TuneViewHolderOnCreate = TuneItem,
            };
        }

        protected virtual bool CheckSectionHasHeader(object item)
        {
            var result = false;

            if (item is IOrderFieldsGroupVM orderFields)
                result = !orderFields.Name.IsNullOrEmtpy();
            else
            {
                var firstDeliveryItem = ViewModel.Items.FirstOrDefault(x => x is IDeliveryTypeItemVM);
                if (firstDeliveryItem != null && ViewModel.Items.IndexOf(firstDeliveryItem) == ViewModel.Items.IndexOf((IMvxViewModel)item))
                    result = true;
            }

            return result;
        }

        protected virtual bool CheckSectionHasFooter(object item)
        {
            var result = false;

            if (item is IOrderFieldsGroupVM)
                result = true;
            else
            {
                var lastDeliveryItem = ViewModel.Items.LastOrDefault(x => x is IDeliveryTypeItemVM);
                if (lastDeliveryItem != null && ViewModel.Items.IndexOf(lastDeliveryItem) == ViewModel.Items.IndexOf((IMvxViewModel)item))
                    result = true;
            }

            return result;
        }

        protected virtual IARFlatGroupTemplateSelector SetupTemplateSelector()
        {
            return new FullOrderTemplateSelector();
        }

        protected virtual IEnumerable GetInnerItems(object item)
        {
            if (item is IOrderFieldsGroupVM orderFields)
                return orderFields.Items;

            return new[] { item };
        }

        protected virtual void TuneSectionHeader(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            var title = viewHolder.ItemView.FindViewById<TextView>(Resource.Id.app_basket_full_order_header_title);
            if (title != null)
            {
                var dataContext = (viewHolder as IMvxRecyclerViewHolder)?.DataContext;
                if (dataContext != null)
                {
                    if (dataContext is IOrderFieldsGroupVM orderGroup)
                        title.Text = orderGroup.Name;
                    else if (dataContext is IDeliveryTypeItemVM)
                        title.Text = "Тип доставки";
                }
            }
        }

        protected virtual void TuneSectionFooter(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            var dataContext = (viewHolder as IMvxRecyclerViewHolder)?.DataContext as IDeliveryTypeItemVM;
            if (dataContext != null)
            {
                (viewHolder as IMvxRecyclerViewHolder).DataContext = BindingContext.DataContext;
                viewHolder.ItemView.SetOnClickListener(new AROnClickListener(OnDateTimeViewClick));
            }
        }

        protected virtual void OnDateTimeViewClick()
        {
            var dialogView = this.BindingInflate(Resource.Layout.app_basket_full_order_item_deliveryTime_dialog, null);

            var headerSubtitle = dialogView.FindViewById<TextView>(Resource.Id.app_basket_full_order_item_deliveryTime_dialog_headerLayout_subtitle);
            var headerTitle = dialogView.FindViewById<TextView>(Resource.Id.app_basket_full_order_item_deliveryTime_dialog_headerLayout_title);

            var dateListView = dialogView.FindViewById<ARNumberPicker>(Resource.Id.app_basket_full_order_item_deliveryTime_dialog_bodyLayout_listViewLeft);
            dateListView.WrapSelectorWheel = false;
            dateListView.DescendantFocusability = Android.Views.DescendantFocusability.BlockDescendants;

            var timeListView = dialogView.FindViewById<ARNumberPicker>(Resource.Id.app_basket_full_order_item_deliveryTime_dialog_bodyLayout_listViewRight);
            timeListView.WrapSelectorWheel = false;
            timeListView.DescendantFocusability = Android.Views.DescendantFocusability.BlockDescendants;

            var positiveButton = dialogView.FindViewById<Button>(Resource.Id.app_basket_full_order_item_deliveryTime_dialog_positiveButton);
            var negativeButton = dialogView.FindViewById<Button>(Resource.Id.app_basket_full_order_item_deliveryTime_dialog_negativeButton);

            var set = this.CreateBindingSet<FullOrderFragment, IFullOrderViewModel>();

            set.Bind(headerSubtitle).To(vm => vm.DeliveryViewModel.SelectedDeliveryTime.Name);
            set.Bind(headerTitle).To(vm => vm.DeliveryViewModel.SelectedDeliveryDay.Name);

			set.Bind(dateListView).For("SelectedItem").To(vm => vm.DeliveryViewModel.SelectedDeliveryDay);
            set.Bind(dateListView).For(v => v.ItemsSource).To(vm => vm.DeliveryViewModel.DaysItems);

			set.Bind(timeListView).For("SelectedItem").To(vm => vm.DeliveryViewModel.SelectedDeliveryTime);
            set.Bind(timeListView).For(v => v.ItemsSource).To(vm => vm.DeliveryViewModel.TimeItems);

            set.Apply();

            var dialog = new AlertDialog.Builder(Context)
                                        .SetCancelable(true)
                                        .SetView(dialogView)
                                        .Show();

            positiveButton.Text = "ГОТОВО";
            positiveButton.Click += (s, args) =>
            {
                ViewModel.DeliveryViewModel.ApplyDeliveryTimeCommand.Execute(timeListView.SelectedItem);
                dialog.Dismiss();
            };

            negativeButton.Text = "ОТМЕНИТЬ";
            negativeButton.Click += (s, args) => dialog.Cancel();
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

        protected virtual void BindContols(Android.Views.View view)
        {
            var set = this.CreateBindingSet<FullOrderFragment, IFullOrderViewModel>();

            BindDeliveryPrice(view.FindViewById<TextView>(Resource.Id.app_basket_full_order_deliveryLayout_price), set);

            set.Apply();
        }

        protected virtual void BindDeliveryPrice(TextView textView, MvxFluentBindingDescriptionSet<FullOrderFragment, IFullOrderViewModel> set)
        {
            set.Bind(textView).To(vm => vm.DeliveryViewModel.DeliveryPrice).WithConversion("StringFormat", new StringFormatParameter
            {
                StringFormat = (arg) =>
                {
                    return arg == null ? (Mvx.Resolve<IBasketConfigService>().Config.DeliveryEmptyText ?? "от 0 \u20BD") : ((decimal)arg).ToString("# ### ##0.## \u20BD;;бесплатно").Trim();
                }
            });
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_basket_full_order_recyclerView);
            SetupRecyclerView(view, _recyclerView);

            BindContols(view);

            ViewModel.DeliveryViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public override void OnDestroy()
        {
            if (ViewModel?.DeliveryViewModel != null)
                ViewModel.DeliveryViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            base.OnDestroy();
        }
    }
}
