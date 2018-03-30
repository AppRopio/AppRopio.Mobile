using AppRopio.ECommerce.Marked.Core.ViewModels.Marked;
using AppRopio.ECommerce.Products.Droid.Views.Catalog;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace AppRopio.ECommerce.Marked.Droid.Views.Marked
{
    public class MarkedFragment : CatalogFragment<IMarkedViewModel>
    {
        public MarkedFragment()
            : base (Resource.Layout.app_marked, "Избранное")
        {
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            RecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_marked_items);

            return view;
        }

        protected override bool HasHeader(object arg)
        {
            return false;
        }
    }
}
