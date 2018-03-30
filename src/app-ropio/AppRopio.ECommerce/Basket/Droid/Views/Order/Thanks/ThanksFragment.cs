using System;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Thanks;
namespace AppRopio.ECommerce.Basket.Droid.Views.Order.Thanks
{
    public class ThanksFragment : CommonFragment<IThanksViewModel>
    {
        public ThanksFragment()
            : base (Resource.Layout.app_basket_thanks)
        {
        }
    }
}
