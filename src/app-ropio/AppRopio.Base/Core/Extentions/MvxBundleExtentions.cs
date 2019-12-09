using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppRopio.Base.Core.Attributes;
using MvvmCross.Core;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;

namespace AppRopio.Base.Core.Extentions
{
    public static class MvxBundleExtentions
    {
        private static object Read(IDictionary<string, string> data, Type type)
        {
            var t = Activator.CreateInstance(type);
            var propertyList =
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Where(p => p.CanWrite);

            foreach (var propertyInfo in propertyList)
            {
                string textValue;
                if (!data.TryGetValue(propertyInfo.GetCustomAttribute<DeeplinkPropertyAttribute>()?.Name ?? propertyInfo.Name, out textValue) &&
                    !data.TryGetValue(propertyInfo.Name, out textValue))
                    continue;
                
                var typedValue = MvxSingletonCache.Instance.Parser.ReadValue(textValue, propertyInfo.PropertyType, propertyInfo.Name);
                
                if (typedValue == null)
                {
                    try
                    {
                        typedValue = JsonConvert.DeserializeObject(textValue, propertyInfo.PropertyType);
                    }
                    catch (Exception ex)
                    {
                        typedValue = null;
                    }
                }

                propertyInfo.SetValue(t, typedValue, new object[0]);
            }

            return t;
        }

        public static T ReadAs<T>(this IMvxBundle bundle)
            where T : new()
        {
            return (T)Read(bundle.Data, typeof(T));
        }

        public static object ReadAs(this IMvxBundle bundle, Type type)
        {
            return Read(bundle.Data, type);
        }
    }
}
