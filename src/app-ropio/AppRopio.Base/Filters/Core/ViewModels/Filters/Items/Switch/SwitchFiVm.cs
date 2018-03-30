using AppRopio.Models.Filters.Responses;

namespace AppRopio.Base.Filters.Core.ViewModels.Filters.Items.Switch
{
    public class SwitchFiVm : FiltersItemVm, ISwitchFiVm
    {
        public override ApplyedFilter SelectedValue { get; protected set; }

        private bool _enabled;
        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                _enabled = value;
                RaisePropertyChanged(() => Enabled);

                SelectedValue = new ApplyedFilter { Id = this.Id, DataType = this.DataType, Enabled = value };
            }
        }

        public SwitchFiVm(Filter filter, bool enabled)
            : base(filter)
        {
            Enabled = enabled;
        }

        #region Public

        #region IFiltersItemVm implementation

        public override void ClearSelectedValue()
        {
            Enabled = false;
            SelectedValue = null;
        }

        #endregion

        #region ISelectableFilterItemVM implementation

        public void OnSelected()
        {
            Enabled = !Enabled;
        }

        #endregion

        #endregion
    }
}
