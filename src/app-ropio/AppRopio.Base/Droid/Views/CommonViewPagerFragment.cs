using System;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Droid.Support.V4;
using Android.Views;

namespace AppRopio.Base.Droid.Views
{
    public class CommonViewPagerFragment : MvxFragment
    {
        private static string SAVE_STATE_LAYOUT_ID = "MvxPagerSaveStateLayoutID";

        private int layoutId = -1;
        public int LayoutId 
        { 
            get => layoutId;
            set
            {
                layoutId = value;
            }
        }

        public CommonViewPagerFragment()
        {

        }

        public CommonViewPagerFragment(int layoutId)
        {
            LayoutId = layoutId;
        }

        private void GetLayoutId(Android.OS.Bundle savedInstanceState)
        {
            if (savedInstanceState != null && savedInstanceState.ContainsKey(SAVE_STATE_LAYOUT_ID))
            {
                LayoutId = savedInstanceState.GetInt(SAVE_STATE_LAYOUT_ID);
            }
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            GetLayoutId(savedInstanceState);

            this.EnsureBindingContextIsSet(inflater);

            return LayoutId > 0 ? this.BindingInflate(LayoutId, null) : new View(Context);
        }

        public override void OnSaveInstanceState(Android.OS.Bundle outState)
        {
            outState.PutInt(SAVE_STATE_LAYOUT_ID, LayoutId);
            base.OnSaveInstanceState(outState);
        }

        public override void OnViewStateRestored(Android.OS.Bundle savedInstanceState)
        {
            GetLayoutId(savedInstanceState);
            base.OnViewStateRestored(savedInstanceState);
        }

        public override void OnCreate(Android.OS.Bundle savedInstanceState)
        {
            GetLayoutId(savedInstanceState);
            base.OnCreate(savedInstanceState);
        }
    }
}
