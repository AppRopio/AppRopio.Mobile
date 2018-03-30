using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;
using AppRopio.Models.Filters.Responses;
using Newtonsoft.Json;

namespace AppRopio.Base.Filters.Core.Models.Bundle
{
    public class FiltersBundle : BaseBundle
    {
        public string CategoryId { get; set; }

        public List<ApplyedFilter> ApplyedFilters { get { return JsonConvert.DeserializeObject<List<ApplyedFilter>>(ApplyedFiltersString); } }

        public string ApplyedFiltersString { get; set; }

        public FiltersBundle()
        {

        }

        public FiltersBundle(string categoryId, NavigationType navigationType, List<ApplyedFilter> applyedFilters)
            : base(navigationType, new Dictionary<string, string>
            {
                { nameof(CategoryId), categoryId },
                { nameof(ApplyedFiltersString), JsonConvert.SerializeObject(applyedFilters) }
            })
        {
        }
    }
}
