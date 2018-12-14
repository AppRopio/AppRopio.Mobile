using System;
using System.Collections;
using Android.Content;
using Android.Graphics;
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
            SetupLayoutManager(recyclerView);
            SetupItemDecoration(recyclerView);
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

        protected virtual void SetupItemDecoration(RecyclerView recyclerView)
        {
            if (CollectionType == CollectionType.Grid) {
                var itemDecoration = new CategoriesItemDecoration(Context, Resource.Dimension.app_products_sscategories_item_grid_spacing, ViewModel);
                recyclerView.AddItemDecoration(itemDecoration);
//                recyclerView.SetClipToPadding(false);
            }
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
        private readonly GridLayoutManager _layoutManager;
        private readonly ISSCategoriesViewModel _viewModel;

        public CategoriesSpanSizeLookup(GridLayoutManager layoutManager, ISSCategoriesViewModel viewModel)
        {
            _layoutManager = layoutManager;
            _viewModel = viewModel;
        }
        //TODO: test with header, footer, both, without
        public override int GetSpanSize(int position)
        {
            if (_viewModel == null || _viewModel.Items.IsNullOrEmpty())
                return 1;

            bool hasHeader = !_viewModel.TopBanners.IsNullOrEmpty();
            bool isHeader = position == 0 && hasHeader;

            bool hasFooter = !_viewModel.BottomBanners.IsNullOrEmpty();
            bool isFooter = (position == (_viewModel.Items.Count + (hasHeader ? 1 : 0)) && hasFooter);

            return (isHeader || isFooter) ? _layoutManager.SpanCount : 1;
        }
    }

    public class CategoriesItemDecoration : RecyclerView.ItemDecoration
    {
        private readonly int _spacing;
        private readonly ISSCategoriesViewModel _viewModel;

        public CategoriesItemDecoration(int spacing, ISSCategoriesViewModel viewModel)
        {
            _spacing = spacing;
            _viewModel = viewModel;
        }
        public CategoriesItemDecoration(Context context, int id, ISSCategoriesViewModel viewModel)
            : this(context.Resources.GetDimensionPixelSize(id), viewModel) {}

        public override void GetItemOffsets(Rect outRect, View view, RecyclerView parent, RecyclerView.State state)
        {
            base.GetItemOffsets(outRect, view, parent, state);

            int position = parent.GetChildAdapterPosition(view);

            bool hasHeader = !_viewModel.TopBanners.IsNullOrEmpty();
            bool isHeader = position == 0 && hasHeader;

            bool hasFooter = !_viewModel.BottomBanners.IsNullOrEmpty();
            bool isFooter = (position == (_viewModel.Items.Count + (hasHeader ? 1 : 0)) && hasFooter);

            if (isHeader || isFooter) {
                outRect.Set(0, 0, 0, 0);
            } else {
                if (parent.GetLayoutManager() is GridLayoutManager layoutManager) {
                    //having header so adjust indexing for "smart" spacing
                    if (hasHeader)
                        position--;

                    int spanCount = layoutManager.SpanCount;
                    int column = position % spanCount;

                    int left = (int)(_spacing - (float)column * _spacing / spanCount),
                        right = (int)((column + 1) * (float)_spacing / spanCount),
                        top = position < spanCount ? _spacing : 0,
                        bottom = _spacing;

                    outRect.Set(left, top, right, bottom);
                } else {
                    outRect.Set(_spacing, _spacing, _spacing, _spacing);
                }
            }
        }
    }
}
