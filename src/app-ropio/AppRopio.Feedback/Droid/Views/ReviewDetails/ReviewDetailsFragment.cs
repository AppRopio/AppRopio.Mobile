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
using AppRopio.Base.Droid.Views;
using AppRopio.Feedback.Core.ViewModels.ReviewDetails;
using AppRopio.Feedback.Core;
using Android.Views;
using Android.OS;
using AppRopio.Feedback.Droid.Views.Score;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Views;
using Android.Widget;

namespace AppRopio.Feedback.Droid.Views.ReviewDetails
{
    public class ReviewDetailsFragment : CommonFragment<IReviewDetailsViewModel>
    {
        protected const int DELETE_ID = 111;

        public ReviewDetailsFragment()
            : base(Resource.Layout.app_feedback_reviewdetails)
        {
            Title = LocalizationService.GetLocalizableString(Core.FeedbackConstants.RESX_NAME, "ReviewDetails_Title");
        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IReviewDetailsViewModel.CanEdit))
            {
                HasOptionsMenu = ViewModel?.CanEdit ?? false;
            }
        }


        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var scoreView = view.FindViewById<ScoreView>(Resource.Id.app_feedback_myreviews_score);
            scoreView.BindingContext = new MvxAndroidBindingContext(Context, new MvxSimpleLayoutInflaterHolder(Activity.LayoutInflater), ViewModel.ScoreViewModel);

            ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            var menuItem = menu.Add(0, DELETE_ID, 0, new Java.Lang.String(LocalizationService.GetLocalizableString(Core.FeedbackConstants.RESX_NAME, "ReviewDetails_Delete")));
            menuItem.SetShowAsAction(Android.Views.ShowAsAction.Always);
            menuItem.SetActionView(Resource.Layout.app_feedback_reviewdetails_delete);
            menuItem.ActionView.Click += (sender, e) =>
            {
                ViewModel?.DeleteReviewCommand.Execute(null);
            };
            (menuItem.ActionView as Button).Text = LocalizationService.GetLocalizableString(Core.FeedbackConstants.RESX_NAME, "ReviewDetails_Delete");
        }

        public override void OnDestroyView()
        {
            if (ViewModel != null)
                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;

            base.OnDestroyView();
        }
    }
}
