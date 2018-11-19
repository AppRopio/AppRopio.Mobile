using System;
using System.Collections;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Controls;
using AppRopio.ECommerce.Products.Core.Models;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories;
using AppRopio.ECommerce.Products.Core.ViewModels.Categories.Items;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace AppRopio.ECommerce.Products.Droid.Views.Categories.StepByStep
{
    public class SSCategoriesFragment : ProductsFragment<ISSCategoriesViewModel>
    {
        private MvxRecyclerView _recyclerView;

        protected CollectionType CollectionType { get; set; }

        public SSCategoriesFragment()
            : base(Resource.Layout.app_products_sscategories)
        {
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.TopBanners))
                (_recyclerView.Adapter as ARFlatGroupAdapter)?.ReloadData();
            if (e.PropertyName == nameof(ViewModel.BottomBanners))
                (_recyclerView.Adapter as ARFlatGroupAdapter)?.ReloadData();
        }

        #region RecyclerView adapter

        protected virtual void SetupRecyclerView(View view, MvxRecyclerView recyclerView)
        {
            recyclerView.Adapter = SetupAdapter(recyclerView);
        }

        protected virtual IMvxRecyclerAdapter SetupAdapter(MvxRecyclerView recyclerView)
        {
            return new ARFlatGroupAdapter(GetInnerItems, SetupTemplateSelector(), BindingContext)
            {
				HasHeader = CheckSectionHasHeader,
                HasFooter = CheckSectionHasFooter,
                TuneSectionHeaderOnBind = TuneSectionHeader,
                TuneSectionFooterOnBind = TuneSectionFooter,
                TuneViewHolderOnCreate = TuneViewHolder
            };
        }

        protected virtual void SetupLayoutManager(RecyclerView recyclerView)
        {
            if (CollectionType == CollectionType.Grid)
            {
                var gridLayoutManager = new GridLayoutManager(Context, 2, GridLayoutManager.Vertical, false);
                gridLayoutManager.SetSpanSizeLookup(new CategoriesSpanSizeLookup(gridLayoutManager, ViewModel));

                recyclerView.SetLayoutManager(gridLayoutManager);
            }
            else
                recyclerView.SetLayoutManager(new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false));
        }

        protected virtual IEnumerable GetInnerItems(object item)
        {
            return new[] { item };
        }

        protected virtual IARFlatGroupTemplateSelector SetupTemplateSelector()
        {
            return new CategoriesTemplateSelector(CollectionType);
        }

        protected virtual bool CheckSectionHasHeader(object arg)
        {
            return !ViewModel.TopBanners.IsNullOrEmpty() && ViewModel.Items.IndexOf((arg as ICategoriesItemVM)) == 0;
        }

        protected virtual bool CheckSectionHasFooter(object arg)
        {
            return !ViewModel.BottomBanners.IsNullOrEmpty() && ViewModel.Items.IndexOf((arg as ICategoriesItemVM)) == (ViewModel.Items.Count - 1);
        }

        protected virtual void TuneSectionHeader(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            SetupHeaderView(viewHolder.ItemView);
            (viewHolder as IMvxRecyclerViewHolder).DataContext = BindingContext.DataContext; 
        }

        protected virtual void TuneSectionFooter(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
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

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var listTypeValue = new TypedValue();
            Activity.Theme.ResolveAttribute(Resource.Attribute.app_products_categories_collectionType, listTypeValue, false);

            CollectionType = (CollectionType)listTypeValue.Data;

            _recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_products_sscategories_items);
            SetupRecyclerView(view, _recyclerView);

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public override void OnDestroy()
        {
            ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            base.OnDestroy();
        }
    }

    public class CategoriesSpanSizeLookup : GridLayoutManager.SpanSizeLookup
    {
        private GridLayoutManager _layoutManager;
//        private ISSCategoriesViewModel _viewModel;

        public CategoriesSpanSizeLookup(GridLayoutManager layoutManager, ISSCategoriesViewModel viewModel)
        {
            _layoutManager = layoutManager;
//            _viewModel = viewModel;
        }
        //TODO: count proper span size
        public override int GetSpanSize(int position)
        {
            return _layoutManager.SpanCount;// (position == 0 && _viewModel.HeaderVm != null) ? _layoutManager.SpanCount : 1;
        }
    }
}
