using System;
using Android.Widget;
using AppRopio.ECommerce.Products.Core.ViewModels.ModalProductCard;
using AppRopio.ECommerce.Products.Droid.Views.ProductCard;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace AppRopio.ECommerce.Products.Droid.Views.ModalProductCard
{
    public class ModalProductCardFragment : ProductCardFragment<IModalProductCardViewModel>
    {
        public ModalProductCardFragment()
            : base(Resource.Layout.app_products_modalProductCard, " ")
        {
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            RecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_products_modalProductCard_recyclerView);

            BasketLayout = view.FindViewById<FrameLayout>(Resource.Id.app_products_modalProductCard_basketLayout);

            return view;
        }
    }
}
