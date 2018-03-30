using System;
using AppRopio.Models.Filters.Responses;
using System.Collections.Generic;
using AppRopio.Base.Filters.Core.ViewModels.Filters.Messages;
using System.Linq;
using System.Threading.Tasks;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Selection.OneSelection
{
    public class OneSelectionFiVm : BaseSelectionFiVm, IOneSelectionFiVm
    {
        #region Properties

        private string _valueName;
        public string ValueName
        {
            get
            {
                return _valueName;
            }
            set
            {
                _valueName = value;
                RaisePropertyChanged(() => ValueName);
            }
        }

        #endregion

        #region Constructor

        public OneSelectionFiVm(Filter filter, List<ApplyedFilterValue> applyedFilterValues)
            : base(filter, applyedFilterValues)
        {
        }

        #endregion

        #region Private

        private void LoadContent()
        {
            var newSelectedValue = ApplyedFilterValues?.SingleOrDefault();
            if (newSelectedValue == null)
                return;

            var filterValue = Values.FirstOrDefault(x => x.Id == newSelectedValue.Id);
            if (filterValue != null)
            {
                InvokeOnMainThread(() => ValueName = filterValue.ValueName);
                SelectedValue = new ApplyedFilter
                {
                    Id = this.Id,
                    DataType = this.DataType,
                    Values = new List<ApplyedFilterValue> { newSelectedValue }
                };
            }
        }

        #endregion

        #region Public

        public override Task Initialize()
        {
            return Task.Run(() => LoadContent());
        }

        #region IFilterItemVM implementation

        public override void ClearSelectedValue()
        {
            SelectedValue = null;
            ApplyedFilterValues = new List<ApplyedFilterValue>();

            ValueName = string.Empty;
        }

        #endregion

        #region BaseSelectionFiVm implementation

        protected override void OnSelectionMessageReceived(FiltersSelectionChangedMessage msg)
        {
            if (this.Id != msg.FilterId)
                return;

            var newSelectedValue = msg.ApplyedFilterValues?.SingleOrDefault();
            if (newSelectedValue == null)
                ClearSelectedValue();
            else
            {
                var filterValue = Values.FirstOrDefault(x => x.Id == newSelectedValue.Id);
                if (filterValue != null)
                {
                    ValueName = filterValue.ValueName;
                    SelectedValue = new ApplyedFilter
                    {
                        Id = this.Id,
                        DataType = this.DataType,
                        Values = new List<ApplyedFilterValue> { newSelectedValue }
                    };
                }
            }

            Messenger.Publish(new FiltersReloadWhenValueChangedMessage(this));
        }

        #endregion

        #endregion
    }
}
