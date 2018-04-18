using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using MvvmCross.Droid.Support.V7.RecyclerView;
using AppRopio.ECommerce.HistoryOrders.Core;

namespace AppRopio.ECommerce.HistoryOrders.Droid.Views.HistoryOrders
{
    public class HistoryOrdersFragment : CommonFragment<IHistoryOrdersViewModel>
    {
        public HistoryOrdersFragment()
            : base (Resource.Layout.app_historyorders)
        {
            Title = LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "History_Title");
        }

        protected virtual void SetupRecyclerView(MvxRecyclerView recyclerView)
        {
            SetupAdapter(recyclerView);
        }

        private void SetupAdapter(MvxRecyclerView recyclerView)
        {
            recyclerView.Adapter = new ARPagingFlatGroupRecyclerAdapter(ViewModel, null, null, BindingContext);
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SetupRecyclerView(view.FindViewById<MvxRecyclerView>(Resource.Id.app_historyorders_items));
        }
    }
}
