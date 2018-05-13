//
//  Copyright 2018  AppRopio
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
using AppRopio.Feedback.Core.ViewModels.ReviewDetails;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Droid.Views;

namespace AppRopio.Feedback.Droid.Views.Score
{
    public class ScoreView : RelativeLayout, IMvxAndroidView<IScoreViewModel>
    {
        IMvxViewModel IMvxView.ViewModel
        {
            get
            {
                return ViewModel;
            }
            set
            {
                ViewModel = (IScoreViewModel)value;
            }
        }

        protected bool IsBinded { get; private set; }

        public IScoreViewModel ViewModel { get; set; }

        public object DataContext
        {
            get => ViewModel;
            set => ViewModel = (IScoreViewModel)value;
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

        public ScoreView(Context context) :
            base(context)
        {
            Initialize();
        }

        public ScoreView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public ScoreView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        public ScoreView(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) 
            : base(context, attrs, defStyleAttr, defStyleRes)
        {
            Initialize();
        }

        protected ScoreView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
            Initialize();
        }

        private void Initialize()
        {
        }

        private void BindControls()
        {
            if (BindingContext != null && !IsBinded)
            {
                var view = this.BindingInflate(Resource.Layout.app_feedback_scoreview, this, true);
            }

            IsBinded = true;
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            //nothing
        }
    }
}
