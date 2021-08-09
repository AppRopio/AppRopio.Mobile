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
using AppRopio.Base.Droid.Adapters;

namespace AppRopio.Base.Settings.Droid.Views.Regions
{
    public class RegionsTemplateSelector : IARFlatGroupTemplateSelector
    {
        public int ItemTemplateId { get; set; }

        public virtual int GetFooterViewType(object forItemObject)
        {
            throw new System.NotImplementedException();
        }

        public virtual int GetHeaderViewType(object forItemObject)
        {
            return Resource.Layout.app_settings_regions_header;
        }

        public virtual int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

        public virtual int GetItemViewType(object forItemObject)
        {
            return Resource.Layout.app_settings_regions_item;
        }
    }
}