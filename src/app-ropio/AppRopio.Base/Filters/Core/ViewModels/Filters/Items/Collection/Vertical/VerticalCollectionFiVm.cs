using System;
using System.Collections.Generic;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items;
using AppRopio.Models.Filters.Responses;
using System.Threading.Tasks;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Vertical
{
    public class VerticalCollectionFiVm : BaseCollectionFiVm, IVerticalCollectionFiVm
    {
        public VerticalCollectionFiVm(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
            : base(filter, applyedFilterValues)
        {

        }

        protected override void OnItemSelected(CollectionItemVM item)
        {
            item.Selected = !item.Selected;

            Task.Run(BuildSelectedValue);
        }
    }
}
