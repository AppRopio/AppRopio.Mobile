using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using AppRopio.Base.Droid.Listeners;
using AppRopio.ECommerce.Basket.Core.ViewModels.CatalogCard;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace AppRopio.ECommerce.Basket.Droid.Views.CatalogCard
{
    public class BasketCatalogItemView : RelativeLayout, IMvxAndroidView<IBasketCatalogItemVM>
    {
        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (IBasketCatalogItemVM)value;
            }
        }

        protected bool IsBinded { get; private set; }

        public IBasketCatalogItemVM ViewModel { get; set; }

        public object DataContext
        {
            get => ViewModel;
            set => ViewModel = (IBasketCatalogItemVM)value;
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

        public BasketCatalogItemView(Context context) :
            base(context)
        {
            Initialize();
        }

        public BasketCatalogItemView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public BasketCatalogItemView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
        }

        private void BindControls()
        {
            if (BindingContext != null && !IsBinded)
            {
                var view = this.BindingInflate(Resource.Layout.app_basket_catalogCardView, null, false);

                AddView(view);
            }

            IsBinded = true;
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            //nothing
        }
    }
}
