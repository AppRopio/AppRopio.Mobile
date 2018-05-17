using System;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection;
using AppRopio.ECommerce.Products.Core;
using Android.Widget;

namespace AppRopio.ECommerce.Products.Droid.Views.ProductCard.Selection
{
    public class SelectionFragment : CommonFragment<IProductDetailsSelectionViewModel>
    {
        protected const int CLEAR_ID = 1;

        public SelectionFragment()
            : base (Resource.Layout.app_products_selection)
        {
            HasOptionsMenu = true;
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            Title = ViewModel.Name;
        }

        public override void OnCreateOptionsMenu(Android.Views.IMenu menu, Android.Views.MenuInflater inflater)
        {
            var menuItem = menu.Add(0, CLEAR_ID, 0, new Java.Lang.String(LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "Selection_Clear")));
            menuItem.SetShowAsAction(Android.Views.ShowAsAction.Always);
            menuItem.SetActionView(Resource.Layout.app_products_selection_clearButton);
            menuItem.ActionView.Click += (sender, e) =>
            {
                ViewModel?.ClearCommand.Execute(null);
            };
            (menuItem.ActionView as Button).Text = LocalizationService.GetLocalizableString(ProductsConstants.RESX_NAME, "Selection_Clear");
        }
    }
}
