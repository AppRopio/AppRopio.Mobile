using System;
using AppRopio.Base.Droid.Adapters;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.Droid.Views.Filters
{
    public class FiltersTemplateSelector : IARFlatGroupTemplateSelector
    {
        public int GetHeaderViewType(object forItemObject)
        {
            throw new NotImplementedException();
        }

        public int GetFooterViewType(object forItemObject)
        {
            throw new NotImplementedException();
        }

        public int GetItemViewType(object forItemObject)
        {
            var itemVm = forItemObject as IFiltersItemVM;

            switch (itemVm.WidgetType)
            {
                case FilterWidgetType.HorizontalCollection:
                    return Resource.Layout.app_filters_filters_horizontalCollection;
                case FilterWidgetType.VerticalCollection:
                    return Resource.Layout.app_filters_filters_verticalCollection;
                case FilterWidgetType.MinMax:
                    return itemVm.DataType == FilterDataType.Date ?
                        Resource.Layout.app_filters_filters_dateMinMax :
                        Resource.Layout.app_filters_filters_numberMinMax;
                case FilterWidgetType.Picker:
                    return Resource.Layout.app_filters_filters_picker;
                case FilterWidgetType.OneSelection:
                    return Resource.Layout.app_filters_filters_oneSelection;
                case FilterWidgetType.MultiSelection:
                    return Resource.Layout.app_filters_filters_multiSelection;
                case FilterWidgetType.Switch:
                    return Resource.Layout.app_filters_filters_switch;
            }

            return -1;
        }

        public int GetItemLayoutId(int fromViewType)
        {
            return fromViewType;
        }

    }
}