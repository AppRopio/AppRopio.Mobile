using System;
using Android.Support.V4.App;
using Android.Support.V7.App;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Droid.Listeners;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Products.Core.ViewModels.ContentSearch;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Platforms.Android.Views;
using MvvmCross;

namespace AppRopio.ECommerce.Products.Droid.Views.ContentSearch
{
    public class ContentSearchFragment : CommonFragment<IContentSearchViewModel>
    {
        protected const int MENU_CANCEL_SEARCH_ID = 1;

        protected int FragmentContentId { get; set; }

        public ContentSearchFragment()
            : base(Resource.Layout.app_products_contentsearch, null)
        {
            HasOptionsMenu = true;

            FragmentContentId = Resource.Id.app_products_contentsearch_content;
        }

        protected virtual IMvxFragmentView CreateFragment(string fragmentName)
        {
            try
            {
                var fragment = (IMvxFragmentView)Fragment.Instantiate(Activity, fragmentName);
                return fragment;
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot create Fragment '{fragmentName}'. Are you use the wrong base class?", ex);
            }
        }

        protected virtual void PushChildFragment(FragmentManager fragmentManager, FragmentTransaction fragmentTransaction, string fragmentName, IMvxViewModel viewModel)
        {
            var newFragment = fragmentManager.FindFragmentByTag(fragmentName);
            if (newFragment != null)
            {
                if (newFragment.IsDetached)
                {
                    if (newFragment is IMvxFragmentView mvxFragment)
                        mvxFragment.ViewModel = viewModel;

                    fragmentTransaction.Attach(newFragment);
                }
            }
            else
            {
                var fragment = CreateFragment(fragmentName);
                fragment.ViewModel = viewModel;

                var fragmentView = fragment as Fragment;

                if (fragmentView != null)
                    fragmentTransaction.Add(FragmentContentId, fragmentView, fragmentName);
            }
        }

        protected virtual void PopChildFragment(FragmentManager fragmentManager, FragmentTransaction fragmentTransaction, string fragmentName)
        {
            var oldFragment = fragmentManager.FindFragmentByTag(fragmentName);
            if (oldFragment != null)
                fragmentTransaction.Remove(oldFragment);
        }

        protected virtual void AddSearchContentFragment()
        {
            var viewType = Mvx.IoCProvider.Resolve<IViewLookupService>().Resolve<IContentSearchInternalViewModel>();

            var fragmentName = Java.Lang.Class.FromType(viewType).Name;

            var fragmentTransaction = ChildFragmentManager.BeginTransaction();

            fragmentTransaction.DisallowAddToBackStack();

            PushChildFragment(ChildFragmentManager, fragmentTransaction, fragmentName, ViewModel.ContentVm);

            fragmentTransaction.CommitNow();
        }

        protected virtual void RemoveSearchContentFragment()
        {
            var viewType = Mvx.IoCProvider.Resolve<IViewLookupService>().Resolve<IContentSearchInternalViewModel>();

            var fragmentName = Java.Lang.Class.FromType(viewType).Name;

            var fragmentTransaction = ChildFragmentManager.BeginTransaction();

            fragmentTransaction.DisallowAddToBackStack();

            PopChildFragment(ChildFragmentManager, fragmentTransaction, fragmentName);

            fragmentTransaction.CommitNow();
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var searchView = this.BindingInflate(Resource.Layout.app_products_contentsearch_search, null, false);

            var supportActionBar = (Activity as AppCompatActivity).SupportActionBar;
            supportActionBar.CustomView = searchView;
        }

        public override void OnCreateOptionsMenu(Android.Views.IMenu menu, Android.Views.MenuInflater inflater)
        {
            var menuItem = menu.Add(0, MENU_CANCEL_SEARCH_ID, 0, new Java.Lang.String(""));
            menuItem.SetIcon(Resource.Drawable.abc_ic_clear_material);
            menuItem.SetShowAsAction(Android.Views.ShowAsAction.Always);
        }

        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            switch (item.ItemId)
            {
                case MENU_CANCEL_SEARCH_ID:
                    ViewModel?.CancelSearchCommand.Execute(null);
                    return base.OnOptionsItemSelected(item);
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnResume()
        {
            base.OnResume();

            var supportActionBar = (Activity as AppCompatActivity)?.SupportActionBar;
            supportActionBar?.SetDisplayShowTitleEnabled(false);
            supportActionBar?.SetDisplayShowCustomEnabled(true);

            if (supportActionBar != null)
            {
                var inputView = supportActionBar.CustomView?.FindViewById(Resource.Id.app_products_contentsearch_searchInput);
                if (inputView != null)
                {
                    ShowKeyboard(inputView); 

                    inputView.SetOnKeyListener(new AROnEnterKeyListener(Activity, () =>
                    {
                        if (!ViewModel.SearchText.IsNullOrEmpty())
                            ViewModel.SearchCommand.Execute(null);
                    }));
                }
            }

            AddSearchContentFragment();
        }

        public override void OnPause()
        {
            base.OnPause();

            var supportActionBar = (Activity as AppCompatActivity)?.SupportActionBar;
            supportActionBar?.SetDisplayShowTitleEnabled(true);
            supportActionBar?.SetDisplayShowCustomEnabled(false);

            HideKeyboard();

            RemoveSearchContentFragment();
        }
    }
}
