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
using AppRopio.ECommerce.Basket.Core.ViewModels.ProductCard;
using AppRopio.Base.Droid.Listeners;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace AppRopio.ECommerce.Basket.Droid.Views.ProductCard
{
    public class BasketProductCardView : RelativeLayout, IMvxAndroidView<IBasketProductCardViewModel>
    {
        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (IBasketProductCardViewModel)value;
            }
        }

        protected bool IsBinded { get; private set; }

        public IBasketProductCardViewModel ViewModel { get; set; }

        public object DataContext
        {
            get => ViewModel;
            set => ViewModel = (IBasketProductCardViewModel)value;
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

        public BasketProductCardView(Context context) :
            base(context)
        {
            Initialize();
        }

        public BasketProductCardView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public BasketProductCardView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        private void Initialize()
        {
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, Resources.GetDimensionPixelSize(Resource.Dimension.app_button_height));
        }

        private void BindControls()
        {
            if (BindingContext != null && !IsBinded)
            {
                var view = this.BindingInflate(Resource.Layout.app_basket_productCardView, null, false);

                var editText = view.FindViewById<EditText>(Resource.Id.app_basket_productCardView_editText);
                editText.SetOnKeyListener(new AROnEnterKeyListener(Mvx.Resolve<IMvxAndroidCurrentTopActivity>()?.Activity, HandleAction));
                editText.ClearFocus();

                AddView(view);
            }

            IsBinded = true;
        }

        private void HandleAction()
        {
            ViewModel.QuantityChangedCommand.Execute();

            this.ClearFocus();
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            //nothing
        }
    }
}
