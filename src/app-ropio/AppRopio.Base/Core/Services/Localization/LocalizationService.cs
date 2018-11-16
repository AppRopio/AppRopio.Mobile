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

namespace AppRopio.Base.Core.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private const string LOCAL_REPO_NAME = "LocalizationResx";

        protected IEnumerable<TypeInfo> Resources { get; private set; }

        public LocalizationService(Assembly[] assemblies)
        {
            Resources = assemblies.SelectMany(assembly => assembly.DefinedTypes)
                                  .Where(typeInfo => typeInfo.AssemblyQualifiedName.Contains(LOCAL_REPO_NAME));
        }

        public void SetCurrentUICulture(CultureInfo currentUiCulture)
        {
            try
            {
                Resources.ForEach(resource =>
                {
                    var propertyInfo = resource.GetDeclaredProperty("Culture");
                    propertyInfo?.SetValue(resource, currentUiCulture);
                });
            }
            catch (ReflectionTypeLoadException)
            {

            }
        }

        public string GetLocalizableString(string resourceName, string propertyName)
        {
            string result = string.Empty;

            var resource = Resources.FirstOrDefault(x => x.Name.StartsWith(resourceName, StringComparison.OrdinalIgnoreCase));
            if (resource != null)
            {
                var resourceManager = resource.GetDeclaredProperty("ResourceManager")
                                              .GetValue(resource) as System.Resources.ResourceManager;

                result = resourceManager.GetString(propertyName, AppSettings.SettingsCulture);
            }

            return result;
        }

        public void RegisterRecourceType(Type resourceType) => Resources = new List<TypeInfo>(Resources) { resourceType.GetTypeInfo() };
    }
}
