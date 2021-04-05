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
using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Settings.Core;
using AppRopio.Base.Settings.Core.ViewModels.Languages;
using AppRopio.Base.Settings.Core.ViewModels.Regions;
using AppRopio.Base.Settings.Core.ViewModels.Settings;
using AppRopio.Base.Settings.Droid.Views.Languages;
using AppRopio.Base.Settings.Droid.Views.Regions;
using AppRopio.Base.Settings.Droid.Views.Settings;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Settings.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Settings";

        public override void Load()
        {
            base.Load();

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<ISettingsViewModel, SettingsFragment>();
            viewLookupService.Register<IRegionsViewModel, RegionsFragment>();
            viewLookupService.Register<ILanguagesViewModel, LanguagesFragment>();
        }
    }
}
