using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppRopio.Base.Core.ViewModels.Selection.Items;
using AppRopio.Base.Core.ViewModels.Services;
using AppRopio.ECommerce.Products.Core.Messages;
using AppRopio.Models.Products.Responses;
using MvvmCross;
using MvvmCross.Plugin.Messenger;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection.Services
{
    public class ProductDetailsSelectionVmService : BaseVmService, IProductDetailsSelectionVmService
    {
        #region Services

        protected IMvxMessenger MessengerService { get { return Mvx.Resolve<IMvxMessenger>(); } }

        #endregion

        #region Protected

        protected virtual ISelectionItemVM SetupItem(ProductParameterValue value, bool selected)
        {
            return new SelectionItemVM(value.Id, value.ValueName, selected);
        }

        #endregion

        #region IProductDetailsSelectionVmService implementation

        public Task<ObservableCollection<ISelectionItemVM>> LoadItemsFor(List<ProductParameterValue> values, List<ApplyedProductParameterValue> selectedValues, string searchText)
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

        public void ChangeSelectedParameterValuesTo(string parameterId, List<ApplyedProductParameterValue> selectedValues)
        {
            MessengerService.Publish(new ProductDetailsSelectionChangedMessage(this) { ParameterId = parameterId, ApplyedParameterValues = selectedValues });
        }

        #endregion
    }
}
