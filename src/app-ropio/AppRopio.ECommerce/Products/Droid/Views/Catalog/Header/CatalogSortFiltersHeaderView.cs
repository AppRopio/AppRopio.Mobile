using System;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using AppRopio.ECommerce.Products.Core.ViewModels.Catalog.Header;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.Platforms.Android.Views;

namespace AppRopio.ECommerce.Products.Droid.Views.Catalog.Header
{
    public class CatalogSortFiltersHeaderView : RelativeLayout, IMvxAndroidView<CatalogSortFiltersHeaderVM>
    {
        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (CatalogSortFiltersHeaderVM)value;
            }
        }

        protected bool IsBinded { get; private set; }

        public CatalogSortFiltersHeaderVM ViewModel { get; set; }

        public object DataContext
        {
            get => ViewModel;
            set => ViewModel = (CatalogSortFiltersHeaderVM)value;
        }

        public LayoutInflater LayoutInflater { get; set; }

        public IMvxBindingContext _bindingContext;
        public IMvxBindingContext BindingContext
        {
            get
            {
                return _bindingContext;
            }
            set
            {
                _bindingContext = value;
                LayoutInflater = (_bindingContext as IMvxAndroidBindingContext).LayoutInflaterHolder.LayoutInflater;
                DataContext = _bindingContext.DataContext;

                BindControls();
            }
        }

        public CatalogSortFiltersHeaderView(Context context) :
            base(context)
        {
            Initialize();
        }

        public CatalogSortFiltersHeaderView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public CatalogSortFiltersHeaderView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, Resources.GetDimensionPixelSize(Resource.Dimension.app_products_catalog_headerHeight));
        }

        private void BindControls()
        {
            if (BindingContext != null && !IsBinded)
                this.BindingInflate(Resource.Layout.app_products_catalog_sort_filters_header, this, true);

            IsBinded = true;
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            //nothing
        }
    }
}
