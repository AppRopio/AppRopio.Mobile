using System;
using Android.Support.V7.Widget;
using Android.Util;
using Android.Views;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Droid.Adapters;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Items;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;

namespace AppRopio.ECommerce.Products.Droid.Views.Catalog
{
    public class CatalogFragment : CatalogFragment<ICatalogViewModel>
    {
        public CatalogFragment()
            : base(Resource.Layout.app_products_catalog)
        {
        }

        public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, Android.Views.ViewGroup container, Android.OS.Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            RecyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_products_catalog_items);

            return view;
        }
    }

    public abstract class CatalogFragment<T> : ProductsFragment<T>
        where T : class, ICatalogViewModel
    {
        private bool _headerInitialized;

        protected MvxRecyclerView RecyclerView { get; set; }

        protected CatalogCollectionType CollectionType { get; set; }

        protected CatalogFragment(int layoutId)
            : base(layoutId)
        {
        }

        protected CatalogFragment(int layoutId, string title)
            : base(layoutId, title)
        {
        }

        protected virtual void SetupRecyclerView(MvxRecyclerView recyclerView)
        {
            _headerInitialized = false;
            SetupAdapter(recyclerView);
            SetupLayoutManager(recyclerView);
        }

        protected virtual void SetupAdapter(MvxRecyclerView recyclerView)
        {
            recyclerView.Adapter = new ARPagingFlatGroupRecyclerAdapter(ViewModel, null, SetupItemTemplateSelector(), BindingContext)
            {
                HasHeader = HasHeader,
                TuneSectionHeaderOnBind = TuneSectionHeaderOnBind,
                TuneSectionItemOnBind = TuneSectionItemOnBind
            };
        }

        protected virtual bool HasHeader (object arg)
        {
            return ViewModel.Items.IndexOf((ICatalogItemVM)arg) == 0 && ViewModel.HeaderVm != null;
        }

        protected virtual void TuneSectionHeaderOnBind(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            if (!_headerInitialized)
            {
                var config = Mvx.Resolve<IProductConfigService>().Config;
                var viewLookupService = Mvx.Resolve<IViewLookupService>();
                if (config.Header != null && viewLookupService.IsRegistered(config.Header.TypeName))
                {
                    if (Activator.CreateInstance(viewLookupService.Resolve(config.Header.TypeName), Context) is IMvxAndroidView headerView)
                    {
                        headerView.BindingContext = new MvxAndroidBindingContext(Context, new MvxSimpleLayoutInflaterHolder(LayoutInflater), ViewModel.HeaderVm);

                        (viewHolder.ItemView as ViewGroup)?.AddView((View)headerView);
                    }

                    _headerInitialized = true;
                }
            }
        }

        private void TuneSectionItemOnBind(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            var config = Mvx.Resolve<IProductConfigService>().Config;
            var viewLookupService = Mvx.Resolve<IViewLookupService>();
            if (config.Basket?.ItemAddToCart != null && viewLookupService.IsRegistered(config.Basket?.ItemAddToCart.TypeName))
            {
                var viewModel = (viewHolder as IMvxRecyclerViewHolder)?.DataContext as ICatalogItemVM;
                var basketView = viewModel.BasketBlockViewModel == null ? null : Activator.CreateInstance(viewLookupService.Resolve(config.Basket?.ItemAddToCart.TypeName), Context) as IMvxAndroidView;
                if (basketView != null)
                {
                    basketView.BindingContext = new MvxAndroidBindingContext(Context, new MvxSimpleLayoutInflaterHolder(LayoutInflater), viewModel.BasketBlockViewModel);

                    var basketLayout = viewHolder.ItemView.FindViewById<ViewGroup>(Resource.Id.app_products_catalogCard_basketLayout);
                    basketLayout?.AddView((View)basketView);
                    basketLayout.Visibility = ViewStates.Visible;
                }
            }
        }

        protected virtual void SetupLayoutManager(RecyclerView recyclerView)
        {
            if (CollectionType == CatalogCollectionType.Grid)
            {
                var gridLayoutManager = new GridLayoutManager(Context, 2, GridLayoutManager.Vertical, false);
                gridLayoutManager.SetSpanSizeLookup(new CatalogSpanSizeLookup(gridLayoutManager, ViewModel));

                recyclerView.SetLayoutManager(gridLayoutManager);
            }
            else
                recyclerView.SetLayoutManager(new LinearLayoutManager(Context, LinearLayoutManager.Vertical, false));
        }

        protected virtual IARFlatGroupTemplateSelector SetupItemTemplateSelector()
        {
            return new CatalogTemplateSelector(CollectionType);
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var listTypeValue = new TypedValue();
            Activity.Theme.ResolveAttribute(Resource.Attribute.app_products_catalog_collectionType, listTypeValue, false);

            CollectionType = (CatalogCollectionType)listTypeValue.Data;

            SetupRecyclerView(RecyclerView);

            HasOptionsMenu = ViewModel?.VmNavigationType != Base.Core.Models.Navigation.NavigationType.None && ViewModel?.VmNavigationType != Base.Core.Models.Navigation.NavigationType.InsideScreen;
        }
    }

    public class CatalogSpanSizeLookup : GridLayoutManager.SpanSizeLookup
    {
        private GridLayoutManager _layoutManager;
        private ICatalogViewModel _viewModel;

        public CatalogSpanSizeLookup(GridLayoutManager layoutManager, ICatalogViewModel viewModel)
        {
            _layoutManager = layoutManager;
            _viewModel = viewModel;
        }

        public override int GetSpanSize(int position)
        {
            return (position == 0 && _viewModel.HeaderVm != null) ? _layoutManager.SpanCount : 1;
        }
    }
}
