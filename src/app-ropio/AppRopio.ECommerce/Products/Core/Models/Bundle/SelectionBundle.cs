using System;
using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Models.Products.Responses;
using Newtonsoft.Json;

namespace AppRopio.ECommerce.Products.Core.Models.Bundle
{
    public class SelectionBundle : BaseBundle
    {
        public string ParameterId { get; set; }

        public string ParameterName { get; set; }

        public List<ProductParameterValue> Values { get { return ValuesString.IsNullOrEmtpy() ? null : JsonConvert.DeserializeObject<List<ProductParameterValue>>(ValuesString); } }

        public string ValuesString { get; set; }

        public List<ApplyedProductParameterValue> SelectedValues { get { return SelectedValuesString.IsNullOrEmtpy() ? null : JsonConvert.DeserializeObject<List<ApplyedProductParameterValue>>(SelectedValuesString); } }

        public string SelectedValuesString { get; set; }

        public ProductWidgetType WidgetType { get; set; }

        public SelectionBundle()
        {

        }

        public SelectionBundle(string filterId, string filterName, ProductWidgetType widgetType, List<ProductParameterValue> values, List<ApplyedProductParameterValue> selectedValues)
             : base(new Dictionary<string, string>
            {
                { nameof(ParameterId), filterId  },
                { nameof(ParameterName), filterName  },
                { nameof(WidgetType), ((int)widgetType).ToString()  },
                { nameof(ValuesString), JsonConvert.SerializeObject(values)  },
                { nameof(SelectedValuesString), selectedValues.IsNullOrEmpty() ? string.Empty : JsonConvert.SerializeObject(selectedValues) }
            })
        {
        }
    }
}
