using System.Collections.Generic;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using AppRopio.Base.Core.ViewModels;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Controls;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories;
using MvvmCross.Binding.Droid.BindingContext;

namespace AppRopio.ECommerce.Products.Droid.Views.Categories.Cataloged
{
    public class CCategoriesPagerFragment : ProductsFragment<ICCategoriesViewModel>
    {
        private ARViewPager _viewPager;
		private TabLayout _tabLayout;
        private List<int> _initializedVmPositions = new List<int>();

        public CCategoriesPagerFragment()
            : base(Resource.Layout.app_products_ccategories)
        {
        }

        protected virtual void SetupViewPager(ARViewPager viewPager)
        {
            viewPager.Adapter = new ARFragmentPagerAdapter(Context, (IMvxAndroidBindingContext)BindingContext, ChildFragmentManager)
            {
                FragmentCreator = (position, vm) =>
                {
                    //init viewModel onle when fragment requested by user
                    if (!_initializedVmPositions.Contains(position))
                    {
                        _initializedVmPositions.Add(position);
                        vm.Initialize();
                    }

                    return new CCategoriesCatalogFragment();
                },
                TitleForViewModel = (itemVM) => ((IBaseViewModel)itemVM)?.Title
            };
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _viewPager = view.FindViewById<ARViewPager>(Resource.Id.app_products_cccategories_pager);

            SetupViewPager(_viewPager);
        }

        public override void OnResume()
        {
            base.OnResume();

            var toolbar = Activity.FindViewById<Toolbar>(Resource.Id.app_toolbar);
            if (toolbar != null && toolbar.Parent is AppBarLayout appBarLayout)
            {
                _tabLayout = (TabLayout)LayoutInflater.Inflate(Resource.Layout.app_products_ccategories_tabs, appBarLayout, false);

                appBarLayout.AddView(_tabLayout);

                _tabLayout.SetupWithViewPager(_viewPager);
            }
        }

        public override void OnPause()
        {
            (_tabLayout?.Parent as ViewGroup)?.RemoveView(_tabLayout);
            
            base.OnPause();
        }
    }
}
