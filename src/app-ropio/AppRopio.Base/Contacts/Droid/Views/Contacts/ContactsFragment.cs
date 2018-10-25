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
using AppRopio.Base.Contacts.Core;
using AppRopio.Base.Contacts.Core.ViewModels.Contacts;
using AppRopio.Base.Droid.Views;

namespace AppRopio.Base.Contacts.Droid.Views.Contacts
{
    public class ContactsFragment : CommonFragment<IContactsViewModel>
    {
        public ContactsFragment()
            : base (Resource.Layout.app_contacts)
        {
            Title = LocalizationService.GetLocalizableString(ContactsConstants.RESX_NAME, "Title");
        }
    }
}
