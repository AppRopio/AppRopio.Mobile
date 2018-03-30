using AppRopio.Base.Core.ViewModels;
using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items
{
    public abstract class FiltersItemVm : BaseViewModel, IFiltersItemVM
    {
        protected string Id { get; private set; }

        public string Name { get; private set; }

        public FilterWidgetType WidgetType { get; private set; }

        public FilterDataType DataType { get; private set; }

        public abstract ApplyedFilter SelectedValue { get; protected set; }

        protected FiltersItemVm(Filter filter)
        {
            Id = filter.Id;
            Name = filter.Name;
            WidgetType = filter.WidgetType;
            DataType = filter.DataType;

            TrackInAnalytics = false;
        }

        public abstract void ClearSelectedValue();
    }
}
