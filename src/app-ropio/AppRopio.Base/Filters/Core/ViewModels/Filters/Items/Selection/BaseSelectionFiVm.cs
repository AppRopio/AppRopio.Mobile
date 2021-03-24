using System;
using AppRopio.Models.Filters.Responses;
using System.Collections.Generic;
using AppRopio.Base.Filters.Core.Services;
using MvvmCross;
using MvvmCross.Plugin.Messenger;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Messages;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection
{
    public abstract class BaseSelectionFiVm : FiltersItemVm, IBaseSelectionFiVm
    {
        #region Fields

        private MvxSubscriptionToken _subscribtionToken;

        #endregion

        #region Properties

        public override ApplyedFilter SelectedValue { get; protected set; }

        protected List<FilterValue> Values { get; set; }

        protected List<ApplyedFilterValue> ApplyedFilterValues { get; set; }

        #endregion

        #region Services

        protected IFiltersNavigationVmService NavigationVmService { get { return Mvx.Resolve<IFiltersNavigationVmService>(); } }

        #endregion

        #region Constructor

        protected BaseSelectionFiVm(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
            : base(filter)
        {
            Values = filter.Values;
            ApplyedFilterValues = applyedFilterValues;
        }

        #region Protected

        protected abstract void OnSelectionMessageReceived(FiltersSelectionChangedMessage msg);

        #endregion

        #endregion

        #region Public

        #region IBaseSelectionFiVm implementation

        public void OnSelected()
        {
            if (_subscribtionToken == null)
                _subscribtionToken = Messenger.Subscribe<FiltersSelectionChangedMessage>(OnSelectionMessageReceived);

            NavigationVmService.NavigateToSelection(new Models.Bundle.SelectionBundle(this.Id, this.Name, this.WidgetType, Values, SelectedValue?.Values));
        }

        #endregion

        #endregion
    }
}
