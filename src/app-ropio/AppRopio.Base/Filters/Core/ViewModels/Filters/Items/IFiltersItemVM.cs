using System;
using AppRopio.Models.Filters.Responses;
using MvvmCross.ViewModels;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items
{
    public interface IFiltersItemVM : IMvxViewModel
    {
        string Name { get; }

        FilterWidgetType WidgetType { get; }

        FilterDataType DataType { get; }

        ApplyedFilter SelectedValue { get; }

        void ClearSelectedValue();
    }
}
