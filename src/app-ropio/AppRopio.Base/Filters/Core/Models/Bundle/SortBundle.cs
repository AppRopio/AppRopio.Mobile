using System.Collections.Generic;
using AppRopio.Base.Core.Models.Bundle;
using AppRopio.Base.Core.Models.Navigation;

namespace AppRopio.Base.Filters.Core.Models.Bundle
{
    public class SortBundle : BaseBundle
    {
        public string CategoryId { get; set; }

        public string SelectedSortId { get; set; }

        public SortBundle()
        {

        }

        public SortBundle(string categoryId, NavigationType navigationType, string selectedSortId)
            : base(navigationType, new Dictionary<string, string>
            {
                { nameof(CategoryId), categoryId },
                { nameof(SelectedSortId), selectedSortId }
            })
        {
        }
    }
}
