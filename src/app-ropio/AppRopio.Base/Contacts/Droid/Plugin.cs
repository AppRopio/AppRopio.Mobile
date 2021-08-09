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
using AppRopio.Base.Contacts.Core;
using AppRopio.Base.Contacts.Core.ViewModels.Contacts;
using AppRopio.Base.Contacts.Droid.Views.Contacts;
using AppRopio.Base.Core.Plugins;
using AppRopio.Base.Core.Services.ViewLookup;
using MvvmCross;
using MvvmCross.Plugin;

namespace AppRopio.Base.Contacts.Droid
{
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public class Plugin : BasePlugin<App>
    {
		protected override string Name => "Contacts";

        public override void Load()
        {
            base.Load();

            var viewLookupService = Mvx.IoCProvider.Resolve<IViewLookupService>();

            viewLookupService.Register<IContactsViewModel, ContactsFragment>();
        }
    }
}
