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
using System.Threading.Tasks;
using AppRopio.Base.Auth.Core.Models.OAuth;
using AppRopio.Base.Auth.Core.Services;
namespace AppRopio.Base.Auth.Droid.Services.Implementation
{
    public class OAuthService : IOAuthService
    {
        public Task<string> SignInTo(OAuthType socialType)
        {
            return Task.FromResult(string.Empty);
        }
    }
}
