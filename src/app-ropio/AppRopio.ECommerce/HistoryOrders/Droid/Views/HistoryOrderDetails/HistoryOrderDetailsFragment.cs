using System;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.HistoryOrders.Core.ViewModels.HistoryOrders;
using AppRopio.ECommerce.HistoryOrders.Core;

namespace AppRopio.ECommerce.HistoryOrders.Droid.Views.HistoryOrderDetails
{
    public class HistoryOrderDetailsFragment : CommonFragment<IHistoryOrderDetailsViewModel>
    {
        public HistoryOrderDetailsFragment()
            : base (Resource.Layout.app_historyorders_details)
        {
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Title = $"{LocalizationService.GetLocalizableString(HistoryOrdersConstants.RESX_NAME, "HistoryOrderDetails_Title")} {ViewModel?.OrderNumber}";
        }
    }
}
