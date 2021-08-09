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
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Views;
using AppRopio.Feedback.Core.ViewModels.Items;
using AppRopio.Feedback.Core.ViewModels.Items.Score;
using AppRopio.Feedback.Core.ViewModels.Items.TotalScore;
using AppRopio.Feedback.Core.ViewModels.ReviewPost;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Droid.Support.V7.RecyclerView;

namespace AppRopio.Feedback.Droid.Views.ReviewPost
{
    public class ReviewPostFragment : CommonFragment<IReviewPostViewModel>
    {
        public ReviewPostFragment()
            : base(Resource.Layout.app_feedback_reviewpost)
        {
        }

        protected virtual void SetupRecyclerView(View view, MvxRecyclerView recyclerView) => recyclerView.Adapter = SetupAdapter(recyclerView);

        protected virtual IMvxRecyclerAdapter SetupAdapter(MvxRecyclerView recyclerView) => new ARSimpleTuneRecyclerAdapter((IMvxAndroidBindingContext)BindingContext)
        {
            TuneViewHolderOnBind = TuneItemOnBind
        };

        protected virtual void TuneItemOnBind(bool first, bool last, RecyclerView.ViewHolder viewHolder)
        {
            var linearLayout = viewHolder.ItemView.FindViewById<LinearLayout>(Resource.Id.app_feedback_reviewpost_item_score_layout);
            if (linearLayout != null)
            {
                if (linearLayout.ChildCount > 0)
                    linearLayout.RemoveAllViews();

                var reviewItem = ((viewHolder as IMvxRecyclerViewHolder).DataContext as IReviewParameterItemVm);

                int maxValue = 0;
                int value = 0;

                if (reviewItem.WidgetType == Models.Feedback.Responses.ReviewWidgetType.Score)
                {
                    maxValue = (reviewItem as IScoreItemVm).MaxValue;
                    value = (reviewItem as IScoreItemVm).Value;
                }
                else
                {
                    maxValue = (reviewItem as ITotalScoreItemVm).MaxValue;
                    value = (reviewItem as ITotalScoreItemVm).Value;
                }

                for (int i = 1; i <= maxValue; i++)
                {
                    var view = Activity.LayoutInflater.Inflate(Resource.Layout.app_feedback_reviewpost_item_score_item, null, false);
                    var button = view.FindViewById<Button>(Resource.Id.app_feedback_reviewpost_item_score_item_button);

                    button.Tag = i;
                    button.Text = i.ToString();
                    button.Selected = i - 1 == value;
                    button.Click += (sender, e) =>
                    {
                        var layout = (sender as Button).Parent.Parent as ViewGroup;
                        var tag = (int)(sender as View).Tag;

                        for (int j = 0; j < layout.ChildCount; j++)
                            layout.GetChildAt(j).FindViewById<Button>(Resource.Id.app_feedback_reviewpost_item_score_item_button).Selected = (j + 1 == tag);

                        var item = ((viewHolder as IMvxRecyclerViewHolder).DataContext as IReviewParameterItemVm);
                        if (item.WidgetType == Models.Feedback.Responses.ReviewWidgetType.Score)
                        {
                            (item as IScoreItemVm).Value = tag;
                        }
                        else
                        {
                            (item as ITotalScoreItemVm).Value = tag;
                        }
                    };

                    linearLayout.AddView(view, new LinearLayout.LayoutParams(0, LinearLayout.LayoutParams.WrapContent, 1));
                }
            }
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_feedback_reviewpost_recyclerView);
            SetupRecyclerView(view, recyclerView);

            return view;
        }
    }
}
