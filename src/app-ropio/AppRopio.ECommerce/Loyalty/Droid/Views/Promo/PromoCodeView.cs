//
//  Copyright 2018 AppRopio
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AppRopio.Base.Droid.Listeners;
using AppRopio.ECommerce.Loyalty.Core.ViewModels.Promo;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.ViewModels;
using MvvmCross.Views;
using MvvmCross.Platforms.Android.Views;
using MvvmCross;
using MvvmCross.Platforms.Android;

namespace AppRopio.ECommerce.Loyalty.Droid.Views.Promo
{
    public class PromoCodeView : FrameLayout, IMvxAndroidView<IPromoCodeViewModel>
    {
        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (IPromoCodeViewModel)value;
            }
        }

        protected bool IsBinded { get; private set; }

        public IPromoCodeViewModel ViewModel { get; set; }

        public object DataContext
        {
            get => ViewModel;
            set => ViewModel = (IPromoCodeViewModel)value;
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

        public PromoCodeView(Context context) 
            : base(context)
        {
            Initialize();
        }

        public PromoCodeView(Context context, IAttributeSet attrs) 
            : base(context, attrs)
        {
            Initialize();
        }

        public PromoCodeView(Context context, IAttributeSet attrs, int defStyleAttr) 
            : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        public PromoCodeView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) 
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize();
        }

        protected PromoCodeView(IntPtr javaReference, JniHandleOwnership transfer) 
            : base(javaReference, transfer)
        {
            Initialize();
        }

        private void Initialize()
        {
            LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, Resources.GetDimensionPixelSize(Resource.Dimension.app_loyalty_promocode_height));
        }

        private void BindControls()
        {
            if (BindingContext != null && !IsBinded)
            {
                var view = this.BindingInflate(Resource.Layout.app_loyalty_promocode, this, true);

                var editText = view.FindViewById<EditText>(Resource.Id.app_loyalty_promocode_editText);
                editText.SetOnKeyListener(new AROnEnterKeyListener(Mvx.Resolve<IMvxAndroidCurrentTopActivity>()?.Activity, HandleAction));
                editText.ClearFocus();
            }

            IsBinded = true;
        }

        private void HandleAction()
        {
            ViewModel.ApplyCommand.Execute();

            this.ClearFocus();
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            //nothing
        }
    }
}
