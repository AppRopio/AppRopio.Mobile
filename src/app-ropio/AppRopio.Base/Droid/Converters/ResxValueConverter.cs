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
using AppRopio.Base.Core.Services.Localization;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;

namespace AppRopio.Base.Droid.Converters
{
    public class ResxValueConverter : MvxValueConverter<string, string>
    {
        protected override string Convert(string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return parameter == null ?
                Mvx.Resolve<ILocalizationService>().GetLocalizableString(null, value)
                       :
                Mvx.Resolve<ILocalizationService>().GetLocalizableString(parameter as string, value);
        }
    }
}
