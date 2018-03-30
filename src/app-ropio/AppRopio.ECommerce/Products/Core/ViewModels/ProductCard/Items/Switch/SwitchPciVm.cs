using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Switch
{
    public class SwitchPciVm : ProductDetailsItemVM, ISwitchPciVm
    {
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

                SelectedValue = new ApplyedProductParameter { Id = this.Id, DataType = this.DataType, Enabled = value };
            }
        }

        public SwitchPciVm(ProductParameter parameter)
            : base(parameter)
        {
            _enabled = parameter.Enabled.HasValue && parameter.Enabled.Value;
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
