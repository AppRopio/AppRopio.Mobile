using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.Core.ViewModels.Selection.Items;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Messages;
using AppRopio.Models.Filters.Responses;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Selection.Services
{
    public class FilterSelectionVmService : BaseVmService, IFilterSelectionVmService
    {
        #region Services

        protected IMvxMessenger MessengerService { get { return Mvx.IoCProvider.Resolve<IMvxMessenger>(); } }

        #endregion

        #region Protected

        protected virtual ISelectionItemVM SetupItem(FilterValue value, bool selected)
        {
            return new SelectionItemVM(value.Id, value.ValueName, selected);
        }

        #endregion

        #region IFilterSelectionVmService implementation

        public Task<ObservableCollection<ISelectionItemVM>> LoadItemsFor(List<FilterValue> values, List<ApplyedFilterValue> selectedValues, string searchText)
        {
            return Task.Run(() =>
            {
                ObservableCollection<ISelectionItemVM> dataSource = null;

                if (searchText.IsNullOrEmtpy())
                    dataSource = new ObservableCollection<ISelectionItemVM>(values
                                                                            .Select(x => SetupItem(x, !selectedValues.IsNullOrEmpty() && selectedValues.Any(y => y.Id == x.Id)))
                                                                           );
                else
                    dataSource = new ObservableCollection<ISelectionItemVM>(values
                                                                            .Where(v => v.ValueName.Contains(searchText))
                                                                            .Select(x => SetupItem(x, !selectedValues.IsNullOrEmpty() && selectedValues.Any(y => y.Id == x.Id)))
                                                                           );

                return dataSource;
            });
        }

        public void ChangeSelectedFiltersTo(string filterId, List<ApplyedFilterValue> selectedValues)
        {
            MessengerService.Publish(new FiltersSelectionChangedMessage(this, filterId, selectedValues));
        }

        #endregion
    }
}
