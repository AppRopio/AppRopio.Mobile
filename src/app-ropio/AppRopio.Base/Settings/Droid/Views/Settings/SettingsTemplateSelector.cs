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
using AppRopio.Base.Settings.Core.ViewModels.Items;

namespace AppRopio.Base.Settings.Droid.Views.Settings
{
    public class SettingsTemplateSelector : IMvxTemplateSelector
    {
        public int ItemTemplateId { get; set; }

        public int GetItemViewType(object forItemObject)
        {
            var itemVm = forItemObject as ISettingsItemVm;

            switch (itemVm.ElementType)
            {
                case Core.Models.SettingsElementType.Geolocation:
                case Core.Models.SettingsElementType.Notifications:
                    return Resource.Layout.app_settings_settings_item_switch;
                default:
                    return Resource.Layout.app_settings_settings_item_picker;
            }
        }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }
    }
}
