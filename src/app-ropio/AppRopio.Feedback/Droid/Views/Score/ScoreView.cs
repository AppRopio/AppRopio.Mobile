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
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
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
            Visibility = ViewStates.Gone;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IScoreViewModel.Score))
            {
                var view = this.BindingInflate(Resource.Layout.app_feedback_scoreview, this, true);

                var grayBack = view.FindViewById<View>(Resource.Id.app_feedback_scoreview_grayBackground);
                var accentBack = view.FindViewById<View>(Resource.Id.app_feedback_scoreview_accentBackground);
                var textView = view.FindViewById<TextView>(Resource.Id.app_feedback_scoreview_textView);

                using (var drawable = new GradientDrawable())
                {
                    using (var grayBackground = new TypedValue())
                    {
                        Context.Theme.ResolveAttribute(Resource.Attribute.app_color_textGray, grayBackground, true);

                        drawable.SetShape(ShapeType.Rectangle);
                        drawable.SetCornerRadius(Application.Context.Theme.Resources.GetDimension(Resource.Dimension.app_feedback_reviewdetails_scoreHeight) / 2);
                        drawable.SetColor(new Color(grayBackground.Data));
                    }

                    grayBack.Background = drawable;
                }

                using (var drawable = new GradientDrawable())
                {
                    using (var accentBackground = new TypedValue())
                    {
                        Context.Theme.ResolveAttribute(Resource.Attribute.app_color_accent, accentBackground, true);

                        drawable.SetShape(ShapeType.Rectangle);
                        drawable.SetCornerRadius(Application.Context.Theme.Resources.GetDimension(Resource.Dimension.app_feedback_reviewdetails_scoreHeight) / 2);
                        drawable.SetColor(new Color(accentBackground.Data));
                    }

                    accentBack.Background = drawable;
                }

                var score = ((IScoreViewModel)ViewModel).Score;
                textView.Text = score.Value.ToString();

                var lp = accentBack.LayoutParameters;
                lp.Width = this.LayoutParameters.Width * score.Value / score.Max;
                accentBack.LayoutParameters = lp;

                Visibility = ViewStates.Visible;
            }
        }


        private void BindControls()
        {
            if (BindingContext != null && !IsBinded)
            {
                if (ViewModel.Score != null)
                    ViewModel_PropertyChanged(null, new System.ComponentModel.PropertyChangedEventArgs(nameof(IScoreViewModel.Score)));
                else
                    ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            }

            IsBinded = true;
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            //nothing
        }
    }
}
