using System.Collections.Generic;
using AppRopio.Base.Core.ViewModels.Selection.Services;
using AppRopio.Models.Products.Responses;

namespace AppRopio.ECommerce.Products.Core.ViewModels.ProductCard.Selection.Services
{
    public interface IProductDetailsSelectionVmService : IBaseSelectionVmService<ProductParameterValue, ApplyedProductParameterValue>
    {
        void ChangeSelectedParameterValuesTo(string parameterId, List<ApplyedProductParameterValue> selectedValues);
    }
}
