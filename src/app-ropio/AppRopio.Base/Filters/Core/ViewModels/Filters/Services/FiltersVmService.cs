using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Filters.API;
using AppRopio.Base.Filters.Core.Messages;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Horizontal;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Collection.Vertical;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax.Date;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.MinMax.Number;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Picker;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.MultiSelection;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.OneSelection;
using AppRopio.Models.Filters.Responses;
using MvvmCross;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugin.Messenger;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Switch;
using AppRopio.Base.API.Exceptions;

#pragma warning disable IDE0004 // Remove Unnecessary Cast

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Services
{
    public class FiltersVmService : BaseVmService, IFiltersVmService
    {
        #region Services

        protected IFiltersService FiltersService { get { return Mvx.Resolve<IFiltersService>(); } }

        protected IMvxMessenger MessengerService { get { return Mvx.Resolve<IMvxMessenger>(); } }

        #endregion

        #region Private

        private IFiltersItemVM CreateItemForFilter(Filter filter, List<ApplyedFilter> applyedFilters)
        {
            IFiltersItemVM itemVm = null;

            var applyedFilter = applyedFilters?.FirstOrDefault(x => x.Id == filter.Id);

            switch (filter.WidgetType)
            {
                case FilterWidgetType.HorizontalCollection:
                case FilterWidgetType.VerticalCollection:
                    itemVm = CreateCollection(filter, applyedFilter?.Values);
                    break;
                case FilterWidgetType.MinMax:
                    itemVm = CreateMinMax(filter, applyedFilter?.MinValue, applyedFilter?.MaxValue);
                    break;
                case FilterWidgetType.Picker:
                    itemVm = CreatePicker(filter, applyedFilter?.Values);
                    break;
                case FilterWidgetType.OneSelection:
                case FilterWidgetType.MultiSelection:
                    itemVm = CreateSelection(filter, applyedFilter?.Values);
                    break;
                case FilterWidgetType.Switch:
                    itemVm = CreateSwitch(filter, applyedFilter?.Enabled);
                    break;
            }

            if (itemVm != null)
                itemVm.Initialize();

            return itemVm;
        }

        #endregion

        #region Protected

        #region Collection

        protected virtual IBaseCollectionFiVm CreateCollection(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
        {
            return filter.WidgetType == FilterWidgetType.HorizontalCollection ?

                                                 (IBaseCollectionFiVm)CreateHorizontalCollection(filter, applyedFilterValues) :
                                                 (IBaseCollectionFiVm)CreateVerticalCollection(filter, applyedFilterValues);
        }

        protected virtual IHorizontalCollectionFiVm CreateHorizontalCollection(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
        {
            return new HorizontalCollectionFiVm(filter, applyedFilterValues);
        }

        protected virtual IVerticalCollectionFiVm CreateVerticalCollection(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
        {
            return new VerticalCollectionFiVm(filter, applyedFilterValues);
        }

        #endregion

        #region MinMax

        protected virtual IBaseMinMaxFiVm CreateMinMax(Filter filter, string minSelectedValue, string maxSelectedValue)
        {
            return filter.DataType == FilterDataType.Number ?
                         (IBaseMinMaxFiVm)CreateNumberMinMax(filter, minSelectedValue, maxSelectedValue) :
                         (IBaseMinMaxFiVm)CreateDateMinMax(filter, minSelectedValue, maxSelectedValue);
        }

        protected virtual INumberMinMaxFiVm CreateNumberMinMax(Filter filter, string minSelectedValue, string maxSelectedValue)
        {
            return new NumberMinMaxFiVm(filter, minSelectedValue, maxSelectedValue);
        }

        protected virtual IDateMinMaxFiVm CreateDateMinMax(Filter filter, string minSelectedValue, string maxSelectedValue)
        {
            return new DateMinMaxFiVm(filter, minSelectedValue, maxSelectedValue);
        }

        #endregion

        #region Picker

        protected virtual IPickerFiVm CreatePicker(Filter filter, List<ApplyedFilterValue> values)
        {
            return new PickerFiVm(filter, values);
        }

        #endregion

        #region Collection

        protected virtual IBaseSelectionFiVm CreateSelection(Filter filter, List<ApplyedFilterValue> values)
        {
            return filter.WidgetType == FilterWidgetType.OneSelection ?
                         (IBaseSelectionFiVm)CreateOneSelection(filter, values) :
                         (IBaseSelectionFiVm)CreateMultiSelection(filter, values);
        }

        protected virtual IOneSelectionFiVm CreateOneSelection(Filter filter, List<ApplyedFilterValue> values)
        {
            return new OneSelectionFiVm(filter, values);
        }

        protected virtual IMultiSelectionFiVm CreateMultiSelection(Filter filter, List<ApplyedFilterValue> values)
        {
            return new MultiSelectionFiVm(filter, values);
        }

        #endregion

        #region Switch

        protected virtual ISwitchFiVm CreateSwitch(Filter filter, bool? enabled)
        {
            return new SwitchFiVm(filter, enabled.HasValue && enabled.Value);
        }

        #endregion

        #endregion

        #region IFiltersVmService implementation

        public async Task<ObservableCollection<IFiltersItemVM>> LoadFiltersFor(string categoryId, List<ApplyedFilter> applyedFilters)
        {
            ObservableCollection<IFiltersItemVM> dataSource = null;

            try
            {
                IEnumerable<Filter> filters = null;

                if (categoryId.IsNullOrEmtpy())
                    filters = await FiltersService.LoadFilters(categoryId);
                else if (!CachedObjects.ContainsKey(categoryId))
                {
                    filters = await FiltersService.LoadFilters(categoryId);

                    if (!filters.IsNullOrEmpty())
                        CachedObjects.Add(categoryId, filters);
                }
                else if (CachedObjects.ContainsKey(categoryId))
                    filters = CachedObjects[categoryId].Cast<Filter>();

                dataSource = new ObservableCollection<IFiltersItemVM>(filters
                                                                      .Select(filter => CreateItemForFilter(filter, applyedFilters))
                                                                      .Where(x => x != null)
                                                                     );
            }
            catch (ConnectionException ex)
            {
                OnConnectionException(ex);
            }
            catch (Exception ex)
            {
                OnException(ex);
            }

            return dataSource;
        }

        public void ChangeFiltersTo(string categoryId, List<ApplyedFilter> applyedFilters)
        {
            MessengerService.Publish(new FiltersChangedMessage(this, categoryId, applyedFilters));
        }

        #endregion
    }
}
