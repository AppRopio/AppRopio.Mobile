using System;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductTextContent;
using Android.Widget;
using Android.Text.Method;
namespace AppRopio.ECommerce.Products.Droid.Views.ProductTextContent
{
    public class ProductTextContentFragment : CommonFragment<IProductTextContentViewModel>
    {
        public ProductTextContentFragment()
            : base (Resource.Layout.app_products_textContent)
        {
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Title = ViewModel.Title;

            if (view is TextView textView)
                textView.MovementMethod = new ScrollingMovementMethod();
        }
    }
}
