using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.Droid.Views.Catalog;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace AppRopio.ECommerce.Products.Droid.Views.Categories.Cataloged
{
    public class CCategoriesCatalogFragment : CatalogFragment<ICatalogViewModel>
    {
        public CCategoriesCatalogFragment()
            : base(Resource.Layout.app_ccategories_products_catalog)
        {
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            RecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_ccategories_products_catalog_items);

            return view;
        }

        protected override void SetupTitle()
        {
            
        }
    }
}