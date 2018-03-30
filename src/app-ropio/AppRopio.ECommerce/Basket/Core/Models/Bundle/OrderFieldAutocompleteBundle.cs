using System;
using System.Collections.Generic;
using System.Globalization;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.ECommerce.Basket.Core.ViewModels.Order.Items;
using AppRopio.Models.Basket.Responses.Enums;
using Newtonsoft.Json;

namespace AppRopio.ECommerce.Basket.Core.Models.Bundle
{
    public class OrderFieldAutocompleteBundle : BaseBundle
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public OrderFieldType Type { get; set; }

        public bool IsRequired { get; set; }

        public int AutocompleteStartIndex { get; set; }

        public Dictionary<string, string> DependentFieldsValues { get; set; }

        public OrderFieldAutocompleteBundle()
        {
        }

        public OrderFieldAutocompleteBundle(IOrderFieldItemVM orderField, Dictionary<string, string> dependentFieldsValues, NavigationType navigationType = NavigationType.Push)
            : base(navigationType, new Dictionary<string, string> 
        {
            { nameof(Id), orderField.Id },
            { nameof(Name), orderField.Name },
            { nameof(Value), orderField.Value },
            { nameof(Type), ((int)orderField.Type).ToString() },
            { nameof(IsRequired), orderField.IsRequired.ToString() },
            { nameof(AutocompleteStartIndex), orderField.AutocompleteStartIndex.ToString(NumberFormatInfo.InvariantInfo) },
            { nameof(DependentFieldsValues), JsonConvert.SerializeObject(dependentFieldsValues) }
        })
        {
            Id = orderField.Id;
            Name = orderField.Name;
            Value = orderField.Value;
            Type = orderField.Type;
            IsRequired = orderField.IsRequired;
            AutocompleteStartIndex = orderField.AutocompleteStartIndex;
            DependentFieldsValues = dependentFieldsValues;
        }
    }
}
