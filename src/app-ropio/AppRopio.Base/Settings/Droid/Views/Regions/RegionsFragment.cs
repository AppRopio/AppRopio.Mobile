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
using Android.OS;
using Android.Views;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Droid.Views;
using AppRopio.Base.Settings.Core;
using AppRopio.Base.Settings.Core.ViewModels.Regions;
using MvvmCross.Droid.Support.V7.RecyclerView;
using AppRopio.Base.Settings.Core.ViewModels.Regions.Items;

namespace AppRopio.Base.Settings.Droid.Views.Regions
{
    public class RegionsFragment : CommonFragment<IRegionsViewModel>
    {
        private MvxRecyclerView _recyclerView;

        public RegionsFragment()
            : base (Resource.Layout.app_settings_regions)
        {
            Title = LocalizationService.GetLocalizableString(SettingsConstants.RESX_NAME, "Regions_Title");
        }

        protected virtual void SetupRecyclerView(MvxRecyclerView recyclerView)
        {
            SetupAdapter(recyclerView);
        }

        protected virtual void SetupAdapter(MvxRecyclerView recyclerView)
        {
            recyclerView.Adapter = new ARFlatGroupAdapter((item) => ((IRegionGroupItemVm)item).Items, SetupItemTemplateSelector(), BindingContext)
            {
                HasHeader = group => !((IRegionGroupItemVm)group).Title.IsNullOrEmtpy()
            };
        }

        protected virtual IARFlatGroupTemplateSelector SetupItemTemplateSelector()
        {
            return new RegionsTemplateSelector();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = base.OnCreateView(inflater, container, savedInstanceState);

            _recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.app_information_recyclerView);

            return view;
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SetupRecyclerView(_recyclerView);
        }
    }
}
