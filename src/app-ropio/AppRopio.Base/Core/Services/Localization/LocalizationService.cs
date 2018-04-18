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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MvvmCross.Platform.IoC;

namespace AppRopio.Base.Core.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        protected List<Type> Resources { get; } = new List<Type>();

        public LocalizationService(Assembly[] assemblies)
        {
            var resources = from assembly in assemblies
                            from type in assembly.ExceptionSafeGetTypes()
                            where type.AssemblyQualifiedName.Contains("LocalizationResx")
                            select type;

            Resources = resources.ToList();
        }

        public void SetCurrentUICulture(CultureInfo currentUiCulture)
        {
            Resources.ForEach(resource =>
            {
                var propertyInfo = resource.GetTypeInfo().GetDeclaredProperty("Culture");
                propertyInfo?.SetValue(resource, currentUiCulture);
            });
        }

        public string GetLocalizableString(string resourceName, string propertyName)
        {
            string result = string.Empty;

            var resource = Resources.FirstOrDefault(x => x.Name.StartsWith(resourceName, StringComparison.OrdinalIgnoreCase));
            if (resource != null)
            {
                var resourceManager = resource.GetTypeInfo()
                                 .GetDeclaredProperty("ResourceManager")
                                 .GetValue(resource) as System.Resources.ResourceManager;

                result = resourceManager.GetString(propertyName, AppSettings.SettingsCulture);
            }

            return result;
        }

        public void RegisterRecourceType(Type resourceType) => Resources.Add(resourceType);
    }
}
