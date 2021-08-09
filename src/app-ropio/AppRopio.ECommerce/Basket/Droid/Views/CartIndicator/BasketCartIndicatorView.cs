using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using AppRopio.ECommerce.Basket.Core.ViewModels.CartIndicator;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.Platforms.Android.Views;

namespace AppRopio.ECommerce.Basket.Droid.Views.CartIndicator
{
    public class BasketCartIndicatorView : RelativeLayout, IMvxAndroidView<IBasketCartIndicatorViewModel>
    {
        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (IBasketCartIndicatorViewModel)value;
            }
        }

        protected bool IsBinded { get; private set; }

        public IBasketCartIndicatorViewModel ViewModel { get; set; }

        public object DataContext
        {
            get => ViewModel;
            set => ViewModel = (IBasketCartIndicatorViewModel)value;
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

        public BasketCartIndicatorView(Context context) :
            base(context)
        {
            Initialize();
        }

        public BasketCartIndicatorView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public BasketCartIndicatorView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            var sizeValue = new TypedValue();
            Context.Theme.ResolveAttribute(Resource.Attribute.actionBarSize, sizeValue, true);

            LayoutParameters = new ViewGroup.LayoutParams(Resources.GetDimensionPixelSize(sizeValue.ResourceId), Resources.GetDimensionPixelSize(sizeValue.ResourceId));
        }

        private void BindControls()
        {
            if (BindingContext != null && !IsBinded)
                this.BindingInflate(Resource.Layout.app_basket_cartIndicator, this, true);

            IsBinded = true;
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            //nothing
        }
    }
}
