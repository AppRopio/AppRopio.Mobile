using System;
using AppRopio.ECommerce.Products.Droid.Views.Catalog;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Disabled;
using MvvmCross.Droid.Support.V7.RecyclerView;
using AppRopio.Base.Droid.Adapters;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using Android.Support.V7.Widget;
using AppRopio.Base.Droid.Controls;
using MvvmCross.Binding.Droid.BindingContext;

namespace AppRopio.ECommerce.Products.Droid.Views.Categories.Disabled
{
    public class DCategoriesFragment : CatalogFragment<IDCategoriesViewModel>
    {
        public DCategoriesFragment()
            : base(Resource.Layout.app_products_dcategories)
        {
        }

        #region Private

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.TopBanners))
                (RecyclerView.Adapter as ARFlatGroupAdapter)?.ReloadData();
            if (e.PropertyName == nameof(ViewModel.BottomBanners))
                (RecyclerView.Adapter as ARFlatGroupAdapter)?.ReloadData();
        }

        #endregion

        #region Protected

        #region RecyclerView adapter

        protected override void SetupAdapter(MvxRecyclerView recyclerView)
        {
            recyclerView.Adapter = new ARPagingFlatGroupRecyclerAdapter(ViewModel, null, SetupItemTemplateSelector(), BindingContext)
            {
                HasHeader = CheckSectionHasHeader,
                HasFooter = CheckSectionHasFooter,
                TuneSectionHeaderOnBind = TuneSectionHeader,
                TuneSectionFooterOnBind = TuneSectionFooter,
                TuneViewHolderOnCreate = TuneViewHolder
            };
        }

        protected override IARFlatGroupTemplateSelector SetupItemTemplateSelector()
        {
            return new DCatalogTemplateSelector(CollectionType);
        }

        protected virtual bool CheckSectionHasHeader(object arg)
        {
            return !ViewModel.TopBanners.IsNullOrEmpty() && ViewModel.Items.IndexOf((arg as ICatalogItemVM)) == 0;
        }

        protected virtual bool CheckSectionHasFooter(object arg)
        {
            return !ViewModel.BottomBanners.IsNullOrEmpty() && ViewModel.Items.IndexOf((arg as ICatalogItemVM)) == (ViewModel.Items.Count - 1);
        }

        protected virtual void TuneSectionHeader(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            var layoutParams = new StaggeredGridLayoutManager.LayoutParams(viewHolder.ItemView.LayoutParameters);
            layoutParams.FullSpan = true;

            viewHolder.ItemView.LayoutParameters = layoutParams;

            SetupHeaderView(viewHolder.ItemView);
            (viewHolder as IMvxRecyclerViewHolder).DataContext = BindingContext.DataContext;
        }

        protected virtual void TuneSectionFooter(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            var layoutParams = new StaggeredGridLayoutManager.LayoutParams(viewHolder.ItemView.LayoutParameters);
            layoutParams.FullSpan = true;

            viewHolder.ItemView.LayoutParameters = layoutParams;

            SetupFooterView(viewHolder.ItemView);
            (viewHolder as IMvxRecyclerViewHolder).DataContext = BindingContext.DataContext;
        }

        protected virtual void TuneViewHolder(RecyclerView.ViewHolder viewHolder, int viewType)
        {

        }

        #endregion

        protected virtual void SetupHeaderView(Android.Views.View view)
        {
            var headerView = view.FindViewById<ARViewPager>(Resource.Id.app_products_sscategories_banners_top);
            SetupViewPager(headerView);
        }

        protected virtual void SetupFooterView(Android.Views.View view)
        {
            var footerView = view.FindViewById<ARViewPager>(Resource.Id.app_products_sscategories_banners_bottom);
            SetupViewPager(footerView);
        }

        protected virtual void SetupViewPager(ARViewPager viewPager)
        {
            var adapter = new ARViewPagerAdapter(Context, (IMvxAndroidBindingContext)BindingContext);
            viewPager.Adapter = adapter;
        }

        #endregion

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            RecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_products_dcategories_items);

            return view;
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public override void OnDestroy()
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            base.OnDestroy();
        }
    }
}
