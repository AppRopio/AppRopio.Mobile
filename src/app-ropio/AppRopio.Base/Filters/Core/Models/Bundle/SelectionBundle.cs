using System.Collections.Generic;
using AppRopio.Models.Filters.Responses;
using MvvmCross.Core.ViewModels;
using Newtonsoft.Json;
using System;
using AppRopio.Base.Core.Models.Bundle;

namespace AppRopio.Base.Filters.Core.Models.Bundle
{
    public class SelectionBundle : BaseBundle
    {
        public string FilterId { get; set; }

        public string FilterName { get; set; }

        public List<FilterValue> Values { get { return ValuesString.IsNullOrEmtpy() ? null : JsonConvert.DeserializeObject<List<FilterValue>>(ValuesString); } }

        public string ValuesString { get; set; }

        public List<ApplyedFilterValue> SelectedValues { get { return SelectedValuesString.IsNullOrEmtpy() ? null : JsonConvert.DeserializeObject<List<ApplyedFilterValue>>(SelectedValuesString); } }

        public string SelectedValuesString { get; set; }

        public FilterWidgetType WidgetType { get; set; }

        public SelectionBundle()
        {

        }

        public SelectionBundle(string filterId, string filterName, FilterWidgetType widgetType, List<FilterValue> values, List<ApplyedFilterValue> selectedValues)
             : base(new Dictionary<string, string>
            {
                { nameof(FilterId), filterId  },
                { nameof(FilterName), filterName  },
                { nameof(WidgetType), ((int)widgetType).ToString()  },
                { nameof(ValuesString), JsonConvert.SerializeObject(values)  },
                { nameof(SelectedValuesString), selectedValues.IsNullOrEmpty() ? string.Empty : JsonConvert.SerializeObject(selectedValues) }
            })
        {
        }
    }
}
