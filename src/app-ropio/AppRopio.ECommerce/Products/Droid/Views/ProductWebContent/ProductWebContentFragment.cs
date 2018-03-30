using System;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductWebContent;
namespace AppRopio.ECommerce.Products.Droid.Views.ProductWebContent
{
    public class ProductWebContentFragment : CommonFragment<IProductWebContentViewModel>
    {
        public ProductWebContentFragment()
            : base (Resource.Layout.app_products_webContent)
        {
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Title = ViewModel.Title;
        }
    }
}
