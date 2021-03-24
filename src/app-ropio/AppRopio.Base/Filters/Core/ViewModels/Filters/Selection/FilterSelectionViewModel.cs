using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AppRopio.Base.Core.Extentions;
using AppRopio.Base.Core.ViewModels.Selection;
using AppRopio.Base.Core.ViewModels.Selection.Items;
using AppRopio.Base.Core.ViewModels.Selection.Services;
using AppRopio.Base.Filters.Core.Models.Bundle;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Selection.Services;
using AppRopio.Models.Filters.Responses;
using MvvmCross;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Selection
{
    public class FilterSelectionViewModel : BaseSelectionViewModel<FilterValue, ApplyedFilterValue>, IFilterSelectionViewModel
    {
        #region Fields

        private string _filterId;

        private FilterWidgetType _filterWidgetType;

        #endregion

        #region Properties

        protected CancellationTokenSource CTS { get; private set; }

        public override string ApplyTitle => LocalizationService.GetLocalizableString(FiltersConstants.RESX_NAME, "Selection_Apply");

        #endregion

        #region Services

        protected override IBaseSelectionVmService<FilterValue, ApplyedFilterValue> VmService => Mvx.Resolve<IFilterSelectionVmService>();

        #endregion

        #region Private

        protected override async Task LoadContent()
        {
            Loading = true;

            var dataSource = await VmService.LoadItemsFor(Values, SelectedValues, SearchText);

            if (_filterWidgetType == FilterWidgetType.MultiSelection)
                dataSource = new ObservableCollection<ISelectionItemVM>(dataSource.OrderByDescending(x => x.Selected));

            Items = dataSource;

            Loading = false;
        }

        private Task BuildSelectedValues()
        {
            if (CTS != null)
                CTS.Cancel(false);

            CTS = new CancellationTokenSource();

            return Task.Run(() =>
            {
                SelectedValues = Items.Where(x => x.Selected).Select(y => new ApplyedFilterValue { Id = y.Id }).ToList();
            }, CTS.Token);
        }

        #endregion

        #region Protected

        protected override void OnItemSelected(ISelectionItemVM item)
        {
            if (_filterWidgetType == FilterWidgetType.MultiSelection)
                item.Selected = !item.Selected;
            else
            {
                Items.ForEach(x => x.Selected = false);
                item.Selected = true;
            }

            Task.Run(BuildSelectedValues);
        }

        protected override void OnApplyExecute()
        {
            (VmService as IFilterSelectionVmService).ChangeSelectedFiltersTo(_filterId, SelectedValues);

            Close(this);
        }

        protected override void OnClearExecute()
        {
            SelectedValues = null;

            Task.Run(LoadContent);
        }

        #region Init

        public override void Prepare(MvvmCross.ViewModels.IMvxBundle parameters)
        {
            var sortParameters = parameters.ReadAs<SelectionBundle>();
            this.InitFromBundle(sortParameters);
        }

        protected virtual void InitFromBundle(SelectionBundle parameters)
        {
            _filterId = parameters.FilterId;
            _filterWidgetType = parameters.WidgetType;

            Name = parameters.FilterName;

            Values = parameters.Values;
            SelectedValues = parameters.SelectedValues;
        }

        #endregion

        #region Search

        protected override void OnSearchTextChanged(string searchText)
        {
            //nothing
        }

        protected override void SearchCommandExecute()
        {
            Task.Run(LoadContent);
        }

        protected override void CancelSearchExecute()
        {
            SearchText = null;

            Task.Run(LoadContent);
        }

        #endregion

        #endregion

    }
}
