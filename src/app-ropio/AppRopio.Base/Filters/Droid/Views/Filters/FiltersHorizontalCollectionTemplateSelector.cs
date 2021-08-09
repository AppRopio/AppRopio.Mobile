using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Items;
using MvvmCross.Droid.Support.V7.RecyclerView.ItemTemplates;

namespace AppRopio.Base.Filters.Droid.Views.Filters
{
    public class FiltersHorizontalCollectionTemplateSelector : IMvxTemplateSelector
    {
        public int ItemTemplateId { get; set; }

        public int GetItemViewType(object forItemObject)
        {
            var itemVm = forItemObject as CollectionItemVM;

            switch (itemVm.DataType)
            {
                case AppRopio.Models.Filters.Responses.FilterDataType.Color:
                    return Resource.Layout.app_filters_filters_horizontalCollection_color;
                default:
                    return Resource.Layout.app_filters_filters_horizontalCollection_text;
            }
        }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }
    }
}
