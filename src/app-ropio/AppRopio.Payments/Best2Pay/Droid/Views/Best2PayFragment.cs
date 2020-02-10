﻿//
//  Copyright 2018  
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
using AppRopio.Payments.Best2Pay.Core.ViewModels.Best2Pay;
namespace AppRopio.Payments.Best2Pay.Droid.Views
{
    public class Best2PayFragment : CommonFragment<IBest2PayViewModel>
    {
        public Best2PayFragment()
            : base(Resource.Layout.app_payment_best2pay)
        {
        }
    }
}
