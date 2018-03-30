using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Items.Selection.OneSelection
{
    public class OneSelectionPciVm : BaseSelectionPciVm, IOneSelectionPciVm
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

        public OneSelectionPciVm(ProductParameter parameter)
            : base(parameter)
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
                SelectedValue = new ApplyedProductParameter
                {
                    Id = this.Id,
                    DataType = this.DataType,
                    Values = new List<ApplyedProductParameterValue> { newSelectedValue }
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
            ApplyedFilterValues = new List<ApplyedProductParameterValue>();

            ValueName = string.Empty;
        }

        #endregion

        #region BaseSelectionFiVm implementation

        protected override void OnSelectionMessageReceived(ProductDetailsSelectionChangedMessage msg)
        {
            if (this.Id != msg.ParameterId)
                return;

            var newSelectedValue = msg.ApplyedParameterValues?.SingleOrDefault();
            if (newSelectedValue == null)
                ClearSelectedValue();
            else
            {
                var filterValue = Values.FirstOrDefault(x => x.Id == newSelectedValue.Id);
                if (filterValue != null)
                {
                    ValueName = filterValue.ValueName;
                    SelectedValue = new ApplyedProductParameter
                    {
                        Id = this.Id,
                        DataType = this.DataType,
                        Values = new List<ApplyedProductParameterValue> { newSelectedValue }
                    };
                }
            }

            Messenger.Publish(new ProductDetailsReloadWhenValueChangedMessage(this));
        }

        #endregion

        #endregion
    }
}
