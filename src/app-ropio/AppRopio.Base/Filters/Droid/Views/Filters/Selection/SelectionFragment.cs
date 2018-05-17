using System;
using AppRopio.Base.Droid.Views;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Selection;
using AppRopio.Base.Filters.Core;
using Android.Widget;

namespace AppRopio.Base.Filters.Droid.Views.Filters.Selection
{
    public class SelectionFragment : CommonFragment<IFilterSelectionViewModel>
    {
        protected const int CLEAR_ID = 1;

        public SelectionFragment()
            : base (Resource.Layout.app_filters_selection)
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
            var menuItem = menu.Add(0, CLEAR_ID, 0, new Java.Lang.String(LocalizationService.GetLocalizableString(FiltersConstants.RESX_NAME, "Selection_Clear")));
            menuItem.SetShowAsAction(Android.Views.ShowAsAction.Always);
            menuItem.SetActionView(Resource.Layout.app_filters_filters_clearButton);
            menuItem.ActionView.Click += (sender, e) =>
            {
                ViewModel?.ClearCommand.Execute(null);
            };
            (menuItem.ActionView as Button).Text = LocalizationService.GetLocalizableString(FiltersConstants.RESX_NAME, "Selection_Clear");
        }
    }
}
