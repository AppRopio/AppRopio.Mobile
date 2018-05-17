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
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;
using AppRopio.Feedback.Core.ViewModels.Items;

namespace AppRopio.Feedback.Droid.Views.ReviewPost
{
    public class ReviewPostItemTemplateSelector : IMvxTemplateSelector
    {
        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public int GetItemViewType(object forItemObject)
        {
            var item = forItemObject as IReviewParameterItemVm;

            if (item.WidgetType == Models.Feedback.Responses.ReviewWidgetType.Text)
                return Resource.Layout.app_feedback_reviewpost_item_text;

            return Resource.Layout.app_feedback_reviewpost_item_score;
        }
    }
}
