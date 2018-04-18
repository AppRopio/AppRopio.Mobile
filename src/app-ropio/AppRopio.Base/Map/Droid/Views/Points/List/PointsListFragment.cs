using System;
using Android.Util;
using Android.Views;
using AppRopio.Base.Droid.Views;
using AppRopio.Base.Map.Core.ViewModels.Points.List;
using MvvmCross.Droid.Support.V7.RecyclerView;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Map.Core;

namespace AppRopio.Base.Map.Droid.Views.Points.List
{
    public class PointsListFragment : CommonFragment<IPointsListViewModel>
    {
        protected const int MENU_MAP_ID = 1;
        
        public PointsListFragment()
            : base(Resource.Layout.app_map_list)
        {
            HasOptionsMenu = true;
            Title = LocalizationService.GetLocalizableString(MapConstants.RESX_NAME, "List_Title");
        }

        protected virtual void SetupRecyclerView(MvxRecyclerView recyclerView)
        {
            SetupAdapter(recyclerView);
        }

        private void SetupAdapter(MvxRecyclerView recyclerView)
        {
            recyclerView.Adapter = new ARPagingFlatGroupRecyclerAdapter(ViewModel, null, null, BindingContext);
        }

        public override void OnViewCreated(View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_map_listrecyclerView);
            SetupRecyclerView(recyclerView);
        }
        public override void OnCreateOptionsMenu(Android.Views.IMenu menu, Android.Views.MenuInflater inflater)
        {
            var typedValue = new TypedValue();
            Activity.Theme.ResolveAttribute(Resource.Attribute.app_map_ic_map, typedValue, true);

            var mapMenuItem = menu.Add(0, MENU_MAP_ID, 0, new Java.Lang.String(LocalizationService.GetLocalizableString(MapConstants.RESX_NAME, "Map_Title")));
            mapMenuItem.SetIcon(Resources.GetDrawable(typedValue.ResourceId, Context.Theme));
            mapMenuItem.SetShowAsAction(ShowAsAction.Always);
        }

        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            switch (item.ItemId)
            {
                case MENU_MAP_ID:
                    ViewModel?.MapCommand.Execute(null);
                    return base.OnOptionsItemSelected(item);
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}
